using DataLogic;
using DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace DataView
{
    public partial class FrmReportes : Form
    {
        DLReporte _dlr = new DLReporte();
        List<Animal> _listA = new List<Animal>();
        public FrmReportes()
        {
            InitializeComponent();
            LlenarBoxEspecies();
        }

        public void LlenarBoxEspecies()
        {
            cBoxTipoAnimal.Items.Insert(0, "Seleccionar");
            cBoxTipoAnimal.SelectedIndex = 0;
            cBoxTipoAnimal.Items.Add("Perro");
            cBoxTipoAnimal.Items.Add("Gato");
            cBoxTipoAnimal.Items.Add("Halcón");
            cBoxTipoAnimal.Items.Add("Coballo");
            cBoxTipoAnimal.Items.Add("Paloma");
            cBoxTipoAnimal.Items.Add("Tortuga");
            cBoxTipoAnimal.Items.Add("Garrobo");
            cBoxTipoAnimal.Items.Add("Conejo");
            cBoxTipoAnimal.Items.Add("Ardilla");
            cBoxTipoAnimal.Items.Add("Otro");
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Dispose();
            new FrmPrincipal().Show();
        }

        private void rbCodigo_CheckedChanged(object sender, EventArgs e)
        {
            LlenarGridPorCod(txtCodigo.Text);
        }

        private void rbFechaIngreso_CheckedChanged(object sender, EventArgs e)
        {
            LlenarGridPorFecha();
        }

        private void rbTipoAnimal_CheckedChanged(object sender, EventArgs e)
        {
            LlenarGridPorEspecie();
        }

        private void rbConsultaGeneral_CheckedChanged(object sender, EventArgs e)
        {
            LlenarGridGeneral();
        }

        private void LlenarGridPorCod(string cod)
        {
            
            try
            {
                
                GVReportes.DataSource = _dlr.LlenarGridPorCod(txtCodigo.Text);
                GVReportes.Refresh();
                
            }
            catch (Exception)
            {
                MessageBox.Show("Error al cargar tabla de consulta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlenarGridPorFecha()
        {
            
            try
            {
                GVReportes.DataSource = _dlr.LlenarGridPorFecha();
                GVReportes.Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al cargar tabla de consulta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlenarGridPorEspecie()
        {
            
            try
            {
                GVReportes.DataSource = _dlr.LlenarGridPorEspecie(cBoxTipoAnimal.SelectedItem.ToString());
                GVReportes.Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al cargar tabla de consulta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlenarGridGeneral()
        {
            
            try
            {
                GVReportes.DataSource = _dlr.LlenarGridGeneral();
                GVReportes.Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al cargar tabla de consulta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < GVReportes.Rows.Count; i++)
                {
                    Animal _animal = new Animal();
                    _animal.CodigoAnimal= GVReportes.Rows[i].Cells[0].Value.ToString();
                    _animal.Imagen= (byte []) GVReportes.Rows[i].Cells[1].Value;
                    _animal.Tamano = GVReportes.Rows[i].Cells[2].Value.ToString();
                    _animal.EdadAprox = GVReportes.Rows[i].Cells[3].Value.ToString();
                    _animal.PesoAprox = GVReportes.Rows[i].Cells[4].Value.ToString();
                    _animal.Color = GVReportes.Rows[i].Cells[5].Value.ToString();
                    _animal.Estado = GVReportes.Rows[i].Cells[6].Value.ToString();
                    _animal.Descripcion = GVReportes.Rows[i].Cells[7].Value.ToString();
                    _animal.Especie = GVReportes.Rows[i].Cells[8].Value.ToString();
                    _animal.FechaIngreso = Convert.ToDateTime(GVReportes.Rows[i].Cells[9].Value.ToString());
                    _animal.IDUsuario = Convert.ToInt32(GVReportes.Rows[i].Cells[10].Value.ToString());
                    _listA.Add(_animal);
                }
                new FrmImpresionReportes("DataSetAnimales", _listA.Cast<Object>().ToList()).Show();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
