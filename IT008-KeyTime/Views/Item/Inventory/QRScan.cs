using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IT008_KeyTime.Views.Item.Inventory
{
    public partial class QRScan : Form
    {
        MJPEGStream stream;
        bool isConnected = false;
        const string urlStream = "http://113.161.85.182/webcapture.jpg?command=snap&channel=1?1701774507";
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        public QRScan()
        {
            InitializeComponent();
        }
        private void ConnectMJPEG()
        {
            if (!isConnected)
            {
                //stream = new MJPEGStream(urlStream);
                //stream.NewFrame += stream_NewFrame;
                //stream.Start();
                CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                for (int i = 0; i < CaptureDevice.Count; i++)
                {
                    //comboBox1.Items.Add(CaptureDevice[i].Name);
                    Console.WriteLine(CaptureDevice[i].Name);
                }
                FinalFrame = new VideoCaptureDevice(CaptureDevice[0].MonikerString);
                VideoCapabilities[] vcs = FinalFrame.VideoCapabilities;
                FinalFrame.VideoResolution = vcs[0];
                FinalFrame.NewFrame += new NewFrameEventHandler(stream_NewFrame);
                FinalFrame.VideoSourceError += new VideoSourceErrorEventHandler(errorHandler);
                FinalFrame.Start();
                //timer1.Enabled = true;
                //timer1.Start();
                isConnected = true;
            }
            else
            {
                
            }

        }
        private void errorHandler(object sender, VideoSourceErrorEventArgs eventArgs)
        {
            Console.WriteLine("Video feed source error: " + eventArgs.Description);
        }

        public void stream_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bmp;
            if (timer1.Enabled == false)
            {
                timer1.Enabled = true;
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Bitmap img = (Bitmap)pictureBox1.Image;
            Console.WriteLine("timer1_Tick");
            if (img != null)
            {
                Console.WriteLine("img != null");
                ZXing.BarcodeReader Reader = new ZXing.BarcodeReader();
                Result result = Reader.Decode(img);
                try
                {
                    string decoded = result.ToString().Trim();
                    MessageBox.Show("Inventory for item id: " + decoded);

                    img.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "");
                }

            }
        }

        private void QRScan_Load(object sender, EventArgs e)
        {
            ConnectMJPEG();
        }

        private void QRScan_FormClosing(object sender, FormClosingEventArgs e)
        {
            isConnected = false;
            timer1.Stop();
            FinalFrame.Stop();
        }
    }
}
