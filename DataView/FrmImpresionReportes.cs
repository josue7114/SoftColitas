using DataModel;
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
    public partial class FrmImpresionReportes : Form
    {
        List<Usuario> _list = new List<Usuario>();
        public FrmImpresionReportes(List<Usuario> list)
        {
            InitializeComponent();
            _list = list;
        }

        private void FrmImpresionReportes_Load(object sender, EventArgs e)
        {
            CargarReporte();
        }

        private void CargarReporte()
        {
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetUsuarios", _list));
            this.reportViewer.RefreshReport();
        }
    }
}
