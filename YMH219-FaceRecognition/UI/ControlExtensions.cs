namespace FaceRecognition.UI;

internal static class ControlExtensions
{
    // Arka plan iş parçacığından arayüzü güvenle günceller.
    // Form kapanmışsa false döner (çağıran, elindeki kaynakları serbest bırakmalıdır).
    public static bool TryBeginInvoke(this Control control, Action action)
    {
        if (control.IsDisposed || !control.IsHandleCreated)
            return false;
        try
        {
            control.BeginInvoke(action);
            return true;
        }
        catch (InvalidOperationException)
        {
            // Form kapanırken tutamaç yok edilmiş olabilir
            return false;
        }
    }
}
