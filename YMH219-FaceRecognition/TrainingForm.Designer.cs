namespace FaceRecognition
{
    partial class TrainingForm
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
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.cardPreview = new FaceRecognition.Controls.CardPanel();
            this.videoPreview = new FaceRecognition.Controls.VideoView();
            this.pnlCollectInfo = new System.Windows.Forms.Panel();
            this.progressCollect = new FaceRecognition.Controls.ProgressLine();
            this.lblCollectCount = new System.Windows.Forms.Label();
            this.lblCollectStatus = new System.Windows.Forms.Label();
            this.pnlSide = new System.Windows.Forms.Panel();
            this.cardLabels = new FaceRecognition.Controls.CardPanel();
            this.btnTrain = new FaceRecognition.Controls.ModernButton();
            this.btnDeleteLabel = new FaceRecognition.Controls.ModernButton();
            this.lblModelState = new System.Windows.Forms.Label();
            this.lvLabels = new System.Windows.Forms.ListView();
            this.colLabel = new System.Windows.Forms.ColumnHeader();
            this.colSamples = new System.Windows.Forms.ColumnHeader();
            this.lblLabelsTitle = new System.Windows.Forms.Label();
            this.cardCollect = new FaceRecognition.Controls.CardPanel();
            this.btnCancelCollect = new FaceRecognition.Controls.ModernButton();
            this.btnCollect = new FaceRecognition.Controls.ModernButton();
            this.lblCollectHint = new System.Windows.Forms.Label();
            this.txtLabel = new System.Windows.Forms.TextBox();
            this.lblLabelName = new System.Windows.Forms.Label();
            this.lblCollectTitle = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.cardPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoPreview)).BeginInit();
            this.pnlCollectInfo.SuspendLayout();
            this.pnlSide.SuspendLayout();
            this.cardLabels.SuspendLayout();
            this.cardCollect.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlHeader
            //
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1000, 60);
            this.pnlHeader.TabIndex = 0;
            //
            // lblSubtitle
            //
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblSubtitle.Location = new System.Drawing.Point(18, 35);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(420, 15);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Kameradan yüz örnekleri toplayın, etiketleri yönetin ve LBPH modelini eğitin";
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(16, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Eğitim / Veri Ekleme";
            //
            // pnlBody
            //
            this.pnlBody.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.pnlBody.Controls.Add(this.cardPreview);
            this.pnlBody.Controls.Add(this.pnlSide);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 60);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Padding = new System.Windows.Forms.Padding(16);
            this.pnlBody.Size = new System.Drawing.Size(1000, 590);
            this.pnlBody.TabIndex = 1;
            //
            // cardPreview
            //
            this.cardPreview.BackColor = System.Drawing.Color.White;
            this.cardPreview.Controls.Add(this.videoPreview);
            this.cardPreview.Controls.Add(this.pnlCollectInfo);
            this.cardPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardPreview.Location = new System.Drawing.Point(16, 16);
            this.cardPreview.Name = "cardPreview";
            this.cardPreview.Padding = new System.Windows.Forms.Padding(14);
            this.cardPreview.Size = new System.Drawing.Size(612, 558);
            this.cardPreview.TabIndex = 0;
            //
            // videoPreview
            //
            this.videoPreview.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.videoPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoPreview.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.videoPreview.Location = new System.Drawing.Point(14, 14);
            this.videoPreview.Name = "videoPreview";
            this.videoPreview.PlaceholderText = "Kamera kapalı\r\n\r\n\"Örnek Topla\" kamerayı otomatik başlatır";
            this.videoPreview.Size = new System.Drawing.Size(584, 466);
            this.videoPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.videoPreview.TabIndex = 0;
            this.videoPreview.TabStop = false;
            //
            // pnlCollectInfo
            //
            this.pnlCollectInfo.Controls.Add(this.progressCollect);
            this.pnlCollectInfo.Controls.Add(this.lblCollectCount);
            this.pnlCollectInfo.Controls.Add(this.lblCollectStatus);
            this.pnlCollectInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCollectInfo.Location = new System.Drawing.Point(14, 480);
            this.pnlCollectInfo.Name = "pnlCollectInfo";
            this.pnlCollectInfo.Size = new System.Drawing.Size(584, 64);
            this.pnlCollectInfo.TabIndex = 1;
            //
            // progressCollect
            //
            this.progressCollect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressCollect.Location = new System.Drawing.Point(0, 46);
            this.progressCollect.Maximum = 25;
            this.progressCollect.Name = "progressCollect";
            this.progressCollect.Size = new System.Drawing.Size(584, 8);
            this.progressCollect.TabIndex = 2;
            //
            // lblCollectCount
            //
            this.lblCollectCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCollectCount.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblCollectCount.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblCollectCount.Location = new System.Drawing.Point(474, 14);
            this.lblCollectCount.Name = "lblCollectCount";
            this.lblCollectCount.Size = new System.Drawing.Size(110, 24);
            this.lblCollectCount.TabIndex = 1;
            this.lblCollectCount.Text = "0 / 25";
            this.lblCollectCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblCollectStatus
            //
            this.lblCollectStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCollectStatus.AutoEllipsis = true;
            this.lblCollectStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCollectStatus.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblCollectStatus.Location = new System.Drawing.Point(0, 14);
            this.lblCollectStatus.Name = "lblCollectStatus";
            this.lblCollectStatus.Size = new System.Drawing.Size(468, 24);
            this.lblCollectStatus.TabIndex = 0;
            this.lblCollectStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // pnlSide
            //
            this.pnlSide.Controls.Add(this.cardLabels);
            this.pnlSide.Controls.Add(this.cardCollect);
            this.pnlSide.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSide.Location = new System.Drawing.Point(628, 16);
            this.pnlSide.Name = "pnlSide";
            this.pnlSide.Size = new System.Drawing.Size(356, 558);
            this.pnlSide.TabIndex = 1;
            //
            // cardLabels
            //
            this.cardLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cardLabels.BackColor = System.Drawing.Color.White;
            this.cardLabels.Controls.Add(this.btnTrain);
            this.cardLabels.Controls.Add(this.btnDeleteLabel);
            this.cardLabels.Controls.Add(this.lblModelState);
            this.cardLabels.Controls.Add(this.lvLabels);
            this.cardLabels.Controls.Add(this.lblLabelsTitle);
            this.cardLabels.Location = new System.Drawing.Point(16, 214);
            this.cardLabels.Name = "cardLabels";
            this.cardLabels.Size = new System.Drawing.Size(340, 344);
            this.cardLabels.TabIndex = 1;
            //
            // btnTrain
            //
            this.btnTrain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTrain.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnTrain.Location = new System.Drawing.Point(172, 290);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(152, 38);
            this.btnTrain.TabIndex = 4;
            this.btnTrain.Text = "Modeli Eğit";
            this.btnTrain.Variant = FaceRecognition.Controls.ButtonVariant.Success;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            //
            // btnDeleteLabel
            //
            this.btnDeleteLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnDeleteLabel.Location = new System.Drawing.Point(16, 290);
            this.btnDeleteLabel.Name = "btnDeleteLabel";
            this.btnDeleteLabel.Size = new System.Drawing.Size(148, 38);
            this.btnDeleteLabel.TabIndex = 3;
            this.btnDeleteLabel.Text = "Etiketi Sil";
            this.btnDeleteLabel.Variant = FaceRecognition.Controls.ButtonVariant.Danger;
            this.btnDeleteLabel.Click += new System.EventHandler(this.btnDeleteLabel_Click);
            //
            // lblModelState
            //
            this.lblModelState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblModelState.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblModelState.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblModelState.Location = new System.Drawing.Point(16, 240);
            this.lblModelState.Name = "lblModelState";
            this.lblModelState.Size = new System.Drawing.Size(308, 42);
            this.lblModelState.TabIndex = 2;
            this.lblModelState.Text = "Model durumu";
            this.lblModelState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lvLabels
            //
            this.lvLabels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLabels.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvLabels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colLabel,
            this.colSamples});
            this.lvLabels.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lvLabels.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lvLabels.FullRowSelect = true;
            this.lvLabels.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvLabels.HideSelection = false;
            this.lvLabels.Location = new System.Drawing.Point(16, 40);
            this.lvLabels.MultiSelect = false;
            this.lvLabels.Name = "lvLabels";
            this.lvLabels.Size = new System.Drawing.Size(308, 192);
            this.lvLabels.TabIndex = 1;
            this.lvLabels.UseCompatibleStateImageBehavior = false;
            this.lvLabels.View = System.Windows.Forms.View.Details;
            this.lvLabels.SelectedIndexChanged += new System.EventHandler(this.lvLabels_SelectedIndexChanged);
            //
            // colLabel
            //
            this.colLabel.Text = "Etiket";
            this.colLabel.Width = 200;
            //
            // colSamples
            //
            this.colSamples.Text = "Örnek sayısı";
            this.colSamples.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colSamples.Width = 100;
            //
            // lblLabelsTitle
            //
            this.lblLabelsTitle.AutoSize = true;
            this.lblLabelsTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F);
            this.lblLabelsTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblLabelsTitle.Location = new System.Drawing.Point(16, 14);
            this.lblLabelsTitle.Name = "lblLabelsTitle";
            this.lblLabelsTitle.Size = new System.Drawing.Size(132, 15);
            this.lblLabelsTitle.TabIndex = 0;
            this.lblLabelsTitle.Text = "ETİKETLER VE ÖRNEKLER";
            //
            // cardCollect
            //
            this.cardCollect.BackColor = System.Drawing.Color.White;
            this.cardCollect.Controls.Add(this.btnCancelCollect);
            this.cardCollect.Controls.Add(this.btnCollect);
            this.cardCollect.Controls.Add(this.lblCollectHint);
            this.cardCollect.Controls.Add(this.txtLabel);
            this.cardCollect.Controls.Add(this.lblLabelName);
            this.cardCollect.Controls.Add(this.lblCollectTitle);
            this.cardCollect.Location = new System.Drawing.Point(16, 0);
            this.cardCollect.Name = "cardCollect";
            this.cardCollect.Size = new System.Drawing.Size(340, 198);
            this.cardCollect.TabIndex = 0;
            //
            // btnCancelCollect
            //
            this.btnCancelCollect.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnCancelCollect.Location = new System.Drawing.Point(224, 144);
            this.btnCancelCollect.Name = "btnCancelCollect";
            this.btnCancelCollect.Size = new System.Drawing.Size(100, 38);
            this.btnCancelCollect.TabIndex = 5;
            this.btnCancelCollect.Text = "İptal";
            this.btnCancelCollect.Click += new System.EventHandler(this.btnCancelCollect_Click);
            //
            // btnCollect
            //
            this.btnCollect.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnCollect.Location = new System.Drawing.Point(16, 144);
            this.btnCollect.Name = "btnCollect";
            this.btnCollect.Size = new System.Drawing.Size(200, 38);
            this.btnCollect.TabIndex = 4;
            this.btnCollect.Text = "Örnek Topla (25)";
            this.btnCollect.Variant = FaceRecognition.Controls.ButtonVariant.Primary;
            this.btnCollect.Click += new System.EventHandler(this.btnCollect_Click);
            //
            // lblCollectHint
            //
            this.lblCollectHint.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblCollectHint.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblCollectHint.Location = new System.Drawing.Point(16, 98);
            this.lblCollectHint.Name = "lblCollectHint";
            this.lblCollectHint.Size = new System.Drawing.Size(308, 40);
            this.lblCollectHint.TabIndex = 3;
            this.lblCollectHint.Text = "Kameraya bakın ve başınızı hafifçe çevirin. Karede yalnızca bir yüz olmalı. Mevcut etikete örnek eklemek için listeden seçin.";
            //
            // txtLabel
            //
            this.txtLabel.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.txtLabel.Location = new System.Drawing.Point(16, 62);
            this.txtLabel.MaxLength = 40;
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.PlaceholderText = "ör. Baki (gözlüklü)";
            this.txtLabel.Size = new System.Drawing.Size(308, 26);
            this.txtLabel.TabIndex = 2;
            //
            // lblLabelName
            //
            this.lblLabelName.AutoSize = true;
            this.lblLabelName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLabelName.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.lblLabelName.Location = new System.Drawing.Point(15, 40);
            this.lblLabelName.Name = "lblLabelName";
            this.lblLabelName.Size = new System.Drawing.Size(58, 15);
            this.lblLabelName.TabIndex = 1;
            this.lblLabelName.Text = "Etiket adı";
            //
            // lblCollectTitle
            //
            this.lblCollectTitle.AutoSize = true;
            this.lblCollectTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F);
            this.lblCollectTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblCollectTitle.Location = new System.Drawing.Point(16, 14);
            this.lblCollectTitle.Name = "lblCollectTitle";
            this.lblCollectTitle.Size = new System.Drawing.Size(96, 15);
            this.lblCollectTitle.TabIndex = 0;
            this.lblCollectTitle.Text = "ÖRNEK TOPLAMA";
            //
            // TrainingForm
            //
            this.AcceptButton = this.btnCollect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(900, 640);
            this.Name = "TrainingForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Eğitim / Veri Ekleme";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.cardPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.videoPreview)).EndInit();
            this.pnlCollectInfo.ResumeLayout(false);
            this.pnlSide.ResumeLayout(false);
            this.cardLabels.ResumeLayout(false);
            this.cardLabels.PerformLayout();
            this.cardCollect.ResumeLayout(false);
            this.cardCollect.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel pnlBody;
        private FaceRecognition.Controls.CardPanel cardPreview;
        private FaceRecognition.Controls.VideoView videoPreview;
        private System.Windows.Forms.Panel pnlCollectInfo;
        private System.Windows.Forms.Label lblCollectStatus;
        private System.Windows.Forms.Label lblCollectCount;
        private FaceRecognition.Controls.ProgressLine progressCollect;
        private System.Windows.Forms.Panel pnlSide;
        private FaceRecognition.Controls.CardPanel cardCollect;
        private System.Windows.Forms.Label lblCollectTitle;
        private System.Windows.Forms.Label lblLabelName;
        private System.Windows.Forms.TextBox txtLabel;
        private System.Windows.Forms.Label lblCollectHint;
        private FaceRecognition.Controls.ModernButton btnCollect;
        private FaceRecognition.Controls.ModernButton btnCancelCollect;
        private FaceRecognition.Controls.CardPanel cardLabels;
        private System.Windows.Forms.Label lblLabelsTitle;
        private System.Windows.Forms.ListView lvLabels;
        private System.Windows.Forms.ColumnHeader colLabel;
        private System.Windows.Forms.ColumnHeader colSamples;
        private System.Windows.Forms.Label lblModelState;
        private FaceRecognition.Controls.ModernButton btnDeleteLabel;
        private FaceRecognition.Controls.ModernButton btnTrain;
    }
}
