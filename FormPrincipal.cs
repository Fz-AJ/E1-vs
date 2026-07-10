using System;
using System.Windows.Forms;

namespace E1_vs
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private bool InstanciaAbierta(string nombreFormulario)
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child.Name == nombreFormulario)
                {
                    child.WindowState = FormWindowState.Maximized;
                    child.BringToFront();
                    child.Activate();
                    return true;
                }
            }
            return false;
        }

        private void menuArtistas_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = "FormArtistas";
                if (!InstanciaAbierta(nombre))
                {
                    FormArtistas frm = new FormArtistas();
                    frm.Name = nombre;
                    frm.MdiParent = this;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir Artistas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuAlbumes_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = "FormAlbumes";
                if (!InstanciaAbierta(nombre))
                {
                    FormAlbumes frm = new FormAlbumes();
                    frm.Name = nombre;
                    frm.MdiParent = this;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir Álbumes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuCanciones_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = "FormCanciones";
                if (!InstanciaAbierta(nombre))
                {
                    FormCanciones frm = new FormCanciones();
                    frm.Name = nombre;
                    frm.MdiParent = this;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir Canciones: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuSalir_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar la aplicación: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuAyuda_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sistema de Control de Música v1.0\nDiseñado para pruebas de bases de datos relacionales.", "Acerca de", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
