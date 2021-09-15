using Google.Apis.Auth.OAuth2;
using Google.Apis.PhotosLibrary.v1;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WinPhotos.Lib
{
    internal static class PhotosProxy
    {
        private static PhotosLibraryService _svc = null;

        private static async Task<UserCredential> GenerarCredencial()
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

        internal static async Task<PhotosLibraryService> CrearServicio()
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

        internal static async void CerrarSesiónUsuarioGoogle()
        {
            var cred = await GenerarCredencial();
            await cred.RevokeTokenAsync(CancellationToken.None);
        }
    }
}
