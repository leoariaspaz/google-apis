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
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool CrearServicio()
        {
            try
            {
                _svc = Lib.DriveLibrary.CrearServicioComoCuentaUsuario();
                _svc.HttpClient.Timeout = TimeSpan.FromMinutes(1);
                _log.Debug("Se creó el servicio Drive");
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
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
                _log.Error(ex);
                throw;
            }
        }
    }
}
