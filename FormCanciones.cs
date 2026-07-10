using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic;

namespace E1_vs
{
    public partial class FormCanciones : Form
    {
        private string modo = "LECTURA";

        public FormCanciones()
        {
            InitializeComponent();
        }

        private void FormCanciones_Load(object sender, EventArgs e)
        {
            CargarComboAlbumes();
            CargarGrid();
            CambiarEstadoFormulario("LECTURA");
        }

        private void CambiarEstadoFormulario(string estado)
        {
            modo = estado;
            bool lectura = estado == "LECTURA";
            bool nuevoOmod = estado == "NUEVO" || estado == "MODIFICAR";

            txtTitulo.Enabled = nuevoOmod;
            txtDuracion.Enabled = nuevoOmod;
            cmbAlbum.Enabled = nuevoOmod;

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
            txtDuracion.Text = string.Empty;
            cmbAlbum.SelectedIndex = -1;
        }

        private void CargarComboAlbumes()
        {
            try
            {
                using (MySqlConnection conn = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT id_album, titulo FROM albumes ORDER BY titulo";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            cmbAlbum.DisplayMember = "titulo";
                            cmbAlbum.ValueMember = "id_album";
                            cmbAlbum.DataSource = dt;
                            cmbAlbum.SelectedIndex = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar álbumes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarGrid()
        {
            try
            {
                using (MySqlConnection conn = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT c.id_cancion AS ID, c.titulo AS Título, c.duracion AS Duración, a.titulo AS Álbum FROM canciones c INNER JOIN albumes a ON c.id_album = a.id_album ORDER BY c.id_cancion";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgvCanciones.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar canciones: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string entrada = Interaction.InputBox("Ingrese ID o título de la canción:", "Buscar Canción", "");
                if (string.IsNullOrWhiteSpace(entrada))
                {
                    return;
                }

                using (MySqlConnection conn = Conexion.ObtenerConexion())
                {
                    conn.Open();
                    string sql = "SELECT id_cancion, titulo, duracion, id_album FROM canciones WHERE id_cancion = @id OR titulo LIKE @titulo LIMIT 1";
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
                                txtId.Text = reader["id_cancion"].ToString();
                                txtTitulo.Text = reader["titulo"].ToString();
                                txtDuracion.Text = reader["duracion"] == DBNull.Value ? string.Empty : reader["duracion"].ToString();
                                cmbAlbum.SelectedValue = reader["id_album"];
                                CambiarEstadoFormulario("LECTURA");
                            }
                            else
                            {
                                MessageBox.Show("No se encontró la canción.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Seleccione una canción para modificar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            CambiarEstadoFormulario("MODIFICAR");
            txtTitulo.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitulo.Text) || cmbAlbum.SelectedIndex < 0)
                {
                    MessageBox.Show("Debe completar el título y seleccionar un álbum.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                TimeSpan duracion;
                MySqlParameter duracionParam;
                if (TimeSpan.TryParse(txtDuracion.Text, out duracion))
                {
                    duracionParam = new MySqlParameter("@duracion", MySqlDbType.Time);
                    duracionParam.Value = duracion;
                }
                else
                {
                    duracionParam = new MySqlParameter("@duracion", MySqlDbType.Time);
                    duracionParam.Value = DBNull.Value;
                }

                using (MySqlConnection conn = Conexion.ObtenerConexion())
                {
                    conn.Open();
                    if (modo == "NUEVO")
                    {
                        string sql = "INSERT INTO canciones (titulo, duracion, id_album) VALUES (@titulo, @duracion, @id_album)";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@titulo", txtTitulo.Text.Trim());
                            cmd.Parameters.Add(duracionParam);
                            cmd.Parameters.AddWithValue("@id_album", cmbAlbum.SelectedValue);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Canción insertada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (modo == "MODIFICAR")
                    {
                        string sql = "UPDATE canciones SET titulo = @titulo, duracion = @duracion, id_album = @id_album WHERE id_cancion = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@titulo", txtTitulo.Text.Trim());
                            cmd.Parameters.Add(duracionParam);
                            cmd.Parameters.AddWithValue("@id_album", cmbAlbum.SelectedValue);
                            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                            int afectados = cmd.ExecuteNonQuery();
                            if (afectados == 0)
                            {
                                MessageBox.Show("No se encontró la canción para actualizar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        MessageBox.Show("Canción actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("Seleccione una canción para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var dr = MessageBox.Show("¿Está seguro de eliminar la canción seleccionada?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (MySqlConnection conn = Conexion.ObtenerConexion())
                    {
                        conn.Open();
                        string sql = "DELETE FROM canciones WHERE id_cancion = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                            int afectados = cmd.ExecuteNonQuery();
                            if (afectados == 0)
                            {
                                MessageBox.Show("No se encontró la canción para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }

                    MessageBox.Show("Canción eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dgvCanciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCanciones.Rows[e.RowIndex];
                txtId.Text = row.Cells["ID"].Value.ToString();
                txtTitulo.Text = row.Cells["Título"].Value.ToString();
                txtDuracion.Text = row.Cells["Duración"].Value.ToString();
                // obtener id_album mediante consulta
                try
                {
                    using (MySqlConnection conn = Conexion.ObtenerConexion())
                    {
                        conn.Open();
                        string sql = "SELECT id_album FROM canciones WHERE id_cancion = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtId.Text));
                            object val = cmd.ExecuteScalar();
                            if (val != null && val != DBNull.Value)
                            {
                                cmbAlbum.SelectedValue = Convert.ToInt32(val);
                            }
                            else
                            {
                                cmbAlbum.SelectedIndex = -1;
                            }
                        }
                    }
                }
                catch
                {
                    cmbAlbum.SelectedIndex = -1;
                }

                CambiarEstadoFormulario("LECTURA");
            }
        }
    }
}
