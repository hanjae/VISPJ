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


    public partial class result : Form
    {
        public const String dirpath = @"D:\image";
        private String[] files;
        Image img;
        List<double> approx_result;

        public result()
        {
            InitializeComponent();
            files = System.IO.Directory.GetFiles(dirpath, "*.jpg");
            approx_result = new List<double>();

        }
        public result(Image image)
        {
            InitializeComponent();
            this.img = image;
            files = System.IO.Directory.GetFiles(dirpath, "*.jpg");
            approx_result = new List<double>();

           //startSearch(image);
            
        }

        public void Thumbnail_List()
        {
            int width = 100;
            int height = 80;
            listView1.Clear();
            listView1.View = View.LargeIcon;

            if (approx_result.Count == 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    Image original = Bitmap.FromFile(files[i]);
                    Image thumbnail = createThumbnail(original, width, height);

                    this.imageList1.Images.Add(thumbnail);

                    this.listView1.Items.Add(files[i], i);

                    original.Dispose();
                    thumbnail.Dispose();
                }
            }
            else
            {
                for (int i = 0; i < files.Length; i++)
                {
                    Image original = Bitmap.FromFile(files[i]);
                    Image thumbnail = createThumbnail(original, width, height);

                    this.imageList1.Images.Add(thumbnail);
                    double temp = Math.Round((1 - approx_result[i]) * 100, 4, MidpointRounding.AwayFromZero);
                    this.listView1.Items.Add(temp.ToString() + "%", i);

                    original.Dispose();
                    thumbnail.Dispose();
                }
            }
            listView1.LargeImageList = imageList1;

        }
        public Image createThumbnail(Image image, int w, int h)
        {

            Bitmap canvas = new Bitmap(w, h);

            Graphics g = Graphics.FromImage(canvas);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);

            float fw = (float)w / (float)image.Width;
            float fh = (float)h / (float)image.Height;

            float scale = Math.Min(fw, fh);
            fw = image.Width * scale;
            fh = image.Height * scale;

            g.DrawImage(image, (w - fw) / 2, (h - fh) / 2, fw, fh);
            g.Dispose();

            return canvas;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void result_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {


            Hist_Calculater histlist = new Hist_Calculater(files);

             Bitmap input = new Bitmap(this.img);
     
            histlist.compare_Hist(input, approx_result);
            Thumbnail_List();

        }
     

    }
}
