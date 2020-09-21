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
    internal static class PhotosLibrary
    {
        internal static async Task<PhotosLibraryService> CrearServicio()
        {
            UserCredential credential;
            //using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            //{
            //    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        new[] { BooksService.Scope.Books },
            //        "user", CancellationToken.None, new FileDataStore("Books.ListMyLibrary"));
            //}

            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                            new ClientSecrets
                            {
                                ClientId = "",
                                ClientSecret = ""
                            },
                            new[] {
                                PhotosLibraryService.Scope.Photoslibrary,
                                PhotosLibraryService.Scope.PhotoslibraryReadonly,
                                PhotosLibraryService.Scope.PhotoslibraryReadonlyAppcreateddata
                            },
                            "user",
                            CancellationToken.None
                         //,
                         //new FileDataStore("Books.ListMyLibrary")
                         );

            // Create the service.
            var svc = new PhotosLibraryService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Pruebas",
            });
            return svc;
        }
    }
}
