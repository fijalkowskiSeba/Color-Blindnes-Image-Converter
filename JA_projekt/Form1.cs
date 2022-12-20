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
                        var alpha = pixel.A;
                        var red = pixel.R;
                        var green = pixel.G;
                        var blue = pixel.B;
                    //MessageBox.Show(String.Format("A: {0}  R: {1}  G:  {2}  B: {3}", alpha,red,green,blue ));
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