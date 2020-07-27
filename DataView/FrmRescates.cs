using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Transactions;
using DataModel;
using DataLogic;

namespace DataView
{
    public partial class FrmRescates : Form
    {
        Rescate _rescate = new Rescate();
        DLRescate _dlr = new DLRescate();
        DLAnimal _dla = new DLAnimal();
        private List<OpenFileDialog> arrayImag = new List<OpenFileDialog>();

        public FrmRescates()
        {
            InitializeComponent();
            CargarComboAnimales();
            llenarComboEspecie();
            LlenarFecha();
        }


        public void llenarComboEspecie()
        {
            cBoxTipoAnimal.Items.Insert(0, "Seleccionar");
            cBoxTipoAnimal.SelectedIndex = 0;
            cBoxTipoAnimal.Items.Add("Halcon");
            cBoxTipoAnimal.Items.Add("Paloma");
            cBoxTipoAnimal.Items.Add("Ardilla");
            cBoxTipoAnimal.Items.Add("Perro");
            cBoxTipoAnimal.Items.Add("Gato");
            cBoxTipoAnimal.Items.Add("Coballo");
            cBoxTipoAnimal.Items.Add("Garrobo");
            cBoxTipoAnimal.Items.Add("Conejo");
            cBoxTipoAnimal.Items.Add("Tortuga");
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Dispose();
            new FrmPrincipal().Show();
        }

