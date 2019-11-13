using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Drive.Lib
{
    internal static class DriveLibrary
    {
        internal static DriveService CrearServicioComoCuentaUsuario()
        {
            UserCredential credential;

            #region Login con archivo json
            //using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            //{
            //    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        new[] { BooksService.Scope.Books },
            //        "user", CancellationToken.None, new FileDataStore("Books.ListMyLibrary"));
            //}
            #endregion

            var secrets = new ClientSecrets
            {
                ClientId = "1040777060117-bo0002q9hof8h6e43k5reo22aquj3cn7.apps.googleusercontent.com",
                ClientSecret = "NCAVdbVz02ZbmKCb5MmtnsnY"
            };

            string[] scopes = new[] {
                        DriveService.Scope.Drive,
                        DriveService.Scope.DriveFile,
                };

            string usr = "admin"; //debe ser usuario admin
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, scopes, usr,
                CancellationToken.None).Result;

            var init = new BaseClientService.Initializer() {
                HttpClientInitializer = credential,
                ApplicationName = "SqlBackup"
            };
            var svc = new DriveService(init);

            //Long Operations like file uploads might timeout. 10 is just precautionary value, can be 
            //set to any reasonable value depending on what you use your service for.
            svc.HttpClient.Timeout = TimeSpan.FromMinutes(10);

            return svc;
        }

        internal static DriveService CrearServicioDriveComoCuentaServicio()
        {
            var fs = new System.IO.FileStream(@"C:\Proyectos\Varios\Google\Drive\backup-de-base-de-datos-7a63bd05a0be.json",
                System.IO.FileMode.Open);
            string[] scopes = new string[] { DriveService.Scope.Drive,
                                             DriveService.Scope.DriveFile };
            GoogleCredential credential = GoogleCredential.FromStream(fs).CreateScoped(scopes);

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "MyAppName",
            });

            //Long Operations like file uploads might timeout. 100 is just precautionary value, can be set to any reasonable value depending on what you use your service for.
            //service.HttpClient.Timeout = TimeSpan.FromMinutes(100);

            return service;
        }
    }
}
