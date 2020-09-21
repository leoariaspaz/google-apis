using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.Lib;

namespace Drive
{
    class Program
    {
        static DriveService _svc = null;

        static void Main(string[] args)
        {
            try
            {
                try
                {
                    //_svc = Lib.DriveLibrary.CrearServicioDriveComoCuentaServicio();
                    //UploadData(_svc);

                    //var s = uploadFile(_svc, @"C:\Users\Administrador\Pictures\wallpapers\20f3c1c9b45142f13fc4d103c2dbaddc.jpg", null);
                    //Console.WriteLine(s);

                    //CrearServicio();
                    //UploadData();

                    CrearServicio();
                    Archivos.UploadDB(_svc);

                    //var s = uploadFile(_svc, @"C:\Users\Administrador\Pictures\wallpapers\20f3c1c9b45142f13fc4d103c2dbaddc.jpg", null);
                    //Console.WriteLine(s);

                }
                catch (AggregateException ex)
                {
                    foreach (var e in ex.InnerExceptions)
                    {
                        //Log.Error(e);
                        Console.WriteLine(e);
                    }
                }
                //Console.WriteLine("Press any key to continue...");
                //Console.ReadKey();
            }
            catch (Exception ex)
            {
                //Log.Error(ex);
                Console.WriteLine(ex);
            }

            //Log.Info("Sale");

        }

        private static void CrearServicio()
        {
            if (_svc == null)
            {
                _svc = Lib.DriveLibrary.CrearServicioComoCuentaUsuario();
                _svc.HttpClient.Timeout = TimeSpan.FromMinutes(1);
                Console.WriteLine("Se creó el servicio Drive");
            }
        }
    }
}
