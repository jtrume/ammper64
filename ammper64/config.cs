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
    public partial class config : Form
    {
        public string zcadena = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public config()
        {
            InitializeComponent();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void config_Load(object sender, EventArgs e)
        {
            cargapart();
        }

        public void cargapart()
        {
            part.Items.Clear();
            acceso aces = new acceso();
            DataTable dt = new DataTable();
            dt = aces.Conectar("Select participante from participante order by id");

            foreach (DataRow row in dt.Rows)
            {
                string valor = row[0].ToString();
                part.Items.Add(valor);
            }
            part.ItemHeight = 20;

            part.SelectedIndex = 0;
        }

   
        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void part_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            acceso aces = new acceso();
            DataTable dt = new DataTable();
            dt = aces.Conectar("Select * from participante where participante = '" + part.Text + "'");
            if (dt.Rows.Count > 0)
            {
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

            }

            tparticipante.Enabled = false;
        }

        public Boolean validaparticipante()
        {
            Boolean regre = true;
            try
            {
                if (tparticipante.Text == string.Empty)
                {
                    return false;
                }
                if (trfc.Text == string.Empty)
                {
                    return false;
                }
                if (tnombre.Text == string.Empty)
                {
                    return false;
                }
                if (tusecfdi.Text == string.Empty)
                {
                    return false;
                }
                if (tbanco.Text == string.Empty)
                {
                    return false;
                }
                if (tsucursal.Text == string.Empty)
                {
                    return false;
                }
                if (tcuenta.Text == string.Empty)
                {
                    return false;
                }
                if (tclave.Text == string.Empty)
                {
                    return false;
                }
                if (tcontacto.Text == string.Empty)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                String respus = ex.Message;
            }

            return regre;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int xidex = tabControl1.SelectedIndex;
            switch (xidex)
            {
                case 0:
                    magregar.Visible = true;
                    mcerrar.Location = new Point(269, 33);
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
                    magregar.Visible = false;
                    mcerrar.Location = new Point(155, 33);
                    break;
                case 2:
                    acceso aces = new acceso();
                    DataTable dt2 = new DataTable();
                    dt2 = aces.Conectar("Select * from facturador where id = 1");
                    if (dt2.Rows.Count > 0)
                    {
                        txtrfc.Text = dt2.Rows[0].Field<string>("rfc");
                        txtnombre.Text = dt2.Rows[0].Field<string>("nombre");
                        txtregimen.Text = dt2.Rows[0].Field<string>("regimenf");
                        txtusuario.Text = dt2.Rows[0].Field<string>("usuario");
                        txtpass.Text = dt2.Rows[0].Field<string>("contraseña");
                        txt_ruta.Text = dt2.Rows[0].Field<string>("ruta");
                        txt_lugar.Text = dt2.Rows[0].Field<string>("lugarexpedicion");
                        ckprod.Checked = dt2.Rows[0].Field<Boolean>("productivo");
                        fb.SelectedPath = txt_ruta.Text;
                    }
                    magregar.Visible = false;
                    mcerrar.Location = new Point(155, 33);
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fb.ShowDialog();

            String pasaf = fb.SelectedPath;
            txt_ruta.Text = pasaf;
        }

        private void magregar_Click(object sender, EventArgs e)
        {
            tparticipante.Enabled = true;
            tparticipante.Text = "";
            trfc.Text = "";
            tnombre.Text = "";
            tusecfdi.Text = "";
            tbanco.Text = "";
            tsucursal.Text = "";
            tcuenta.Text = "";
            tclave.Text = "";
            tcontacto.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool _activo;
            if (tparticipante.Enabled == true)  //guarda nuevo participante
            {

                if (validaparticipante())
                {
                    if (ractivo.Checked == true)
                    {
                        _activo = true;
                    }
                    else
                    {
                        _activo = false;
                    }

                    string sql = string.Empty;
                    sql = " INSERT INTO PARTICIPANTE(PARTICIPANTE,RFC,NOMBRE,USOCFDI,BANCO,SUCURSAL,CUENTA,CLAVE,CONTACTO,ACTIVO) ";
                    sql += "VALUES(@PARTICIPANTE,@RFC,@NOMBRE,@USOCFDI,@BANCO,@SUCURSAL,@CUENTA,@CLAVE,@CONTACTO,@ACTIVO) ";

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
                    tparticipante.Enabled = false;
                    cargapart();
                }
                else
                {
                    MessageBox.Show("Es necesario capturar todos los campos...");
                }
            }
            else //Editar participante
            {
                if (validaparticipante())
                {
                    if (ractivo.Checked == true)
                    {
                        _activo = true;
                    }
                    else
                    {
                        _activo = false;
                    }
                    string sql = string.Empty;
                    sql = " UPDATE PARTICIPANTE SET RFC = @RFC,NOMBRE = @NOMBRE,USOCFDI=@USOCFDI,BANCO = @BANCO,SUCURSAL=@SUCURSAL,CUENTA=@CUENTA,CLAVE=@CLAVE,CONTACTO=@CONTACTO,ACTIVO=@ACTIVO ";
                    sql += " WHERE PARTICIPANTE = @PARTICIPANTE";
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
                    tparticipante.Enabled = false;
                    cargapart();
                }
                else
                {
                    MessageBox.Show("Es necesario capturar todos los campos...");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                string sql = string.Empty;
                sql = " UPDATE FACTURADOR SET RFC=@RFC,NOMBRE=@NOMBRE,REGIMENF=@REGIMENF,USUARIO=@USUARIO,CONTRASEÑA=@CONTRASEÑA,LUGAREXPEDICION=@LUGAREXPEDICION,RUTA=@RUTA,PRODUCTIVO=@PRODUCTIVO";
                sql += " WHERE ID = 1";
                SqlConnection cadena = new SqlConnection(zcadena);
                cadena.Open();
                SqlCommand cmd = cadena.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@RFC", txtrfc.Text);
                cmd.Parameters.AddWithValue("@NOMBRE", txtnombre.Text);
                cmd.Parameters.AddWithValue("@REGIMENF", txtregimen.Text);
                cmd.Parameters.AddWithValue("@USUARIO", txtusuario.Text);
                cmd.Parameters.AddWithValue("@CONTRASEÑA", txtpass.Text);
                cmd.Parameters.AddWithValue("@LUGAREXPEDICION", txt_lugar.Text);
                cmd.Parameters.AddWithValue("@RUTA", txt_ruta.Text);
                cmd.Parameters.AddWithValue("@PRODUCTIVO", ckprod.Checked);
                cmd.ExecuteNonQuery();
                cadena.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
}