        public void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (VerificarCampos())
                    {
                        _rescate.LugarRescate = txtLugar.Text;
                        _rescate.FechaRescate = Convert.ToDateTime(txtFecha.Text);
                        _rescate.EspecieAnimal = cBoxTipoAnimal.SelectedItem.ToString();
                        _rescate.NombreQuienReporta = txtAlertaAnimal.Text;
                        _rescate.Descripcion = txtDescripcion.Text;
                        _rescate.IDUsuario = 1;
                    }
                    int res = _dlr.AgregarRescate(_rescate);
                    if (res == 1 && BuscarAnimal())
                    {
                        MessageBox.Show("Registro agregado correctamente", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        scope.Complete();
                    }
                    else {
                        MessageBox.Show("Error al crear registro, compruebe que el animal se encuentre registrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Limpiar();
                    errorProvider.Clear();
                    btnRegistrar.Enabled = false;
                    this.cboBoxEstado.Items.Clear();
                }
                insertImage(_dlr.ObtenerIDRescate());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el registro "+ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void insertImage(int pk)
        {
            
            foreach (OpenFileDialog img in this.arrayImag)
            {
                Foto _foto = new Foto();
                FileStream stream = new FileStream(img.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(stream);
                byte[] binData = new byte[stream.Length];
                stream.Read(binData, 0, Convert.ToInt32(stream.Length));
                _foto.IDRescate = pk;
                _foto.Imagen = binData;
                _foto.NombreImg = img.FileName;
                _dlr.AgregarImagenRescate(_foto);
            }
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            OpenFileDialog archivo = new OpenFileDialog();
            DialogResult resultado = archivo.ShowDialog();
            archivo.Filter = "Archivos de imagen|*.jpg;*.png;*.gif;";
            try
            {
                if (archivo.FileName.Equals("") == false)
                {
                    pBoxFoto.Load(archivo.FileName);
                    this.arrayImag.Add(archivo);
                    this.cargarCombo();
                }
            }
            catch
            {
                MessageBox.Show("No se pudo cargar imagen.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cargarCombo()
        {
            try
            {
                this.cboBoxEstado.Items.Clear();
                
                if (this.arrayImag.Count == 0)
                {
                    List<Foto> _fotos = _dlr.ObtenerFotos(cboxAnimales.SelectedItem.ToString());
                    foreach (Foto foto in _fotos)
                    {
                        this.cboBoxEstado.Items.Add(foto.NombreImg);
                    }   
                }
                else
                {
                    foreach (OpenFileDialog img in this.arrayImag)
                    {
                        this.cboBoxEstado.Items.Add(img.SafeFileName);
                    }
                }

            }
            catch
            {
                MessageBox.Show("Error al cargar datos de imágenes","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _rescate.Descripcion = txtDescripcion.Text;
                    int res = _dlr.ModificarRescate(_rescate);
                    if (res == 1)
                    {
                        MessageBox.Show("Registro modificado correctamente", "Modificación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        scope.Complete();
                    }
                    else
                    {
                        MessageBox.Show("Error al modificar registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Limpiar();
                    insertImage(_dlr.ObtenerIDRescate());
                    this.enableData(true);
                    this.Limpiar();
                    scope.Complete();
                }
            }
            catch
            {
                MessageBox.Show("Error al cargar datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void enableData(Boolean status)
        {
            this.txtAlertaAnimal.Enabled = status;
            this.txtLugar.Enabled = status;
            this.txtFecha.Enabled = status;
        }

        private void Limpiar()
        {
            this.txtAlertaAnimal.Text = "";
            this.txtLugar.Text = "";
            this.cBoxTipoAnimal.Text = "";
            this.cboxAnimales.Refresh();
            this.txtDescripcion.Text = "";
            this.cboBoxEstado.Refresh();
        }

        private void CboBoxEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.arrayImag.Count == 0)
            {
                Foto foto = _dlr.ObtenerImagenPorNombre(cboBoxEstado.SelectedItem.ToString());
                {
                    byte[] img = foto.Imagen;
                    MemoryStream ms = new MemoryStream(img);
                    Image imagen = Image.FromStream(ms);
                    pBoxFoto.Image = imagen;
                }
            }
            else
            {
                foreach (OpenFileDialog img in this.arrayImag)
                {
                    if (this.cboBoxEstado.SelectedItem.ToString().Equals(img.SafeFileName))
                    {
                        pBoxFoto.Load(img.FileName);
                    }
                }
            }
        }

        public bool VerificarCampos() {
            bool ok = true;
            if (this.txtLugar.Text.Trim() == "")
            {
                ok = false;
                errorProvider.SetError(txtLugar,"Debe ingresar un lugar");
            }
            if (this.cBoxTipoAnimal.SelectedIndex == 0)
            {
                ok = false;
                errorProvider.SetError(cBoxTipoAnimal, "Debe seleccionar una especie");
            }
            if (this.cboBoxEstado.SelectedIndex == 0)
            {
                ok = false;
                errorProvider.SetError(cboBoxEstado, "Debe subir al menos una imagen");
            }
            if (this.txtDescripcion.Text.Trim() == "")
            {
                ok = false;
                errorProvider.SetError(txtDescripcion, "Debe ingresar una descripción del rescate");
            }
            return ok;
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

        private void cBoxAnimales_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Rescate _resc = _dlr.ConsultarRescate(cboxAnimales.SelectedItem.ToString());
                if (cboxAnimales.SelectedIndex != -1)
                {
                    if (_resc.CodigoAnimal == cboxAnimales.SelectedItem.ToString())
                    {
                        this.txtAlertaAnimal.Text = _resc.NombreQuienReporta;
                        this.cboxAnimales.SelectedItem = _resc.CodigoAnimal;
                        this.txtDescripcion.Text = _resc.Descripcion;
                        this.cBoxTipoAnimal.SelectedItem = _resc.EspecieAnimal;
                        this.txtFecha.Text = _resc.FechaRescate.ToString();
                        this.txtLugar.Text = _resc.LugarRescate;
                        this.enableData(false);
                        this.cargarCombo();
                        this.btnRegistrar.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("No existe expediente de rescate para el animal indicado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LlenarFecha();
                        this.enableData(true);
                        this.btnRegistrar.Enabled = true;
                        Limpiar();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error al cargar datos ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LlenarFecha();
            }
        }

        private void CargarComboAnimales()
        {
            List<Animal> _animal = _dla.CargarAnimales();
            cboxAnimales.Items.Insert(0, "Seleccionar");
            cboxAnimales.SelectedIndex = 0;
            foreach (Animal animal in _animal)
            {
                this.cboxAnimales.Items.Add(animal.CodigoAnimal);
            }
        }

        public void LlenarFecha()
        {
            string fecha = DateTime.Now.ToString("dd/MM/yyyy");
            txtFecha.Text = fecha;
        }
    }

}
