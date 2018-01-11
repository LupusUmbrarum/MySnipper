using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            this.Visible = true;
            this.parent = parent;
        }

        private void initialize()
        {
            menu = new MenuStrip();
            menu.BackColor = Color.FromName("GradientActiveCaption");
            cancel = new ToolStripMenuItem("Cancel");
            options = new ToolStripMenuItem("Capture Mode");

            picture = new ToolStripMenuItem("Picture");

            picture_static = new ToolStripMenuItem("Static");
            picture.DropDownItems.Add(picture_static);
            
            picture_dynamic = new ToolStripMenuItem("Dynamic");
            picture.DropDownItems.Add(picture_dynamic);
            
            options.DropDownItems.Add(picture);

            video = new ToolStripMenuItem("Video");

            video_static = new ToolStripMenuItem("Static");

            video_static30 = new ToolStripMenuItem("30 fps");
            video_static.DropDownItems.Add(video_static30);
            video_static60 = new ToolStripMenuItem("60 fps");
            video_static.DropDownItems.Add(video_static60);

            video.DropDownItems.Add(video_static);

            video_dynamic = new ToolStripMenuItem("Dynamic");

            video_dynamic30 = new ToolStripMenuItem("30 fps");
            video_dynamic.DropDownItems.Add(video_dynamic30);
            video_dynamic60 = new ToolStripMenuItem("60 fps");
            video_dynamic.DropDownItems.Add(video_dynamic60);

            video.DropDownItems.Add(video_dynamic);

            options.DropDownItems.Add(video);

            menu.Items.Add(cancel);
            menu.Items.Add(options);
            this.Controls.Add(menu);
        }
    }
}
