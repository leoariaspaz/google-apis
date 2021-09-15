using System.Collections.Generic;
using System.Linq;
using WinPhotos.Lib.Settings;
using WinPhotos.Models;

namespace WinPhotos.Controllers
{
    public class ConfigurationController
    {
        public void ActualizarÁlbumes(List<ÁlbumViewModel> álbumes)
        {
            MySettings settings = MySettings.Load();
            try
            {
                settings.Albums = (from a in álbumes select a.Id).ToList();
            }
            finally
            {
                MySettings.Save(settings);
            }
        }

        public List<string> GetÁlbumesId()
        {
            MySettings settings = MySettings.Load();
            return settings.Albums ?? new List<string>();
        }
    }
}
