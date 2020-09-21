using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PhotosLibrary.v1;
using Google.Apis.PhotosLibrary.v1.Data;
using Google.Apis.Services;
using Photos.Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Photos.Controllers
{
    class ChangeWallpaperController
    {
        private List<string> _idFotos = null;
        private PhotosLibraryService _svc = null;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal async Task Start()
        {
            int i = 0;
            while (true)
            {
                try
                {
                    MySettings settings = MySettings.Load();
                    if (_svc == null)
                    {
                        _svc = await PhotosLibrary.CrearServicio();
                        _idFotos = new List<string>();
                        if (settings.Albums == null || settings.Albums.Count == 0)
                        {
                            Console.WriteLine("No hay álbums seleccionados.");
                            return;
                        }
                        foreach (var item in settings.Albums)
                        {
                            var body = new SearchMediaItemsRequest { AlbumId = item, PageSize = 100 };
                            _idFotos.AddRange(await CargarFotos(body));
                        }
                        Console.WriteLine("Se cargaron {0} fotos de {1} álbums", _idFotos.Count, settings.Albums.Count);
                    }
                    var id = new Random().Next(0, _idFotos.Count - 1);
                    var path = await GrabarImagenProporcional(_idFotos.ElementAt(id), ImageFormat.Bmp);
                    Wallpaper.SetBackground(path, Wallpaper.Style.Stretched);
                    Console.WriteLine("Se estableció el fondo de pantalla.");
                    i = 0;
                    var sleepTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SleepTime"]);
                    Thread.Sleep(TimeSpan.FromMinutes(sleepTime));
                }
                catch (AggregateException ex)
                {
                    i++;
                    Log.Error("Error " + i);
                    foreach (var e in ex.InnerExceptions)
                    {
                        Log.Error(e);
                    }
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    if (i == 3) return;
                }
                catch (HttpRequestException ex)
                {
                    i++;
                    Log.Error("Error " + i);
                    Log.Error(ex);
                    if (ex.InnerException != null) Log.Error(ex.InnerException);
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    if (i == 3) return;
                }
                catch (GoogleApiException ex)
                {
                    i++;
                    Log.Error("Error " + i);
                    Log.Error(ex);
                    if (ex.InnerException != null) Log.Error(ex.InnerException);
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    if (i == 3) return;
                }
                catch (Exception ex)
                {
                    i++;
                    Log.Error("Error " + i);
                    Log.Error(ex);
                    if (ex.InnerException != null) Log.Error(ex.InnerException);
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    if (i == 3) return;
                }
            }
        }

        private async Task<string> GrabarImagenProporcional(string idPhoto, ImageFormat format)
        {
            var w = Screen.PrimaryScreen.Bounds.Width;
            var h = Screen.PrimaryScreen.Bounds.Height;
            var item = await _svc.MediaItems.Get(idPhoto).ExecuteAsync();
            //string filename = String.Format(@"{0}\{1}", Path.GetTempPath(), item.Filename);
            //filename = Path.ChangeExtension(filename, ".bmp");
            string filename = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
            Console.WriteLine("Se grabó la imagen {0} en disco", filename);
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(item.BaseUrl + "=d");
                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (var yourImage = Image.FromStream(mem))
                    {
                        //yourImage.Save(filename, format);
                        using (var newImage = ScaleImage(yourImage, w, h))
                        {
                            newImage.Save(filename, format);
                            //Log.Info("Se estableció " + item.BaseUrl);
                            Log.Info("Se estableció " + item.Id);
                            Log.Info("Se estableció " + item.ProductUrl);
                        }
                    }
                }
            }
            return filename;
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                if (newImage.Width < maxWidth || newImage.Height < maxHeight)
                {
                    Bitmap newImage2 = new Bitmap(maxWidth, maxHeight, image.PixelFormat);
                    using (Graphics g = Graphics.FromImage(newImage2))
                    {
                        // fill target image with color
                        g.FillRectangle(Brushes.Black, 0, 0, maxWidth, maxHeight);

                        // place source image inside the target image
                        var dstX = (maxWidth - newImage.Width) / 2;
                        var dstY = (maxHeight - newImage.Height) / 2;
                        g.DrawImage(newImage, dstX, dstY);
                    }
                    return newImage2;
                }
            }
            return newImage;
        }

        private async Task<List<string>> CargarFotos(SearchMediaItemsRequest body)
        {
            int pág = 1;
            var response = await _svc.MediaItems.Search(body).ExecuteAsync();
            var photos = response.MediaItems.Where(m => m.MediaMetadata.Photo != null);
            List<string> ids = new List<string>();
            ids.AddRange(photos.Select(m => m.Id));
            while (response != null)
            {
                if (response.NextPageToken == null)
                {
                    break;
                }
                else
                {
                    body.PageToken = response.NextPageToken;
                    response = await _svc.MediaItems.Search(body).ExecuteAsync();
                    photos = response.MediaItems.Where(m => m.MediaMetadata.Photo != null);
                    ids.AddRange(photos.Select(m => m.Id));
                    pág++;
                }
            }
            return ids;
        }
    }
}
