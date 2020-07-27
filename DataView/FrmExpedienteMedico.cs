using DataLogic;
using DataModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Transactions;
using System.Windows.Forms;

namespace DataView
{
    public partial class FrmExpedienteMedico : Form
    {
        DLAnimal _dla = new DLAnimal();
        DLExpediente _dlex = new DLExpediente();
        Expediente expediente = new Expediente();
        string resumen = "";
        public FrmExpedienteMedico()
        {
            InitializeComponent();
            LlenarFecha();
            LlenarBox();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Dispose();
            new FrmPrincipal().Show();
        }

        public bool BuscarAnimal()
        {
            Animal _animal = _dla.BuscarAnimal(cboxAnimales.SelectedItem.ToString());
            if (_animal != null)
            {
                string estado = _animal.Estado;
                if (estado == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        public void LlenarFecha()
        {
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        public void LlenarBox() {
            List<Animal> _animal = _dla.CargarAnimales();
            cboxAnimales.Items.Insert(0, "Seleccionar");
            cboxAnimales.SelectedIndex = 0;
            foreach (Animal animal in _animal)
            {
                this.cboxAnimales.Items.Add(animal.CodigoAnimal);
            }
        }

        private void cboxAnimales_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboxAnimales.SelectedIndex != 0)
                {
                    List<Animal> _list = _dla.CargarAnimales();
                    foreach (Animal animal in _list)
                    {
                        if (animal.CodigoAnimal.Equals(cboxAnimales.SelectedItem))
                        {
                            byte[] img = animal.Imagen;
                            MemoryStream ms = new MemoryStream(img);
                            Image imagen = Image.FromStream(ms);
                            pBoxFoto.Image = imagen;
                            if (animal.Estado == "Albergue")
                            {
                                expediente = GetExpediente(animal.CodigoAnimal);
                                if (expediente != null)
                                {
                                    txtPeso.Text = expediente.Peso;
                                    txtFecha.Text = expediente.FechaAtencion.ToString();
                                    txtResumen.Text = expediente.Resumen;
                                    resumen = expediente.Resumen;
                                    if (expediente.Vacuna == "No")
                                    {
                                        rbNo.Checked = true;
                                    }
                                    else {
                                        rbSi.Checked = true;
                                    }
                                }
                                txtPeso.Text = animal.PesoAprox;
                                btnRegistrar.Enabled = true;
                                MessageBox.Show("El animal se encuentra disponible para agregar información", "Información", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                break;
                            }
                            else
                            {
                                btnRegistrar.Enabled = false;
                                MessageBox.Show("El animal NO se encuentra disponible para agregar información", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Debe seleccionar un código", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                Animal animal = _dla.BuscarAnimal(cboxAnimales.SelectedItem.ToString());
                using (TransactionScope scope = new TransactionScope()) {
                    if (animal.Estado == "Albergue")
                    {
                        expediente.Peso = txtPeso.Text;
                        expediente.FechaAtencion = DateTime.Now;
                        expediente.ProcedimientosRealizados = txtProcedimientos.Text;
                        expediente.Resumen = string.Format("{0}{1}:{2} \r\n", resumen , DateTime.Now.ToString(), txtProcedimientos.Text, Environment.NewLine);
                        expediente.CodigoAnimal = cboxAnimales.SelectedItem.ToString();
                        if (rbNo.Checked)
                        {
                            expediente.Vacuna = rbNo.Text;
                        }
                        else {
                            expediente.Vacuna = rbSi.Text;
                        }
                        int resultado = _dlex.AgregarExpediente(expediente);
                        if (resultado == 1)
                        {
                            MessageBox.Show("Registro agregado correctamente", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            scope.Complete();
                            btnRegistrar.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("Error al crear registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Animal animal = _dla.BuscarAnimal(cboxAnimales.SelectedItem.ToString());
        }

        private Expediente GetExpediente(String cod_animal) {
            Expediente ex = _dlex.ObtenerExpediente(cod_animal);
            return ex;
        }
    }
}
