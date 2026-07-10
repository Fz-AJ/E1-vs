using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic;

namespace E1_vs
{
    public partial class FormAlbumes : Form
    {
        private string modo = "LECTURA";

        public FormAlbumes()
        {
            InitializeComponent();
        }

        private void FormAlbumes_Load(object sender, EventArgs e)
        {
            CargarComboArtistas();
            CargarGrid();
            CambiarEstadoFormulario("LECTURA");
        }

        private void CambiarEstadoFormulario(string estado)
        {
            modo = estado;
            bool lectura = estado == "LECTURA";
            bool nuevoOmod = estado == "NUEVO" || estado == "MODIFICAR";

            txtTitulo.Enabled = nuevoOmod;
            txtAnio.Enabled = nuevoOmod;
            cmbArtista.Enabled = nuevoOmod;

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
            txtTitulo.Text = string.Empty;
            txtAnio.Text = string.Empty;
            cmbArtista.SelectedIndex = -1;
        }

        private void CargarComboArtistas()
        {
            try
            {
                using (MySqlConnection conn = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT id_artista, nombre FROM artista ORDER BY nombre";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            cmbArtista.DisplayMember = "nombre";
                            cmbArtista.ValueMember = "id_artista";
                            cmbArtista.DataSource = dt;
                            cmbArtista.SelectedIndex = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar artistas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarGrid()
        {
            try
            {
                using (MySqlConnection conn = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT a.id_album AS ID, a.titulo AS Título, a.anio_lanzamiento AS Año, ar.nombre AS Artista FROM albumes a INNER JOIN artista ar ON a.id_artista = ar.id_artista ORDER BY a.id_album";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvAlbumes.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar álbumes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string entrada = Interaction.InputBox("Ingrese ID o título del álbum:", "Buscar Álbum", "");
                if (string.IsNullOrWhiteSpace(entrada))
                {
                    return;
                }

                using (MySqlConnection conn = Conexion.ObtenerConexion())
                {
                    conn.Open();
                    string sql = "SELECT id_album, titulo, anio_lanzamiento, id_artista FROM albumes WHERE id_album = @id OR titulo LIKE @titulo LIMIT 1";
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
                        cmd.Parameters.AddWithValue("@titulo", "%" + entrada + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtId.Text = reader["id_album"].ToString();
                                txtTitulo.Text = reader["titulo"].ToString();
                                txtAnio.Text = reader["anio_lanzamiento"] == DBNull.Value ? string.Empty : reader["anio_lanzamiento"].ToString();
                                cmbArtista.SelectedValue = reader["id_artista"];
                                CambiarEstadoFormulario("LECTURA");
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el álbum.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtTitulo.Focus();
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
                MessageBox.Show("Seleccione un álbum para modificar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            CambiarEstadoFormulario("MODIFICAR");
            txtTitulo.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitulo.Text) || cmbArtista.SelectedIndex < 0)
                {
                    MessageBox.Show("Debe completar el título y seleccionar un artista.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int anio;
                if (!int.TryParse(txtAnio.Text, out anio))
                {
                    anio = 0;
                }

                using (MySqlConnection conn = Conexion.ObtenerConexion())
                {
                    conn.Open();
                    if (modo == "NUEVO")
                    {
                        string sql = "INSERT INTO albumes (titulo, anio_lanzamiento, id_artista) VALUES (@titulo, @anio, @id_artista)";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@titulo", txtTitulo.Text.Trim());
                            if (anio > 0)
                                cmd.Parameters.AddWithValue("@anio", anio);
                            else
                                cmd.Parameters.AddWithValue("@anio", DBNull.Value);
                            cmd.Parameters.AddWithValue("@id_artista", cmbArtista.SelectedValue);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Álbum insertado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (modo == "MODIFICAR")
                    {
                        string sql = "UPDATE albumes SET titulo = @titulo, anio_lanzamiento = @anio, id_artista = @id_artista WHERE id_album = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@titulo", txtTitulo.Text.Trim());
                            if (anio > 0)
                                cmd.Parameters.AddWithValue("@anio", anio);
                            else
                                cmd.Parameters.AddWithValue("@anio", DBNull.Value);
                            cmd.Parameters.AddWithValue("@id_artista", cmbArtista.SelectedValue);
                            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                            int afectados = cmd.ExecuteNonQuery();
                            if (afectados == 0)
                            {
                                MessageBox.Show("No se encontró el álbum para actualizar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        MessageBox.Show("Álbum actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("Seleccione un álbum para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var dr = MessageBox.Show("¿Está seguro de eliminar el álbum seleccionado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (MySqlConnection conn = Conexion.ObtenerConexion())
                    {
                        conn.Open();
                        string sql = "DELETE FROM albumes WHERE id_album = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                            int afectados = cmd.ExecuteNonQuery();
                            if (afectados == 0)
                            {
                                MessageBox.Show("No se encontró el álbum para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }

                    MessageBox.Show("Álbum eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dgvAlbumes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAlbumes.Rows[e.RowIndex];
                txtId.Text = row.Cells["ID"].Value.ToString();
                txtTitulo.Text = row.Cells["Título"].Value.ToString();
                txtAnio.Text = row.Cells["Año"].Value.ToString();
                // buscar el id_artista cargado en grid realizando otra consulta corta
                try
                {
                    using (MySqlConnection conn = Conexion.ObtenerConexion())
                    {
                        conn.Open();
                        string sql = "SELECT id_artista FROM albumes WHERE id_album = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                            object val = cmd.ExecuteScalar();
                            if (val != null && val != DBNull.Value)
                            {
                                cmbArtista.SelectedValue = Convert.ToInt32(val);
                            }
                            else
                            {
                                cmbArtista.SelectedIndex = -1;
                            }
                        }
                    }
                }
                catch
                {
                    cmbArtista.SelectedIndex = -1;
                }

                CambiarEstadoFormulario("LECTURA");
            }
        }
    }
}
