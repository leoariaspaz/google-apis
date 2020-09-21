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
            //using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            //{
            //    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        new[] { BooksService.Scope.Books },
            //        "user", CancellationToken.None, new FileDataStore("Books.ListMyLibrary"));
            //}

            var secrets = new ClientSecrets
            {
                ClientId = "",
                ClientSecret = ""
            };

            string[] scopes = new[] {
                        DriveService.Scope.Drive,
                        //DriveService.Scope.DriveAppdata,
                        DriveService.Scope.DriveFile,
                        //DriveService.Scope.DriveMetadata,
                        //DriveService.Scope.DriveMetadataReadonly,
                        //DriveService.Scope.DrivePhotosReadonly,
                        //DriveService.Scope.DriveReadonly,
                        //DriveService.Scope.DriveScripts
                };

            //debe ser usuario admin
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, scopes, 
                "admin",
                //Environment.UserName,
                CancellationToken.None).Result;
            //, new Google.Apis.Util.Store.FileDataStore("MyAppsToken"));

            // Create the service.
            var svc = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "SqlBackup",
            });

            //Long Operations like file uploads might timeout. 100 is just precautionary value, can be set to any reasonable value depending on what you use your service for.
            svc.HttpClient.Timeout = TimeSpan.FromMinutes(10);

            return svc;
        }

        internal static async Task<DriveService> GetDriveService()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive,
                                             DriveService.Scope.DriveFile };
            var clientId = "xxxxxx";      // From https://console.developers.google.com
            var clientSecret = "xxxxxxx";          // From https://console.developers.google.com
                                                   // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                scopes,
                Environment.UserName,
                CancellationToken.None,
                new Google.Apis.Util.Store.FileDataStore("MyAppsToken"));
            //Once consent is recieved, your token will be stored locally on the AppData directory, so that next time you wont be prompted for consent. 

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "MyAppName",
            });
            //Long Operations like file uploads might timeout. 100 is just precautionary value, can be set to any reasonable value depending on what you use your service for.
            service.HttpClient.Timeout = TimeSpan.FromMinutes(100);

            return service;
        }

        internal static DriveService CrearServicioDriveComoCuentaServicio()
        {
            //GoogleCredential credential = GoogleCredential.fromStream(new FileInputStream("MyProject-1234.json"))
            //    .createScoped(Collections.singleton(SQLAdminScopes.SQLSERVICE_ADMIN));

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
            //service.HttpClient.Timeout = TimeSpan.FromMinutes(100);

            //Long Operations like file uploads might timeout. 100 is just precautionary value, can be set to any reasonable value depending on what you use your service for.
            return service;
        }
    }
}
