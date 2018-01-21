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
        SaveFileDialog sfd;
        int offsetX, offsetY;
        Camera cam;
        public Size originalSize;
        Point startPoint, endPoint, deltaPoint;
        public Point pointOfReturn;
        bool useSelectedArea, leftClickDown, isCtrlDown, isCDown, isSDown;
        public bool isCapturing;
        Bitmap bm;
        Rectangle rect, selectionBox;
        CaptureMode currentMode;
        ControlsWindow cw;

        public Form1()
        {
            InitializeComponent();
            isCapturing = false;
            useSelectedArea = false;
            leftClickDown = false;
            isCtrlDown = false;
            isCDown = false;
            isSDown = false;
            //default mode
            currentMode = CaptureMode.PICTURE_STATIC;
            pictureBox1.Visible = false;
            originalSize = this.Size;
            cw = new ControlsWindow(this.Location.X, this.Location.Y, this);
            saveAsButton.Enabled = false;
            copyToolStripMenuItem.Visible = false;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if(isCapturing)
            {
                startPoint = MousePosition;
                deltaPoint = MousePosition;
                highlightTimer.Start();
                offsetX = 0;
                if(Screen.AllScreens.Count() > 1)
                {
                    foreach (Screen x in Screen.AllScreens)
                    {
                        /*if ((x.Bounds.Location.X < startPoint.X && (x.Bounds.Width + x.Bounds.Location.X) > startPoint.X))
                        {
                            offset = x.Bounds.Width;
                        }*/
                        if(x.Bounds.X < 0)
                        {
                            offsetX += x.Bounds.Width;
                        }
                    }
                }
            }
            leftClickDown = true;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if(isCapturing)
            {
                cw.move(Screen.PrimaryScreen.Bounds.Width * 10, Screen.PrimaryScreen.Bounds.Height * 10);
                cw.Hide();
                endPoint = MousePosition;
                isCapturing = false;
                useSelectedArea = true;
                rect = new Rectangle(new Point((startPoint.X > endPoint.X ? endPoint.X : startPoint.X), (startPoint.Y > endPoint.Y ? endPoint.Y : startPoint.Y)), new Size(Math.Abs(endPoint.X - startPoint.X), Math.Abs(endPoint.Y - startPoint.Y)));
                
                pictureBox1.Visible = true;
                pictureBox1.Size = new Size(rect.Width, rect.Height);

                this.Size = new Size((rect.Width < 375 ? 375 : rect.Width + rect.Width / 2), (rect.Height < 72 ? 144 : rect.Height + rect.Height / 2));

                if (currentMode == CaptureMode.VIDEO_STATIC || currentMode == CaptureMode.PICTURE_STATIC)
                {
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
                if(currentMode == CaptureMode.PICTURE_DYNAMIC || currentMode == CaptureMode.PICTURE_STATIC)
                {
                    enableSaveAndCopy();
                }
                copyToolStripMenuItem.Visible = true;
            }
            
            leftClickDown = false;
        }

        /// <summary>
        /// Maximize the form to cover all screens, including the taskbar, in order to take a picture
        /// </summary>
        private void maximizeForSelection()
        {
            //This sets up the form to be maximized. It removes the border, and makes the form overlap all things, even the taskbar
            //It also sets the opacity to 50%, so as to facilitate better image capturing
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Opacity = 0.5;
            pointOfReturn = this.Location;
            menuStrip1.Visible = false;
            cw.Show();

            //this assumes the screens are all aligned horizontally, and there are no screens located in different vertical positions
            Screen farthestLeftScreen = Screen.PrimaryScreen;
            if(Screen.AllScreens.Count() > 1)
            {
                foreach(Screen scr in Screen.AllScreens)
                {
                    this.Width += scr.Bounds.Width + 10;
                    if (scr.Bounds.Location.X < 0)
                    {
                        farthestLeftScreen = scr;
                    }
                }

                this.Location = farthestLeftScreen.Bounds.Location;
                this.Height = Screen.PrimaryScreen.Bounds.Height + 20;
            }
            else
            {
                if(this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                }
                this.WindowState = FormWindowState.Maximized;
                this.Location = new Point(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            }

            this.TopMost = true;
            
            useSelectedArea = true;

            cw.move(this.pointOfReturn.X, this.pointOfReturn.Y);

            cw.TopMost = true;
        }

        /// <summary>
        /// This puts the form back in its default state
        /// </summary>
        public void minimize()
        {
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Opacity = 1.0;
            menuStrip1.Visible = true;
            this.isCapturing = false;
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
                        //this is a bit of a hack. The form would be visible in pictures if you tried to capture the wrong area.
                        //It never happened for video capture. This would be a case of "don't touch it, it works"
                        this.Location = new Point(10000, 10000);

                        minimize();
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
            isCapturing = true;
            pictureBox1.Visible = false;
            maximizeForSelection();
        }

        private void saveAsButton_Click(object sender, EventArgs e)
        {
            initSaveFileDialog();
            
            //Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);//gets the folder path to pictures
            //bm.Save(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)+"\\test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void Sfd_FileOk(object sender, CancelEventArgs e)
        {
            saveImage();
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
            changeCaptureMode(0);
        }

        private void pictureStaticMenuItem_Click(object sender, EventArgs e)
        {
            changeCaptureMode(1);
        }

        private void videoStatic30FPSMenuItem_Click(object sender, EventArgs e)
        {
            changeCaptureMode(2);
        }

        private void videoStatic60FPSMenuItem_Click(object sender, EventArgs e)
        {
            changeCaptureMode(3);
        }

        private void videoDynamic30FPSMenuItem_Click(object sender, EventArgs e)
        {
            changeCaptureMode(4);
        }

        private void videoDynamic60FPSMenuItem_Click(object sender, EventArgs e)
        {
            changeCaptureMode(5);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.C:
                    isCDown = true;
                    break;
                case Keys.ControlKey:
                    isCtrlDown = true;
                    break;
                case Keys.S:
                    isSDown = true;
                    break;
            }

            if(isCDown && isCtrlDown)
            {
                copyImage();
            }

            if(isSDown && isCtrlDown)
            {
                if (bm != null)
                {
                    initSaveFileDialog();
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.C:
                    isCDown = false;
                    break;
                case Keys.ControlKey:
                    isCtrlDown = false;
                    break;
                case Keys.S:
                    isSDown = false;
                    break;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyImage();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (leftClickDown && isCapturing)
            {
                Rectangle ee = new Rectangle((Cursor.Position.X + offsetX < startPoint.X + offsetX ? Cursor.Position.X + offsetX : startPoint.X + offsetX), (Cursor.Position.Y > startPoint.Y ? startPoint.Y : Cursor.Position.Y), Math.Abs(Cursor.Position.X - startPoint.X), Math.Abs(Cursor.Position.Y - startPoint.Y));

                using (Pen pen = new Pen(Color.Red, 1))
                {
                    e.Graphics.DrawRectangle(pen, ee);
                }
            }
        }

        private void initSaveFileDialog()
        {
            if (sfd != null)
            {
                sfd.ShowDialog();
            }
            else
            {
                sfd = new SaveFileDialog();
                sfd.FileOk += Sfd_FileOk;
                sfd.Filter = "Portable Network Graphic file (PNG) | *.PNG | JPEG file | *.JPG";
                sfd.FilterIndex = 2;
                sfd.RestoreDirectory = true;
                sfd.ShowDialog();
            }
        }

        private void saveImage()
        {
            switch (sfd.FilterIndex)
            {
                case 1:
                    bm.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    break;
                case 2:
                    bm.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
            }
        }

        private void copyImage()
        {
            switch (currentMode)
            {
                case CaptureMode.PICTURE_STATIC:
                    try
                    {
                        Clipboard.SetImage((Image)bm);
                    }
                    catch (Exception ex) { }
                    break;
                case CaptureMode.PICTURE_DYNAMIC:
                    try
                    {
                        using (Bitmap tempBtm = new Bitmap(bm, pictureBox1.Size))
                        {
                            Clipboard.SetImage((Image)tempBtm);
                        }
                    }
                    catch (Exception ex) { }
                    break;
            }
        }

        public void changeCaptureMode(int x)
        {
            switch(x)
            {
                case 0:
                    currentMode = CaptureMode.PICTURE_DYNAMIC;
                    pictureBox1.Dock = DockStyle.Fill;
                    positionPictureBox(1);
                    try
                    {
                        timer1.Stop();
                    }
                    catch (Exception ex) { }
                    //hideCaptureControls();
                    break;
                case 1:
                    currentMode = CaptureMode.PICTURE_STATIC;
                    pictureBox1.Dock = DockStyle.None;
                    positionPictureBox(0);
                    try
                    {
                        timer1.Stop();
                    }
                    catch (Exception ex) { }
                    //hideCaptureControls();
                    break;
                case 2:
                    currentMode = CaptureMode.VIDEO_STATIC;
                    pictureBox1.Dock = DockStyle.None;
                    timer1.Interval = 32;
                    if (rect != null)
                    {
                        timer1.Start();
                    }
                    positionPictureBox(0);
                    //showCaptureControls();
                    disableSaveAndCopy();
                    break;
                case 3:
                    currentMode = CaptureMode.VIDEO_STATIC;
                    pictureBox1.Dock = DockStyle.None;
                    timer1.Interval = 16;
                    if (rect != null)
                    {
                        timer1.Start();
                    }
                    positionPictureBox(0);
                    //showCaptureControls();
                    disableSaveAndCopy();
                    break;
                case 4:
                    currentMode = CaptureMode.VIDEO_DYNAMIC;
                    pictureBox1.Dock = DockStyle.Fill;
                    timer1.Interval = 32;
                    if (rect != null)
                    {
                        timer1.Start();
                    }
                    positionPictureBox(1);
                    //showCaptureControls();
                    disableSaveAndCopy();
                    break;
                case 5:
                    currentMode = CaptureMode.VIDEO_DYNAMIC;
                    pictureBox1.Dock = DockStyle.Fill;
                    timer1.Interval = 16;
                    if (rect != null)
                    {
                        timer1.Start();
                    }
                    positionPictureBox(1);
                    //showCaptureControls();
                    disableSaveAndCopy();
                    break;
            }
        }

        private void hideCaptureControls()
        {
            beginCaptureMenuItem.Visible = false;
            endCaptureMenuItem.Visible = false;
        }

        private void showCaptureControls()
        {
            beginCaptureMenuItem.Visible = true;
            endCaptureMenuItem.Visible = true;
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

        public void disableSaveAndCopy()
        {
            saveAsButton.Enabled = false;
            copyToolStripMenuItem.Visible = false;
        }

        public void enableSaveAndCopy()
        {
            saveAsButton.Enabled = true;
            copyToolStripMenuItem.Visible = true;
        }
    }
}