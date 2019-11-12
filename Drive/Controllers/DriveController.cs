using Drive.Lib;
using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.Controllers
{
    class DriveController
    {
        private DriveService _svc = null;

        private bool CrearServicio()
        {
            try
            {
                _svc = Lib.DriveLibrary.CrearServicioComoCuentaUsuario();
                _svc.HttpClient.Timeout = TimeSpan.FromMinutes(1);
                Console.WriteLine("Se creó el servicio Drive");
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool SubirDB()
        {
            try
            {
                if (CrearServicio())
                {
                    using (_svc)
                    {
                        Archivos.UploadDB(_svc);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
