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
using System.Threading;
using System.Threading.Tasks;

namespace Photos.Controllers
{
    class TesterController
    {
        private List<string> _idFotos = null;
        private PhotosLibraryService _svc = null;

        public async Task Run()
        {
            Console.WriteLine("Photos API Sample: List Photos");
            Console.WriteLine("================================");

            _svc = await PhotosLibrary.CrearServicio();

            var photos = await _svc.Albums.List().ExecuteAsync();
            int i = 1;
            foreach (var a in photos.Albums)
            {
                Console.WriteLine("{0} - {1}", i, a.Title);
                i++;
            }

            var album = photos.Albums.FirstOrDefault();
            Console.WriteLine("Listando {0} ítems del álbum {1}", album.MediaItemsCount, album.Title);

            _idFotos = new List<string>();

            var body = new SearchMediaItemsRequest { AlbumId = album.Id, PageSize = 100 };
            await Buscar(body);

            _idFotos.Clear();
            Console.WriteLine("Filtrando solamente fotos de toda la librería");
            body = new SearchMediaItemsRequest
            {
                PageSize = 100,
                Filters = new Filters
                {
                    MediaTypeFilter = new MediaTypeFilter
                    {
                        MediaTypes = new string[] { "PHOTO" }.ToList()
                    }
                }
            };
            await Buscar(body);

            SaveImage(_idFotos.First(), ImageFormat.Jpeg).Wait();
        }

        private async Task Buscar(SearchMediaItemsRequest body)
        {
            int pág = 1;
            var response = await _svc.MediaItems.Search(body).ExecuteAsync();
            var photos = response.MediaItems.Where(m => m.MediaMetadata.Photo != null);
            _idFotos.AddRange(photos.Select(m => m.Id));
            Console.WriteLine("Cargando fotos");
            while (response != null)
            {
                //Console.WriteLine("Listando fotos de página {0}", pág);
                //Console.WriteLine(String.Join(", ", photos.Select(m => m.Filename)));

                Console.Write(".");
                if (response.NextPageToken == null)
                {
                    break;
                }
                else
                {
                    body.PageToken = response.NextPageToken;
                    response = await _svc.MediaItems.Search(body).ExecuteAsync();
                    photos = response.MediaItems.Where(m => m.MediaMetadata.Photo != null);
                    _idFotos.AddRange(photos.Select(m => m.Id));
                    pág++;
                }
            }

            Console.WriteLine();
            Console.WriteLine("{0} ítems leídos", _idFotos.Count);
        }

        public async Task<string> SaveImage(string idPhoto, ImageFormat format)
        {
            var item = await _svc.MediaItems.Get(idPhoto).ExecuteAsync();
            string filename = String.Format(@"c:\temp\{0}", item.Filename);

            //SaveImage(items.MediaItems.First().BaseUrl + "=d", @"c:\temp\foto.jpg", ImageFormat.Jpeg);
            //Console.WriteLine();
            Console.WriteLine("Se grabó la imagen {0} en disco", filename);

            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(item.BaseUrl + "=d");
                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (var yourImage = Image.FromStream(mem))
                    {
                        yourImage.Save(filename, format);
                    }
                }
            }

            return filename;
        }
    }
}
