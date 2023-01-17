using System.Runtime.InteropServices;
using Color = System.Drawing.Color;
using Size = System.Drawing.Size;

namespace JA_projekt
{
    public partial class Form1 : Form
    {
        Bitmap original;
        Bitmap newImage = null;


        static String cResultsTitle = String.Format("C# results in ticks:{0}", Environment.NewLine);
        static String asmResultsTitle = String.Format("ASM results in ticks:{0}", Environment.NewLine);

        String cResults = cResultsTitle;
        String asmResults = asmResultsTitle;

        int cIterator = 0;
        int amsIterator = 0;


        public Form1()
        {
            InitializeComponent();
            textBoxASM.Text = asmResults;
            textBoxC.Text = cResults;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void scrool_changed(object sender, EventArgs e)
        {
            int value = trackBarThredNumber.Value;
            textThredNumber.Text = value.ToString();
        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer3_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            DialogResult dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                //geting file from dialog
                String fileName = openFileDialog.FileName;
                //loading file to bitmap
                original = new Bitmap(fileName);


                //new resized bitmap to display in small window
                Bitmap bitmapToDisplay = new Bitmap(original, new System.Drawing.Size(pictureBoxLeft.Width, pictureBoxLeft.Height));
                pictureBoxLeft.Image = bitmapToDisplay;

            }
        }




        
        [DllImport(@"D:\JA_projekt\JA_projekt\x64\Debug\ASM.dll")]
        static extern void rgbEdit(float[,] rgb, float[,]result );
        private void buttonConvert_Click(object sender, EventArgs e)
        {
            

                //Gdy nie ma obrazu
                if (original == null)
                {
                    MessageBox.Show("Choose file to convert first!!!");
                    return;
                }

                newImage = new Bitmap(original);

                var threadNumber = Int32.Parse(textThredNumber.Text.Trim());
                var message = "";

                List<(int x, int y)> pixelCoordinates = new List<(int x, int y)>();
                for (int y = 0; y < original.Height; y++)
                {
                    for (int x = 0; x < original.Width; x++)
                    {
                        pixelCoordinates.Add((x, y));
                    }
                }

                var watch = System.Diagnostics.Stopwatch.StartNew();
                LockBitmap lockBitmap = new LockBitmap(newImage);
                lockBitmap.LockBits();

                Parallel.ForEach(pixelCoordinates, new ParallelOptions() { MaxDegreeOfParallelism = threadNumber }, (coordinates) =>
                {
                    // coordinates is a tuple containing the x and y coordinates of a pixel
                    int x = coordinates.x;
                    int y = coordinates.y;

                    // Process the pixel at (x, y)
                    Color originalPixel = lockBitmap.GetPixel(x, y);



                    float[,] rgb = new[,]
                    {
                            {(float)originalPixel.R },
                            {(float)originalPixel.G },
                            {(float)originalPixel.B }
                    };

                    if (radioC.Checked)
                    {
                        // C# dll
                        rgb =
                        MatrixMultiplication.LMStoRGB(
                            MatrixMultiplication.LMStoProtanopia(
                                MatrixMultiplication.RGBtoLMS(
                                    rgb)));
                    }
                    else
                    {
                        //ASM dll TODO
                        float[,] result = new float[3, 1]; 

                        rgbEdit(rgb, result);

                        rgb = result;
                    }

                    int r = Math.Abs((int)rgb[0, 0]);
                    int g = Math.Abs((int)rgb[1, 0]);
                    int b = Math.Abs((int)rgb[2, 0]);


                    // nowa macierz rgb -> pixel
                    Color pixel = Color.FromArgb(r,g,b );

                    lockBitmap.SetPixel(x, y, pixel);

                });

                lockBitmap.UnlockBits();
                watch.Stop();
                var time = watch.ElapsedTicks;



                if (radioC.Checked)
                {
                    cIterator++;
                    message += String.Format("{0}. ", cIterator);
                }
                else if (radioAsm.Checked)
                {
                    amsIterator++;
                    message += String.Format("{0}. ", amsIterator);
                }

                message += String.Format("{0} ticks ", time);
                if (threadNumber == 1)
                {
                    message += String.Format("using {0} thread", threadNumber);
                }
                else
                {
                    message += String.Format("using {0} threads", threadNumber);
                }

                message += Environment.NewLine;

                if (radioC.Checked)
                {
                    cResults += message;
                    textBoxC.Text = cResults;
                }
                else
                {
                    asmResults += message;
                    textBoxASM.Text = asmResults;
                }

                Bitmap bitmapToDisplay = new Bitmap(newImage, new Size(pictureBox2.Width, pictureBox2.Height));
                pictureBox2.Image = bitmapToDisplay;

            
        }

        private void textChanged_treadNumber(object sender, EventArgs e)
        {
            bool error = true;

            for (int i = 1; i <= 64; i++)
            {
                if (textThredNumber.Text.Equals(i.ToString()))
                {
                    error = false;
                    break;
                }
            }

            if (error)
            {

            }

        }

        private void textBox_ThreadNumber_keyPress(object sender, KeyPressEventArgs e)
        {
            String text = textThredNumber.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want clear results?", "Really?", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                cResults = cResultsTitle;
                textBoxC.Text = cResults;

                asmResults = asmResultsTitle;
                textBoxASM.Text = asmResults;

                cIterator = 0;
                amsIterator = 0;
            }

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (newImage == null)
            {
                MessageBox.Show("First convert some picture !!!");
                return;
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                newImage.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                fs.Close();
            }
        }
    }
}