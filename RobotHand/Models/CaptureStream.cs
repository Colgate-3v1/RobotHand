using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RobotHand.Models
{
    public class CaptureStream
    {
        private static IPEndPoint consumerEndPoint;
        private static UdpClient udpClient = new UdpClient();
        private VideoCaptureDevice videoSource;

        public event EventHandler<byte[]>? ChangedBuffer;

        public static CaptureStream Instance { get; private set; }

        public CaptureStream()
        {
            Work();
            Instance = this;
        }

        public void Work()
        {
            var ip = "192.168.0.134";
            var port = 48654;
            consumerEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            Debug.WriteLine($"consumer: {consumerEndPoint}");

            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            Debug.WriteLine(videoDevices.Count);
            Debug.WriteLine(videoDevices[0].Name);
            Debug.WriteLine(videoDevices[0].MonikerString);
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            Debug.WriteLine(videoSource.IsRunning);
            VideoCapabilities[] vcs = videoSource.VideoCapabilities;
            videoSource.VideoResolution = vcs[0];
            videoSource.NewFrame += VideoSource_NewFrame;
            videoSource.Start();

            videoSource.VideoSourceError += VideoSource_VideoSourceError;
            Debug.WriteLine(videoSource.IsRunning);
        }

        public void Stop()
        {
            videoSource.SignalToStop();
            videoSource.VideoSourceError -= VideoSource_VideoSourceError;
        }

        private void VideoSource_VideoSourceError(object sender, VideoSourceErrorEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.Description);
        }

        private async void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //var bmp = CropToSquare(new Bitmap(eventArgs.Frame));
            //var bmp = new Bitmap(eventArgs.Frame);
            try
            {
                using (var bmp = new Bitmap(eventArgs.Frame, new Size(1920, 1080)))
                {
                    await Task.Run(() =>
                    {
                        using (var ms = new MemoryStream())
                        {
                            bmp.Save(ms, ImageFormat.Jpeg);
                            var bytes = ms.ToArray();
                            ChangedBuffer?.Invoke(this, bytes);
                            //udpClient.Send(bytes, bytes.Length, consumerEndPoint);
                        }
                    });
                }
            }
            catch
            {

            }
        }
    }
}
