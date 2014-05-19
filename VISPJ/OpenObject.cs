using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VISPJ
{
    public partial class OpenObject : Form
    {
        public OpenObject()
        {
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
            this.Close();
        }
    }
}
