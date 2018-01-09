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
        PICTURE,
        VIDEO
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
            currentMode = CaptureMode.PICTURE;
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
                this.Size = new Size(rect.Width + rect.Width/2, rect.Height + rect.Height/2);
                pictureBox1.Location = new Point(this.ClientSize.Width/2 - rect.Width/2, 12 + this.ClientSize.Height/2 - rect.Height/2);                
                setBackground();
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

        private void setBackground()
        {
            switch(currentMode)
            {
                case CaptureMode.VIDEO:
                    try
                    {
                        bm = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            g.CopyFromScreen(rect.X, rect.Y, 0, 0, bm.Size);
                            this.BackgroundImage = bm;
                            pictureBox1.Image = bm;
                        }
                    }
                    catch (Exception ex) { }
                    break;
                case CaptureMode.PICTURE:
                    try
                    {
                        bm = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                        using (Graphics g = Graphics.FromImage(bm))
                        {
                            g.CopyFromScreen(rect.X, rect.Y, 0, 0, bm.Size);
                            //this.BackgroundImage = bm;
                            pictureBox1.Image = bm;
                        }
                    }
                    catch (Exception ex) { }
                    break;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            
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

        private void pictureModeButton_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.PICTURE;
            timer1.Stop();
        }

        private void videoModeButton_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.VIDEO;
            timer1.Start();
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
            setBackground();
        }

        private void fpsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                timer1.Stop();
            }
            catch (Exception ex) { }
            timer1.Interval = 17;
            timer1.Start();
        }

        private void fpsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                timer1.Stop();
            }
            catch (Exception ex) { }
            timer1.Interval = 33;
            timer1.Start();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if(leftClickDown && isCapturing)
            {

            }
                
        }
    }
}
