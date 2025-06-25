using Avalonia.Media.Imaging;
using ReactiveUI;
using RobotHand.Models;
using RobotHand.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobotHand.ViewModels
{
    public class VideoViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly CaptureStream _captureStream;

        private static Timer timer;

        public VideoViewModel(NavigationService navigationService,
                              CaptureStream captureStream)
        {
            _navigationService = navigationService;
            _captureStream = captureStream;

            _captureStream.ChangedBuffer += OnBitmapChanged;

            timer = new Timer(GetMatrix, null, 0, 250);
        }

        public static void StopTimer()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void GetMatrix(object o)
        {
            int[,] mas = new int[100, 100];
            Random random = new Random();
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 100; j++)
                    mas[i, j] = random.Next(0, 20);

            BitmapPollution = MatrixToBitmap(mas);
        }

        private async void OnBitmapChanged(object sender, byte[] e)
        {
            try
            {
                using (var ms = new MemoryStream(e))
                {
                    var bitmap = new System.Drawing.Bitmap(ms);
                    Bitmap = ConvertToAvaloniaBitmap(CropToSquare(bitmap));
                    //Bitmap = ConvertToAvaloniaBitmap(bitmap);
                    // binding к image
                }
            }
            catch { }
        }

        public static System.Drawing.Bitmap CropToSquare(System.Drawing.Bitmap original)
        {
            int width = original.Width;
            int height = original.Height;
            int size = Math.Min(width, height);
            int x = (width - size) / 2;
            int y = (height - size) / 2;

            System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(x, y, size, size);
            System.Drawing.Bitmap squareImage = new System.Drawing.Bitmap(size, size);

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(squareImage))
            {
                g.DrawImage(original, new System.Drawing.Rectangle(0, 0, size, size), cropRect, System.Drawing.GraphicsUnit.Pixel);
            }

            return squareImage;
        }

        //public async void Load()
        //{
        //    var port = 48654;

        //    var client = new UdpClient(port);


        //    while (true)
        //    {
        //        try
        //        {
        //            var data = await client.ReceiveAsync();
        //            using (var ms = new MemoryStream(data.Buffer))
        //            {
        //                var bitmap = new System.Drawing.Bitmap(ms);
        //                Bitmap = ConvertToAvaloniaBitmap(bitmap);
        //                // binding к image
        //            }
        //        }
        //        catch { }
        //    }
        //}

        //public async void LoadEvent()
        //{
        //    var port = 48654;

        //    var client = new UdpClient(port);


        //    while (true)
        //    {
        //        try
        //        {
        //            var data = await client.ReceiveAsync();
        //            using (var ms = new MemoryStream(data.Buffer))
        //            {
        //                var bitmap = new System.Drawing.Bitmap(ms);
        //                Bitmap = ConvertToAvaloniaBitmap(bitmap);
        //                // binding к image
        //            }
        //        }
        //        catch { }
        //    }
        //}

        private Bitmap _bitmap;
        public Bitmap Bitmap
        {
            get => _bitmap;
            set => this.RaiseAndSetIfChanged(ref _bitmap, value);
        }

        private Bitmap _bitmapPollution;
        public Bitmap BitmapPollution
        {
            get => _bitmapPollution;
            set => this.RaiseAndSetIfChanged(ref _bitmapPollution, value);
        }

        public static Bitmap ConvertToAvaloniaBitmap(System.Drawing.Bitmap bitmap)
        {
            if (bitmap == null)
                return null;
            System.Drawing.Bitmap bitmapTmp = new System.Drawing.Bitmap(bitmap);
            var bitmapdata = bitmapTmp.LockBits(new System.Drawing.Rectangle(0, 0, bitmapTmp.Width, bitmapTmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Bitmap bitmap1 = new Bitmap(Avalonia.Platform.PixelFormat.Bgra8888, Avalonia.Platform.AlphaFormat.Premul,
                bitmapdata.Scan0,
                new Avalonia.PixelSize(bitmapdata.Width, bitmapdata.Height),
                new Avalonia.Vector(96, 96),
                bitmapdata.Stride);
            bitmapTmp.UnlockBits(bitmapdata);
            bitmapTmp.Dispose();
            return bitmap1;
        }

        public static Bitmap MatrixToBitmap(int[,] matrix)
        {
            int width = matrix.GetLength(1);
            int height = matrix.GetLength(0);

            // Создаем новый Bitmap
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Проверяем значение элемента матрицы
                    if (matrix[y, x] == 1)
                    {
                        // Закрашиваем пиксель черным цветом, где значение равно 1
                        bitmap.SetPixel(x, y, System.Drawing.Color.White);
                    }
                    else
                    {
                        // Закрашиваем пиксель белым цветом, где значение равно 0
                        bitmap.SetPixel(x, y, System.Drawing.Color.Transparent);
                    }
                }
            }

            return ConvertToAvaloniaBitmap(bitmap);
        }

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken cancellationToken;

        public async void Start()
        {
            cancellationToken = _cancellationTokenSource.Token;
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Delay(2000);
                    Debug.WriteLine($"-{i}-");
                }
            }
            catch
            {
                Debug.WriteLine("stop");
            }
        }

    }
}
