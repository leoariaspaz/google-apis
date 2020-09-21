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
        public List<Album> ListarÁlbumes()
        {
            PhotosLibraryService _svc = null;
            var result = new List<Album>();
            _svc = AsyncHelpers.RunSync(() => PhotosLibrary.CrearServicio());
            var albumsList = _svc.Albums.List();
            var photos = AsyncHelpers.RunSync(() => albumsList.ExecuteAsync());
            result.AddRange(photos.Albums.ToList());
            var token = photos.NextPageToken;
            while (!String.IsNullOrEmpty(token))
            {
                albumsList.PageToken = token;
                photos = AsyncHelpers.RunSync(() => albumsList.ExecuteAsync());
                result.AddRange(photos.Albums.ToList());
                token = photos.NextPageToken;
            }
            return result;
        }

        public void ActualizarÁlbumes(List<Álbum> álbumes)
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
