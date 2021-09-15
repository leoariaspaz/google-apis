using Google.Apis.Auth.OAuth2;
using Google.Apis.PhotosLibrary.v1;
using Google.Apis.PhotosLibrary.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace WinPhotos.Lib
{
    public class PhotosProxy
    {
        private static PhotosLibraryService _svc = null;

        private async Task<UserCredential> GenerarCredencial()
        {
            UserCredential credential;
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                            new ClientSecrets
                            {
                                //ClientId y ClientSecret deben ser IDs de cliente de OAuth 2.0
                                //obtenidos desde https://console.developers.google.com/apis/credentials/
                                ClientId = System.Configuration.ConfigurationManager.AppSettings["ClientId"],
                                ClientSecret = System.Configuration.ConfigurationManager.AppSettings["ClientSecret"]
                            },
                            new[] {
                                PhotosLibraryService.Scope.Photoslibrary,
                                PhotosLibraryService.Scope.PhotoslibraryReadonly,
                                PhotosLibraryService.Scope.PhotoslibraryReadonlyAppcreateddata
                            },
                            "user",
                            CancellationToken.None,
                         new Google.Apis.Util.Store.FileDataStore("WinPhotos")
                         );
            return credential;
        }

        public async Task<PhotosLibraryService> CrearServicio()
        {
            if (_svc == null)
            {
                _svc = new PhotosLibraryService(
                            new BaseClientService.Initializer()
                            {
                                HttpClientInitializer = await GenerarCredencial(),
                                ApplicationName = "Pruebas",
                            });
            }
            return _svc;
        }

        public async void CerrarSesiónUsuarioGoogle()
        {
            var cred = await GenerarCredencial();
            await cred.RevokeTokenAsync(CancellationToken.None);
        }

        public async Task<string> ObtenerTituloAlbum(string albumId)
        {
            var a = await _svc.Albums.Get(albumId).ExecuteAsync();
            return a.Title;
        }

        public async Task<List<string>> CargarFotos(SearchMediaItemsRequest body)
        {
            int pág = 1;
            var response = await _svc.MediaItems.Search(body).ExecuteAsync();
            var photos = response.MediaItems.Where(m => m.MediaMetadata.Photo != null);
            List<string> ids = new List<string>();
            ids.AddRange(photos.Select(m => m.Id));
            while (response != null && response.NextPageToken != null)
            {
                body.PageToken = response.NextPageToken;
                response = await _svc.MediaItems.Search(body).ExecuteAsync();
                photos = response.MediaItems.Where(m => m.MediaMetadata.Photo != null);
                ids.AddRange(photos.Select(m => m.Id));
                pág++;
            }
            return ids;
        }

        public async Task<(byte[], string)> DescargarImagen(string idPhoto)
        {
            var item = await _svc.MediaItems.Get(idPhoto).ExecuteAsync();
            using (WebClient webClient = new WebClient())
            {
                var url = item.BaseUrl + "=d";
                byte[] data = webClient.DownloadData(url);
                return (data, url);
            }
        }

        public async Task<List<Album>> ListarÁlbumes()
        {
            var result = new List<Album>();
            var albumsList = _svc.Albums.List();
            do
            {
                ListAlbumsResponse photos;
                photos = await albumsList.ExecuteAsync();
                if (photos.Albums == null) break;
                result.AddRange(photos.Albums);
                albumsList.PageToken = photos.NextPageToken;
            } while (!String.IsNullOrEmpty(albumsList.PageToken));
            return result;
        }
    }
}
