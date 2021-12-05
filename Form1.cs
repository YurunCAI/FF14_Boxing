using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FF14Felling
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int count = 0;
        int n = 150;
        int r, g, b, r2, g2, b2, r3, g3, b3;
        bool colorCheck = false;
        bool controlFlag = false;
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        private static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);
        [DllImport("user32.dll")]
        private static extern void keybd_event(Byte bVk, Byte bScan, Int32 dwFlags, Int32 dwExtraInfo);
        [DllImport("gdi32.dll")]
        private static extern int GetPixel(IntPtr hdc, Point p);



        //click botton to start
        private void colorBtn_Click(object sender, EventArgs e)
        {
            controlFlag = true;
            maintain(sender, e);
        }


        Timer countDown_0 = new Timer();
        Timer countDownFelling = new Timer();
        //maintain
        private void maintain(object sender, EventArgs e)
        {
            //press 0
            
            countDown_0.Tick += PressZero;
            countDown_0.Enabled = true;
            countDown_0.Interval = 10;
            countDown_0.Start();


        }

        private void PressZero(object sender, EventArgs e)
        {
            count++;
            lblCount.Text = count.ToString();
            if (count == n) { keybd_event(96, 1, 0, 0); keybd_event(96, 1, 2, 0); lblCheck.Text = "Press 0"; } //select boxing machine
            else if (count == n + 25) { keybd_event(96, 1, 0, 0); keybd_event(96, 1, 2, 0); lblCheck.Text = "Press 0"; } //start boxing machine
            else if (count == n + 100) { keybd_event(98, 1, 0, 0); keybd_event(98, 1, 2, 0); lblCheck.Text = "Press 2"; } //go confirm
            else if (count == n + 125) { keybd_event(96, 1, 0, 0); keybd_event(96, 1, 2, 0); lblCheck.Text = "Press 0"; }//confirm
            else if (count > n + 175 && count < n + 300)
            {
                //Felling
                Felling(sender, e);
            }
            else if (count > n + 400)
            {

                Point p3 = new Point(822, 512);
                IntPtr hdc3 = GetDC(IntPtr.Zero);
                int c3 = GetPixel(hdc3, p3);
                r3 = (c3 & 0xFF);
                g3 = (c3 & 0xFF00) / 256;
                b3 = (c3 & 0xFF0000) / 65536;
                pictureBox3.BackColor = Color.FromArgb(r3, g3, b3);
                if (r3 > 130 && r3 < 145 && g3 > 95 && g3 < 110 && b3 > 70 && b3 < 80)
                {
                    keybd_event(111, 0, 0, 0); keybd_event(111, 0, 2, 0);//esc
                    count = 100;
                    ReleaseDC(IntPtr.Zero, hdc3);

                    countDown_0.Tick -= PressZero;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    countDown_0.Tick += PressZero;
                }
                else
                {
                    count = 100;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
        }

        private void Felling(object sender, EventArgs e)
        {
            Point p = new Point(907, 969); // get position
            IntPtr hdc = GetDC(IntPtr.Zero);
            Point p2 = new Point(940, 969); //get second position
            IntPtr hdc2 = GetDC(IntPtr.Zero);
            int c = GetPixel(hdc, p); //get color
            int c2 = GetPixel(hdc2, p2);                          
            r = (c & 0xFF);
            g = (c & 0xFF00) / 256;
            b = (c & 0xFF0000) / 65536;
            r2 = (c2 & 0xFF);
            g2 = (c2 & 0xFF00) / 256;
            b2 = (c2 & 0xFF0000) / 65536;
            pictureBox1.BackColor = Color.FromArgb(r, g, b);
            pictureBox2.BackColor = Color.FromArgb(r2, g2, b2);
            ColorCheck(r, g, b, r2, g2, b2);
            if (colorCheck) //left the qq window rgb
            {
                lblCheck.Text = "Check";
                keybd_event(96, 0, 0, 0);
                keybd_event(96, 0, 2, 0);
                ReleaseDC(IntPtr.Zero, hdc);
                ReleaseDC(IntPtr.Zero, hdc2);
            }
            else
            {
                lblCheck.Text = "UnCheck";
            }
        }

        private void ColorCheck(int r, int g, int b, int r2, int g2, int b2)
        {
            //r
            if (r > 202 && r < 212 && r2 >87 && r2 <97)
            {
                //g
                if (g > 121 && g < 131 )
                {
                    //b
                    if (b > 33 && b < 42) colorCheck = true;
                }
            }
            else
            {
                colorCheck = false;
            }
        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            if (controlFlag)
            {
                count = 0;
                countDown_0.Dispose();
            }
        }
    }
}
