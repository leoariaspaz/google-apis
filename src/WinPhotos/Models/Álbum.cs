namespace WinPhotos.Models
{
    public class Álbum
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }

        public Álbum(string id, string nombre)
        {
            Id = id;
            Nombre = nombre ?? "";
        }
    }
}
