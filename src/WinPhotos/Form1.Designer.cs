namespace WinPhotos
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnSeleccionarÁlbumes = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSeleccionarNuevaImagen = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRecargarAlbumes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSalirGoogle = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGrabar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.clbÁlbumes = new System.Windows.Forms.CheckedListBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "WinPhotos";
            this.notifyIcon1.BalloonTipTitle = "Descarga una imagen de Google Photos y la establece como fondo de pantalla";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "WinPhotos - Establece de wallpaper una imagen de Google Photos";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSeleccionarÁlbumes,
            this.btnSeleccionarNuevaImagen,
            this.btnRecargarAlbumes,
            this.toolStripSeparator2,
            this.btnSalirGoogle,
            this.toolStripSeparator1,
            this.salirToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(339, 209);
            // 
            // btnSeleccionarÁlbumes
            // 
            this.btnSeleccionarÁlbumes.Name = "btnSeleccionarÁlbumes";
            this.btnSeleccionarÁlbumes.Size = new System.Drawing.Size(338, 32);
            this.btnSeleccionarÁlbumes.Text = "Seleccionar álbumes";
            this.btnSeleccionarÁlbumes.Click += new System.EventHandler(this.BtnSeleccionarÁlbumes_Click);
            // 
            // btnSeleccionarNuevaImagen
            // 
            this.btnSeleccionarNuevaImagen.Name = "btnSeleccionarNuevaImagen";
            this.btnSeleccionarNuevaImagen.Size = new System.Drawing.Size(338, 32);
            this.btnSeleccionarNuevaImagen.Text = "Seleccionar una nueva imagen";
            this.btnSeleccionarNuevaImagen.Click += new System.EventHandler(this.BtnSeleccionarNuevaImagen_Click);
            // 
            // btnRecargarAlbumes
            // 
            this.btnRecargarAlbumes.Name = "btnRecargarAlbumes";
            this.btnRecargarAlbumes.Size = new System.Drawing.Size(338, 32);
            this.btnRecargarAlbumes.Text = "Cargar nuevamente los álbumes";
            this.btnRecargarAlbumes.Click += new System.EventHandler(this.BtnRecargarAlbumes_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(335, 6);
            // 
            // btnSalirGoogle
            // 
            this.btnSalirGoogle.Name = "btnSalirGoogle";
            this.btnSalirGoogle.Size = new System.Drawing.Size(338, 32);
            this.btnSalirGoogle.Text = "Cerrar sesión de usuario Google";
            this.btnSalirGoogle.Click += new System.EventHandler(this.BtnSalirGoogle_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(335, 6);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(338, 32);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.SalirToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnGrabar);
            this.panel1.Controls.Add(this.btnCancelar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 249);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(658, 57);
            this.panel1.TabIndex = 2;
            // 
            // btnGrabar
            // 
            this.btnGrabar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGrabar.Location = new System.Drawing.Point(406, 9);
            this.btnGrabar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGrabar.Name = "btnGrabar";
            this.btnGrabar.Size = new System.Drawing.Size(112, 35);
            this.btnGrabar.TabIndex = 0;
            this.btnGrabar.Text = "Grabar";
            this.btnGrabar.UseVisualStyleBackColor = true;
            this.btnGrabar.Click += new System.EventHandler(this.BtnGrabar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Location = new System.Drawing.Point(528, 9);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(112, 35);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.clbÁlbumes);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(658, 249);
            this.panel2.TabIndex = 3;
            // 
            // clbÁlbumes
            // 
            this.clbÁlbumes.ColumnWidth = 200;
            this.clbÁlbumes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbÁlbumes.FormattingEnabled = true;
            this.clbÁlbumes.Location = new System.Drawing.Point(0, 0);
            this.clbÁlbumes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.clbÁlbumes.MultiColumn = true;
            this.clbÁlbumes.Name = "clbÁlbumes";
            this.clbÁlbumes.Size = new System.Drawing.Size(658, 249);
            this.clbÁlbumes.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 306);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Álbumes";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnGrabar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.ToolStripMenuItem btnSeleccionarÁlbumes;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckedListBox clbÁlbumes;
        private System.Windows.Forms.ToolStripMenuItem btnRecargarAlbumes;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem btnSeleccionarNuevaImagen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem btnSalirGoogle;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

