using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowInTaskbar = false;
            Hide();
            Procesar();
        }

        private async Task Procesar()
        {
            (bool descarga, List<String> ids) = await new ChangeWallpaperController().DescargarListaFotos();
            if (descarga) _idFotos = ids;

            CrearNuevoTokenDeRotacion();
            Task.Run(async () => { await RotarFondosPantalla(); }, _globalToken.Token);
        }

        private async Task RotarFondosPantalla()
        {
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
                        TimeSpan.FromSeconds(15),
                        tkn
                    );
                }
                catch (TaskCanceledException)
                {
                    Log.Debug("Tarea cancelada.");
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

        private async void btnSeleccionarÁlbumes_Click(object sender, EventArgs e)
        {
            var ctrlr = new ConfigurationController();
            clbÁlbumes.Items.Clear();
            var alb = from a in await ctrlr.ListarÁlbumes()
                      where !String.IsNullOrEmpty(a.Title)
                      orderby a.Title
                      select new Álbum(a.Id, a.Title);
            clbÁlbumes.Items.AddRange(alb.ToArray());
            var ids = ctrlr.GetÁlbumesId();
            for (int i = 0; i < clbÁlbumes.Items.Count; i++)
            {
                var item = (Álbum)clbÁlbumes.Items[i];
                if (ids.Contains(item.Id))
                {
                    clbÁlbumes.SetItemChecked(i, true);
                }
            }
            Show();
            ShowInTaskbar = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            var tmp = new List<Álbum>();
            foreach (var item in clbÁlbumes.CheckedItems)
            {
                tmp.Add((Álbum)item);
            }
            new ConfigurationController().ActualizarÁlbumes(tmp);
            Hide();
            btnRecargarAlbumes.PerformClick();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _globalToken.Cancel();
        }

        private void btnSeleccionarNuevaImagen_Click(object sender, EventArgs e)
        {
            Log.Debug("Seleccionando una nueva imagen");
            _cancellationTokenSource.Cancel();
        }

        private void btnSalirGoogle_Click(object sender, EventArgs e)
        {
            _globalToken.Cancel();
            PhotosLibrary.CerrarSesiónUsuarioGoogle();
            MessageBox.Show("Se cerró la sesión del usuario.", "Cerrar sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnSeleccionarÁlbumes.PerformClick();
        }

        private async void btnRecargarAlbumes_Click(object sender, EventArgs e)
        {
            _globalToken.Cancel();
            _idFotos.Clear();
            Procesar();
        }
    }
}
