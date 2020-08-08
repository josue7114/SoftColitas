using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataView
{
    public partial class FrmReporteUsuarios : Form
    {
        List<Object> _list = new List<Object>();
        String _type = "";
        public FrmReporteUsuarios(String type, List<Object> list)
        {
            InitializeComponent();
            _list = list;
            _type = type;
        }

        private void CargarReporte()
        {
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource(_type, _list));
            this.reportViewer1.RefreshReport();
        }
        private void FrmReporteUsuarios_Load(object sender, EventArgs e)
        {
            CargarReporte();
        }
    }
}
