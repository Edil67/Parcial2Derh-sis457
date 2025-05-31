using CadParcial2Derh;
using ClnParcialDerh;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpParcial2Derh
{
    public partial class FrmPrincipalSeries : Form
    {
        private bool esNuevo = false;
        public FrmPrincipalSeries()
        {
            InitializeComponent();
        }

        public void listar()
        {
            var lista = SerieCln.listarPa(txtParametro.Text.Trim());
            dgvListaSerie.DataSource = lista;
            dgvListaSerie.Columns["id"].Visible = false;
            dgvListaSerie.Columns["estado"].Visible = false;
            dgvListaSerie.Columns["titulo"].HeaderText = "TÍTULO";
            dgvListaSerie.Columns["sinopsis"].HeaderText = "SINOPSIS";
            dgvListaSerie.Columns["director"].HeaderText = "DIRECTOR";
            dgvListaSerie.Columns["episodios"].HeaderText = "EPISODIO";
            dgvListaSerie.Columns["fechaEstreno"].HeaderText = "FECHA DE ESTRENO";
            dgvListaSerie.Columns["usuarioRegistro"].HeaderText = "USUARIO REGISTRO";
            dgvListaSerie.Columns["fechaRegistro"].HeaderText = "FECHA DE REGISTRO";

            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;

            if (lista.Count > 0) dgvListaSerie.CurrentCell = dgvListaSerie.Rows[0].Cells["titulo"];
        }


        private void FrmPrincipalSeries_Load(object sender, EventArgs e)
        {
            Size = new Size(835, 363);
            listar();
        }

        private void limpiar()
        {
            txtTitulo.Clear();
            txtSinopsis.Clear();
            txtDirector.Clear();
            nudEpisodio.Value = 1;
            dtpFechaEstreno.Value = DateTime.Now;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Size = new Size(835, 363);
            limpiar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(835, 487);
            txtTitulo.Focus();
            limpiar();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            Size = new Size(835, 487);

            int index = dgvListaSerie.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvListaSerie.Rows[index].Cells["id"].Value);
            var serie = SerieCln.obtenerUno(id);
            txtTitulo.Text = serie.titulo;
            txtSinopsis.Text = serie.sinopsis;
            txtDirector.Text = serie.director;
            nudEpisodio.Value = serie.episodios;
            dtpFechaEstreno.Value = serie.fechaEstreno.HasValue ? serie.fechaEstreno.Value : DateTime.Now;
            txtTitulo.Focus();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private bool validar()
        {
            bool esValido = true;
            erpTitulo.SetError(txtTitulo, "");
            erpSinopsis.SetError(txtSinopsis, "");
            erpDirector.SetError(txtDirector, "");
            erpEpisodio.SetError(nudEpisodio, "");
            erpFechaEstreno.SetError(dtpFechaEstreno, "");

            if (string.IsNullOrEmpty(txtTitulo.Text))
            {
                esValido = false;
                erpTitulo.SetError(txtTitulo, "El campo Titulo es obligatorio");
            }
            if (string.IsNullOrEmpty(txtSinopsis.Text))
            {
                esValido = false;
                erpSinopsis.SetError(txtSinopsis, "El campo Sinopsis es obligatorio");
            }
            if (string.IsNullOrEmpty(txtDirector.Text))
            {
                esValido = false;
                erpDirector.SetError(txtDirector, "El campo Director es obligatorio");
            }
            if (nudEpisodio.Value < 0)
            {
                esValido = false;
                erpEpisodio.SetError(nudEpisodio, "El campo Episodios no debe ser negativo");
            }
            if (dtpFechaEstreno.Value > DateTime.Now)
            {
                esValido = false;
                erpFechaEstreno.SetError(dtpFechaEstreno, "La fecha de estreno no puede ser futura");
            }
            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var serie = new Serie();
                serie.titulo = txtTitulo.Text.Trim();
                serie.sinopsis = txtSinopsis.Text.Trim();
                serie.director = txtDirector.Text.Trim();
                serie.episodios = (int)nudEpisodio.Value;
                serie.fechaEstreno = dtpFechaEstreno.Value;
                serie.usuarioRegistro = "admin";

                if (esNuevo)
                {
                    serie.fechaRegistro = DateTime.Now;
                    serie.estado = 1;
                    SerieCln.insertar(serie);
                }
                else
                {
                    int index = dgvListaSerie.CurrentCell.RowIndex;
                    serie.id = Convert.ToInt32(dgvListaSerie.Rows[index].Cells["id"].Value);
                    SerieCln.actualizar(serie);
                }
                listar();
                btnCancelar.PerformClick();
                MessageBox.Show("Serie guardado correctamente", "::: Parcial2Derh - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int index = dgvListaSerie.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvListaSerie.Rows[index].Cells["id"].Value);
            string titulo = dgvListaSerie.Rows[index].Cells["titulo"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"¿Está seguro de eliminar la serie {titulo}?",
                "::: Minerva - Mensaje :::", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                SerieCln.eliminar(id, "admin");
                listar();
                MessageBox.Show("Producto dado de baja correctamente", "::: Minerva - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtParametro.Text = string.Empty;
            listar();
        }
    }
}
