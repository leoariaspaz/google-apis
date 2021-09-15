using Google.Apis.Auth.OAuth2;
using Google.Apis.PhotosLibrary.v1;
using Google.Apis.Services;
using WinPhotos.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WinPhotos.Lib.Settings;
using Google.Apis.PhotosLibrary.v1.Data;
using WinPhotos.Models;

namespace WinPhotos.Controllers
{
    class ConfigurationController
    {
        public async Task<List<Album>> ListarÁlbumes()
        {
            var result = new List<Album>();
            var svc = await PhotosProxy.CrearServicio();
            var albumsList = svc.Albums.List();
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
