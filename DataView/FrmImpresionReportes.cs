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
        public FrmImpresionReportes()
        {
            InitializeComponent();
        }

        private void FrmImpresionReportes_Load(object sender, EventArgs e)
        {
            this.reportViewer.RefreshReport();
        }
    }
}
