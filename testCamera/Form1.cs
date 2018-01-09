using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;

namespace testCamera
{
    enum CaptureMode
    {
        PICTURE_STATIC, //this CaptureMode captures the image, and leaves it alone
        PICTURE_DYNAMIC, //this CaptureMode captures the image, but makes the image fill the form; stretching to fit the form as the form changes
        VIDEO_STATIC, //video version of PICTURE_STATIC
        VIDEO_DYNAMIC //video version of PICTURE_DYNAMIC
    }

    public partial class Form1 : Form
    {
        Camera cam;
        Point startPoint, endPoint;
        bool isCapturing, useSelectedArea, leftClickDown;
        Bitmap bm;
        Rectangle rect, selectionBox;
        CaptureMode currentMode;

        public Form1()
        {
            InitializeComponent();
            cam = new Camera(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            isCapturing = false;
            useSelectedArea = false;
            leftClickDown = false;
            //default mode
            currentMode = CaptureMode.PICTURE_STATIC;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if(isCapturing)
            {
                startPoint = MousePosition;
            }
            leftClickDown = true;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if(isCapturing)
            {
                endPoint = MousePosition;
                isCapturing = false;
                useSelectedArea = true;
                rect = new Rectangle(new Point((startPoint.X > endPoint.X ? endPoint.X : startPoint.X), (startPoint.Y > endPoint.Y ? endPoint.Y : startPoint.Y)), new Size(Math.Abs(endPoint.X - startPoint.X), Math.Abs(endPoint.Y - startPoint.Y)));
                minimize();
                pictureBox1.Visible = true;
                pictureBox1.Size = new Size(rect.Width, rect.Height);
                if(currentMode == CaptureMode.VIDEO_STATIC || currentMode == CaptureMode.PICTURE_STATIC)
                {
                    this.Size = new Size(rect.Width + rect.Width / 2, rect.Height + rect.Height / 2);
                    positionPictureBox(0);
                }
                else
                {
                    positionPictureBox(1);
                }

                if(currentMode == CaptureMode.VIDEO_DYNAMIC || currentMode == CaptureMode.VIDEO_STATIC)
                {
                    timer1.Start();
                }
                captureImage();
                this.Location = new Point(rect.X, rect.Y);
                //minimize();
            }
            leftClickDown = false;
        }

        private void maximizeForSelection()
        {
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Opacity = 0.5;
            this.WindowState = FormWindowState.Maximized;
            useSelectedArea = true;
        }

        private void minimize()
        {
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Opacity = 1.0;
        }

        private void captureImage()
        {
            switch(currentMode)
            {
                case CaptureMode.VIDEO_STATIC:
                case CaptureMode.VIDEO_DYNAMIC:
                    try
                    {
                        bm = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            g.CopyFromScreen(rect.X, rect.Y, 0, 0, bm.Size);
                            pictureBox1.Image = bm;
                        }
                    }
                    catch (Exception ex) { }
                    break;
                case CaptureMode.PICTURE_STATIC:
                case CaptureMode.PICTURE_DYNAMIC:
                    try
                    {
                        bm = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            g.CopyFromScreen(rect.X, rect.Y, 0, 0, bm.Size);
                            pictureBox1.Image = bm;
                        }
                    }
                    catch (Exception ex) { }
                    break;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if(currentMode == CaptureMode.PICTURE_STATIC || currentMode == CaptureMode.VIDEO_STATIC)
            {
                positionPictureBox(0);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                timer1.Stop();
            }
            catch (Exception ex) { }
            this.BackgroundImage = null;
            maximizeForSelection();
            isCapturing = true;
            pictureBox1.Visible = false;
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.WindowState != FormWindowState.Normal)
                this.WindowState = FormWindowState.Normal;
            if(this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.Sizable)
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            if (this.Opacity < 1.0)
                this.Opacity = 1.0;
            if (!pictureBox1.Visible)
                pictureBox1.Visible = true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {

        }

        private void saveAsButton_Click(object sender, EventArgs e)
        {

        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            captureImage();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if(isCapturing && leftClickDown)
            {
                this.Refresh();
            }
        }

        private void pictureDynamicMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.PICTURE_DYNAMIC;
            pictureBox1.Dock = DockStyle.Fill;
            positionPictureBox(1);
        }

        private void pictureStaticMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.PICTURE_STATIC;
            pictureBox1.Dock = DockStyle.None;
            positionPictureBox(0);
        }

        private void videoStatic30FPSMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.VIDEO_STATIC;
            pictureBox1.Dock = DockStyle.None;
            timer1.Interval = 17;
            positionPictureBox(0);
        }

        private void videoStatic60FPSMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.VIDEO_STATIC;
            pictureBox1.Dock = DockStyle.None;
            timer1.Interval = 33;
            positionPictureBox(0);
        }

        private void videoDynamic30FPSMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.VIDEO_DYNAMIC;
            pictureBox1.Dock = DockStyle.Fill;
            timer1.Interval = 17;
            positionPictureBox(1);
        }

        private void videoDynamic60FPSMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.VIDEO_DYNAMIC;
            pictureBox1.Dock = DockStyle.Fill;
            timer1.Interval = 33;
            positionPictureBox(1);
        }

        private void positionPictureBox(int positionChoice)
        {
            switch(positionChoice)
            {
                // default location. This is used for static picture or video options
                case 0:
                    pictureBox1.Location = new Point(this.ClientSize.Width / 2 - rect.Width / 2, 12 + this.ClientSize.Height / 2 - rect.Height / 2);
                    break;
                // location when using dynamic picture or video options
                case 1:
                    pictureBox1.Location = new Point(0, 24);
                    break;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (leftClickDown && isCapturing)
            {
                Rectangle ee = new Rectangle(startPoint.X, startPoint.Y, Cursor.Position.X - startPoint.X, Cursor.Position.Y - startPoint.Y);
                using (Pen pen = new Pen(Color.Red, 1))
                {
                    e.Graphics.DrawRectangle(pen, ee);
                }
            }
        }
    }
}
