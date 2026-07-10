using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic;

namespace E1_vs
{
    public partial class FormArtistas : Form
    {
        private string modo = "LECTURA";

        public FormArtistas()
        {
            InitializeComponent();
        }

        private void FormArtistas_Load(object sender, EventArgs e)
        {
            CambiarEstadoFormulario("LECTURA");
            CargarGrid();
        }

        private void CambiarEstadoFormulario(string estado)
        {
            modo = estado;
            bool lectura = estado == "LECTURA";
            bool nuevoOmod = estado == "NUEVO" || estado == "MODIFICAR";

            txtNombre.Enabled = nuevoOmod;
            cmbTipo.Enabled = nuevoOmod;

            btnNuevo.Enabled = lectura;
            btnBuscar.Enabled = lectura;

            bool tieneId = !string.IsNullOrWhiteSpace(txtId.Text);
            btnModificar.Enabled = lectura && tieneId;
            btnEliminar.Enabled = lectura && tieneId;

            btnGuardar.Enabled = nuevoOmod;
            btnCancelar.Enabled = nuevoOmod;
        }

        private void LimpiarCampos()
        {
            txtId.Text = string.Empty;
            txtNombre.Text = string.Empty;
            cmbTipo.SelectedIndex = -1;
        }

        private void CargarGrid()
        {
            try
            {
                using (MySqlConnection conn = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT id_artista AS ID, nombre AS Nombre, tipo AS Tipo FROM artista ORDER BY id_artista";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvArtistas.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar artistas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string entrada = Interaction.InputBox("Ingrese ID o nombre del artista:", "Buscar Artista", "");
                if (string.IsNullOrWhiteSpace(entrada))
                {
                    return;
                }

                using (MySqlConnection conn = Conexion.ObtenerConexion())
                {
                    conn.Open();
                    string sql = "SELECT id_artista, nombre, tipo FROM artista WHERE id_artista = @id OR nombre LIKE @nombre LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        int id;
                        if (int.TryParse(entrada, out id))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@id", DBNull.Value);
                        }
                        cmd.Parameters.AddWithValue("@nombre", "%" + entrada + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtId.Text = reader["id_artista"].ToString();
                                txtNombre.Text = reader["nombre"].ToString();
                                cmbTipo.SelectedItem = reader["tipo"].ToString();
                                CambiarEstadoFormulario("LECTURA");
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el artista.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LimpiarCampos();
                                CambiarEstadoFormulario("LECTURA");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en búsqueda: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            CambiarEstadoFormulario("NUEVO");
            txtNombre.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            CambiarEstadoFormulario("LECTURA");
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Seleccione un artista para modificar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            CambiarEstadoFormulario("MODIFICAR");
            txtNombre.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text) || cmbTipo.SelectedIndex < 0)
                {
                    MessageBox.Show("Debe completar el nombre y seleccionar el tipo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (MySqlConnection conn = Conexion.ObtenerConexion())
                {
                    conn.Open();
                    if (modo == "NUEVO")
                    {
                        string sql = "INSERT INTO artista (nombre, tipo) VALUES (@nombre, @tipo)";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                            cmd.Parameters.AddWithValue("@tipo", cmbTipo.SelectedItem.ToString());
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Artista insertado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (modo == "MODIFICAR")
                    {
                        string sql = "UPDATE artista SET nombre = @nombre, tipo = @tipo WHERE id_artista = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                            cmd.Parameters.AddWithValue("@tipo", cmbTipo.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                            int afectados = cmd.ExecuteNonQuery();
                            if (afectados == 0)
                            {
                                MessageBox.Show("No se encontró el artista para actualizar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        MessageBox.Show("Artista actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                LimpiarCampos();
                CargarGrid();
                CambiarEstadoFormulario("LECTURA");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    MessageBox.Show("Seleccione un artista para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var dr = MessageBox.Show("¿Está seguro de eliminar el artista seleccionado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (MySqlConnection conn = Conexion.ObtenerConexion())
                    {
                        conn.Open();
                        string sql = "DELETE FROM artista WHERE id_artista = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                            int afectados = cmd.ExecuteNonQuery();
                            if (afectados == 0)
                            {
                                MessageBox.Show("No se encontró el artista para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }

                    MessageBox.Show("Artista eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarGrid();
                    CambiarEstadoFormulario("LECTURA");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvArtistas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvArtistas.Rows[e.RowIndex];
                txtId.Text = row.Cells["ID"].Value.ToString();
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                cmbTipo.SelectedItem = row.Cells["Tipo"].Value.ToString();
                CambiarEstadoFormulario("LECTURA");
            }
        }
    }
}
