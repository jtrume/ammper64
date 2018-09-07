using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ammper64
{
    public partial class viewpdf : Form
    {
        public string Identidicadorruta { get; set; }
        public viewpdf()
        {
            InitializeComponent();
        }

        private void viewpdf_Load(object sender, EventArgs e)
        {
            //wb.Navigate(this.Identidicadorruta);
            radPdfViewer1.LoadDocument(this.Identidicadorruta);
            //PDFWrapper _pdfDoc = new PDFWrapper();
            //_pdfDoc.LoadPDF(this.Identidicadorruta);

            //System.Diagnostics.Process.Start(this.Identidicadorruta);
        }
    }
}
