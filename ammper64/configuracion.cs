using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ammper64
{
    public partial class configuracion : Form
    {
        Boolean newpari = false;
        public string zcadena = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public configuracion()
        {
            InitializeComponent();
        }

        private void configuracion_Load(object sender, EventArgs e)
        {
            acceso acesm = new acceso();
            DataTable dtm = new DataTable();
            dtm = acesm.Conectar("Select rfc from facturador order by id");

            foreach (DataRow rowm in dtm.Rows)
            {
                string valorm = rowm[0].ToString();
                mparti.Items.Add(valorm);
            }
            mparti.SelectedIndex = 0;
            trfc.Text = mparti.Text;


            cargaparti(mparti.Text);

            //cargasub(mparti.Text);


            //part.Text = parti.Text;
        }

        public void cargasub(string rfc)
        {
            parti.Items.Clear();
            acceso aces = new acceso();
            DataTable dt = new DataTable();
            dt = aces.Conectar("Select participante from participante where prfc = '" + rfc + "' order by id");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string valor = row[0].ToString();
                    parti.Items.Add(valor);
                }

                parti.ItemHeight = 20;

                parti.SelectedIndex = 0;

                
            }
            cargaSubdet(parti.Text);
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Boolean isprog = false;
            int xtprograma = 0;
            string xvaldiassemana = string.Empty;

            DateTime xfecha = Convert.ToDateTime(rdhora.Value);

            string xrdhora = xfecha.ToString("HH:mm");

            if (rC1.Checked == true) { xvaldiassemana = xvaldiassemana + "1"; }
            if (rC2.Checked == true) { xvaldiassemana = xvaldiassemana + ",2"; }
            if (rC3.Checked == true) { xvaldiassemana = xvaldiassemana + ",3"; }
            if (rC4.Checked == true) { xvaldiassemana = xvaldiassemana + ",4"; }
            if (rC5.Checked == true) { xvaldiassemana = xvaldiassemana + ",5"; }
            if (rC6.Checked == true) { xvaldiassemana = xvaldiassemana + ",6"; }
            if (rC7.Checked == true) { xvaldiassemana = xvaldiassemana + ",7"; }
            string Xdprograms = dprograms.Text;
            if (tprograma.Checked)
            {
                xtprograma = 0;
            }
            else
            {
                xtprograma = 1;
            }

            if (rprogramado.Checked == true)
            {
                isprog = true;
            }
            else
            {
                isprog = false;
            }

            string sql = string.Empty;
            sql = " UPDATE PROGRAMA SET TPROGRAMA=@TPROGRAMA,XDIASEMANA=@XDIASEMANA,XDIAS=@XDIAS,HORA=@HORA,PROGRAMADO=@PROGRAMADO,PMASMENOS=@PMASMENOS";
            sql += " WHERE ID = 1";
            SqlConnection cadena = new SqlConnection(zcadena);
            cadena.Open();
            SqlCommand cmd = cadena.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@TPROGRAMA", xtprograma);
            cmd.Parameters.AddWithValue("@XDIASEMANA", xvaldiassemana);
            cmd.Parameters.AddWithValue("@XDIAS", Xdprograms);
            cmd.Parameters.AddWithValue("@HORA", xrdhora);
            cmd.Parameters.AddWithValue("@PROGRAMADO", isprog);
            cmd.Parameters.AddWithValue("@PMASMENOS", tmasmenos.Text);
            cmd.ExecuteNonQuery();
            cadena.Close();
            activavarhoras();
        }

        private void mparti_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargasub(mparti.Text);
        }

        private void cargaparti(string rfc)
        {
            acceso aces = new acceso();
            DataTable dt2 = new DataTable();
            dt2 = aces.Conectar("Select * from facturador where rfc = '" + rfc + "'");
            if (dt2.Rows.Count > 0)
            {
                txtid.Text = Convert.ToString(dt2.Rows[0].Field<int>("id"));
                txtrfc.Text = dt2.Rows[0].Field<string>("rfc");
                txtnombre.Text = dt2.Rows[0].Field<string>("nombre");
                txtregimen.Text = dt2.Rows[0].Field<string>("regimenf");
                txtusuario.Text = dt2.Rows[0].Field<string>("usuario");
                txtpass.Text = dt2.Rows[0].Field<string>("contraseña");
                txt_ruta.Text = dt2.Rows[0].Field<string>("ruta");
                txt_lugar.Text = dt2.Rows[0].Field<string>("lugarexpedicion");
                ckprod.Checked = dt2.Rows[0].Field<Boolean>("productivo");
                chpartiactivo.Checked = dt2.Rows[0].Field<Boolean>("ACTIVO");
                fb.SelectedPath = txt_ruta.Text;
            }
            cargasub(mparti.Text);
        }

        public void cargaSubdet(string parte)
        {
            acceso aces = new acceso();
            DataTable dt = new DataTable();
            dt = aces.Conectar("Select * from participante where participante = '" + parte + "'");
            if (dt.Rows.Count > 0)
            {
                txtidS.Text = Convert.ToString(dt.Rows[0].Field<int>("id"));
                tparticipante.Text = dt.Rows[0].Field<string>("participante");
                trfc.Text = dt.Rows[0].Field<string>("rfc");
                tnombre.Text = dt.Rows[0].Field<string>("nombre");
                tusecfdi.Text = dt.Rows[0].Field<string>("usocfdi");
                tbanco.Text = dt.Rows[0].Field<string>("banco");
                tsucursal.Text = dt.Rows[0].Field<string>("sucursal");
                tcuenta.Text = dt.Rows[0].Field<string>("cuenta");
                tclave.Text = dt.Rows[0].Field<string>("clave");
                tcontacto.Text = dt.Rows[0].Field<string>("contacto");

                bool _activo = dt.Rows[0].Field<bool>("activo");
                if (_activo)
                {
                    ractivo.Checked = true;
                }
                else
                {
                    ractivo.Checked = false;
                }
                picsavesub.Visible = true;
            }
            else {
                txtidS.Text = "0";
                tparticipante.Text = "";
                trfc.Text = "";
                tnombre.Text = "";
                tusecfdi.Text = "";
                tbanco.Text = "";
                tsucursal.Text = "";
                tcuenta.Text = "";
                tclave.Text = "";
                tcontacto.Text = "";
                ractivo.Checked = false;
            }
        }

        private void parti_Click(object sender, EventArgs e)
        {
            cargaSubdet(parti.Text);
        }

        private void mparti_Click(object sender, EventArgs e)
        {
            cargaparti(mparti.Text);
        }

        private void pic_newparti_Click(object sender, EventArgs e)
        {
            txtid.Text = "0";
            newpari = true;
            txtrfc.Text = "";
            txtnombre.Text = "";
            txtregimen.Text = "";
            txtusuario.Text = "";
            txtpass.Text = "";
            txt_ruta.Text = "";
            txt_lugar.Text = "";
            ckprod.Checked = false;
            fb.SelectedPath = "";

            parti.Items.Clear();

            tparticipante.Text = "";
            trfc.Text = "";
            tnombre.Text = "";
            tusecfdi.Text = "";
            tbanco.Text = "";
            tsucursal.Text = "";
            tcuenta.Text = "";
            tclave.Text = "";
            tcontacto.Text = "";

            ractivo.Checked = false;
            picsavesub.Visible = false;

            groupBox2.Enabled = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            fb.ShowDialog();

            String pasaf = fb.SelectedPath;
            txt_ruta.Text = pasaf;
        }

        private void picnewsubcta_Click(object sender, EventArgs e)
        {
            if (txtrfc.Text == "")
            {
                MessageBox.Show("Por favor capture primero un participate...");
            }
            txtidS.Text = "0";
            tparticipante.Text = "";
            trfc.Text = "";
            tnombre.Text = "";
            tusecfdi.Text = "";
            tbanco.Text = "";
            tsucursal.Text = "";
            tcuenta.Text = "";
            tclave.Text = "";
            tcontacto.Text = "";

            ractivo.Checked = false;

        }

        private void picsaveparti_Click(object sender, EventArgs e)
        {
            groupBox2.Enabled = true;        
            string valida = valpari();
            if (valida == "")
            {
                int siprod = 0;
                int siactivo = 0;
                if (newpari) // para guardar si es nuevo
                {

                    if (ckprod.Checked)
                    {
                        siprod = 1;
                    }
                    else {
                        siprod = 0;
                    }

                    if (chpartiactivo.Checked)
                    {
                        siactivo = 1;
                    }
                    else {
                        siactivo = 0;
                    }
                    try
                    {
                        string sql = string.Empty;
                        sql = " INSERT INTO facturador(RFC,NOMBRE,LUGAREXPEDICION,REGIMENF,USUARIO,CONTRASEÑA,RUTA,PRODUCTIVO,ACTIVO) ";
                        sql += "VALUES(@RFC,@NOMBRE,@LUGAREXPEDICION,@REGIMENF,@USUARIO,@CONTRASEÑA,@RUTA,@PRODUCTIVO,@ACTIVO) ";

                        SqlConnection cadena = new SqlConnection(zcadena);
                        cadena.Open();
                        SqlCommand cmd = cadena.CreateCommand();
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@RFC", txtrfc.Text);
                        cmd.Parameters.AddWithValue("@NOMBRE", txtnombre.Text);
                        cmd.Parameters.AddWithValue("@LUGAREXPEDICION", txt_lugar.Text);
                        cmd.Parameters.AddWithValue("@REGIMENF", txtregimen.Text);
                        cmd.Parameters.AddWithValue("@USUARIO", txtusuario.Text);
                        cmd.Parameters.AddWithValue("@CONTRASEÑA", txtpass.Text);
                        cmd.Parameters.AddWithValue("@RUTA", txt_ruta.Text);
                        cmd.Parameters.AddWithValue("@PRODUCTIVO", siprod);
                        cmd.Parameters.AddWithValue("@ACTIVO", siactivo);
                        cmd.ExecuteNonQuery();
                        cadena.Close();
                        MessageBox.Show("El registro fue registrado");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else { // si modifica
                    if (ckprod.Checked)
                    {
                        siprod = 1;
                    }
                    else
                    {
                        siprod = 0;
                    }
                    if (chpartiactivo.Checked)
                    {
                        siactivo = 1;
                    }
                    else
                    {
                        siactivo = 0;
                    }
                    try
                    {
                        string sqlU = string.Empty;
                        sqlU = " UPDATE facturador SET RFC = @RFC,NOMBRE = @NOMBRE,LUGAREXPEDICION=@LUGAREXPEDICION,REGIMENF=@REGIMENF,";
                        sqlU += "USUARIO=@USUARIO,CONTRASEÑA=@CONTRASEÑA,RUTA=@RUTA,PRODUCTIVO=" + siprod + ",ACTIVO="+siactivo+" WHERE id =" + txtid.Text;

                        SqlConnection cadena = new SqlConnection(zcadena);
                        cadena.Open();
                        SqlCommand cmd = cadena.CreateCommand();
                        cmd.CommandText = sqlU;
                        cmd.Parameters.AddWithValue("@RFC", txtrfc.Text);
                        cmd.Parameters.AddWithValue("@NOMBRE", txtnombre.Text);
                        cmd.Parameters.AddWithValue("@LUGAREXPEDICION", txt_lugar.Text);
                        cmd.Parameters.AddWithValue("@REGIMENF", txtregimen.Text);
                        cmd.Parameters.AddWithValue("@USUARIO", txtusuario.Text);
                        cmd.Parameters.AddWithValue("@CONTRASEÑA", txtpass.Text);
                        cmd.Parameters.AddWithValue("@RUTA", txt_ruta.Text);
                        cmd.Parameters.AddWithValue("@PRODUCTIVO", siprod);
                        cmd.Parameters.AddWithValue("@ACTIVO", siactivo);
                        cmd.ExecuteNonQuery();
                        cadena.Close();

                        MessageBox.Show("El registro fue modificado");
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
            else {
                MessageBox.Show(valida);
            }
            //txtrfc.Text = dt2.Rows[0].Field<string>("rfc");
            //txtnombre.Text = dt2.Rows[0].Field<string>("nombre");
            //txtregimen.Text = dt2.Rows[0].Field<string>("regimenf");
            //txtusuario.Text = dt2.Rows[0].Field<string>("usuario");
            //txtpass.Text = dt2.Rows[0].Field<string>("contraseña");
            //txt_ruta.Text = dt2.Rows[0].Field<string>("ruta");
            //txt_lugar.Text = dt2.Rows[0].Field<string>("lugarexpedicion");
            //ckprod.Checked = dt2.Rows[0].Field<Boolean>("productivo");
            //fb.SelectedPath = txt_ruta.Text;



        }

        private string valpari()
        {
            string respu = "";
            if (txtrfc.Text == "") { respu = "Por favor capture el RFC del participante"; }
            if (txtnombre.Text == "") { respu = "Por favor capture el Nombre del participante"; }
            if (txtregimen.Text == "") { respu = "Por favor capture el Regimen Fiscal del participante"; }
            if (txtusuario.Text == "") { respu = "Por favor capture el Usuario del PAC del participante"; }
            if (txtpass.Text == "") { respu = "Por favor capture la contraseña del PAC del participante"; }
            if (txt_ruta.Text == "") { respu = "Por favor capture el ruta para guardar los documentos del participante"; }
            if (txt_lugar.Text == "") { respu = "Por favor capture el Lugar de Expedición del participante"; }

            return respu;
        }

        private string valsub()
        {
          
            string respu = "";
            if (tparticipante.Text == "") { respu = "Por favor capture la clave de SubCuenta"; return respu; }
            if (trfc.Text == "") { respu = "Por favor capture el RFC de la SubCuenta"; return respu; }
            if (tnombre.Text == "") { respu = "Por favor capture el Nombre de la SubCuenta"; return respu; }
            if (tusecfdi.Text == "") { respu = "Por favor capture el Uso de CFDI"; return respu; }
            if (tbanco.Text == "") { respu = "Por favor capture el Nombre del banco"; return respu; }
            if (tsucursal.Text == "") { respu = "Por favor capture la Sucursal"; return respu; }
            if (tcuenta.Text == "") { respu = "Por favor capture la cuenta"; return respu; }
            if (tclave.Text == "") { respu = "Por favor capture la clave"; return respu; }
            if (tcontacto.Text == "") { respu = "Por favor capture el correo del contacto"; return respu; }

            return respu;
        }

        private void picsavesub_Click(object sender, EventArgs e)
        {
            string xresp = valsub();
            if (xresp == "")
            {
                int _activo = 0;
                if (txtidS.Text == "0") //si es nuevo
                {
                    try
                    {
                        if (ractivo.Checked == true)
                        {
                            _activo = 1;
                        }
                        else
                        {
                            _activo = 0;
                        }
                        string sql = string.Empty;
                        sql = " INSERT INTO PARTICIPANTE(PARTICIPANTE,RFC,NOMBRE,USOCFDI,BANCO,SUCURSAL,CUENTA,CLAVE,CONTACTO,ACTIVO,PRFC) ";
                        sql += "VALUES(@PARTICIPANTE,@RFC,@NOMBRE,@USOCFDI,@BANCO,@SUCURSAL,@CUENTA,@CLAVE,@CONTACTO,@ACTIVO,@PRFC) ";

                        SqlConnection cadena = new SqlConnection(zcadena);
                        cadena.Open();
                        SqlCommand cmd = cadena.CreateCommand();
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@PARTICIPANTE", tparticipante.Text);
                        cmd.Parameters.AddWithValue("@RFC", trfc.Text);
                        cmd.Parameters.AddWithValue("@NOMBRE", tnombre.Text);
                        cmd.Parameters.AddWithValue("@USOCFDI", tusecfdi.Text);
                        cmd.Parameters.AddWithValue("@BANCO", tbanco.Text);
                        cmd.Parameters.AddWithValue("@SUCURSAL", tsucursal.Text);
                        cmd.Parameters.AddWithValue("@CUENTA", tcuenta.Text);
                        cmd.Parameters.AddWithValue("@CLAVE", tclave.Text);
                        cmd.Parameters.AddWithValue("@CONTACTO", tcontacto.Text);
                        cmd.Parameters.AddWithValue("@ACTIVO", _activo);
                        cmd.Parameters.AddWithValue("@PRFC", txtrfc.Text);
                        cmd.ExecuteNonQuery();
                        cadena.Close();
                        MessageBox.Show("Registro insertado correctamente...");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    //tparticipante.Enabled = false;
                }
                else
                {  //modifica
                    try
                    {
                        if (ractivo.Checked == true)
                        {
                            _activo = 1;
                        }
                        else
                        {
                            _activo = 0;
                        }
                        string sql = string.Empty;
                        sql = " UPDATE PARTICIPANTE SET PARTICIPANTE=@PARTICIPANTE,RFC=@RFC,NOMBRE=@NOMBRE,USOCFDI=@USOCFDI,";
                        sql += "BANCO=@BANCO,SUCURSAL=@SUCURSAL,CUENTA=@CUENTA,CLAVE=@CLAVE,CONTACTO=@CONTACTO,ACTIVO=" + _activo + " WHERE ID =" + txtidS.Text;

                        SqlConnection cadena = new SqlConnection(zcadena);
                        cadena.Open();
                        SqlCommand cmd = cadena.CreateCommand();
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@PARTICIPANTE", tparticipante.Text);
                        cmd.Parameters.AddWithValue("@RFC", trfc.Text);
                        cmd.Parameters.AddWithValue("@NOMBRE", tnombre.Text);
                        cmd.Parameters.AddWithValue("@USOCFDI", tusecfdi.Text);
                        cmd.Parameters.AddWithValue("@BANCO", tbanco.Text);
                        cmd.Parameters.AddWithValue("@SUCURSAL", tsucursal.Text);
                        cmd.Parameters.AddWithValue("@CUENTA", tcuenta.Text);
                        cmd.Parameters.AddWithValue("@CLAVE", tclave.Text);
                        cmd.Parameters.AddWithValue("@CONTACTO", tcontacto.Text);
                        cmd.Parameters.AddWithValue("@ACTIVO", _activo);
                        cmd.ExecuteNonQuery();
                        cadena.Close();
                        MessageBox.Show("Registro modificado correctamente...");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                cargasub(mparti.Text);
            }
            else {
                MessageBox.Show(xresp);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string xname = tabControl1.SelectedTab.Name;
            int xidex = tabControl1.SelectedIndex;
            switch (xidex)
            {
                case 0:                 
                    break;
                case 1:
                    acceso aces0 = new acceso();
                    DataTable dt0 = new DataTable();
                    dt0 = aces0.Conectar("Select * from programa where id = 1");
                    if (dt0.Rows.Count > 0)
                    {
                        dprograms.Text = dt0.Rows[0].Field<string>("xdias");
                        string xrdhora = dt0.Rows[0].Field<string>("hora");

                        rdhora.Value = Convert.ToDateTime(xrdhora);

                        rprogramado.Checked = dt0.Rows[0].Field<Boolean>("programado");

                        if (dt0.Rows[0].Field<int>("tprograma") == 0)
                        {
                            tprograma.Checked = true; // System.Windows.Forms.CheckState.Checked;
                        }
                        else
                        {
                            tprograma1.Checked = true;  //System.Windows.Forms.CheckState.Checked;
                        }

                        String xdiass = dt0.Rows[0].Field<string>("xdiasemana");

                        int _pmasmenos = dt0.Rows[0].Field<int>("pmasmenos");

                        tmasmenos.Text = Convert.ToString(_pmasmenos);

                        string[] words = xdiass.Split(',');
                        foreach (string word in words)
                        {
                            switch (word)
                            {
                                case "1":
                                    rC1.Checked = true;
                                    break;
                                case "2":
                                    rC2.Checked = true;
                                    break;
                                case "3":
                                    rC3.Checked = true;
                                    break;
                                case "4":
                                    rC4.Checked = true;
                                    break;
                                case "5":
                                    rC5.Checked = true;
                                    break;
                                case "6":
                                    rC6.Checked = true;
                                    break;
                                case "7":
                                    rC7.Checked = true;
                                    break;
                            }
                        }
                    }
                  
                    break;               
            }
        }

        public void activavarhoras()
        {
            acceso aces = new acceso();
            DataTable dt = new DataTable();
            string sql = "Select * from programa where id = 1";
            dt = aces.Conectar(sql);
            if (dt.Rows.Count > 0)
            {
                Funciones.activado = dt.Rows[0].Field<Boolean>("programado");
                Funciones.horario = dt.Rows[0].Field<string>("hora");
                Funciones.buscapor = dt.Rows[0].Field<int>("tprograma");
                Funciones.diasemana = dt.Rows[0].Field<string>("xdiasemana");
                Funciones.diames = dt.Rows[0].Field<string>("xdias");
                Funciones.hora = dt.Rows[0].Field<string>("hora");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
