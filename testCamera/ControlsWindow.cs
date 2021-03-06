﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace testCamera
{
    class ControlsWindow : Form
    {
        Form1 parent;
        MenuStrip menu;
        ToolStripMenuItem cancel, options, picture, picture_static, picture_dynamic, video, video_static, video_dynamic,
            video_static30, video_static60, video_dynamic30, video_dynamic60;

        public ControlsWindow(int x, int y, Form1 parent)
        {
            initialize();
            this.Location = new Point(x, y);
            this.Width = 250;
            this.Height = 63;
            this.Visible = false;
            this.parent = parent;
            this.TopMost = true;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlsWindow));
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ControlsWindow";
            this.ResumeLayout(false);
        }

        public void move(int x, int y)
        {
            this.Location = new Point(x, y);
        }

        public void placeOnTop()
        {
            this.TopMost = true;
        }

        private void initialize()
        {
            //create menustrip
            menu = new MenuStrip();
            menu.BackColor = Color.FromName("GradientActiveCaption");
            //create cancel button
            cancel = new ToolStripMenuItem("Cancel");
            cancel.Click += cancelClick;
            //create capturemode/options button
            options = new ToolStripMenuItem("Capture Mode");
            //create first base option
            picture = new ToolStripMenuItem("Picture");
            //create and add the static picture option
            picture_static = new ToolStripMenuItem("Static");
            picture_static.Click += pictureStatic_Click;
            picture.DropDownItems.Add(picture_static);
            //create and add dynamic picture option
            picture_dynamic = new ToolStripMenuItem("Dynamic");
            picture_dynamic.Click += pictureDynamic_Click;
            picture.DropDownItems.Add(picture_dynamic);
            //add the first base option
            options.DropDownItems.Add(picture);
            //create the second base option
            video = new ToolStripMenuItem("Video");
            //create the static video option
            video_static = new ToolStripMenuItem("Static");
            //create and add the 30fps and 60fps video options
            video_static30 = new ToolStripMenuItem("30 fps");
            video_static30.Click += videoStatic30FPS_Click;
            video_static.DropDownItems.Add(video_static30);
            video_static60 = new ToolStripMenuItem("60 fps");
            video_static60.Click += videoStatic60FPS_Click;
            video_static.DropDownItems.Add(video_static60);

            video.DropDownItems.Add(video_static);

            video_dynamic = new ToolStripMenuItem("Dynamic");

            video_dynamic30 = new ToolStripMenuItem("30 fps");
            video_dynamic30.Click += videoDynamic30FPS_Click;
            video_dynamic.DropDownItems.Add(video_dynamic30);
            video_dynamic60 = new ToolStripMenuItem("60 fps");
            video_dynamic60.Click += videoDynamic60FPS_Click;
            video_dynamic.DropDownItems.Add(video_dynamic60);

            video.DropDownItems.Add(video_dynamic);
            //add video options
            options.DropDownItems.Add(video);
            //add all of the options, then add the menustrip
            menu.Items.Add(cancel);
            menu.Items.Add(options);
            this.Controls.Add(menu);

            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }

        private void cancelClick(object sender, EventArgs e)
        {
            //return the application to default state
            this.Hide();
            parent.Location = parent.pointOfReturn;
            parent.Size = parent.originalSize;
            parent.disableSaveAndCopy();
            parent.minimize();
        }

        private void pictureDynamic_Click(object sender, EventArgs e)
        {
            parent.changeCaptureMode(0);
        }

        private void pictureStatic_Click(object sender, EventArgs e)
        {
            parent.changeCaptureMode(1);
        }

        private void videoStatic30FPS_Click(object sender, EventArgs e)
        {
            parent.changeCaptureMode(2);
        }

        private void videoStatic60FPS_Click(object sender, EventArgs e)
        {
            parent.changeCaptureMode(3);
        }

        private void videoDynamic30FPS_Click(object sender, EventArgs e)
        {
            parent.changeCaptureMode(4);
        }

        private void videoDynamic60FPS_Click(object sender, EventArgs e)
        {
            parent.changeCaptureMode(5);
        }

        //had to override the formClosing event, it wasn't firing on its own
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            parent.Location = parent.pointOfReturn;
            parent.Size = parent.originalSize;
            parent.minimize();
            base.OnClosing(e);
        }
    }
}