using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;


namespace VISPJ
{

    class Hist_Calculater
    {
        List<String> files;
        public List<CvHistogram> hist_H;
        public List<CvHistogram> hist_S;
        public List<CvHistogram> hist_V;
        //public Hist_Calculater();

        public Hist_Calculater(String[] fs)
        {
            files = new List<String>();
            hist_H = new List<CvHistogram>();
            hist_S = new List<CvHistogram>();
            hist_V = new List<CvHistogram>();
            int sch = 0;
            int hist_size = 256;
            int[] hdims = { hist_size };
            float[] hranges = { 0, 256 };
            float[][] ranges = { hranges };
            
            
            

            for (int i = 0; i < fs.Length; i++)
            {

                IplImage[] dst_img = new IplImage[4];
                CvHistogram[] hist = new CvHistogram[4];
                
                files.Add(fs[i]);

                IplImage temp = new IplImage(fs[i]);
                IplImage src_img = Cv.CreateImage(temp.Size, BitDepth.U8, 3);
                Cv.CvtColor(temp, src_img, ColorConversion.BgrToHsv);
                sch = src_img.NChannels;

                for (int j = 0; j < sch; j++)
                    dst_img[j] = Cv.CreateImage(Cv.Size(src_img.Width, src_img.Height), src_img.Depth, 1);


                hist[0] = Cv.CreateHist(hdims, HistogramFormat.Array, ranges, true);
                //CvHistogram temph = Cv.CreateHist(hdims, HistogramFormat.Array, ranges, true);
                if(sch == 1)
                {
                    Cv.Copy(src_img,dst_img[0]);
                }
                else
                {
                    Cv.Split(src_img, dst_img[0], dst_img[1], dst_img[2], dst_img[3]);
                }
                for (int l = 0; l < sch; l++)
                {
                    Cv.CalcHist(dst_img[l], hist[l], false);
                    Cv.NormalizeHist(hist[l], 10000);
                    if (l < 3)
                    {
                       Cv.CopyHist(hist[l], ref hist[l + 1]);
                    }
                }

                
                hist_H.Add(hist[0]);
                hist_S.Add(hist[1]);
                hist_V.Add(hist[2]);



            }

 
        }



        

        public void compare_Hist(Bitmap image, List<double> out_approx)
        {
            IplImage[] dst_img = new IplImage[4];
            CvHistogram[] hist = new CvHistogram[4];
            CvHistogram hist2;
            double tmp, dist = 0;
            List<double> appH = new List<double>();
            List<double> appS = new List<double>();
            List<double> appV = new List<double>();
            List<double> outapp = new List<double>();


            int hist_size = 256;
            int[] hdims = { hist_size };
            float[] hranges = { 0, 256 };
            float[][] ranges = { hranges };

            IplImage src2 = (OpenCvSharp.IplImage)OpenCvSharp.Extensions.BitmapConverter.ToIplImage(image);

            IplImage src = Cv.CreateImage(src2.Size, BitDepth.U8, 3);
            Cv.CvtColor(src2, src, ColorConversion.BgrToHsv);
            int sch = src.NChannels;

            for (int j = 0; j < sch; j++)
                dst_img[j] = Cv.CreateImage(Cv.Size(src.Width, src.Height), src.Depth, 1);

            hist2 = Cv.CreateHist(hdims, HistogramFormat.Array, ranges, true);
            if (sch == 1)
            {
                Cv.Copy(src, dst_img[0]);
            }
            else
            {
                Cv.Split(src, dst_img[0], dst_img[1], dst_img[2], dst_img[3]);
            }

            hist[0] = Cv.CreateHist(hdims, HistogramFormat.Array, ranges, true);
            for (int l = 0; l < sch; l++)
            {
                Cv.CalcHist(dst_img[l], hist[l], false);
                Cv.NormalizeHist(hist[l], 10000);
                if (l < 3)
                {
                    Cv.CopyHist(hist[l], ref hist[l + 1]);
                }
            }
            foreach (CvHistogram n in hist_H)
            {
                tmp = Cv.CompareHist(n, hist[0], HistogramComparison.Bhattacharyya);
                dist = tmp * tmp;
                appH.Add(Math.Sqrt(dist));
            }
            foreach (CvHistogram n in hist_S)
            {
                tmp = Cv.CompareHist(n, hist[1], HistogramComparison.Bhattacharyya);
                dist = tmp * tmp;
                appS.Add(Math.Sqrt(dist));
            }
            foreach (CvHistogram n in hist_V)
            {
                tmp = Cv.CompareHist(n, hist[2], HistogramComparison.Bhattacharyya);
                dist = tmp * tmp;
                appV.Add(Math.Sqrt(dist));
            }
            Cv.ReleaseHist(hist2);
            Cv.ReleaseImage(src);
            int p = 0;
            foreach(double n in appH)
            {
                out_approx.Add((n + appS[p]) / 2);
                p++;
            }

        }


    }
}
