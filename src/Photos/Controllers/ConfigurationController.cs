using Google.Apis.Auth.OAuth2;
using Google.Apis.PhotosLibrary.v1;
using Google.Apis.Services;
using Photos.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Photos.Controllers
{
    class ConfigurationController
    {
        private List<string> _idFotos = null;
        private PhotosLibraryService _svc = null;

        public async Task Configurar()
        {
            _svc = await PhotosLibrary.CrearServicio();
            var photos = await _svc.Albums.List().ExecuteAsync();
            var albums = new List<string>();
            MySettings settings = MySettings.Load();
            if (settings.Albums == null) settings.Albums = new List<string>();
            albums.AddRange(settings.Albums);
            try
            {
                while (true)
                {
                    Console.WriteLine("Photos API Sample: List Photos");
                    Console.WriteLine("================================");
                    int i = 1;
                    foreach (var a in photos.Albums)
                    {
                        Console.WriteLine("{0} - {1}", i, a.Title);
                        i++;
                    }
                    Console.WriteLine("Seleccione el álbum del cual se obtendrán las fotos (0 para salir):");
                    var s = Console.ReadLine();
                    if (Int32.TryParse(s, out int opción))
                    {
                        if (opción == 0)
                        {
                            return;
                        }
                        else
                        {
                            var e = photos.Albums.ElementAt(opción - 1);
                            if (settings.Albums.Any(a => a == e.Id))
                            {
                                Console.WriteLine("Ya está añadido el álbum seleccionado.");
                            }
                            else
                            {
                                settings.Albums.Add(e.Id);
                                var t = from p in photos.Albums
                                        join a in settings.Albums on p.Id equals a
                                        select "- " + p.Title;
                                Console.WriteLine("Agregado {0}.-", e.Title);
                                Console.WriteLine("Álbums añadidos: \n" + String.Join("\n", t));
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error");
                    }
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            finally
            {
                MySettings.Save(settings);
            }
        }
    }
}
