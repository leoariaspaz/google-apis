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
            if (!fotos.Any())
            {
                return false;
            }
            bool result = false;
            try
            {
                var id = new Random().Next(0, fotos.Count - 1);
                var (img, url) = await new PhotosProxy().DescargarImagen(fotos.ElementAt(id));
                Log.Debug("Se descargó " + url);
                string filename = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
                new GraphicsService().SaveScreenProportionalImage(img, filename, ImageFormat.Bmp);
                WallpaperService.SetBackground(filename, WallpaperStyle.Stretched);
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

        public async Task<(bool, List<string>)> DescargarListaFotos()
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
                var svc = new PhotosProxy();
                var tasks = settings.Albums.Select(async album => await DescargarAlbum(album, ids, svc));
                await Task.WhenAll(tasks);
                Log.DebugFormat("Se descargaron ids de {0} fotos de {1} álbums", ids.Count, settings.Albums.Count);
                return (true, ids);
            }
        }

        private async Task DescargarAlbum(string album, List<string> ids, PhotosProxy svc)
        {
            Log.Debug($"Cargando fotos del álbum \"{await svc.ObtenerTituloAlbum(album)}\"");
            var body = new SearchMediaItemsRequest { AlbumId = album, PageSize = 100 };
            ids.AddRange(await svc.CargarFotos(body));
        }
    }
}
