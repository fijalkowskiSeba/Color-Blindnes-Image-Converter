using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace JA_projekt
{
    public partial class Form1 : Form
    {
        Bitmap original;
        Bitmap newBitmap;

        
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

        static double[,] multiplyMatrixes(double[,] matrixA, double[,] matrixB)
        {
            var rowsA = matrixA.GetLength(0);
            var colsA = matrixA.GetLength(1);
            var rowsB = matrixB.GetLength(0);
            var colsB = matrixB.GetLength(1);

            if (colsA != rowsB)
            {
                return new[,] { { -1.0 }, { -1.0 }, { -1.0 } };
            }

            double[,] result = new double[rowsA, colsB];

            var sum = 0.0;
            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    sum = 0;
                    for (int k = 0; k < colsA; k++)
                    {
                        sum += matrixA[i, k] * matrixB[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return result;
        }

        static double[,] RGBtoLMS(double[,] rgbMatrix) 
        {
            double[,] lms = new[,]
            {
            { 17.8824,  43.5161,    4.1194 },
            { 3.4557,   27.1554,    3.8671 },
            { 0.0300,   0.1843,     1.4671 }
            };
            
            return multiplyMatrixes(lms, rgbMatrix);

        }

        static double[,] LMStoRGB(double[,] lmsMatrix)
        {
            double[,] rgb = new[,]
            {
            { 0.0809,   -0.1305,    0.1167 },
            { -0.0102,  0.0540,     -0.1136 },
            { -0.0004,  -0.0041,    0.6935 }
            };

            return multiplyMatrixes(rgb, lmsMatrix);

        }

        static double[,] LMStoProtanopia(double[,] lmsMatrix)
        {
            double[,] protanopia = new[,]
            {
            { 0.0,    2.0234,   -2.5258 },
            { 0.0,    1.0,      0.0 },
            { 0.0,    0.0,      1.0 }
            };

            return multiplyMatrixes(protanopia, lmsMatrix);

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
            textThredNumber.Text =  value.ToString();   
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
                Bitmap bitmapToDisplay = new Bitmap(original, new Size(pictureBoxLeft.Width, pictureBoxLeft.Height));
                pictureBoxLeft.Image = bitmapToDisplay;

            }
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            if (original == null) {
                MessageBox.Show("Choose file to convert first!!!");
                return;
            }

            newBitmap = original;
            var threadNumber =Int32.Parse(textThredNumber.Text.Trim());
            var message = "";
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (radioC.Checked)
            {
                // C# dll
                for (int i = 0; i < newBitmap.Width; i++)
                {
                    for (int j = 0; j < newBitmap.Height; j++) {

                        Color pixel = newBitmap.GetPixel(0, 0);
                        double[,] rgb = new[,] {
                            { Convert.ToDouble( pixel.R)},
                            { Convert.ToDouble(pixel.G)},
                            { Convert.ToDouble(pixel.B)}
                        };

                        double[,] newPixel = new double[3, 1];
                        newPixel = LMStoRGB( LMStoProtanopia( RGBtoLMS(rgb)));

                        newBitmap.SetPixel(i, j, Color.FromArgb(pixel.A, 
                            Convert.ToByte(newPixel[0, 0] ), 
                            Convert.ToByte(newPixel[1, 0] ), 
                            Convert.ToByte(newPixel[2, 0] )
                            ));

                    //MessageBox.Show(String.Format("R: {0}  G:  {1}  B: {2}", newPixel[0, 0], newPixel[1,0], newPixel[2,0] ));
                }
                }

                //
                cIterator++;
                message += String.Format("{0}. ", cIterator);
            }
            else if (radioAsm.Checked)
            {
                // ASM dll


                //
                amsIterator++;
                message += String.Format("{0}. ", amsIterator);
            }
            watch.Stop();
            var time = watch.ElapsedTicks;
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

            

            Bitmap bitmapToDisplay = new Bitmap(newBitmap, new Size(pictureBox2.Width, pictureBox2.Height));
            pictureBox2.Image = bitmapToDisplay;

            //MessageBox.Show(String.Format("Execution took {0}", time.ToString()));

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
            }

        }
    }

}