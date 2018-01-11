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
        int offset;
        Camera cam;
        Point startPoint, endPoint, deltaPoint;
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
                deltaPoint = MousePosition;
                highlightTimer.Start();
                offset = 0;
                foreach (Screen x in Screen.AllScreens)
                {
                    if ((x.Bounds.Location.X < startPoint.X && (x.Bounds.Width + x.Bounds.Location.X) > startPoint.X))
                    {
                        offset = x.Bounds.Width;
                    }
                }
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
                
                pictureBox1.Visible = true;
                pictureBox1.Size = new Size(rect.Width, rect.Height);

                if(currentMode == CaptureMode.VIDEO_STATIC || currentMode == CaptureMode.PICTURE_STATIC)
                {
                    this.Size = new Size((rect.Width < 275 ? 275 : rect.Width + rect.Width / 2), (rect.Height < 72 ? 144 : rect.Height + rect.Height / 2));
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
                minimize();

                this.Location = new Point(rect.X, rect.Y);
                this.TopMost = false;
                highlightTimer.Stop();
            }
            
            leftClickDown = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isCapturing && leftClickDown)
            {
                
            }
        }

        /// <summary>
        /// Maximize the form to cover all screens, including the taskbar, in order to take a picture
        /// </summary>
        private void maximizeForSelection()
        {
            //This sets up the form to be maximized. It removes the border, and makes the form overlap all things, even the taskbar
            //It also sets the opacity to 50%, so as to facilitate better image capturing
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.TopMost = true;
            //this.Opacity = 0.5;

            //this assumes the screens are all aligned horizontally, and there are no screens located in different vertical positions
            Screen farthestLeftScreen = Screen.PrimaryScreen;
            if(Screen.AllScreens.Count() > 1)
            {
                for (int x = 0; x < Screen.AllScreens.Count(); x++)
                {
                    this.Width += Screen.AllScreens[x].Bounds.Width + 10;//tempWidth += Screen.AllScreens[x].Bounds.Width;
                    if (Screen.AllScreens[x].Bounds.Location.X < 0)
                    {
                        farthestLeftScreen = Screen.AllScreens[x];
                    }
                }
                this.Location = farthestLeftScreen.Bounds.Location;
                this.Height = Screen.PrimaryScreen.Bounds.Height + 20;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.Location = new Point(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            }
            
            useSelectedArea = true;
        }

        /// <summary>
        /// This puts the form back in its default state
        /// </summary>
        private void minimize()
        {
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Opacity = 1.0;
        }

        /// <summary>
        /// Capture image based on highlighted area. Should only be called after an area has been selected
        /// </summary>
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
                        //this is a bit of a hack. The form would be visible in pictures, but not videos if you tried to capture the wrong area.
                        //It never happened for video capture. This would be a case of "don't touch it, it works"
                        this.Location = new Point(10000, 10000);

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
            bm = null;
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
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);//gets the folder path to pictures
            bm.Save(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)+"\\test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
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

        private void highlightTimer_Tick(object sender, EventArgs e)
        {
            if(Math.Abs(deltaPoint.X - Cursor.Position.X) > 1 || Math.Abs(deltaPoint.Y - Cursor.Position.Y) > 1)
            {
                this.Refresh();
                deltaPoint = Cursor.Position;
            }
        }

        private void pictureDynamicMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.PICTURE_DYNAMIC;
            pictureBox1.Dock = DockStyle.Fill;
            positionPictureBox(1);
            try
            {
                timer1.Stop();
            }
            catch (Exception ex) { }
        }

        private void pictureStaticMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.PICTURE_STATIC;
            pictureBox1.Dock = DockStyle.None;
            positionPictureBox(0);
            try
            {
                timer1.Stop();
            }
            catch (Exception ex) { }
        }

        private void videoStatic30FPSMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.VIDEO_STATIC;
            pictureBox1.Dock = DockStyle.None;
            timer1.Interval = 32;
            if(rect != null)
            {
                timer1.Start();
            }
            positionPictureBox(0);
        }

        private void videoStatic60FPSMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.VIDEO_STATIC;
            pictureBox1.Dock = DockStyle.None;
            timer1.Interval = 16;
            if (rect != null)
            {
                timer1.Start();
            }
            positionPictureBox(0);
        }

        private void videoDynamic30FPSMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.VIDEO_DYNAMIC;
            pictureBox1.Dock = DockStyle.Fill;
            timer1.Interval = 32;
            if (rect != null)
            {
                timer1.Start();
            }
            positionPictureBox(1);
        }

        private void videoDynamic60FPSMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = CaptureMode.VIDEO_DYNAMIC;
            pictureBox1.Dock = DockStyle.Fill;
            timer1.Interval = 16;
            if (rect != null)
            {
                timer1.Start();
            }
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

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (leftClickDown && isCapturing)
            {
                testToolStripMenuItem.Text = Cursor.Position.ToString();
                test2ToolStripMenuItem.Text = startPoint.ToString();
                cancelToolStripMenuItem.Text = offset.ToString();
                
                Rectangle ee = new Rectangle((/*Cursor.Position.X < startPoint.X + offset ? */startPoint.X + offset/* : Cursor.Position.X*/), (Cursor.Position.Y > startPoint.Y ? startPoint.Y : Cursor.Position.Y), Math.Abs(Cursor.Position.X - startPoint.X), Math.Abs(Cursor.Position.Y - startPoint.Y));

                using (Pen pen = new Pen(Color.Red, 1))
                {
                    e.Graphics.DrawRectangle(pen, ee);
                }
            }
        }

    }
}
