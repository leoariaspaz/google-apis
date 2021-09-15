using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinPhotos.Controllers;
using WinPhotos.Lib;
using WinPhotos.Models;

namespace WinPhotos
{
    public partial class Form1 : Form
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<string> _idFotos;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationTokenSource _globalToken = new CancellationTokenSource();

        public Form1()
        {
            InitializeComponent();
        }

        private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            ShowInTaskbar = false;
            Hide();
            await new PhotosProxy().CrearServicio();
            Procesar();
        }

        private async void Procesar()
        {
            (bool descarga, List<String> ids) = await new ChangeWallpaperController().DescargarListaFotos();
            if (descarga) _idFotos = ids;

            CrearNuevoTokenDeRotacion();
            _ = Task.Run(async () => { await RotarFondosPantalla(); }, _globalToken.Token);
        }

        private async Task RotarFondosPantalla()
        {
            var sleepTime = Convert.ToInt32(ConfigurationManager.AppSettings["SleepTime"]);
            while (true)
            {
                try
                {
                    var tkn = _cancellationTokenSource.Token;

                    await Task.Run(
                        async () => { await new ChangeWallpaperController().ChangeWallpaper(_idFotos); },
                        tkn
                    );

                    if (tkn.IsCancellationRequested)
                    {
                        tkn = CrearNuevoTokenDeRotacion();
                    }

                    await Task.Delay(
                        TimeSpan.FromMinutes(sleepTime),
                        tkn
                    );
                }
                catch (TaskCanceledException)
                {
                    Log.Debug("La rotación de fondos de pantalla ha sido cancelada.");
                    CrearNuevoTokenDeRotacion();
                }
            }
        }

        private CancellationToken CrearNuevoTokenDeRotacion()
        {
            CancellationToken tkn;
            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            tkn = _cancellationTokenSource.Token;
            return tkn;
        }

        private async void BtnSeleccionarÁlbumes_Click(object sender, EventArgs e)
        {
            var ctrlr = new ConfigurationController();
            var alb = from a in await new PhotosProxy().ListarÁlbumes()
                      where !String.IsNullOrEmpty(a.Title)
                      orderby a.Title
                      select new ÁlbumViewModel(a.Id, a.Title);
            clbÁlbumes.Items.Clear();
            clbÁlbumes.Items.AddRange(alb.ToArray());
            var ids = ctrlr.GetÁlbumesId();
            for (int i = 0; i < clbÁlbumes.Items.Count; i++)
            {
                var item = (ÁlbumViewModel)clbÁlbumes.Items[i];
                if (ids.Contains(item.Id))
                {
                    clbÁlbumes.SetItemChecked(i, true);
                }
            }

            Show();
            ShowInTaskbar = true;
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            var tmp = new List<ÁlbumViewModel>();
            foreach (var item in clbÁlbumes.CheckedItems)
            {
                tmp.Add((ÁlbumViewModel)item);
            }
            new ConfigurationController().ActualizarÁlbumes(tmp);
            Hide();
            btnRecargarAlbumes.PerformClick();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _globalToken.Cancel();
        }

        private void BtnSeleccionarNuevaImagen_Click(object sender, EventArgs e)
        {
            Log.Debug("Seleccionando una nueva imagen");
            _cancellationTokenSource.Cancel();
        }

        private void BtnSalirGoogle_Click(object sender, EventArgs e)
        {
            _globalToken.Cancel();
            new PhotosProxy().CerrarSesiónUsuarioGoogle();
            MessageBox.Show("Se cerró la sesión del usuario.", "Cerrar sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnSeleccionarÁlbumes.PerformClick();
        }

        private void BtnRecargarAlbumes_Click(object sender, EventArgs e)
        {
            _globalToken.Cancel();
            _idFotos.Clear();
            Procesar();
        }
    }
}
