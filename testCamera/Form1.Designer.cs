namespace testCamera
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureModeButton = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureStaticMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureDynamicMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videoModeButton = new System.Windows.Forms.ToolStripMenuItem();
            this.staticToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.videoStatic30FPSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videoStatic60FPSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dynamicToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.videoDynamic30FPSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videoDynamic60FPSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.beginCaptureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.endCaptureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.highlightTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsButton,
            this.toolStripSeparator1,
            this.quitButton});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // saveAsButton
            // 
            this.saveAsButton.Name = "saveAsButton";
            this.saveAsButton.Size = new System.Drawing.Size(114, 22);
            this.saveAsButton.Text = "Save As";
            this.saveAsButton.Click += new System.EventHandler(this.saveAsButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(111, 6);
            // 
            // quitButton
            // 
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(114, 22);
            this.quitButton.Text = "Exit";
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pictureModeButton,
            this.videoModeButton});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(95, 20);
            this.toolStripMenuItem2.Text = "Capture Mode";
            // 
            // pictureModeButton
            // 
            this.pictureModeButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pictureStaticMenuItem,
            this.pictureDynamicMenuItem});
            this.pictureModeButton.Name = "pictureModeButton";
            this.pictureModeButton.Size = new System.Drawing.Size(111, 22);
            this.pictureModeButton.Text = "Picture";
            // 
            // pictureStaticMenuItem
            // 
            this.pictureStaticMenuItem.Name = "pictureStaticMenuItem";
            this.pictureStaticMenuItem.Size = new System.Drawing.Size(121, 22);
            this.pictureStaticMenuItem.Text = "Static";
            this.pictureStaticMenuItem.ToolTipText = "In static video mode, the captured image will not resize with the form. This is t" +
    "he default option";
            this.pictureStaticMenuItem.Click += new System.EventHandler(this.pictureStaticMenuItem_Click);
            // 
            // pictureDynamicMenuItem
            // 
            this.pictureDynamicMenuItem.Name = "pictureDynamicMenuItem";
            this.pictureDynamicMenuItem.Size = new System.Drawing.Size(121, 22);
            this.pictureDynamicMenuItem.Text = "Dynamic";
            this.pictureDynamicMenuItem.ToolTipText = "In dynamic picture mode, the captured image will be stretched to fit the size of " +
    "the window";
            this.pictureDynamicMenuItem.Click += new System.EventHandler(this.pictureDynamicMenuItem_Click);
            // 
            // videoModeButton
            // 
            this.videoModeButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.staticToolStripMenuItem1,
            this.dynamicToolStripMenuItem1});
            this.videoModeButton.Name = "videoModeButton";
            this.videoModeButton.Size = new System.Drawing.Size(111, 22);
            this.videoModeButton.Text = "Video";
            // 
            // staticToolStripMenuItem1
            // 
            this.staticToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.videoStatic30FPSMenuItem,
            this.videoStatic60FPSMenuItem});
            this.staticToolStripMenuItem1.Name = "staticToolStripMenuItem1";
            this.staticToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.staticToolStripMenuItem1.Text = "Static";
            this.staticToolStripMenuItem1.ToolTipText = "In static video mode, the captured video will not resize with the form";
            // 
            // videoStatic30FPSMenuItem
            // 
            this.videoStatic30FPSMenuItem.Name = "videoStatic30FPSMenuItem";
            this.videoStatic30FPSMenuItem.Size = new System.Drawing.Size(105, 22);
            this.videoStatic30FPSMenuItem.Text = "30 fps";
            this.videoStatic30FPSMenuItem.Click += new System.EventHandler(this.videoStatic30FPSMenuItem_Click);
            // 
            // videoStatic60FPSMenuItem
            // 
            this.videoStatic60FPSMenuItem.Name = "videoStatic60FPSMenuItem";
            this.videoStatic60FPSMenuItem.Size = new System.Drawing.Size(105, 22);
            this.videoStatic60FPSMenuItem.Text = "60 fps";
            this.videoStatic60FPSMenuItem.Click += new System.EventHandler(this.videoStatic60FPSMenuItem_Click);
            // 
            // dynamicToolStripMenuItem1
            // 
            this.dynamicToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.videoDynamic30FPSMenuItem,
            this.videoDynamic60FPSMenuItem});
            this.dynamicToolStripMenuItem1.Name = "dynamicToolStripMenuItem1";
            this.dynamicToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.dynamicToolStripMenuItem1.Text = "Dynamic";
            this.dynamicToolStripMenuItem1.ToolTipText = "In dynamic video mode, the captured video will be stretched to fit the size of th" +
    "e window";
            // 
            // videoDynamic30FPSMenuItem
            // 
            this.videoDynamic30FPSMenuItem.Name = "videoDynamic30FPSMenuItem";
            this.videoDynamic30FPSMenuItem.Size = new System.Drawing.Size(105, 22);
            this.videoDynamic30FPSMenuItem.Text = "30 fps";
            this.videoDynamic30FPSMenuItem.Click += new System.EventHandler(this.videoDynamic30FPSMenuItem_Click);
            // 
            // videoDynamic60FPSMenuItem
            // 
            this.videoDynamic60FPSMenuItem.Name = "videoDynamic60FPSMenuItem";
            this.videoDynamic60FPSMenuItem.Size = new System.Drawing.Size(105, 22);
            this.videoDynamic60FPSMenuItem.Text = "60 fps";
            this.videoDynamic60FPSMenuItem.Click += new System.EventHandler(this.videoDynamic60FPSMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.newToolStripMenuItem,
            this.toolStripMenuItem2,
            this.beginCaptureMenuItem,
            this.endCaptureMenuItem,
            this.copyToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(552, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // beginCaptureMenuItem
            // 
            this.beginCaptureMenuItem.Name = "beginCaptureMenuItem";
            this.beginCaptureMenuItem.Size = new System.Drawing.Size(94, 20);
            this.beginCaptureMenuItem.Text = "Begin Capture";
            this.beginCaptureMenuItem.Visible = false;
            // 
            // endCaptureMenuItem
            // 
            this.endCaptureMenuItem.Name = "endCaptureMenuItem";
            this.endCaptureMenuItem.Size = new System.Drawing.Size(84, 20);
            this.endCaptureMenuItem.Text = "End Capture";
            this.endCaptureMenuItem.Visible = false;
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 17;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Location = new System.Drawing.Point(0, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(552, 426);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // highlightTimer
            // 
            this.highlightTimer.Interval = 32;
            this.highlightTimer.Tick += new System.EventHandler(this.highlightTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(552, 450);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(375, 72);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScreenCapture";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveAsButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem quitButton;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem pictureModeButton;
        private System.Windows.Forms.ToolStripMenuItem videoModeButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem pictureStaticMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pictureDynamicMenuItem;
        private System.Windows.Forms.ToolStripMenuItem staticToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem videoStatic30FPSMenuItem;
        private System.Windows.Forms.ToolStripMenuItem videoStatic60FPSMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dynamicToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem videoDynamic30FPSMenuItem;
        private System.Windows.Forms.ToolStripMenuItem videoDynamic60FPSMenuItem;
        private System.Windows.Forms.Timer highlightTimer;
        private System.Windows.Forms.ToolStripMenuItem beginCaptureMenuItem;
        private System.Windows.Forms.ToolStripMenuItem endCaptureMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    }
}

