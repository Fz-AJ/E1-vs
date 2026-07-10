namespace E1_vs
{
    partial class FormPrincipal
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuArtistas;
        private System.Windows.Forms.ToolStripMenuItem menuAlbumes;
        private System.Windows.Forms.ToolStripMenuItem menuCanciones;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuSalir;
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuAyuda;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            archivoToolStripMenuItem = new ToolStripMenuItem();
            abrirToolStripMenuItem = new ToolStripMenuItem();
            menuArtistas = new ToolStripMenuItem();
            menuAlbumes = new ToolStripMenuItem();
            menuCanciones = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            menuSalir = new ToolStripMenuItem();
            ayudaToolStripMenuItem = new ToolStripMenuItem();
            menuAyuda = new ToolStripMenuItem();
            archivoToolStripMenuItem1 = new ToolStripMenuItem();
            abrirToolStripMenuItem1 = new ToolStripMenuItem();
            artistasToolStripMenuItem = new ToolStripMenuItem();
            albumesToolStripMenuItem = new ToolStripMenuItem();
            cancionesToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            salirToolStripMenuItem = new ToolStripMenuItem();
            ayudaToolStripMenuItem1 = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { archivoToolStripMenuItem, ayudaToolStripMenuItem, archivoToolStripMenuItem1 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.ItemClicked += menuStrip1_ItemClicked;
            // 
            // archivoToolStripMenuItem
            // 
            archivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { abrirToolStripMenuItem, toolStripSeparator1, menuSalir });
            archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            archivoToolStripMenuItem.Size = new Size(60, 20);
            archivoToolStripMenuItem.Text = "Archivo";
            // 
            // abrirToolStripMenuItem
            // 
            abrirToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { menuArtistas, menuAlbumes, menuCanciones });
            abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            abrirToolStripMenuItem.Size = new Size(100, 22);
            abrirToolStripMenuItem.Text = "Abrir";
            // 
            // menuArtistas
            // 
            menuArtistas.Name = "menuArtistas";
            menuArtistas.Size = new Size(129, 22);
            menuArtistas.Text = "Artistas";
            menuArtistas.Click += menuArtistas_Click;
            // 
            // menuAlbumes
            // 
            menuAlbumes.Name = "menuAlbumes";
            menuAlbumes.Size = new Size(129, 22);
            menuAlbumes.Text = "Álbumes";
            menuAlbumes.Click += menuAlbumes_Click;
            // 
            // menuCanciones
            // 
            menuCanciones.Name = "menuCanciones";
            menuCanciones.Size = new Size(129, 22);
            menuCanciones.Text = "Canciones";
            menuCanciones.Click += menuCanciones_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(97, 6);
            // 
            // menuSalir
            // 
            menuSalir.Name = "menuSalir";
            menuSalir.Size = new Size(100, 22);
            menuSalir.Text = "Salir";
            menuSalir.Click += menuSalir_Click;
            // 
            // ayudaToolStripMenuItem
            // 
            ayudaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { menuAyuda });
            ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            ayudaToolStripMenuItem.Size = new Size(53, 20);
            ayudaToolStripMenuItem.Text = "Ayuda";
            // 
            // menuAyuda
            // 
            menuAyuda.Name = "menuAyuda";
            menuAyuda.Size = new Size(135, 22);
            menuAyuda.Text = "Acerca de...";
            menuAyuda.Click += menuAyuda_Click;
            // 
            // archivoToolStripMenuItem1
            // 
            archivoToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { abrirToolStripMenuItem1, artistasToolStripMenuItem, albumesToolStripMenuItem, cancionesToolStripMenuItem, toolStripMenuItem2, salirToolStripMenuItem, ayudaToolStripMenuItem1 });
            archivoToolStripMenuItem1.Name = "archivoToolStripMenuItem1";
            archivoToolStripMenuItem1.Size = new Size(60, 20);
            archivoToolStripMenuItem1.Text = "Archivo";
            // 
            // abrirToolStripMenuItem1
            // 
            abrirToolStripMenuItem1.Name = "abrirToolStripMenuItem1";
            abrirToolStripMenuItem1.Size = new Size(209, 22);
            abrirToolStripMenuItem1.Text = "Abrir:";
            // 
            // artistasToolStripMenuItem
            // 
            artistasToolStripMenuItem.Name = "artistasToolStripMenuItem";
            artistasToolStripMenuItem.Size = new Size(209, 22);
            artistasToolStripMenuItem.Text = "Artistas";
            // 
            // albumesToolStripMenuItem
            // 
            albumesToolStripMenuItem.Name = "albumesToolStripMenuItem";
            albumesToolStripMenuItem.Size = new Size(209, 22);
            albumesToolStripMenuItem.Text = "Albumes";
            // 
            // cancionesToolStripMenuItem
            // 
            cancionesToolStripMenuItem.Name = "cancionesToolStripMenuItem";
            cancionesToolStripMenuItem.Size = new Size(209, 22);
            cancionesToolStripMenuItem.Text = "Canciones";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(209, 22);
            toolStripMenuItem2.Text = "---------------------------";
            // 
            // salirToolStripMenuItem
            // 
            salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            salirToolStripMenuItem.Size = new Size(209, 22);
            salirToolStripMenuItem.Text = "Salir";
            // 
            // ayudaToolStripMenuItem1
            // 
            ayudaToolStripMenuItem1.Name = "ayudaToolStripMenuItem1";
            ayudaToolStripMenuItem1.Size = new Size(209, 22);
            ayudaToolStripMenuItem1.Text = "Ayuda...";
            // 
            // FormPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "FormPrincipal";
            Text = "Sistema de Gestión Musical";
            WindowState = FormWindowState.Maximized;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStripMenuItem archivoToolStripMenuItem1;
        private ToolStripMenuItem abrirToolStripMenuItem1;
        private ToolStripMenuItem artistasToolStripMenuItem;
        private ToolStripMenuItem albumesToolStripMenuItem;
        private ToolStripMenuItem cancionesToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem salirToolStripMenuItem;
        private ToolStripMenuItem ayudaToolStripMenuItem1;
    }
}
