using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using System.Drawing.Imaging;

namespace project1
{
    
    public partial class Form1 : Form
    {
         Bitmap originalImage, skin, k ;

            public Form1()
            {
                InitializeComponent();
            }

            public void openToolStripMenuItem_Click(object sender, EventArgs e)
            {
                OpenFileDialog openFile = new OpenFileDialog();
                var returnStatus = openFile.ShowDialog();
                if (returnStatus == DialogResult.OK)
                {
                    originalImage = new Bitmap(openFile.FileName);
                    pictureBox1.Image = originalImage;
                }
            }

        private void fillHoleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            GrayscaleBT709 greyscale = new GrayscaleBT709();
            Bitmap grey = greyscale.Apply(skin);
            Threshold filter = new Threshold(100);
             filter.ApplyInPlace(grey);
            Closing close = new Closing();
            Bitmap j = close.Apply(grey);
            Opening open = new Opening();
             k = open.Apply(j);
            pictureBox3.Image = k;

        }

        private void finalImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            And(originalImage, k);

        }
            public static Bitmap And(Bitmap Image1, Bitmap Image2)
             {
                     Bitmap NewBitmap = new Bitmap(Image1.Width, Image1.Height);
                     BitmapData NewData = Image.LockImage(NewBitmap);
                     BitmapData OldData1 = Image.LockImage(Image1);
                     BitmapData OldData2 = Image.LockImage(Image2);
                     int NewPixelSize = Image.GetPixelSize(NewData);
                     int OldPixelSize1 = Image.GetPixelSize(OldData1);
                     int OldPixelSize2 = Image.GetPixelSize(OldData2);
                     for (int x = 0; x < NewBitmap.Width; ++x)
                         {
                             for (int y = 0; y < NewBitmap.Height; ++y)
                                 {
                                     Color Pixel1 = Image.GetPixel(OldData1, x, y, OldPixelSize1);
                                     Color Pixel2 = Image.GetPixel(OldData2, x, y, OldPixelSize2);
                                     Image.SetPixel(NewData, x, y,
                                         Color.FromArgb(Pixel1.R & Pixel2.R,
                                             Pixel1.G & Pixel2.G,
                                             Pixel1.B & Pixel2.B),
                                         NewPixelSize);
                                 }
                         }
                    Image.UnlockImage(NewBitmap, NewData);
                     Image.UnlockImage(Image1, OldData1);
                     Image.UnlockImage(Image2, OldData2);
                     return NewBitmap;
                 }
        

        private void blobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtractBiggestBlob b = new ExtractBiggestBlob();
            Bitmap blob = b.Apply(k);
            pictureBox4.Image = blob;

        }

        private void skinColorToolStripMenuItem_Click(object sender, EventArgs e)
            {
            Color black = Color.FromArgb(Color.Black.ToArgb());
            Color white = Color.FromArgb(Color.White.ToArgb());
            for (int i = 0; i < originalImage.Width; i++)
            {
                for (int j = 0; j < originalImage.Height; j++)
                {
                    int r = originalImage.GetPixel(i, j).R;
                    int g = originalImage.GetPixel(i, j).G;
                    int b = originalImage.GetPixel(i, j).B;

                    if (((r > 95) && (g > 40) && (b > 20)) && (((Math.Max(r, (Math.Max(g, b)))) - (Math.Min(r, (Math.Min(g, b))))) > 15) && (r > g) && (r > b) && ((r - g) > 15))
                    {
                        originalImage.SetPixel(i, j, white);
                    }
                    else
                    {
                        originalImage.SetPixel(i, j, black);

                    }
                }
            }
            skin = originalImage;
            pictureBox2.Image = skin;

             

            }

           
    }
}
