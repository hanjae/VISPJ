using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VISPJ
{
    public partial class OpenObject : Form
    {
        private Form1 callerForm = null;
        public OpenObject(Form1 theForm)
        {
            callerForm = theForm;
            alphaPen = new Pen(Color.Purple, 10);
            alphaPen.SetLineCap(LineCap.Round, LineCap.Round, DashCap.Flat);
            InitializeComponent();
        }
        public void setImage(Image theImage)
        {
            this.pictureBox1.Image = theImage;
            this.Size = new Size(theImage.Size.Width + 50, theImage.Size.Height + 100);
        }

        public Image getImage()
        {
            return this.pictureBox1.Image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (callerForm != null)
            {
                callerForm.setObjectPicture(this.pictureBox1.Image);
            }
            this.Close();
        }

        private Point? _Previous = null;
        private Pen alphaPen;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _Previous = e.Location;
            pictureBox1_MouseMove(sender, e);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_Previous != null)
            {
                if (pictureBox1.Image == null)
                {
                    Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.White);
                    }
                    pictureBox1.Image = bmp;
                }
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    g.DrawLine(alphaPen, _Previous.Value, e.Location);
                }
                pictureBox1.Invalidate();
                _Previous = e.Location;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _Previous = null;
        }
    }
}
