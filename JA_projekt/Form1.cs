using System.Runtime.InteropServices;
using Color = System.Drawing.Color;
using Size = System.Drawing.Size;
using CSHARP;
using System.Media;

namespace JA_projekt
{
    public partial class Form1 : Form
    {
        Bitmap original;
        Bitmap newImage = null;


        static String cResultsTitle = String.Format("C# results in milliseconds :{0}", Environment.NewLine);
        static String asmResultsTitle = String.Format("ASM results in milliseconds :{0}", Environment.NewLine);

        String cResults = cResultsTitle;
        String asmResults = asmResultsTitle;

        int cIterator = 0;
        int amsIterator = 0;


        public Form1()
        {
            InitializeComponent();
            textBoxASM.Text = asmResults;
            textBoxC.Text = cResults;

            var proccessors = Environment.ProcessorCount;

            trackBarThredNumber.Value = proccessors;
            textThredNumber.Text = proccessors.ToString();

            var text = "Your machine has ";
            text += proccessors.ToString();
            text += " logical processors";

            cpuLabel.Text = text;
            
        }

       

        private void scrool_changed(object sender, EventArgs e)
        {
            int value = trackBarThredNumber.Value;
            textThredNumber.Text = value.ToString();
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
                String extension = Path.GetExtension(fileName);
                if (extension.Equals(".jpg"))
                {
                    //loading file to bitmap
                    original = new Bitmap(fileName);


                    //new resized bitmap to display in small window
                    Bitmap bitmapToDisplay = new Bitmap(original, new System.Drawing.Size(pictureBoxLeft.Width, pictureBoxLeft.Height));
                    pictureBoxLeft.Image = bitmapToDisplay;
                }
            }
        }

        
        //CHANGE IF YOU CHANGE LOCALIZATION OF PROJECT
        [DllImport(@"D:\JA_projekt\JA_projekt\x64\Release\ASM.dll")]
        static extern void rgbEdit(float[,] rgb);
        private void buttonConvert_Click(object sender, EventArgs e)
        {
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
                        rgb= MatrixMultiplication.rgbToPronatopia(rgb);
                    }
                    else
                    {
                        // ASM dll
                        rgbEdit(rgb);
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
                var time = watch.ElapsedMilliseconds;



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

                message += String.Format("{0} ms ", time);
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

        //TIME TESTS
        private void button2_Click(object sender, EventArgs e)
        {

            Bitmap small = null;

            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialog.Title = "Choose small jpg";
            DialogResult dialogResult1 = openFileDialog.ShowDialog();
            if (dialogResult1 == System.Windows.Forms.DialogResult.OK)
            {
                //geting file from dialog
                String fileName = openFileDialog.FileName;
                //loading file to bitmap
                small = new Bitmap(fileName);

            }

            Bitmap medium = null;

            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialog.Title = "Choose medium jpg";
            DialogResult dialogResult2 = openFileDialog.ShowDialog();
            if (dialogResult2 == System.Windows.Forms.DialogResult.OK)
            {
                //geting file from dialog
                String fileName = openFileDialog.FileName;
                //loading file to bitmap
                medium = new Bitmap(fileName);

            }

            Bitmap big = null;

            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialog.Title = "Choose big jpg";
            DialogResult dialogResult3 = openFileDialog.ShowDialog();
            if (dialogResult3 == System.Windows.Forms.DialogResult.OK)
            {
                //geting file from dialog
                String fileName = openFileDialog.FileName;
                //loading file to bitmap
                big = new Bitmap(fileName);
            }

            int[] array = new int[] { 1, 2, 4, 6, 8, 12, 16, 32, 64 };
            var arrayLenghth = array.Length;

            long[,] smallASMResults = new long[arrayLenghth, 1];
            long[,] mediumASMResults = new long[arrayLenghth, 1];
            long[,] bigASMResults = new long[arrayLenghth, 1];

            long[,] smallCSHARPResults = new long[arrayLenghth, 1];
            long[,] mediumCSHARPResults = new long[arrayLenghth, 1];
            long[,] bigCSHARPResults = new long[arrayLenghth, 1];

            

            if (small == null || medium == null || big == null)
            {
                return;
            }
            else
            {

                for (int i = 0; i < arrayLenghth; i++)
                {
                    var threads = array[i];
                    smallASMResults[i, 0] = convertionTest(small, true, threads);
                    mediumASMResults[i, 0] = convertionTest(medium, true, threads);
                    bigASMResults[i, 0] = convertionTest(big, true, threads);
                    
                    smallCSHARPResults[i, 0] = convertionTest(small, false, threads);
                    mediumCSHARPResults[i, 0] = convertionTest(medium, false, threads);
                    bigCSHARPResults[i, 0] = convertionTest(big, false, threads);
                }

            }

           
            using (StreamWriter outfile = new StreamWriter(@"D:\JA_projekt\wyniki.csv"))
            {
                
                string content = "SMALL;ASM;C#;;MEDIUM;ASM;C#;;BIG;ASM;C#";
                outfile.WriteLine(content);

                for (int x = 0; x < arrayLenghth; x++)
                {
                    var threads = array[x];
                    content = "";
                    content += (threads).ToString()+";";
                    content += smallASMResults[x,0].ToString()+";";
                    content += smallCSHARPResults[x,0].ToString()+";;";
                    content += (threads).ToString() + ";";
                    content += mediumASMResults[x, 0].ToString() + ";";
                    content += mediumCSHARPResults[x, 0].ToString() + ";;";
                    content += (threads).ToString() + ";";
                    content += bigASMResults[x, 0].ToString() + ";";
                    content += bigCSHARPResults[x, 0].ToString() + ";;";
                    outfile.WriteLine(content);
                }
            }

            SystemSounds.Exclamation.Play();
            SystemSounds.Exclamation.Play();
            SystemSounds.Exclamation.Play();
            SystemSounds.Exclamation.Play();
            SystemSounds.Exclamation.Play();

        }

        private long convertionTest(Bitmap bitmap, bool asmDll, int threadNumber)
        {
            //long timeSum=0;
            long timeMinimum = long.MaxValue;
            var howManyTIMES = 10;

            for (int i = 0; i < howManyTIMES; i++)
            {

                Bitmap newImage = new Bitmap(bitmap);

                List<(int x, int y)> pixelCoordinates = new List<(int x, int y)>();
                for (int y = 0; y < newImage.Height; y++)
                {
                    for (int x = 0; x < newImage.Width; x++)
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

                    if (!asmDll)
                    {
                        // C# dll
                        rgb = MatrixMultiplication.rgbToPronatopia(rgb);
                    }
                    else
                    {
                        // ASM dll
                        rgbEdit(rgb);
                    }

                    int r = Math.Abs((int)rgb[0, 0]);
                    int g = Math.Abs((int)rgb[1, 0]);
                    int b = Math.Abs((int)rgb[2, 0]);


                    // nowa macierz rgb -> pixel
                    Color pixel = Color.FromArgb(r, g, b);
                    lockBitmap.SetPixel(x, y, pixel);

                });

                lockBitmap.UnlockBits();
                watch.Stop();
                if (timeMinimum > watch.ElapsedMilliseconds)
                {
                    timeMinimum = watch.ElapsedMilliseconds; 
                }


            }

            //return timeSum / howManyTIMES;
            return timeMinimum;

            
        }

    }


 




}