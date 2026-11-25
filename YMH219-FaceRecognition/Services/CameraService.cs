using System.Diagnostics;
using Microsoft.Win32;
using OpenCvSharp;

namespace FaceRecognition.Services;

// Kamera kullanılamadığında anlaşılır mesajla fırlatılan hata
public sealed class CameraException(string message) : Exception(message);

// Yakalanan kare ve o anki kare hızı. Mat yalnızca olay süresince geçerlidir.
public sealed class FrameEventArgs(Mat frame, double fps) : EventArgs
{
    public Mat Frame { get; } = frame;
    public double Fps { get; } = fps;
}

// Kamerayı arka plan döngüsünde okuyan ve her kareyi olay olarak bildiren servis
public sealed class CameraService : IDisposable
{
    private const int MaxReadFailures = 50;

    private CancellationTokenSource? _cts;
    private Task? _loop;
    private bool _disposed;

    // Kamera açıldığında / kapandığında (true/false); arka plan iş parçacığından gelebilir
    public event EventHandler<bool>? RunningChanged;

    // Her yeni kare için; arka plan iş parçacığından gelir
    public event EventHandler<FrameEventArgs>? FrameReady;

    // Çalışırken kamera bağlantısı koparsa
    public event EventHandler<string>? CameraError;

    public bool IsRunning { get; private set; }

    // Kamerayı açar ve okuma döngüsünü başlatır (açılış birkaç saniye sürebilir)
    public async Task StartAsync(int cameraIndex = 0)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (IsRunning)
            return;

        var capture = await Task.Run(() => OpenCapture(cameraIndex));
        _cts?.Dispose();
        var cts = new CancellationTokenSource();
        _cts = cts;
        IsRunning = true;
        _loop = Task.Factory.StartNew(() => RunLoop(capture, cts.Token),
            CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        RunningChanged?.Invoke(this, true);
    }

    // Döngüyü durdurur ve kamerayı serbest bırakır
    public async Task StopAsync()
    {
        var loop = _loop;
        _cts?.Cancel();
        if (loop != null)
            await loop.ConfigureAwait(false);
    }

    // DirectShow hızlı açılır; olmazsa Media Foundation denenir
    private static VideoCapture OpenCapture(int index)
    {
        bool openedButNoFrame = false;
        foreach (var api in new[] { VideoCaptureAPIs.DSHOW, VideoCaptureAPIs.MSMF })
        {
            var capture = new VideoCapture(index, api);
            if (!capture.IsOpened())
            {
                capture.Dispose();
                continue;
            }

            capture.Set(VideoCaptureProperties.FrameWidth, 640);
            capture.Set(VideoCaptureProperties.FrameHeight, 480);

            // Açılsa bile kare gelmiyorsa kamera başka uygulamada olabilir
            using var probe = new Mat();
            for (int i = 0; i < 20; i++)
            {
                if (capture.Read(probe) && !probe.Empty())
                    return capture;
                Thread.Sleep(50);
            }

            openedButNoFrame = true;
            capture.Dispose();
        }

        if (IsCameraAccessDenied())
            throw new CameraException(
                "Windows kamera erişimine izin vermiyor.\n\n" +
                "Ayarlar > Gizlilik ve güvenlik > Kamera bölümünden \"Masaüstü uygulamalarının kameranıza erişmesine izin verin\" seçeneğini açın.");

        throw new CameraException(openedButNoFrame
            ? "Kamera açıldı ancak görüntü alınamadı.\n\nKamera başka bir uygulama (Teams, Zoom, Kamera uygulaması vb.) tarafından kullanılıyor olabilir; o uygulamayı kapatıp tekrar deneyin."
            : "Kamera bulunamadı.\n\nBilgisayara bağlı bir kamera olduğundan ve sürücüsünün çalıştığından emin olun.");
    }

    // Windows gizlilik ayarında kamera erişimi kapalı mı
    private static bool IsCameraAccessDenied()
    {
        const string key = @"Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam";
        static bool Denied(string path) =>
            string.Equals(Registry.CurrentUser.OpenSubKey(path)?.GetValue("Value") as string, "Deny",
                StringComparison.OrdinalIgnoreCase);
        try
        {
            return Denied(key) || Denied(key + @"\NonPackaged");
        }
        catch (Exception)
        {
            return false;
        }
    }

    private void RunLoop(VideoCapture capture, CancellationToken token)
    {
        string? error = null;
        try
        {
            using var frame = new Mat();
            var watch = Stopwatch.StartNew();
            int frames = 0, failures = 0;
            double fps = 0;

            while (!token.IsCancellationRequested)
            {
                if (!capture.Read(frame) || frame.Empty())
                {
                    if (++failures >= MaxReadFailures)
                    {
                        error = "Kamera bağlantısı kesildi. Kamerayı kontrol edip yeniden başlatın.";
                        break;
                    }
                    Thread.Sleep(20);
                    continue;
                }

                failures = 0;
                frames++;
                // Saniyede bir FPS güncellenir
                if (watch.ElapsedMilliseconds >= 1000)
                {
                    fps = frames * 1000.0 / watch.ElapsedMilliseconds;
                    frames = 0;
                    watch.Restart();
                }

                try
                {
                    FrameReady?.Invoke(this, new FrameEventArgs(frame, fps));
                }
                catch (Exception ex)
                {
                    // Abone hatası okuma döngüsünü durdurmasın
                    Debug.WriteLine(ex);
                }
            }
        }
        catch (Exception ex)
        {
            error = "Kamera okunurken hata oluştu: " + ex.Message;
        }
        finally
        {
            capture.Dispose();
            IsRunning = false;
            RunningChanged?.Invoke(this, false);
            if (error != null)
                CameraError?.Invoke(this, error);
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;

        _cts?.Cancel();
        try
        {
            _loop?.Wait(TimeSpan.FromSeconds(2));
        }
        catch (AggregateException)
        {
            // Döngü zaten kendi hatasını bildirdi
        }
        _cts?.Dispose();
    }
}
