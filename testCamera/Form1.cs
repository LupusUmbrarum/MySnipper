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
        private SaveFileDialog sfd;
        private int offsetX, offsetY;
        /// <summary>used when cancelling screen capture</summary>
        public Size originalSize;
        //startPoint and endPoint are used for determining the bitmap's capture region
        //deltaPoint is used to determine when to refresh the form in order to paint the selection box
        private Point startPoint, endPoint, deltaPoint, hidePoint;
        public Point pointOfReturn;
        private bool leftClickDown, isCtrlDown, isCDown, isSDown, isCapturing, imageIsSaved;
        private Bitmap bm;
        private Rectangle rect;
        CaptureMode currentMode;
        private ControlsWindow cw;

        public Form1()
        {
            InitializeComponent();

            pictureBox1.Visible = false;
            saveAsButton.Enabled = false;
            copyToolStripMenuItem.Visible = false;

            //default mode
            currentMode = CaptureMode.PICTURE_STATIC;
            originalSize = this.Size;

            //initialize the ControlsWindow for later use
            cw = new ControlsWindow(this.Location.X, this.Location.Y, this);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if(isCapturing)
            {
                startPoint = MousePosition;
                deltaPoint = MousePosition;

                //begin painting highlight box for easier selection
                highlightTimer.Start();

                offsetX = 0;
                offsetY = 0;

                //if there's more than one screen, loop through all screens to calculate possible offsets
                if(Screen.AllScreens.Count() > 1)
                {
                    foreach (Screen x in Screen.AllScreens)
                    {
                        //calculate the x offset. If there are no screens to the left of the primary screen, this will return 0
                        if(x.Bounds.X < 0)
                        {
                            offsetX += x.Bounds.Width;
                        }

                        //calculate the y offset. If there are no screens above the primary screen, this will return 0
                        if(x.Bounds.Y < 0)
                        {
                            offsetY += x.Bounds.Height;
                        }
                    }
                }
            }

            //set this to true so the highlight box shows up during selection
            leftClickDown = true;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if(isCapturing)
            {
                //hide the controlswindow so it doesn't appear in the captured image
                cw.Hide();
                endPoint = MousePosition;
                isCapturing = false;
                imageIsSaved = false;

                //set the bitmap's capture region
                rect = new Rectangle(new Point((startPoint.X > endPoint.X ? endPoint.X : startPoint.X), (startPoint.Y > endPoint.Y ? endPoint.Y : startPoint.Y)), new Size(Math.Abs(endPoint.X - startPoint.X), Math.Abs(endPoint.Y - startPoint.Y)));
                
                pictureBox1.Visible = true;

                //make the picturebox size equal the captured image's size
                pictureBox1.Size = new Size(rect.Width, rect.Height);

                //set the size of this form. The minimum size of the form is set so that the defalt controls do not disappear
                this.Size = new Size(rect.Width + rect.Width / 2, rect.Height + rect.Height / 2);

                //reposition the pictureBox based on the current capturemode
                if (currentMode == CaptureMode.VIDEO_STATIC || currentMode == CaptureMode.PICTURE_STATIC)
                {
                    positionPictureBox(0);
                }
                else
                {
                    positionPictureBox(1);
                }
                
                //if the capturemode is a video capturemode, start the timer to continually capture images
                if(currentMode == CaptureMode.VIDEO_DYNAMIC || currentMode == CaptureMode.VIDEO_STATIC)
                {
                    timer1.Start();
                }

                //capture the image regardless of capturemode
                captureImage();

                //stop filling the whole screen
                minimize();

                //set the main form's location to the same place as the captured image
                this.Location = new Point(rect.X, rect.Y);
                this.TopMost = false;

                //stop trying to highlight the selected area
                highlightTimer.Stop();

                //if the current capturemode is a picture mode, enable the ability to save the image
                if(currentMode == CaptureMode.PICTURE_DYNAMIC || currentMode == CaptureMode.PICTURE_STATIC)
                {
                    enableSaveAndCopy();
                }

                //show the copy button on the toolstrip now that there's something to copy
                copyToolStripMenuItem.Visible = true;
            }
            
            leftClickDown = false;
        }

        //when the size of the form is changed, make sure the picturebox stays in the same general area
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
        }

        //the event that occurs when the SaveFileDialog OK button is pressed
        private void Sfd_FileOk(object sender, CancelEventArgs e)
        {
            saveImage();
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //this timer repeatedly captures the image at the set coordinates (should only be active during video capturemodes)
        private void timer1_Tick(object sender, EventArgs e)
        {
            captureImage();
        }

        //this timer keeps determines whether or not to redraw the highlighting rectangle during image capture
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

            //if the user is pressing Ctrl + C, try to copy to clipboard
            if(isCDown && isCtrlDown)
            {
                copyImage();
            }

            //if the user is pressing Ctrl + S, try to open the SaveFileDialog
            if(isSDown && isCtrlDown)
            {
                //if there is an image to save, and the current capturemode is not a video mode, start saving
                if (bm != null && currentMode != CaptureMode.VIDEO_DYNAMIC && currentMode != CaptureMode.VIDEO_STATIC)
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

        //attempt to copy to clipboard
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyImage();
        }

        /// <summary>
        /// This has been repurposed to draw a polygon around the area that the user is trying to capture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (leftClickDown && isCapturing)
            {
                //sorry to anyone who reads this, it's lengthy. Basically, this is determining how to draw a rectangle
                //that will encompass the whole area that the user is trying to capture. This makes it easier to 
                //know what you're snipping. It draws directly onto the form.
                Rectangle ee = new Rectangle((Cursor.Position.X + offsetX < startPoint.X + offsetX ? Cursor.Position.X + offsetX : startPoint.X + offsetX), 
                    (Cursor.Position.Y + offsetY > startPoint.Y + offsetY ? startPoint.Y + offsetY : Cursor.Position.Y + offsetY), 
                    Math.Abs(Cursor.Position.X - startPoint.X), Math.Abs(Cursor.Position.Y - startPoint.Y));

                using (Pen pen = new Pen(Color.Red, 1))
                {
                    e.Graphics.DrawRectangle(pen, ee);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            verifyExit(e);
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
            hidePoint = new Point(0, 0);
            //this checks to see if that assumption is correct
            if (Screen.AllScreens.Count() > 1)
            {
                foreach (Screen scr in Screen.AllScreens)
                {
                    //add width of the screen + 10 to ensure total width coverage
                    this.Width += scr.Bounds.Width + 10;
                    //add height of the screen + 10 to ensure total coverage
                    this.Height += scr.Bounds.Height + 10;
                    //find the left-most screen 
                    if (scr.Bounds.Location.X < farthestLeftScreen.Bounds.Location.X)
                    {
                        hidePoint.X = scr.Bounds.Location.X;
                    }
                    //find the top-most screen
                    if(scr.Bounds.Location.Y < hidePoint.Y)
                    {
                        hidePoint.Y = scr.Bounds.Location.Y;
                    }
                }
                
                this.Location = hidePoint;
            }
            else
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                }
                this.WindowState = FormWindowState.Maximized;
                this.Location = new Point(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            }

            this.TopMost = true;

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
            bm = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            try
            {
                //this is a bit of a hack. The form would be visible in pictures if you tried to capture the wrong area.
                //It never happened for video capture. This would be a case of "don't touch it, it works"
                
                if(currentMode == CaptureMode.PICTURE_DYNAMIC || currentMode == CaptureMode.PICTURE_STATIC)
                {
                    int width = 0, height = 0;
                    for (int x = 0; x < Screen.AllScreens.Count(); x++)
                    {
                        width += Screen.AllScreens[x].Bounds.Width;
                        height += Screen.AllScreens[x].Bounds.Height;
                    }
                    width *= 2;
                    height *= 2;
                    this.Location = new Point(width + 10, height + 10);
                }

                minimize();
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.CopyFromScreen(rect.X, rect.Y, 0, 0, bm.Size);
                    pictureBox1.Image = bm;
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Make sure it's safe to exit the program
        /// </summary>
        /// <param name="e"></param>
        private void verifyExit(FormClosingEventArgs e)
        {
            if (bm != null && currentMode != CaptureMode.VIDEO_STATIC && currentMode != CaptureMode.VIDEO_DYNAMIC && !imageIsSaved)
            {
                switch (MessageBox.Show("Do you want to save the image?", "Wait, don't go!", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        initSaveFileDialog();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Create a SaveFileDialog. If one already exists, show it.
        /// </summary>
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

        //save the image from the bitmap
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
            imageIsSaved = true;
        }

        private void copyImage()
        {
            //if the picture is static, save the image as-is. Else, save it as the stretched-out version
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
        //this is a simple options switch. It changes the little details when the capturemode is changed
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
                    disableSaveAndCopy();
                    break;
            }
        }

        //hide capture controls if 
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

        //this is used to disable the save option (primarily used to disable saving when in a video capturemode)
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