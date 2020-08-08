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
    public partial class FrmConsultaUsuarios : Form
    {
        DLUsuario _dlu = new DLUsuario();
        List<Usuario> _listU = new List<Usuario>();
        public FrmConsultaUsuarios()
        {
            InitializeComponent();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Dispose();
            new FrmUsuarios().Show();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            LlenarGridPorCed(txtCedula.Text);
        }

        public void LlenarGridPorCed(string ced)
        {
            try { 
                GVUsuarios.DataSource = _dlu.BuscarUsuarioCed(txtCedula.Text.Trim());
            }
            catch
            {
                MessageBox.Show("Error al cargar tabla de consulta ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LlenarGridGeneral()
        {
            try
            {
                GVUsuarios.DataSource = _dlu.ConsultarUsuarios();
            }
            catch
            {
                MessageBox.Show("Error al cargar tabla de consulta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbCedula_CheckedChanged(object sender, EventArgs e)
        {
            LlenarGridPorCed(txtCedula.Text);
        }

        private void rbGeneral_CheckedChanged(object sender, EventArgs e)
        {
            LlenarGridGeneral();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < GVUsuarios.Rows.Count-1; i++)
                {
                    Usuario _user = new Usuario();
                    _user.IDUsuario = Convert.ToInt32(GVUsuarios.Rows[i].Cells[0].Value.ToString());
                    _user.NombreCompleto = GVUsuarios.Rows[i].Cells[1].Value.ToString();
                    _user.Username = GVUsuarios.Rows[i].Cells[2].Value.ToString();
                    _user.Correo = GVUsuarios.Rows[i].Cells[3].Value.ToString();
                    _user.Cedula = GVUsuarios.Rows[i].Cells[4].Value.ToString();
                    _listU.Add(_user);
                }
                new FrmImpresionReportes(_listU).Show();
            }
            catch (Exception)
            {
                throw;
            }

            
        }
    }
}
