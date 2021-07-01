using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Pictrue_Merge
{
    public partial class Form1 : Form
    {
        Image firstImage, secondImage, secondOpacity;
        public Form1()
        {
            InitializeComponent();
        }

        private void FirstPicBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                firstImage = new Bitmap(openFileDialog1.FileName);
                pictureBox2.Image = firstImage;
            }
            if (firstImage != null && secondImage != null)
            {
                MergeBtn.Visible = true;

            }
        }

        private void btnChooes_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                secondImage = new Bitmap(openFileDialog1.FileName);
                secondOpacity = new Bitmap(openFileDialog1.FileName);
                pictureBox3.Image = secondOpacity;
            }
            if(firstImage!=null&&secondImage!=null)
            {
                MergeBtn.Visible = true;
            }
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
           
        }

        private void MergeBtn_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
             MergePictureBox.Image = Merge((float)(0.7));
             MergePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
     
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            opacityvalue.Text = trackBar1.Value.ToString();
            MergePictureBox.Image = Merge((float)trackBar1.Value / 10);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter= "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf";
            if (saveFileDialog1.ShowDialog()==DialogResult.OK)
            {
                MergePictureBox.Image.Save(saveFileDialog1.FileName,ImageFormat.Png);
            }
        }

        private void chooseBtn_Click(object sender, EventArgs e)
        {
            if (panelChooes.Visible) panelChooes.Visible = false;
            else { panelChooes.Visible = true;  }
        }
        public Image Merge(float opacityvalue)
        {
            if (firstImage == null)
            {
                throw new ArgumentNullException("firstImage");
            }

            if (secondImage == null)
            {
                throw new ArgumentNullException("secondImage");
            }
            secondImage = secondOpacity;
            Bitmap bmp = new Bitmap(secondImage.Width, secondImage.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(secondImage, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, secondImage.Width, secondImage.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics
            int outputImageWidth = firstImage.Width > secondImage.Width ? firstImage.Width : secondImage.Width;

            int outputImageHeight = firstImage.Height + secondImage.Height + 1;

            Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            secondImage = bmp;
            graphics = Graphics.FromImage(outputImage);
            {
                graphics.DrawImage(firstImage, new Rectangle(new Point(), firstImage.Size),
                    new Rectangle(new Point(), firstImage.Size), GraphicsUnit.Pixel);
                graphics.DrawImage(secondImage, new Rectangle(new Point(0, 0), secondImage.Size),
                    new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
            }

            return outputImage;
        }
    }
}
