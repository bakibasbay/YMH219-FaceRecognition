namespace FaceRecognition
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnTraining = new FaceRecognition.Controls.ModernButton();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblModelInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.cardVideo = new FaceRecognition.Controls.CardPanel();
            this.videoView = new FaceRecognition.Controls.VideoView();
            this.flpToolbar = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStartCamera = new FaceRecognition.Controls.ModernButton();
            this.btnStopCamera = new FaceRecognition.Controls.ModernButton();
            this.btnOpenImage = new FaceRecognition.Controls.ModernButton();
            this.lblSource = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.cardModel = new FaceRecognition.Controls.CardPanel();
            this.lblModelStatus = new System.Windows.Forms.Label();
            this.lblModelTitle = new System.Windows.Forms.Label();
            this.cardSettings = new FaceRecognition.Controls.CardPanel();
            this.tbThreshold = new System.Windows.Forms.TrackBar();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.tglSaveFaces = new FaceRecognition.Controls.ToggleSwitch();
            this.tglRecognition = new FaceRecognition.Controls.ToggleSwitch();
            this.lblSettingsTitle = new System.Windows.Forms.Label();
            this.cardPrediction = new FaceRecognition.Controls.CardPanel();
            this.lblPredictionHint = new System.Windows.Forms.Label();
            this.lblPredictionScore = new System.Windows.Forms.Label();
            this.lblPredictionLabel = new System.Windows.Forms.Label();
            this.pnlPredictionAccent = new System.Windows.Forms.Panel();
            this.lblPredictionTitle = new System.Windows.Forms.Label();
            this.cardStats = new FaceRecognition.Controls.CardPanel();
            this.lblFaceCount = new System.Windows.Forms.Label();
            this.lblFacesTitle = new System.Windows.Forms.Label();
            this.pnlStatsDivider = new System.Windows.Forms.Panel();
            this.lblFps = new System.Windows.Forms.Label();
            this.lblFpsTitle = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.cardVideo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoView)).BeginInit();
            this.flpToolbar.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.cardModel.SuspendLayout();
            this.cardSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbThreshold)).BeginInit();
            this.cardPrediction.SuspendLayout();
            this.cardStats.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlHeader
            //
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlHeader.Controls.Add(this.btnTraining);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1200, 64);
            this.pnlHeader.TabIndex = 0;
            //
            // btnTraining
            //
            this.btnTraining.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTraining.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnTraining.Location = new System.Drawing.Point(990, 13);
            this.btnTraining.Name = "btnTraining";
            this.btnTraining.Size = new System.Drawing.Size(194, 38);
            this.btnTraining.TabIndex = 2;
            this.btnTraining.Text = "Eğitim / Veri Ekleme";
            this.btnTraining.Variant = FaceRecognition.Controls.ButtonVariant.Primary;
            this.btnTraining.Click += new System.EventHandler(this.btnTraining_Click);
            //
            // lblSubtitle
            //
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblSubtitle.Location = new System.Drawing.Point(18, 38);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(330, 15);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "OpenCV · Haar cascade ile yüz algılama · LBPH ile yüz tanıma";
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 15F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(15, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(246, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Yüz Algılama ve Tanıma";
            //
            // statusStrip
            //
            this.statusStrip.BackColor = System.Drawing.Color.White;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblModelInfo});
            this.statusStrip.Location = new System.Drawing.Point(0, 758);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.statusStrip.Size = new System.Drawing.Size(1200, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            //
            // lblStatus
            //
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1038, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "Hazır";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblModelInfo
            //
            this.lblModelInfo.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblModelInfo.Name = "lblModelInfo";
            this.lblModelInfo.Size = new System.Drawing.Size(126, 17);
            this.lblModelInfo.Text = "Model: eğitilmemiş";
            //
            // pnlBody
            //
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.pnlBody.Controls.Add(this.cardVideo);
            this.pnlBody.Controls.Add(this.pnlRight);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 64);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Padding = new System.Windows.Forms.Padding(16);
            this.pnlBody.Size = new System.Drawing.Size(1200, 694);
            this.pnlBody.TabIndex = 1;
            //
            // cardVideo
            //
            this.cardVideo.BackColor = System.Drawing.Color.White;
            this.cardVideo.Controls.Add(this.videoView);
            this.cardVideo.Controls.Add(this.flpToolbar);
            this.cardVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardVideo.Location = new System.Drawing.Point(16, 16);
            this.cardVideo.Name = "cardVideo";
            this.cardVideo.Padding = new System.Windows.Forms.Padding(14);
            this.cardVideo.Size = new System.Drawing.Size(852, 662);
            this.cardVideo.TabIndex = 0;
            //
            // videoView
            //
            this.videoView.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.videoView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoView.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.videoView.Location = new System.Drawing.Point(14, 66);
            this.videoView.Name = "videoView";
            this.videoView.PlaceholderText = "Kamera kapalı\r\n\r\n\"Kamera Başlat\" ya da \"Görüntü Seç\" ile başlayın";
            this.videoView.Size = new System.Drawing.Size(824, 582);
            this.videoView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.videoView.TabIndex = 1;
            this.videoView.TabStop = false;
            //
            // flpToolbar
            //
            this.flpToolbar.Controls.Add(this.btnStartCamera);
            this.flpToolbar.Controls.Add(this.btnStopCamera);
            this.flpToolbar.Controls.Add(this.btnOpenImage);
            this.flpToolbar.Controls.Add(this.lblSource);
            this.flpToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpToolbar.Location = new System.Drawing.Point(14, 14);
            this.flpToolbar.Name = "flpToolbar";
            this.flpToolbar.Size = new System.Drawing.Size(824, 52);
            this.flpToolbar.TabIndex = 0;
            this.flpToolbar.WrapContents = false;
            //
            // btnStartCamera
            //
            this.btnStartCamera.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnStartCamera.Location = new System.Drawing.Point(0, 0);
            this.btnStartCamera.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnStartCamera.Name = "btnStartCamera";
            this.btnStartCamera.Size = new System.Drawing.Size(140, 38);
            this.btnStartCamera.TabIndex = 0;
            this.btnStartCamera.Text = "Kamera Başlat";
            this.btnStartCamera.Variant = FaceRecognition.Controls.ButtonVariant.Primary;
            this.btnStartCamera.Click += new System.EventHandler(this.btnStartCamera_Click);
            //
            // btnStopCamera
            //
            this.btnStopCamera.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnStopCamera.Location = new System.Drawing.Point(148, 0);
            this.btnStopCamera.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnStopCamera.Name = "btnStopCamera";
            this.btnStopCamera.Size = new System.Drawing.Size(110, 38);
            this.btnStopCamera.TabIndex = 1;
            this.btnStopCamera.Text = "Durdur";
            this.btnStopCamera.Variant = FaceRecognition.Controls.ButtonVariant.Danger;
            this.btnStopCamera.Click += new System.EventHandler(this.btnStopCamera_Click);
            //
            // btnOpenImage
            //
            this.btnOpenImage.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnOpenImage.Location = new System.Drawing.Point(266, 0);
            this.btnOpenImage.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.btnOpenImage.Name = "btnOpenImage";
            this.btnOpenImage.Size = new System.Drawing.Size(140, 38);
            this.btnOpenImage.TabIndex = 2;
            this.btnOpenImage.Text = "Görüntü Seç…";
            this.btnOpenImage.Click += new System.EventHandler(this.btnOpenImage_Click);
            //
            // lblSource
            //
            this.lblSource.AutoEllipsis = true;
            this.lblSource.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSource.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblSource.Location = new System.Drawing.Point(422, 0);
            this.lblSource.Margin = new System.Windows.Forms.Padding(0);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(400, 38);
            this.lblSource.TabIndex = 3;
            this.lblSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // pnlRight
            //
            this.pnlRight.Controls.Add(this.cardModel);
            this.pnlRight.Controls.Add(this.cardSettings);
            this.pnlRight.Controls.Add(this.cardPrediction);
            this.pnlRight.Controls.Add(this.cardStats);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(868, 16);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(316, 662);
            this.pnlRight.TabIndex = 1;
            //
            // cardModel
            //
            this.cardModel.BackColor = System.Drawing.Color.White;
            this.cardModel.Controls.Add(this.lblModelStatus);
            this.cardModel.Controls.Add(this.lblModelTitle);
            this.cardModel.Location = new System.Drawing.Point(16, 474);
            this.cardModel.Name = "cardModel";
            this.cardModel.Size = new System.Drawing.Size(300, 120);
            this.cardModel.TabIndex = 3;
            //
            // lblModelStatus
            //
            this.lblModelStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblModelStatus.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblModelStatus.Location = new System.Drawing.Point(18, 38);
            this.lblModelStatus.Name = "lblModelStatus";
            this.lblModelStatus.Size = new System.Drawing.Size(266, 74);
            this.lblModelStatus.TabIndex = 1;
            this.lblModelStatus.Text = "Model eğitilmemiş.";
            //
            // lblModelTitle
            //
            this.lblModelTitle.AutoSize = true;
            this.lblModelTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F);
            this.lblModelTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblModelTitle.Location = new System.Drawing.Point(16, 14);
            this.lblModelTitle.Name = "lblModelTitle";
            this.lblModelTitle.Size = new System.Drawing.Size(44, 15);
            this.lblModelTitle.TabIndex = 0;
            this.lblModelTitle.Text = "MODEL";
            //
            // cardSettings
            //
            this.cardSettings.BackColor = System.Drawing.Color.White;
            this.cardSettings.Controls.Add(this.tbThreshold);
            this.cardSettings.Controls.Add(this.lblThreshold);
            this.cardSettings.Controls.Add(this.tglSaveFaces);
            this.cardSettings.Controls.Add(this.tglRecognition);
            this.cardSettings.Controls.Add(this.lblSettingsTitle);
            this.cardSettings.Location = new System.Drawing.Point(16, 282);
            this.cardSettings.Name = "cardSettings";
            this.cardSettings.Size = new System.Drawing.Size(300, 180);
            this.cardSettings.TabIndex = 2;
            //
            // tbThreshold
            //
            this.tbThreshold.BackColor = System.Drawing.Color.White;
            this.tbThreshold.LargeChange = 5;
            this.tbThreshold.Location = new System.Drawing.Point(12, 128);
            this.tbThreshold.Maximum = 150;
            this.tbThreshold.Minimum = 20;
            this.tbThreshold.Name = "tbThreshold";
            this.tbThreshold.Size = new System.Drawing.Size(276, 45);
            this.tbThreshold.TabIndex = 4;
            this.tbThreshold.TickFrequency = 10;
            this.tbThreshold.Value = 70;
            this.tbThreshold.ValueChanged += new System.EventHandler(this.tbThreshold_ValueChanged);
            //
            // lblThreshold
            //
            this.lblThreshold.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblThreshold.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblThreshold.Location = new System.Drawing.Point(16, 106);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(268, 20);
            this.lblThreshold.TabIndex = 3;
            this.lblThreshold.Text = "\"Bilinmiyor\" eşiği: 70";
            //
            // tglSaveFaces
            //
            this.tglSaveFaces.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.tglSaveFaces.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.tglSaveFaces.Location = new System.Drawing.Point(16, 70);
            this.tglSaveFaces.Name = "tglSaveFaces";
            this.tglSaveFaces.Size = new System.Drawing.Size(268, 28);
            this.tglSaveFaces.TabIndex = 2;
            this.tglSaveFaces.Text = "Yüzleri kaydet (DetectedFaces)";
            this.tglSaveFaces.CheckedChanged += new System.EventHandler(this.tglSaveFaces_CheckedChanged);
            //
            // tglRecognition
            //
            this.tglRecognition.Checked = true;
            this.tglRecognition.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tglRecognition.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.tglRecognition.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.tglRecognition.Location = new System.Drawing.Point(16, 38);
            this.tglRecognition.Name = "tglRecognition";
            this.tglRecognition.Size = new System.Drawing.Size(268, 28);
            this.tglRecognition.TabIndex = 1;
            this.tglRecognition.Text = "Tanıma açık";
            this.tglRecognition.CheckedChanged += new System.EventHandler(this.tglRecognition_CheckedChanged);
            //
            // lblSettingsTitle
            //
            this.lblSettingsTitle.AutoSize = true;
            this.lblSettingsTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F);
            this.lblSettingsTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblSettingsTitle.Location = new System.Drawing.Point(16, 14);
            this.lblSettingsTitle.Name = "lblSettingsTitle";
            this.lblSettingsTitle.Size = new System.Drawing.Size(104, 15);
            this.lblSettingsTitle.TabIndex = 0;
            this.lblSettingsTitle.Text = "TANIMA AYARLARI";
            //
            // cardPrediction
            //
            this.cardPrediction.BackColor = System.Drawing.Color.White;
            this.cardPrediction.Controls.Add(this.lblPredictionHint);
            this.cardPrediction.Controls.Add(this.lblPredictionScore);
            this.cardPrediction.Controls.Add(this.lblPredictionLabel);
            this.cardPrediction.Controls.Add(this.pnlPredictionAccent);
            this.cardPrediction.Controls.Add(this.lblPredictionTitle);
            this.cardPrediction.Location = new System.Drawing.Point(16, 92);
            this.cardPrediction.Name = "cardPrediction";
            this.cardPrediction.Size = new System.Drawing.Size(300, 178);
            this.cardPrediction.TabIndex = 1;
            //
            // lblPredictionHint
            //
            this.lblPredictionHint.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblPredictionHint.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblPredictionHint.Location = new System.Drawing.Point(16, 118);
            this.lblPredictionHint.Name = "lblPredictionHint";
            this.lblPredictionHint.Size = new System.Drawing.Size(270, 52);
            this.lblPredictionHint.TabIndex = 4;
            this.lblPredictionHint.Text = "Güven skoru LBPH uzaklığıdır: değer ne kadar düşükse yüz o kadar benzer. Eşiğin üzerindeki tahminler \"Bilinmiyor\" sayılır.";
            //
            // lblPredictionScore
            //
            this.lblPredictionScore.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblPredictionScore.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblPredictionScore.Location = new System.Drawing.Point(30, 82);
            this.lblPredictionScore.Name = "lblPredictionScore";
            this.lblPredictionScore.Size = new System.Drawing.Size(256, 22);
            this.lblPredictionScore.TabIndex = 3;
            this.lblPredictionScore.Text = "Güven skoru: –";
            //
            // lblPredictionLabel
            //
            this.lblPredictionLabel.AutoEllipsis = true;
            this.lblPredictionLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 17F);
            this.lblPredictionLabel.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblPredictionLabel.Location = new System.Drawing.Point(28, 40);
            this.lblPredictionLabel.Name = "lblPredictionLabel";
            this.lblPredictionLabel.Size = new System.Drawing.Size(258, 40);
            this.lblPredictionLabel.TabIndex = 2;
            this.lblPredictionLabel.Text = "Henüz yok";
            this.lblPredictionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // pnlPredictionAccent
            //
            this.pnlPredictionAccent.BackColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.pnlPredictionAccent.Location = new System.Drawing.Point(18, 44);
            this.pnlPredictionAccent.Name = "pnlPredictionAccent";
            this.pnlPredictionAccent.Size = new System.Drawing.Size(4, 58);
            this.pnlPredictionAccent.TabIndex = 1;
            //
            // lblPredictionTitle
            //
            this.lblPredictionTitle.AutoSize = true;
            this.lblPredictionTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F);
            this.lblPredictionTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblPredictionTitle.Location = new System.Drawing.Point(16, 14);
            this.lblPredictionTitle.Name = "lblPredictionTitle";
            this.lblPredictionTitle.Size = new System.Drawing.Size(73, 15);
            this.lblPredictionTitle.TabIndex = 0;
            this.lblPredictionTitle.Text = "SON TAHMİN";
            //
            // cardStats
            //
            this.cardStats.BackColor = System.Drawing.Color.White;
            this.cardStats.Controls.Add(this.lblFaceCount);
            this.cardStats.Controls.Add(this.lblFacesTitle);
            this.cardStats.Controls.Add(this.pnlStatsDivider);
            this.cardStats.Controls.Add(this.lblFps);
            this.cardStats.Controls.Add(this.lblFpsTitle);
            this.cardStats.Location = new System.Drawing.Point(16, 0);
            this.cardStats.Name = "cardStats";
            this.cardStats.Size = new System.Drawing.Size(300, 80);
            this.cardStats.TabIndex = 0;
            //
            // lblFaceCount
            //
            this.lblFaceCount.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            this.lblFaceCount.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblFaceCount.Location = new System.Drawing.Point(164, 32);
            this.lblFaceCount.Name = "lblFaceCount";
            this.lblFaceCount.Size = new System.Drawing.Size(120, 36);
            this.lblFaceCount.TabIndex = 4;
            this.lblFaceCount.Text = "0";
            //
            // lblFacesTitle
            //
            this.lblFacesTitle.AutoSize = true;
            this.lblFacesTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F);
            this.lblFacesTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblFacesTitle.Location = new System.Drawing.Point(166, 14);
            this.lblFacesTitle.Name = "lblFacesTitle";
            this.lblFacesTitle.Size = new System.Drawing.Size(92, 15);
            this.lblFacesTitle.TabIndex = 3;
            this.lblFacesTitle.Text = "ALGILANAN YÜZ";
            //
            // pnlStatsDivider
            //
            this.pnlStatsDivider.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlStatsDivider.Location = new System.Drawing.Point(150, 14);
            this.pnlStatsDivider.Name = "pnlStatsDivider";
            this.pnlStatsDivider.Size = new System.Drawing.Size(1, 52);
            this.pnlStatsDivider.TabIndex = 2;
            //
            // lblFps
            //
            this.lblFps.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            this.lblFps.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblFps.Location = new System.Drawing.Point(14, 32);
            this.lblFps.Name = "lblFps";
            this.lblFps.Size = new System.Drawing.Size(120, 36);
            this.lblFps.TabIndex = 1;
            this.lblFps.Text = "–";
            //
            // lblFpsTitle
            //
            this.lblFpsTitle.AutoSize = true;
            this.lblFpsTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F);
            this.lblFpsTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblFpsTitle.Location = new System.Drawing.Point(16, 14);
            this.lblFpsTitle.Name = "lblFpsTitle";
            this.lblFpsTitle.Size = new System.Drawing.Size(26, 15);
            this.lblFpsTitle.TabIndex = 0;
            this.lblFpsTitle.Text = "FPS";
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.ClientSize = new System.Drawing.Size(1200, 780);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1000, 760);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Yüz Algılama ve Tanıma";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.cardVideo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.videoView)).EndInit();
            this.flpToolbar.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.cardModel.ResumeLayout(false);
            this.cardModel.PerformLayout();
            this.cardSettings.ResumeLayout(false);
            this.cardSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbThreshold)).EndInit();
            this.cardPrediction.ResumeLayout(false);
            this.cardPrediction.PerformLayout();
            this.cardStats.ResumeLayout(false);
            this.cardStats.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private FaceRecognition.Controls.ModernButton btnTraining;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblModelInfo;
        private System.Windows.Forms.Panel pnlBody;
        private FaceRecognition.Controls.CardPanel cardVideo;
        private System.Windows.Forms.FlowLayoutPanel flpToolbar;
        private FaceRecognition.Controls.ModernButton btnStartCamera;
        private FaceRecognition.Controls.ModernButton btnStopCamera;
        private FaceRecognition.Controls.ModernButton btnOpenImage;
        private System.Windows.Forms.Label lblSource;
        private FaceRecognition.Controls.VideoView videoView;
        private System.Windows.Forms.Panel pnlRight;
        private FaceRecognition.Controls.CardPanel cardStats;
        private System.Windows.Forms.Label lblFpsTitle;
        private System.Windows.Forms.Label lblFps;
        private System.Windows.Forms.Panel pnlStatsDivider;
        private System.Windows.Forms.Label lblFacesTitle;
        private System.Windows.Forms.Label lblFaceCount;
        private FaceRecognition.Controls.CardPanel cardPrediction;
        private System.Windows.Forms.Label lblPredictionTitle;
        private System.Windows.Forms.Panel pnlPredictionAccent;
        private System.Windows.Forms.Label lblPredictionLabel;
        private System.Windows.Forms.Label lblPredictionScore;
        private System.Windows.Forms.Label lblPredictionHint;
        private FaceRecognition.Controls.CardPanel cardSettings;
        private System.Windows.Forms.Label lblSettingsTitle;
        private FaceRecognition.Controls.ToggleSwitch tglRecognition;
        private FaceRecognition.Controls.ToggleSwitch tglSaveFaces;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.TrackBar tbThreshold;
        private FaceRecognition.Controls.CardPanel cardModel;
        private System.Windows.Forms.Label lblModelTitle;
        private System.Windows.Forms.Label lblModelStatus;
    }
}
