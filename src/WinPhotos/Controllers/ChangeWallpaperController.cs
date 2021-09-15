using Google.Apis.PhotosLibrary.v1.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinPhotos.Lib;
using WinPhotos.Lib.Settings;

namespace WinPhotos.Controllers
{
    public class ChangeWallpaperController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal async Task<bool> ChangeWallpaper(List<String> fotos)
        {
            bool result = false;
            try
            {
                var id = new Random().Next(0, fotos.Count - 1);
                var path = await GrabarImagenProporcional(fotos.ElementAt(id), ImageFormat.Bmp);
                WallpaperService.SetBackground(path, WallpaperService.Style.Stretched);
                Log.Debug("Se estableció el fondo de pantalla.");
                result = true;
            }
            catch (Exception ex)
            {
                if (ex is AggregateException)
                {
                    foreach (var e in (ex as AggregateException).InnerExceptions)
                        Log.Error(e);
                }
                else
                {
                    Log.Error(ex);
                    if (ex.InnerException != null) Log.Error(ex.InnerException);
                }
            }
            return result;
        }

        public (bool, List<string>) DescargarListaFotos()
        {
            MySettings settings = MySettings.Load();
            if (settings.Albums == null || settings.Albums.Count == 0)
            {
                Log.Info("La lista de álbumes elegidos está vacía.");
                return (false, null);
            }
            else
            {
                Log.InfoFormat("Cargando fotos de {0} álbumes", settings.Albums.Count);
                var ids = new List<string>();
                _ = Parallel.ForEach(settings.Albums,
                    async album =>
                    {
                        var body = new SearchMediaItemsRequest { AlbumId = album, PageSize = 100 };
                        ids.AddRange(await CargarFotos(body));
                    });
                Log.DebugFormat("Se cargaron {0} fotos de {1} álbums", ids.Count, settings.Albums.Count);
                return (true, ids);
            }
        }

        private async Task<string> GrabarImagenProporcional(string idPhoto, ImageFormat format)
        {
            var w = Screen.PrimaryScreen.Bounds.Width;
            var h = Screen.PrimaryScreen.Bounds.Height;
            var svc = await PhotosProxy.CrearServicio();
            var item = await svc.MediaItems.Get(idPhoto).ExecuteAsync();
            string filename = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(item.BaseUrl + "=d");
                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (var yourImage = Image.FromStream(mem))
                    {
                        using (var newImage = new GraphicsService().ScaleImage(yourImage, w, h))
                        {
                            newImage.Save(filename, format);
                            Log.DebugFormat("Se grabó la imagen {0} como {1}", item.ProductUrl, filename);
                        }
                    }
                }
            }
            return filename;
        }

        private async Task<List<string>> CargarFotos(SearchMediaItemsRequest body)
        {
            int pág = 1;
            var svc = await PhotosProxy.CrearServicio();
            var a = await svc.Albums.Get(body.AlbumId).ExecuteAsync();
            Log.Debug($"Cargando fotos del álbum \"{a.Title}\"");
            var response = await svc.MediaItems.Search(body).ExecuteAsync();
            var photos = response.MediaItems.Where(m => m.MediaMetadata.Photo != null);
            List<string> ids = new List<string>();
            ids.AddRange(photos.Select(m => m.Id));
            while (response != null || response.NextPageToken == null)
            {
                body.PageToken = response.NextPageToken;
                response = await svc.MediaItems.Search(body).ExecuteAsync();
                photos = response.MediaItems.Where(m => m.MediaMetadata.Photo != null);
                ids.AddRange(photos.Select(m => m.Id));
                pág++;
            }
            return ids;
        }
    }
}
