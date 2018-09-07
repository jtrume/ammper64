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
    public partial class viewxml : Form
    {
        public string Identidicadorruta { get; set; }
        public viewxml()
        {
            InitializeComponent();
        }

        private void viewxml_Load(object sender, EventArgs e)
        {
            wb.Navigate(this.Identidicadorruta);
        }
    }
}
