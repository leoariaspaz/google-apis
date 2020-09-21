using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Photos
{
    public class AppSettings<T> where T : new()
    {
        private static string _fileName;
        private static T data;

        static AppSettings()
        {
            _fileName = AppDomain.CurrentDomain.BaseDirectory + //"Settings.xml";
                            System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".xml";
            //FileName = AppDomain.CurrentDomain.BaseDirectory + "settings.xml";
        }

        //public static string FileName { get { return _fileName; } set { _fileName = value; } }

        /// <summary>
        /// La ruta del archivo de configuración.
        /// </summary>
        public static string FileName
        {
            get
            {
                return _fileName;
            }
        }

        /// <summary>
        /// Graba los valores de un objeto en un archivo .xml.
        /// </summary>
        /// <param name="settings">El objeto con los valores de configuración.</param>
        /// <param name="path">
        /// Si se especifica, indica la ruta adónde se grabará el archivo.
        /// Si no se especifica, el archivo se grabará en la misma ruta desde donde se obtuvo la última vez que se cargó.
        /// </param>
        public static void Save(T settings, string path = null)
        {
            using (TextWriter tw = new StreamWriter(path ?? _fileName))
            {
                new XmlSerializer(typeof(T)).Serialize(tw, settings);
            }
        }

        //public void Save(string path = null)
        //{
        //    using (TextWriter tw = new StreamWriter(path ?? _fileName))
        //    {
        //        new XmlSerializer(typeof(T)).Serialize(tw, this);
        //        data = default(T);
        //        Load();
        //    }
        //}

        /// <summary>
        /// Obtiene los datos desde el archivo de configuración.
        /// </summary>
        /// <param name="path">
        /// La ruta del archivo a cargar. Si se especifica reemplaza a la ruta original. Si no se especifica, se asume 
        /// que el archivo se encuentra en la misma carpeta que el ejecutable con el nombre del ejecutable y extensión 
        /// .xml. 
        /// </param>
        /// <returns>Un objeto con los datos que contiene el archivo.</returns>
        public static T Load(string path = null)
        {
            if (EqualityComparer<T>.Default.Equals(data, default(T)))
            {
                var ser = new XmlSerializer(typeof(T));
                //T t = new T();
                data = new T();
                string fn = path ?? _fileName;
                if (_fileName != fn)
                {
                    _fileName = fn;
                }
                if (File.Exists(fn))
                {
                    using (var reader = XmlReader.Create(fn))
                    {
                        data = (T)ser.Deserialize(reader);
                    }
                }
            }
            return data;
        }
    }
}
