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
        private Thread _thread;
        private ChangeWallpaperController _changeWallpaperController;

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
            Visible = false;
            _changeWallpaperController = new ChangeWallpaperController();
            _thread = new Thread(new ThreadStart(_changeWallpaperController.CambiarFondoPantalla));
            _thread.Start();
        }

        private void btnSeleccionarÁlbumes_Click(object sender, EventArgs e)
        {
            Show();
            var ctrlr = new ConfigurationController();
            clbÁlbumes.Items.Clear();
            var alb = from a in ctrlr.ListarÁlbumes()
                      where !String.IsNullOrEmpty(a.Title)
                      orderby a.Title
                      select new Álbum(a.Id, a.Title);
            clbÁlbumes.Items.AddRange(alb.ToArray());
            var ids = ctrlr.GetÁlbumesId();
            if (!ids.Any())
            {
                return;
            }
            for (int i = 0; i < clbÁlbumes.Items.Count; i++)
            {
                var item = (Álbum)clbÁlbumes.Items[i];
                if (ids.Contains(item.Id))
                {
                    clbÁlbumes.SetItemChecked(i, true);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            //Close();
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
            //Close();
            Hide();
            btnReiniciarElProceso.PerformClick();
        }

        private void btnReiniciarElProceso_Click(object sender, EventArgs e)
        {
            _changeWallpaperController.RequestStop();
            _thread.Interrupt();
            _thread.Join();
            _thread = new Thread(new ThreadStart(_changeWallpaperController.CambiarFondoPantalla));
            _thread.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _changeWallpaperController.RequestStop();
            _thread.Interrupt();
            _thread.Join();
        }

        private void btnSeleccionarNuevaImagen_Click(object sender, EventArgs e)
        {
            Log.Debug("Seleccionando una nueva imagen");
            _thread.Interrupt();
        }

        private void btnSalirGoogle_Click(object sender, EventArgs e)
        {
            _changeWallpaperController.RequestStop();
            PhotosLibrary.CerrarSesiónUsuarioGoogle();
            MessageBox.Show("Se cerró la sesión del usuario.", "Cerrar sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnSeleccionarÁlbumes.PerformClick();
        }
    }
}
