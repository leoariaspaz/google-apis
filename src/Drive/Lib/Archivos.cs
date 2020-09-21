using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.Lib
{
    static class Archivos
    {
        public static void UploadData(DriveService svc)
        {
            var fileMetadata = new File()
            {
                Name = $"{DateTime.Now.ToString("yyyyMMddHHmmss")} - photo.jpg"
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(@"C:\Proyectos\Varios\Google\Drive\IMG_20180125_113129789_HDR.jpg",
                System.IO.FileMode.Open))
            {
                request = svc.Files.Create(fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                var u = request.Upload();
                Console.WriteLine(u.Exception);
                //Console.WriteLine("Request:\n" + Newtonsoft.Json.JsonConvert.SerializeObject(request));
            }
            var file = request.ResponseBody;
            if (file == null)
                Console.WriteLine("file es null");
            else if (file.Id == null)
            {
                Console.WriteLine("file.id es null");
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(file));
            }
            else
                Console.WriteLine("File ID: " + file.Id);
        }


        public static void UploadDB(DriveService svc)
        {
            var fileMetadata = new File()
            {
                Name = $"{DateTime.Now.ToString("yyyyMMddHHmmss")} - db.7z"
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(@"C:\Proyectos\Varios\Google\Drive\SMPorres.7z",
                System.IO.FileMode.Open))
            {
                request = svc.Files.Create(fileMetadata, stream, "application/x-7z-compressed");
                request.Fields = "id";
                var u = request.Upload();
                Console.WriteLine(u.Exception);
                //Console.WriteLine("Request:\n" + Newtonsoft.Json.JsonConvert.SerializeObject(request));
            }
            var file = request.ResponseBody;
            if (file == null)
                Console.WriteLine("file es null");
            else if (file.Id == null)
            {
                Console.WriteLine("file.id es null");
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(file));
            }
            else
                Console.WriteLine("File ID: " + file.Id);
        }


        public static Google.Apis.Drive.v3.Data.File uploadFile(DriveService _service, string _uploadFile, string _parent, string _descrp = "Uploaded with .NET!")
        {
            if (System.IO.File.Exists(_uploadFile))
            {
                File body = new File();
                body.Name = System.IO.Path.GetFileName(_uploadFile);
                body.Description = _descrp;
                body.MimeType = GetMimeType(_uploadFile);
                //body.Parents = new List<ParentReference>() { new ParentReference() { Id = _parent } };

                byte[] byteArray = System.IO.File.ReadAllBytes(_uploadFile);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    FilesResource.CreateMediaUpload request = _service.Files.Create(body, stream, GetMimeType(_uploadFile));
                    request.Upload();
                    return request.ResponseBody;
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message, "Error Occured");
                    Console.WriteLine(e);
                }
            }
            else
            {
                //MessageBox.Show("The file does not exist.", "404");
                Console.WriteLine("The file does not exist.");
            }

            return null;
        }

        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
    }
}
