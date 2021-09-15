namespace WinPhotos.Models
{
    public class ÁlbumViewModel
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public override string ToString()
        {
            return Nombre;
        }

        public ÁlbumViewModel(string id, string nombre)
        {
            Id = id;
            Nombre = nombre ?? "";
        }
    }
}
