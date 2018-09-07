using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace ammper64
{
    public partial class Form1 : Form
    {
        #region variables

        Point PanelMouseDownLocation;

        public string mesfactura = string.Empty;
        public bool siguarda = false;
        public string tipo = "";
        public string FUECD;
        public string IDPART;
        public string FECHEMI;
        public string FECHOPE;
        public string FUF;
        public string FUL;
        public string DESCRIP;
        public string SUBTOTAL;
        public string IVA;
        public string TOTAL;
        public string SUBTOTALDIF;
        public string IVADIF;
        public string TOTALDIF;
        public string ELEMENTO;
        public string NODO;

        public string zcadena = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();


        public string xfuecd;
        public string xparticipante;
        public string xsubcuenta;
        public string xfechaope;
        public string xfechaemis;
        public string xsistema;
        public string xrefbancaria;
        public string xfechapago;
        public string xsubtotaldif;
        public string xivadif;
        public string xtotaldif;
        public string xbusca;
        public string xclave;

        public Boolean activado = false;
        public string horario = string.Empty;
        public int buscapor = 0;
        public string _diasemana = "";

        public string _fechlimpago;
        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //CultureInfo.CreateSpecificCulture("es-ES")

            activavarhoras();

            //acceso acesm = new acceso();
            //DataTable dtm = new DataTable();
            //dtm = acesm.Conectar("Select rfc from facturador where activo = 1 order by id");

            //foreach (DataRow rowm in dtm.Rows)
            //{
            //    string valorm = rowm[0].ToString();
            //    mparti.Items.Add(valorm);
            //}
            //mparti.SelectedIndex = 0;
            //trfc.Text = mparti.Text;
            cargapartis();

            cargasub(trfc.Text);

            //acceso aces = new acceso();
            //DataTable dt = new DataTable();
            //dt = aces.Conectar("Select participante from participante where prfc = '" + trfc.Text + "' order by id");

            //foreach (DataRow row in dt.Rows)
            //{
            //    string valor = row[0].ToString();
            //    parti.Items.Add(valor);
            //}

            //parti.ItemHeight = 20;

            //parti.SelectedIndex = 0;
            //part.Text = parti.Text;

            this.grid.DefaultCellStyle.Font = new Font("Roboto Condensed", 9);
            //cargadatosgrid();
            pfecha1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            pfecha2.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            //var appLog = new EventLog("Application");
            //appLog.Source = "MySource";
            //appLog.WriteEntry("Test log message");


        }
        private void cargapartis()
        {
            mparti.Items.Clear();
            acceso acesm = new acceso();
            DataTable dtm = new DataTable();
            dtm = acesm.Conectar("Select rfc from facturador where activo = 1 order by id");
            if (dtm.Rows.Count > 0)
            {
                foreach (DataRow rowm in dtm.Rows)
                {
                    string valorm = rowm[0].ToString();
                    mparti.Items.Add(valorm);
                }
                mparti.SelectedIndex = 0;
                trfc.Text = mparti.Text;
            }
        }

        private void cargasub(string rfc)
        {
            parti.Items.Clear();
            acceso aces = new acceso();
            DataTable dt = new DataTable();
            dt = aces.Conectar("Select participante from participante where prfc = '" + rfc + "' and activo=1 order by id");

            foreach (DataRow row in dt.Rows)
            {
                string valor = row[0].ToString();
                parti.Items.Add(valor);
            }

            parti.ItemHeight = 20;

            parti.SelectedIndex = 0;
            part.Text = parti.Text;

            cargadatosgrid();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void activavarhoras()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al acceder a la base de datos...");
            }

        }

        public void cargadatosgrid()
        {
            //try
            //{

                Cursor.Current = Cursors.WaitCursor;

           
                //string sqlI = "Select * from f_emitidas where participante = '" + part.Text + "'"; // order by id desc";

                string sqlI = "Select id as ID,fuf as FUF ,participante as PARTICIPANTE ,uuid AS UUID ,fecha AS FECHA ,version AS VERSION ,emisor AS EMISOR ,receptor AS RECEPTOR";
                sqlI += " ,nreceptor AS NOMBRE ,serie AS SERIE ,folio AS FOLIO ,fpago AS FORMA_PAGO ,subtotal AS SUBTOTAL ,timpuestos AS IMPUESTOS ,total AS TOTAL ,tcomprobante AS COMPROBANTE";
                sqlI += " ,mpago AS METODOPAGO ,fcapturado AS FCAPTURADO ,fecha_emis AS EMISION";
                sqlI += " from f_emitidas where participante = '" + part.Text + "'";

                switch (tdoc.Text)
                {
                    case "Facturas":
                        sqlI += " and SUBSTRING(fuf, 17, 1) = '0'";
                        break;
                    case "Notas de Crédito":
                        sqlI += " and SUBSTRING(fuf, 17, 1) = 'C'";
                        break;
                    case "Notas de Débito":
                        sqlI += " and SUBSTRING(fuf, 17, 1) = 'D'";
                        break;
                }

                DateTime dfe1 = new DateTime(pfecha1.Value.Year, pfecha1.Value.Month, pfecha1.Value.Day);
                DateTime dfe2 = new DateTime(pfecha2.Value.Year, pfecha2.Value.Month, pfecha2.Value.Day);

                string dfe1s = Convert.ToString(dfe1.ToString("yyyy/MM/dd 00:01"));
                string dfe2s = Convert.ToString(dfe2.ToString("yyyy/MM/dd 23:59"));

                sqlI += " and fecha BETWEEN '" + dfe1s + "' AND '" + dfe2s + "'";

                acceso aces = new acceso();
                DataTable dt = new DataTable();
                dt = aces.Conectar(sqlI);

                grid.AutoGenerateColumns = true;

                grid.DataSource = dt;

                //if (dt.Rows.Count > 0)
                //{
                    grid.AutoSizeRowsMode =
                       DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                    for (int i = 0; i < grid.Columns.Count; i++)
                    {
                        int colw = grid.Columns[i].Width;
                        grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        grid.Columns[i].Width = colw;
                    }

                    grid.Columns["SUBTOTAL"].DefaultCellStyle.Format = "N2";
                    grid.Columns["IMPUESTOS"].DefaultCellStyle.Format = "N2";
                    grid.Columns["TOTAL"].DefaultCellStyle.Format = "N2";

            grid.Columns["SUBTOTAL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["IMPUESTOS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            grid.Columns["TOTAL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            grid.Columns["ID"].Width = 40;
                    grid.Columns["ID"].Visible = false;
                    grid.Columns["FCAPTURADO"].Visible = false;
                //}
                //else
                //{
                //    MessageBox.Show("No existe conexión con la base de datos...");
                //}
                Cursor.Current = Cursors.Default;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}


            //this.grid.Columns[11].FormatString = "{0:$#,###0.00;($#,###0.00);0}";
            //this.grid.Columns[12].FormatString = "{0:$#,###0.00;($#,###0.00);0}";
            //this.grid.Columns[13].FormatString = "{0:$#,###0.00;($#,###0.00);0}";
            //grid.MasterTemplate.BestFitColumns();


        }

        private void parti_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargadatosgrid();
        }

        private void parti_Click(object sender, EventArgs e)
        {
            part.Text = parti.Text;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            //config llam = new config();
            //llam.ShowDialog();
            configuracion llam = new configuracion();
            llam.ShowDialog();
            cargapartis();

        }

        private void copyAllToolStripMenuItem()
        {
            grid.SelectAll();
            DataObject dataObj = grid.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            copyAllToolStripMenuItem();
        }

        private void tdoc_SelectedValueChanged(object sender, EventArgs e)
        {
            cargadatosgrid();
        }

        private void pfecha1_VisibleChanged(object sender, EventArgs e)
        {
            cargadatosgrid();
        }

        private void pfecha2_VisibleChanged(object sender, EventArgs e)
        {
            cargadatosgrid();
        }

        private void pfecha1_ValueChanged(object sender, EventArgs e)
        {
            cargadatosgrid();
        }

        private void pfecha2_ValueChanged(object sender, EventArgs e)
        {
            cargadatosgrid();
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string xcarpeta = "";
                string ruta = "";

                if (e.ColumnIndex == 0) //2nd column - DGV_ImageColumn
                {
                    var got1 = grid.Rows[e.RowIndex].Cells[3].Value;
                    var got2 = grid.Rows[e.RowIndex].Cells[8].Value;
                    string _valor = got1.ToString();
                    string _namexml = got2.ToString() + _valor + ".xml";
                    String _anio = _valor.Substring(0, 4);
                    String _mes = regremes(_valor.Substring(4, 2));
                    string _parti = _valor.Substring(8, 7);
                    string _tipo = _valor.Substring(15, 2);

                    switch (_tipo)
                    {
                        case "P0":
                            xcarpeta = "Facturas";
                            break;
                        case "PD":
                            xcarpeta = "Notas de Débito";
                            break;
                        case "PC":
                            xcarpeta = "Notas de Crédito";
                            break;
                    }


                    acceso aces = new acceso();
                    DataTable dt = new DataTable();
                    dt = aces.Conectar("Select * from facturador where rfc = '"+trfc.Text+"'");
                    if (dt.Rows.Count > 0)
                    {
                        ruta = dt.Rows[0].Field<string>("ruta");
                    }


                    viewxml vx = new viewxml();
                    vx.Identidicadorruta = @ruta + "\\" + _parti + "\\" + xcarpeta + "\\" + _anio + "\\" + _mes + "\\" + _namexml;
                    vx.Show();



                }

                if (e.ColumnIndex == 1) //2nd column - DGV_ImageColumn
                {
                    var got1 = grid.Rows[e.RowIndex].Cells[3].Value;
                    var got2 = grid.Rows[e.RowIndex].Cells[8].Value;
                    //20180707G031001PC2
                    string _valor = got1.ToString();
                    string _namepdf = got2.ToString() + _valor + ".pdf";
                    String _anio = _valor.Substring(0, 4);
                    String _mes = regremes(_valor.Substring(4, 2));
                    string _parti = _valor.Substring(8, 7);
                    string _tipo = _valor.Substring(15, 2);

                    switch (_tipo)
                    {
                        case "P0":
                            xcarpeta = "Facturas";
                            break;
                        case "PD":
                            xcarpeta = "Notas de Débito";
                            break;
                        case "PC":
                            xcarpeta = "Notas de Crédito";
                            break;
                    }


                    acceso aces = new acceso();
                    DataTable dt = new DataTable();
                    dt = aces.Conectar("Select * from facturador where rfc = '"+trfc.Text+"'");
                    if (dt.Rows.Count > 0)
                    {
                        ruta = dt.Rows[0].Field<string>("ruta");
                    }


                    viewpdf vxp = new viewpdf();
                    // vxp.namepdf = _namepdf;
                    vxp.Identidicadorruta = @ruta + "\\" + _parti + "\\" + xcarpeta + "\\" + _anio + "\\" + _mes + "\\" + _namepdf;

                    //string xrutapdf =  @ruta + "\\" + _parti + "\\" + xcarpeta + "\\" + _anio + "\\" + _mes + "\\" + _namepdf;

                    //System.Diagnostics.Process.Start(xrutapdf);
                    vxp.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string regremes(string mese)
        {
            string regresa = string.Empty;

            switch (mese)
            {
                case "01":
                    regresa = "enero";
                    break;
                case "02":
                    regresa = "febrero";
                    break;
                case "03":
                    regresa = "marzo";
                    break;
                case "04":
                    regresa = "abril";
                    break;
                case "05":
                    regresa = "mayo";
                    break;
                case "06":
                    regresa = "junio";
                    break;
                case "07":
                    regresa = "julio";
                    break;
                case "08":
                    regresa = "agosto";
                    break;
                case "09":
                    regresa = "septiembre";
                    break;
                case "10":
                    regresa = "octubre";
                    break;
                case "11":
                    regresa = "noviembre";
                    break;
                case "12":
                    regresa = "diciembre";
                    break;
            }
            return regresa;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            String hact = DateTime.Now.ToString("HH:mm");
            if (Funciones.activado)
            {
                if (Funciones.hora == hact)
                {
                    DateTime dateValue = DateTime.Now;
                    if (Funciones.buscapor == 0) // cuando sea programacion por dia
                    {
                        string xpass = dateValue.ToString("dddd", CultureInfo.CreateSpecificCulture("es-ES"));
                        int _diasem = NumDiaSem(xpass);
                        bool re;
                        re = Funciones.diasemana.Contains(Convert.ToString(_diasem));
                        if (re == true)
                        {
                            programado();
                            string ok = "";
                        }
                    }
                    else
                    {
                        int diasem = Convert.ToInt32(dateValue.ToString("dd"));
                        bool re1;
                        re1 = Funciones.diames.Contains(Convert.ToString(diasem));
                        if (re1 == true)
                        {
                             programado();
                        }
                    }
                }

            }
        }

        public int NumDiaSem(string diass)
        {
            int regre = 1;
            switch (diass.ToUpper())
            {
                case "LUNES":
                    regre = 1;
                    break;
                case "MARTES":
                    regre = 2;
                    break;
                case "MIÉRCOLES":
                    regre = 3;
                    break;
                case "JUEVES":
                    regre = 4;
                    break;
                case "VIERNES":
                    regre = 5;
                    break;
                case "SÁBADO":
                    regre = 6;
                    break;
                default:
                    regre = 7;
                    break;
            }
            return regre;
        }


        public void programado()
        {
            int diamm = 0;
            string mensaje = string.Empty;
            foreach (string item in parti.Items)
            {
                part.Text = item;

                acceso accp = new acceso();
                DataTable dtP = new DataTable();
                dtP = accp.Conectar("Select * from programa where id = 1");
                if (dtP.Rows.Count > 0)
                {
                    diamm = dtP.Rows[0].Field<int>("pmasmenos");
                }
                else {
                    diamm = 0;
                }



                DateTime xfecha = DateTime.Today.AddDays(diamm);
                string anio = xfecha.ToString("yyyyMMdd");
                String mesa = xfecha.ToString("MM");
                string files = String.Empty;
                mensaje = "";

                s_wspci.wspciSoapClient asd = new s_wspci.wspciSoapClient();
                string zsale = asd.getEstadoDeCuenta("SAP", "Pci123$", "SIN", part.Text.ToString(), anio);
                if (zsale == "Index was outside the bounds of the array.")
                {
                    mensaje = "Index was outside the bounds of the array.";
                    try
                    {
                        string sql = string.Empty;
                        sql = "Insert into fuecd(fuecd,fecha,estatus) ";
                        sql += "VALUES(@fuecd,@fecha,@estatus) ";
                        string dfuecd = anio + item;
                        SqlConnection cadena = new SqlConnection(zcadena);
                        cadena.Open();
                        SqlCommand cmd = cadena.CreateCommand();
                        //cmd.CommandTimeout .CommandTimeout = 500000;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@fuecd", dfuecd);
                        cmd.Parameters.AddWithValue("@fecha", xfecha.ToString("yyyy/MM/dd"));
                        cmd.Parameters.AddWithValue("@estatus", 0);
                        cmd.ExecuteNonQuery();
                        cadena.Close();
                    }
                    catch (Exception ex)
                    {
                        string mesay = ex.Message.ToString();
                    }


                }
                else
                {
                    acceso aces = new acceso();
                    DataTable dta = new DataTable();
                    dta = aces.Conectar("Select ruta from facturador where rfc = '"+trfc.Text+"'");
                    string xruta = dta.Rows[0].Field<string>("ruta");
                    files = @xruta + "\\" + anio + part.Text.ToString() + ".xml";
                    File.WriteAllBytes(@files, Convert.FromBase64String(zsale));

                    try
                    {
                        string sql = string.Empty;
                        sql = "Insert into fuecd(fuecd,fecha,estatus) ";
                        sql += "VALUES(@fuecd,@fecha,@estatus) ";
                        string dfuecd = anio + item;
                        SqlConnection cadena = new SqlConnection(zcadena);
                        cadena.Open();
                        SqlCommand cmd = cadena.CreateCommand();
                        //cmd.CommandTimeout .CommandTimeout = 500000;
                        cmd.CommandText = sql;
                        cmd.Parameters.AddWithValue("@fuecd", dfuecd);
                        cmd.Parameters.AddWithValue("@fecha", xfecha.ToString("yyyy/MM/dd"));
                        cmd.Parameters.AddWithValue("@estatus", 1);
                        cmd.ExecuteNonQuery();
                        cadena.Close();
                    }
                    catch (Exception ex)
                    {
                        string mesay = ex.Message.ToString();
                    }


                    Boolean siguardo = obtenerestado(files);
                    Application.DoEvents();
                    if (siguardo)
                    {
                        mensaje = "Proceso terminado";

                    }
                    else
                    {
                        mensaje = "Este documento ya fue procesado";
                    }
                }
            }

        }

        public Boolean obtenerestado(String file)
        {
            string fechaECD = "";
            string fechaOPER = "";
            Boolean respuesta = true;
            XmlDocument documento = new XmlDocument();
            documento.Load(file);

            XmlNodeList listini = documento.SelectNodes("estadodecuenta");
            foreach (XmlNode nod in listini)
            {
                xfuecd = nod.Attributes["FUECD"].InnerText;
                xparticipante = nod.Attributes["clv_participante"].InnerText;
                xsubcuenta = nod.Attributes["clv_subcuenta"].InnerText;
                xfechaope = nod.Attributes["fecha_oper"].InnerText;
                xfechaemis = nod.Attributes["fecha_emis"].InnerText;
                xsistema = nod.Attributes["clv_sistema"].InnerText;

                //DateTime xlimite = calculafechalimite(Convert.ToDateTime(xfechaemis), "C");



                DateTime mesfactur = Convert.ToDateTime(xfechaemis);
                String mesx = mesfactur.ToString("MM");
                String aniox = mesfactur.ToString("yyyy");
                fechaECD = mesfactur.ToString("yyyy-MM-dd");

                DateTime foper = Convert.ToDateTime(xfechaope);
                fechaOPER = foper.ToString("yyyy-MM-dd");

                mesfactura = regremes(mesx);

                acceso aces = new acceso();
                DataTable dtbusca = new DataTable();
                dtbusca = aces.Conectar("Select * from wspci where fuecd = '" + xfuecd + "'");
                if (dtbusca.Rows.Count > 0)
                {
                    return false;
                }


                saveestadodecuenta(xfuecd, xsubcuenta, xfechaemis, xfechaope);
            }



            XmlNodeList lisammper = documento.SelectNodes("estadodecuenta/liquidaciones/liquidacion");

            pb.Maximum = lisammper.Count - 1;

            foreach (XmlNode node in lisammper)
            {

                XmlNodeList xnl2 = node.SelectNodes("facturas/factura");
                string xnume = node.Attributes["num_liq"].InnerText;
                pb.Value = Convert.ToInt32(xnume);
                Application.DoEvents();
                foreach (XmlNode node2 in xnl2)
                {
                    xbusca = node2.InnerXml;
                    string xpago = node2.Attributes["tipo"].InnerText; //  .ChildNodes["tipo"].InnerText;
                    string xemisor = node2.Attributes["emisor"].InnerText;
                    string xrfc = node2.Attributes["rfc"].InnerText;
                    string xrazonsocial = node2.Attributes["razon_social"].InnerText;
                    string xfuf = node2.Attributes["fuf"].InnerText;


                    xfechapago = existe(node2, "fecha_pago");
                    // string xfechapago = node2.Attributes["fecha_pago"].InnerText;

                    xrefbancaria = existe(node2, "ref_bancaria"); // node2.Attributes["ref_bancaria"].InnerText;

                    string xclave = xfuf.Substring(xfuf.Length - 3, 3);
                    savefactura(xfuecd, xfuf, xemisor, xrazonsocial, xfechapago, xrefbancaria, xpago, xclave);
                    xsubtotaldif = "";

                    XmlNodeList xnl3 = node2.SelectNodes("conceptos");
                    foreach (XmlNode node3 in xnl3)
                    {
                        XmlNodeList xnl4 = node3.SelectNodes("concepto");
                        foreach (XmlNode node4 in xnl4)
                        {
                            xbusca = node4.InnerXml;


                            string xful = node4.Attributes["ful"].InnerText;
                            string xgrupo = node4.Attributes["grupo"].InnerText;
                            XmlNodeList xnl5 = node4.SelectNodes("descripcion");
                            string xdescripcion = xnl5[0].InnerText;
                            XmlNodeList xnl6 = node4.SelectNodes("monto_total");
                            string xsubtotal = xnl6[0].InnerText;
                            XmlNodeList xnl7 = node4.SelectNodes("iva");
                            string xiva = xnl7[0].InnerText;
                            XmlNodeList xnl8 = node4.SelectNodes("total_neto");
                            string xtotal = xnl8[0].InnerText;

                            if (buscacadena(xbusca, "monto_total_dif"))
                            {
                                XmlNodeList xnl9 = node4.SelectNodes("monto_total_dif");
                                xsubtotaldif = xnl9[0].InnerText;
                            }
                            else
                            {
                                xsubtotaldif = "0";
                            }
                            if (buscacadena(xbusca, "iva_dif"))
                            {
                                XmlNodeList xnl10 = node4.SelectNodes("iva_dif");
                                xivadif = xnl10[0].InnerText;
                            }
                            else
                            {
                                xivadif = "0";
                            }
                            if (buscacadena(xbusca, "total_neto_dif"))
                            {
                                XmlNodeList xnl11 = node4.SelectNodes("total_neto_dif");
                                xtotaldif = xnl11[0].InnerText;
                            }
                            else
                            {
                                xtotaldif = "0";
                            }

                            saveconcepto(xfuecd, xfuf, xful, xsubtotal, xiva, xtotal, xdescripcion, xsubtotaldif, xivadif, xtotaldif);


                            XmlNodeList xnl14 = node4.SelectNodes("anexos");
                            foreach (XmlNode node14 in xnl14)
                            {
                                XmlNodeList xnl15 = node14.SelectNodes("anexo");
                                foreach (XmlNode node15 in xnl15)
                                {
                                    string xelemento = "";
                                    try
                                    {
                                        xelemento = node15.Attributes["elemento"].InnerText;
                                    }
                                    catch { xelemento = ""; }
                                    //iria el <nodo>
                                    XmlNodeList xnl16 = node15.SelectNodes("nodo");
                                    string xnodo1 = "";
                                    if (xnl16.Count > 0)
                                    {
                                        xnodo1 = xnl16[0].InnerText;
                                    }
                                    else
                                    {
                                        xnodo1 = "";
                                    }


                                    XmlNodeList xnl17 = node15.SelectNodes("registroshorarios");

                                    foreach (XmlNode node17 in xnl17)
                                    {
                                        XmlNodeList xnl18 = node17.SelectNodes("registro");
                                        foreach (XmlNode node18 in xnl18)
                                        {
                                            string cadenatotal = node18.OuterXml.ToString();
                                            string xpotencia_mtr = string.Empty;
                                            string xpotencia_mda = string.Empty;
                                            string xpotencia = string.Empty;
                                            string xhora = node18.Attributes["HORA"].InnerText;

                                            int rs0 = cadenatotal.IndexOf("POTENCIA_MTR");
                                            if (rs0 >= 1)
                                            {
                                                xpotencia_mtr = node18.Attributes["POTENCIA_MTR"].InnerText;
                                            }
                                            else
                                            {
                                                xpotencia_mtr = "0";
                                            }
                                            int rs1 = cadenatotal.IndexOf("POTENCIA_MDA");
                                            if (rs1 >= 1)
                                            {
                                                xpotencia_mda = node18.Attributes["POTENCIA_MDA"].InnerText;
                                            }
                                            else
                                            {
                                                xpotencia_mda = "0";
                                            }
                                            int rs2 = cadenatotal.IndexOf("POTENCIA=");
                                            if (rs2 >= 1)
                                            {
                                                xpotencia = node18.Attributes["POTENCIA"].InnerText;
                                            }
                                            else
                                            {
                                                xpotencia = "0";
                                            }

                                            string xprecio = "0";
                                            try
                                            {
                                                int rs2p = cadenatotal.IndexOf("PRECIO");
                                                if (rs2p >= 1)
                                                {
                                                    xprecio = node18.Attributes["PRECIO"].InnerText;
                                                }
                                                else
                                                {
                                                    xprecio = "0";
                                                }
                                            }
                                            catch { }


                                            string xmonto_horario = "0";
                                            int rs2pd = cadenatotal.IndexOf("MONTO_HORARIO");
                                            if (rs2pd >= 1)
                                            {
                                                xmonto_horario = node18.Attributes["MONTO_HORARIO"].InnerText;
                                            }
                                            else
                                            {
                                                xmonto_horario = "0";
                                            }

                                            //string xprecio = node18.Attributes["PRECIO"].InnerText;
                                            //string xmonto_horario = node18.Attributes["MONTO_HORARIO"].InnerText;


                                            savedet(xful, xelemento, xnodo1, xhora, xpotencia, xpotencia_mtr, xpotencia_mda, xprecio, xmonto_horario);


                                        }

                                    }

                                }

                            }
                            Application.DoEvents();

                        }

                    }
                    Application.DoEvents();
                }

            }



            DateTime yfechaope = Convert.ToDateTime(xfechaope);
            DateTime yfechaemis = Convert.ToDateTime(xfechaemis);

            string anio = yfechaemis.ToString("yyyyMMdd");
            string aniof = yfechaemis.ToString("yyyy");
            string anioope = yfechaope.ToString("yyyyMMdd");
            Boolean pasa = Creafact(anio + part.Text.ToString(), xfuecd, part.Text.ToString(), aniof, fechaECD, fechaOPER, xfechaemis);

            return respuesta;

        }


        #region variasfunciones
        private void saveestadodecuenta(string FUECD, string IDPART, string FECHEMI, string FECHOPE)
        {
            acceso ace = new acceso();
            DataTable dt = new DataTable();
            dt = ace.Conectar("Select * from wspci where fuecd = '" + FUECD + "'");
            if (dt.Rows.Count > 0)
            {
                siguarda = false;
            }
            else
            {
                siguarda = true;
                string sql = string.Empty;
                sql = " INSERT INTO WSPCI(FUECD,IDPART,FECHEMI,FECHOPE) ";
                sql += "VALUES(@FUECD,@IDPART,@FECHEMI,@FECHOPE) ";

                SqlConnection cadena = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
                cadena.Open();
                SqlCommand cmd = cadena.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@FUECD", FUECD);
                cmd.Parameters.AddWithValue("@IDPART", IDPART);
                cmd.Parameters.AddWithValue("@FECHEMI", FECHEMI);
                cmd.Parameters.AddWithValue("@FECHOPE", FECHOPE);
                cmd.ExecuteNonQuery();
                cadena.Close();
            }
        }

        private void savefactura(string fucd, string fuf, string emisor, string rsocial, string fpago, string refbancaria, string tipo, string clave)
        {
            if (siguarda)
            {
                if (refbancaria == null)
                {
                    refbancaria = "";
                }

                string sql = string.Empty;
                sql = " INSERT INTO wsfuf(FUECD,FUF,RFCEMI,NOMEMI,FECHPAG,REFBANCO,TIPO,CLAVE) ";
                sql += "VALUES(@FUECD,@FUF,@RFCEMI,@NOMEMI,@FECHPAG,@REFBANCO,@TIPO,@CLAVE)";

                SqlConnection cadena = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
                cadena.Open();
                SqlCommand cmd = cadena.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@FUECD", fucd);
                cmd.Parameters.AddWithValue("@FUF", fuf);
                cmd.Parameters.AddWithValue("@RFCEMI", emisor);
                cmd.Parameters.AddWithValue("@NOMEMI", rsocial);
                cmd.Parameters.AddWithValue("@FECHPAG", fpago);
                cmd.Parameters.AddWithValue("@REFBANCO", refbancaria);
                cmd.Parameters.AddWithValue("@TIPO", tipo);
                cmd.Parameters.AddWithValue("@CLAVE", clave);
                cmd.ExecuteNonQuery();
                cadena.Close();
            }
        }

        private void saveconcepto(string fuecd, string fuf, string ful, string subtot, string iva, string total, string descrip, string subtotaldif, string ivadif, string totaldif)
        {
            string _uuid = "";
            
            if (siguarda)
            {

                //20180716G031004P00

                string _tiporel = "";
                string _tdocrel = fuf.Substring(fuf.Length - 3, 2);
                switch (_tdocrel)
                {
                    case "PC":
                        _tiporel = "01";
                        break;
                    case "PD":
                        _tiporel = "02";
                        break;
                    default:
                        _tiporel = "";
                        break;
                }



                if (_tiporel != "")
                {
                    string[] words = ful.Split('-');
                    string sqlx = "";
                    acceso ace = new acceso();
                    DataTable dt1 = new DataTable();
                    sqlx = "Select uuid from fuftempor where fuf LIKE '%" + words[0] + "%'";
                    dt1 = ace.Conectar(sqlx);
                    if (dt1.Rows.Count > 0)
                    {
                        _uuid = dt1.Rows[0].Field<string>("uuid");
                    }
                    else {
                        _uuid = "";
                    }
                }

                string sql = string.Empty;
                sql = " INSERT INTO wsful(FUECD,FUF,FUL,SUBTOT,IVA,TOTAL,DESCRIP,SUBTOTALDIF,IVADIF,TOTALDIF,CANTIDAD,UUID,TRELACION) ";
                sql += "VALUES(@FUECD,@FUF,@FUL,@SUBTOT,@IVA,@TOTAL,@DESCRIP,@SUBTOTALDIF,@IVADIF,@TOTALDIF,@CANTIDAD,@UUID,@TRELACION)";

                SqlConnection cadena = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
                cadena.Open();
                SqlCommand cmd = cadena.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@FUECD", fuecd);
                cmd.Parameters.AddWithValue("@FUF", fuf);
                cmd.Parameters.AddWithValue("@FUL", ful);
                cmd.Parameters.AddWithValue("@SUBTOT", subtot);
                cmd.Parameters.AddWithValue("@IVA", iva);
                cmd.Parameters.AddWithValue("@TOTAL", total);
                cmd.Parameters.AddWithValue("@DESCRIP", descrip);
                cmd.Parameters.AddWithValue("@SUBTOTALDIF", subtotaldif);
                cmd.Parameters.AddWithValue("@IVADIF", ivadif);
                cmd.Parameters.AddWithValue("@TOTALDIF", totaldif);
                cmd.Parameters.AddWithValue("@CANTIDAD", 0);
                cmd.Parameters.AddWithValue("@UUID", _uuid);
                cmd.Parameters.AddWithValue("@TRELACION", _tiporel);
                cmd.ExecuteNonQuery();
                cadena.Close();

                _uuid = "";
                _tiporel = "";
            }
        }

        public void savedet(string xful, string xelemento, string xnodo, string xhora, string xpotencia, string xpotencia_mtr, string xpotencia_mda, string xprecio, string xmonto_horario)
        {
            string sql = string.Empty;
            sql = " INSERT INTO wsanexos(FUL,ELEMENTO,NODO,HORA,POTENCIA,POTENCIA_MTR,POTENCIA_MDA,PRECIO,MONTO_HORARIO) ";
            sql += "VALUES(@FUL,@ELEMENTO,@NODO,@HORA,@POTENCIA,@POTENCIA_MTR,@POTENCIA_MDA,@PRECIO,@MONTO_HORARIO) ";

            SqlConnection cadena = new SqlConnection(zcadena);
            cadena.Open();
            SqlCommand cmd = cadena.CreateCommand();
            //cmd.CommandTimeout .CommandTimeout = 500000;
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@FUL", xful);
            cmd.Parameters.AddWithValue("@ELEMENTO", xelemento);
            cmd.Parameters.AddWithValue("@NODO", xnodo);
            cmd.Parameters.AddWithValue("@HORA", xhora);
            cmd.Parameters.AddWithValue("@POTENCIA", Convert.ToDecimal(xpotencia));
            cmd.Parameters.AddWithValue("@POTENCIA_MTR", Convert.ToDecimal(xpotencia_mtr));
            cmd.Parameters.AddWithValue("@POTENCIA_MDA", Convert.ToDecimal(xpotencia_mda));
            cmd.Parameters.AddWithValue("@PRECIO", Convert.ToDecimal(xprecio));
            cmd.Parameters.AddWithValue("@MONTO_HORARIO", Convert.ToDecimal(xmonto_horario));
            cmd.ExecuteNonQuery();
            cadena.Close();
        }

        public bool existnode(XmlTextReader elem, string name)
        {
            bool sino = false;
            try
            {
                string VALA1 = elem.GetAttribute(name).ToString();
                sino = true;
            }
            catch
            {
                sino = false;
            }
            return sino;
        }





        public bool buscacadena(string cadena, string busca)
        {
            bool sino = false;
            int bus = cadena.IndexOf(busca);
            if (bus > 0)
            {
                sino = true;
            }

            return sino;
        }

        public string existe(XmlNode noda, string atri)
        {
            string vale = "";
            try
            {
                vale = noda.Attributes[atri].InnerText;
            }
            catch
            {
                vale = "";
            }

            return vale;
        }

        #endregion

        #region creafactura
        public Boolean CreaFactura(string part, string mesfactura, string namefile, string fuf, string cadenaAddenda, string cadenaTXT, string aniof, string carpeta, string fecha_emis)
        {
            String valurlfactura = string.Empty;
            Boolean respufactura = false;
            String oRequest = "";

            try
            {

                //string xcadede = "#01@||99|69295.39|||MXN|80382.65|I| PPD|11550|A|379|1||| #02@|CNC140828PQ4|CENTRO NACIONAL DE CONTROL DE ENERGÍA|||G01| #03@|23.78| MEGAWATT HORA|Venta de energía generada en el MDA|2291.17|54484.14|20180828G031001-A01010|83101800|MWH|| %TRA/54484.14/002/Tasa/0.160000/8717.46||||#03@|6.52| MEGAWATT HORA|Venta de energía generada en el MTR|2271.66|14811.25|20180828G031001-B01010|83101800|MWH|| %TRA/14811.25/002/Tasa/0.160000/2369.80||||#04A@|0.160000|002|11087.26|Tasa|#04TI@|11087.26|0.00|";
                //cadenaTXT
                acceso aceso = new acceso();
                DataTable dt0 = new DataTable();
                dt0 = aceso.Conectar("Select * from facturador where rfc = '"+trfc.Text+"'");

                string emis = dt0.Rows[0].Field<string>("rfc");
                string user = dt0.Rows[0].Field<string>("usuario");
                string pass = dt0.Rows[0].Field<string>("contraseña");
                string rutar = dt0.Rows[0].Field<string>("ruta");
                Boolean produc = dt0.Rows[0].Field<Boolean>("productivo");

                oRequest = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:tem='http://tempuri.org/' xmlns:wcf='http://schemas.datacontract.org/2004/07/WcfAmmper'>";
                oRequest += "   <soapenv:Body>";
                oRequest += "      <tem:TimbrarTxt>        ";
                oRequest += "        <tem:_PeticionCliente> ";
                oRequest += "            <wcf:TipoPDF>GENERICO_CENACE</wcf:TipoPDF>";
                oRequest += "            <wcf:cadenaAddenda>" + cadenaAddenda + "</wcf:cadenaAddenda>";
                oRequest += "            <wcf:cadenaComplemento></wcf:cadenaComplemento>";
                oRequest += "            <wcf:cadenaTXT>" + cadenaTXT + "</wcf:cadenaTXT>";
                oRequest += "            <wcf:contrasena>" + pass + "</wcf:contrasena>";
                oRequest += "            <wcf:datosG></wcf:datosG>";
                oRequest += "            <wcf:pdf>true</wcf:pdf>";
                if (produc)
                {
                    oRequest += "            <wcf:productivo>true</wcf:productivo>";
                }
                else
                {
                    oRequest += "            <wcf:productivo>false</wcf:productivo>";
                }
                oRequest += "            <wcf:rfcEmisor>" + emis + "</wcf:rfcEmisor>";
                oRequest += "            <wcf:tipoAddenda>cenace</wcf:tipoAddenda>";
                oRequest += "            <wcf:tipoCFDI>1</wcf:tipoCFDI>";
                oRequest += "           <wcf:usuario>" + user + "</wcf:usuario>";
                oRequest += "         </tem:_PeticionCliente>";
                oRequest += "      </tem:TimbrarTxt>";
                oRequest += "   </soapenv:Body>";
                oRequest += "</soapenv:Envelope>";

                if (produc)
                {
                    valurlfactura = "http://facturainteligenteintegraciones.com/WcfAmmper/Service1.svc";
                }
                else
                {
                    valurlfactura = "http://facturainteligenteintegraciones.com/WcfAmmper_TEST/Service1.svc";
                }

                //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://facturainteligenteintegraciones.com/WcfAmmper_TEST/Service1.svc");
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(valurlfactura);
                req.Headers.Add("SOAPAction", "http://tempuri.org/IService1/TimbrarTxt");
                req.ContentType = "text/xml;charset=UTF-8";
                req.Accept = "gzip,deflate";
                req.Method = "POST";
                req.UserAgent = "Apache-HttpClient/4.1.1";
                req.KeepAlive = true;


                using (Stream stm = req.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(stm))
                    {
                        stmw.Write(oRequest);
                    }
                }
                WebResponse response = req.GetResponse();

                HttpWebResponse response1 = (HttpWebResponse)req.GetResponse();

                Console.Write(response1.StatusCode.ToString());

                var _response = response1.ToString();

                Stream resp2 = req.GetRequestStream();
                Stream responseStream = response.GetResponseStream();

                IAsyncResult asyncResult = req.BeginGetResponse(null, null);

                string soapResult;
                using (WebResponse webResponse = req.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                    }
                }
                string s = soapResult.ToString();

                XElement xdocument = XElement.Parse(s);


                respuestaWs respu = new respuestaWs();
                respu = ExtraeResultado(s);

                if (respu.exito == "true")
                {
                    byte[] sPDFDecoded = Convert.FromBase64String(respu.pdf);

                    string sa = Encoding.UTF8.GetString(sPDFDecoded);


                    String savename = "AEN160405E56" + fuf;

                    string xanio = aniof;

                    //String xrutasave = @"C:\Users\jtrume\Downloads\ammper\Facturas\" + part + "\\" + mesfactura + "\\";
                    String xrutasave = @rutar + "\\" + part + "\\" + carpeta + "\\" + xanio + "\\" + mesfactura + "\\";

                    

                    if (!Directory.Exists(xrutasave))
                    {
                        Directory.CreateDirectory(xrutasave);
                    }

                    System.IO.FileStream stream = new FileStream(@xrutasave + namefile + ".pdf", FileMode.CreateNew);
                    System.IO.BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(sPDFDecoded, 0, sPDFDecoded.Length);
                    writer.Close();

                    System.IO.File.WriteAllText(@xrutasave + namefile + ".xml", respu.xml);

                    ///guardo en base de datos
                    //databaseFilePut(@xrutasave + namefile + ".xml", respu.xml);

                    string timpuesto = "";
                    Char delimiter = '|';
                    String[] subcfdi = cadenaTXT.Split(delimiter);
                    int cuantos = subcfdi.Length;
                    String uuid1 = respu.uuid;
                    DateTime fecha = DateTime.Today;
                    string version = "3.3";
                    string serie = subcfdi[11];
                    string folio = subcfdi[12];
                    string fpago = subcfdi[2];
                    string subtotal = subcfdi[3];
                    string total = subcfdi[7];
                    string tcomprobante = subcfdi[8];
                    string mpago = subcfdi[9];
                    //string emisor = "TES030201001";
                    string emisor = emis;
                    string receptor = subcfdi[17];
                    string nreceptor = subcfdi[18];
                    //string timpuesto = subcfdi[53];
                    timpuesto = subcfdi[cuantos - 3];

                    string sql = string.Empty;
                    sql = " INSERT INTO f_emitidas(FUF,PARTICIPANTE,UUID,VERSION,EMISOR,RECEPTOR,NRECEPTOR,SERIE,FOLIO,FPAGO,SUBTOTAL,TIMPUESTOS,TOTAL,TCOMPROBANTE,MPAGO,FECHA_EMIS) ";
                    sql += "VALUES(@FUF,@PARTICIPANTE,@UUID,@VERSION,@EMISOR,@RECEPTOR,@NRECEPTOR,@SERIE,@FOLIO,@FPAGO,@SUBTOTAL,@TIMPUESTOS,@TOTAL,@TCOMPROBANTE,@MPAGO,@FECHA_EMIS) ";

                    SqlConnection cadena = new SqlConnection(zcadena);
                    cadena.Open();
                    SqlCommand cmd = cadena.CreateCommand();
                    //cmd.CommandTimeout .CommandTimeout = 500000;
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@FUF", fuf);
                    cmd.Parameters.AddWithValue("@PARTICIPANTE", part);
                    cmd.Parameters.AddWithValue("@UUID", uuid1);
                    cmd.Parameters.AddWithValue("@VERSION", version);
                    cmd.Parameters.AddWithValue("@EMISOR", emisor);
                    cmd.Parameters.AddWithValue("@RECEPTOR", receptor);
                    cmd.Parameters.AddWithValue("@NRECEPTOR", nreceptor);
                    cmd.Parameters.AddWithValue("@SERIE", serie);
                    cmd.Parameters.AddWithValue("@FOLIO", folio);
                    cmd.Parameters.AddWithValue("@FPAGO", fpago);
                    cmd.Parameters.AddWithValue("@SUBTOTAL", Convert.ToDecimal(subtotal));
                    cmd.Parameters.AddWithValue("@TIMPUESTOS", Convert.ToDecimal(timpuesto));
                    cmd.Parameters.AddWithValue("@TOTAL", Convert.ToDecimal(total));
                    cmd.Parameters.AddWithValue("@TCOMPROBANTE", tcomprobante);
                    cmd.Parameters.AddWithValue("@MPAGO", mpago);
                    cmd.Parameters.AddWithValue("@FECHA_EMIS", fecha_emis);
                    cmd.ExecuteNonQuery();
                    cadena.Close();

                    // creaalert("Se creo factura " + uuid1, "Serie: " + serie + " Folio: " + folio + " Fecha de emisión: " + Convert.ToString(fecha_emis.ToString()));

                    cargadatosgrid();
                }
            }
            catch (Exception ex)
            {
                string RESPUERROR = ex.Message;
                respufactura = false;
            }
            return respufactura;
        }



        #endregion

        #region extrares
        public respuestaWs ExtraeResultado(string xml)
        {
            respuestaWs respu = new respuestaWs();
            string result = string.Empty;
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);

                manager.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
                manager.AddNamespace("i", "http://www.w3.org/2001/XMLSchema-instance");
                manager.AddNamespace("a", "http://schemas.datacontract.org/2004/07/WcfAmmper");
                manager.AddNamespace("", "http://tempuri.org/");

                XmlNode node = document.SelectSingleNode("/s:Envelope/s:Body", manager);
                string firstname = node.InnerText;

                node = document.SelectSingleNode("//a:exito", manager);
                respu.exito = node.InnerText;
                node = document.SelectSingleNode("//a:uuid", manager);
                respu.uuid = node.InnerText;
                node = document.SelectSingleNode("//a:pdf", manager);
                respu.pdf = node.InnerText;
                node = document.SelectSingleNode("//a:xmlTimbrado", manager);
                respu.xml = node.InnerText;
            }
            catch (Exception ex)
            {
                result = "Ocurrio un error";
            }

            return respu;

        }
        #endregion

        #region creafact
        public Boolean Creafact(String fuf, string fuecd, string participante, string aniof, string ECD, string OPER, string fecha_emis)
        {
            string xful = "";
            Boolean respu = false;
            String xclave = "";
            String xtipof = "";
            String xcarpeta = "";
            String xfuf = "";            
            String xpago = "";


            String _uuid = "";
            String _trela = "";

            String xlimite = "";


            acceso acesas = new acceso();
            DataTable dtaas = new DataTable();
            String sqlq = "Select fuf,clave,fechpag from wsfuf WHERE clave in ('P00','PC1','PC2','PC3','PC4','PC5','PD1','PD2','PD3','PD4','PD') and fuecd ='" + fuecd + "'";
            dtaas = acesas.Conectar(sqlq);

            if (dtaas.Rows.Count > 0)
            {
                foreach (DataRow rowx in dtaas.Rows)
                {
                    xclave = rowx["clave"].ToString();
                    xfuf = rowx["fuf"].ToString();
                    xpago = rowx["fechpag"].ToString();
                    DateTime zfechapago = Convert.ToDateTime(xpago);


                    xpago = Convert.ToString(zfechapago.ToString("yyyy-MM-dd"));
                    string _tipoclave = xclave.Substring(0, 2);
                    switch (_tipoclave)
                    {
                        case "P0":
                            xtipof = "I";
                            xcarpeta = "Facturas";
                            xlimite = Convert.ToString(calculafechalimite(Convert.ToDateTime(fecha_emis), "P").ToString("yyyy-MM-dd"));
                            break;
                        case "PD":
                            xtipof = "I";
                            xcarpeta = "Notas de Débito";
                            xlimite = Convert.ToString(calculafechalimite(Convert.ToDateTime(fecha_emis), "D").ToString("yyyy-MM-dd"));
                            break;
                        case "PC":
                            xtipof = "E";
                            xcarpeta = "Notas de Crédito";
                            xlimite = Convert.ToString(calculafechalimite(Convert.ToDateTime(fecha_emis), "C").ToString("yyyy-MM-dd"));
                            break;
                        default:
                            xtipof = "X";
                            xcarpeta = "Otros";
                            Console.WriteLine("Default case");
                            break;
                    }

                    if (xtipof != "X")
                    {

                        acceso dtcuentos = new acceso();
                        DataTable dtacceso = new DataTable();
                        dtacceso = dtcuentos.Conectar("Select * from wsful where fuf = '" + xfuf + "' and total<> 0");
                        if (dtacceso.Rows.Count > 0)
                        {
                            xful = dtacceso.Rows[0].Field<string>("ful");
                            acceso acesa = new acceso();
                            DataTable dtaa = new DataTable();
                            String xsql = "Select sum(subtot) as tsubtotal,sum(total) as ttotal,sum(subtotaldif) as tsubtotaldif,sum(totaldif) as ttotaldif from wsful where fuf = '" + xfuf + "' and total<> 0";
                            dtaa = acesa.Conectar(xsql);

                            CreaYTimbra pfac = new CreaYTimbra();
                            DateTime xfecha = DateTime.Now;
                            Decimal xneto = 0;
                            Decimal xtotal = 0;


                            Decimal Timpuesto = 0;
                            String xmpago = "PPD";
                            String cadena1 = "";
                            String _serie = "";
                            String _folio = "";
                            ValSerFol res0 = new ValSerFol();
                            switch (_tipoclave)
                            {
                                case "P0":
                                    xneto = Convert.ToDecimal(dtaa.Rows[0].Field<decimal>("tsubtotal"));
                                    xtotal = Convert.ToDecimal(dtaa.Rows[0].Field<decimal>("ttotal"));
                                    res0 = serfol("P0");
                                    _uuid = "";
                                    _trela = "";
                                    break;
                                case "PD":
                                    xneto = Convert.ToDecimal(dtaa.Rows[0].Field<decimal>("tsubtotaldif"));
                                    xtotal = Convert.ToDecimal(dtaa.Rows[0].Field<decimal>("ttotaldif"));
                                    _uuid = dtacceso.Rows[0].Field<string>("UUID");
                                    _trela = dtacceso.Rows[0].Field<string>("TRELACION");

                                    if (xneto < 0)
                                    {
                                        xneto = xneto * -1;
                                    }

                                    if (xtotal < 0)
                                    {
                                        xtotal = xtotal * -1;
                                    }
                                    res0 = serfol("PD");
                                    break;
                                case "PC":
                                    xneto = Convert.ToDecimal(dtaa.Rows[0].Field<decimal>("tsubtotaldif"));
                                    xtotal = Convert.ToDecimal(dtaa.Rows[0].Field<decimal>("ttotaldif"));
                                    res0 = serfol("PC");
                                    _uuid = dtacceso.Rows[0].Field<string>("UUID");
                                    _trela = dtacceso.Rows[0].Field<string>("TRELACION");
                                    break;
                            }

                            

                            string _lg_cp = "";
                            string _rfc = "";
                            try
                            {
                                acceso aceso = new acceso();
                                DataTable dt0 = new DataTable();
                                dt0 = aceso.Conectar("Select * from facturador where rfc = '"+ trfc.Text+"'");

                                _lg_cp = dt0.Rows[0].Field<string>("lugarexpedicion");
                                _rfc = dt0.Rows[0].Field<string>("rfc");
                            }
                            catch
                            {
                                _lg_cp = "11550";
                                _rfc = "CNC140828PQ4";
                            }

                            cadena1 = pfac.CadenaDatosCFDI(xfecha, xneto, xtotal, xmpago, xtipof, res0.serie, res0.folio, _lg_cp,_uuid,_trela);

                            acceso acedes = new acceso();
                            DataTable dtg = new DataTable();
                            dtg = acedes.Conectar("Select * from participante where participante='" + part.Text + "'");



                            //String rfc = "CNC140828PQ4";
                            //String nombre_razons = "CENTRO NACIONAL DE CONTROL DE ENERGÍA";
                            //String usoC = "G01";

                            String rfc = dtg.Rows[0].Field<string>("rfc");
                            String nombre_razons = dtg.Rows[0].Field<string>("nombre");
                            String usoC = dtg.Rows[0].Field<string>("usocfdi");
                            String cadena02 = pfac.CadenaDatosReceptorLayOut(rfc, nombre_razons, usoC);

                            String cadena03 = "";
                            String cadenaade = "";
                            decimal neto = 0;
                            decimal impuesto = 0;
                            acceso aces = new acceso();
                            DataTable dta = new DataTable();
                            string ysql = "Select subtot,iva,descrip,ful,subtotaldif,ivadif,totaldif from wsful where fuf = '" + xfuf + "'"; // and total <> 0 and totaldif <> 0";
                            switch (_tipoclave)
                            {
                                case "P0":
                                    ysql += " and total <> 0";
                                    break;
                                case "PD":
                                    ysql += " and totaldif <> 0";
                                    break;
                                case "PC":
                                    ysql += " and totaldif <> 0";
                                    break;
                            }
                            dta = aces.Conectar(ysql);
                            foreach (DataRow row in dta.Rows)
                            {
                                String descripo = row[2].ToString();
                                String ful = row[3].ToString();
                                switch (_tipoclave)
                                {
                                    case "P0":
                                        neto = Convert.ToDecimal(row[0].ToString()); // dta.Rows[0].Field<string>("SUBTOT");
                                        impuesto = Convert.ToDecimal(row[1].ToString());
                                        cadenaade = cadenaade + "*" + ful + "/" + impuesto + "/" + (neto + impuesto) + "///";
                                        break;
                                    case "PD":
                                        neto = Convert.ToDecimal(row[4].ToString()); // dta.Rows[0].Field<string>("SUBTOT");
                                        impuesto = Convert.ToDecimal(row[5].ToString());
                                        if (neto < 0)
                                        {
                                            neto = neto * -1;
                                        }
                                        if (impuesto < 0)
                                        {
                                            impuesto = impuesto * -1;
                                        }
                                        ValOriMod regre = new ValOriMod();
                                        regre = calmontosaddenda(xfuf);

                                        cadenaade = cadenaade + "*" + ful + "/" + impuesto + "/" + (neto + impuesto) + "/" + regre.importeOri + "/" + regre.importeMod + "/" + regre.montoAjuste;
                                        xcarpeta = "Notas de Débito";

                                        break;
                                    case "PC":
                                        neto = Convert.ToDecimal(row[4].ToString()); // dta.Rows[0].Field<string>("SUBTOT");
                                        impuesto = Convert.ToDecimal(row[5].ToString());

                                        ValOriMod regres = new ValOriMod();
                                        regres = calmontosaddenda(xfuf);

                                        cadenaade = cadenaade + "*" + ful + "/" + impuesto + "/" + (neto + impuesto) + "/" + regres.importeOri + "/" + regres.importeMod + "/" + regres.montoAjuste;
                                        xcarpeta = "Notas de Crédito";
                                        break;
                                    default:
                                        xtipof = "X";
                                        xcarpeta = "Otros";
                                        Console.WriteLine("Default case");
                                        break;
                                }

                                Timpuesto = Timpuesto + impuesto;
                                cadena03 = cadena03 + pfac.cadenaconceptos(neto, impuesto, descripo, ful, _tipoclave);

                            }



                            acceso acesorecep = new acceso();
                            DataTable dtR = new DataTable();
                            dtR = acesorecep.Conectar("Select * from participante where participante = '" + part.Text + "'");
                            string _banco = dtR.Rows[0].Field<string>("banco");
                            string _sucursal = dtR.Rows[0].Field<string>("sucursal");
                            string _cuenta = dtR.Rows[0].Field<string>("cuenta");
                            string _clave = dtR.Rows[0].Field<string>("clave");
                            string _contacto = dtR.Rows[0].Field<string>("contacto");

                            String cadena04A = pfac.Cadena04A(Timpuesto);
                            String savename = _rfc + xfuf;
                            string cadxml = cadena1 + cadena02 + cadena03 + cadena04A;

                            String cadenaA = "";
                            switch (_tipoclave)
                            {
                                case "P0":
                                    cadenaA = "1|" + xfuf + "|" + xlimite + "| " + part.Text.ToString() + "|" + _banco + "|" + _sucursal + "|" + _cuenta + "|" + _clave + "|DO " + OPER + " " + part.Text.ToString() + "|" + _contacto + "|" + cadenaade + "| " + ECD;
                                    break;
                                case "PD":
                                    cadenaA = "3|" + xfuf + "|" + xlimite + "| " + part.Text.ToString() + "|" + _banco + "|" + _sucursal + "|" + _cuenta + "|" + _clave + "|DO " + OPER + " " + part.Text.ToString() + "|" + _contacto + "|" + cadenaade + "| " + ECD;
                                    break;
                                case "PC":
                                    cadenaA = "2|" + xfuf + "|" + xlimite + "| " + part.Text.ToString() + "|" + _banco + "|" + _sucursal + "|" + _cuenta + "|" + _clave + "|DO " + OPER + " " + part.Text.ToString() + "|" + _contacto + "|" + cadenaade + "| " + ECD;
                                    break;
                            }


                            Boolean creafactur = CreaFactura(part.Text.ToString(), mesfactura, savename, xfuf, cadenaA, cadxml, aniof, xcarpeta, fecha_emis);

                        }
                    }
                }
            }

            return respu;

        }
        #endregion

        #region calmontoadd
        public ValOriMod calmontosaddenda(string fuf)
        {
            ValOriMod regresa = new ValOriMod();
            acceso acex = new acceso();
            DataTable dt = new DataTable();
            dt = acex.Conectar("Select subtot from wsful where fuf ='" + fuf + "' order by id");
            if (dt.Rows.Count > 0)
            {
                regresa.importeOri = Convert.ToString(dt.Rows[0].Field<decimal>("subtot"));
                regresa.importeMod = Convert.ToString(dt.Rows[1].Field<decimal>("subtot"));
                regresa.montoAjuste = Convert.ToString(dt.Rows[1].Field<decimal>("subtot") - dt.Rows[0].Field<decimal>("subtot"));

                if ((dt.Rows[1].Field<decimal>("subtot") - dt.Rows[0].Field<decimal>("subtot")) < 0)
                {
                    regresa.montoAjuste = Convert.ToString(dt.Rows[0].Field<decimal>("subtot") - dt.Rows[1].Field<decimal>("subtot"));
                }
            }
            else
            {
                regresa.importeOri = "";
                regresa.importeMod = "";
                regresa.montoAjuste = "";
            }


            return regresa;
        }
        #endregion


        #region serfol
        public ValSerFol serfol(string tipo)
        {
            ValSerFol sf = new ValSerFol();
            acceso aces = new acceso();
            DataTable dt = new DataTable();
            dt = aces.Conectar("Select * from serfol where tipo ='" + tipo + "'");
            if (dt.Rows.Count > 0)
            {
                sf.serie = dt.Rows[0].Field<string>("serie");
                sf.folio = Convert.ToString(dt.Rows[0].Field<int>("folio"));

                acceso aces1 = new acceso();
                aces1.Actualiza("Update serfol set folio=folio+1 where tipo='" + tipo + "'");
            }
            else
            {
                sf.serie = "";
                sf.folio = "";
            }


            return sf;
        }
        #endregion

        #region calfechañlim
        public DateTime calculafechalimite(DateTime fechaemi, string tipodoc)
        {
            DateTime respu = fechaemi;
            string xpass = fechaemi.ToString("dddd", CultureInfo.CreateSpecificCulture("es-ES"));
            int diasem = NumDiaSem(xpass);
            // domingo 7

            int diferadom = 7 - diasem;
            DateTime fechaprocdomingo = fechaemi.AddDays(diferadom);
            DateTime fechaprocmiercoles = fechaprocdomingo.AddDays(3);
            switch (tipodoc)
            {
                case "P":
                    respu = fechaprocdomingo.AddDays(10);
                    break;
                case "C":
                    respu = fechaprocdomingo.AddDays(3);
                    Console.WriteLine(5);
                    break;
                case "D":
                    respu = fechaprocdomingo.AddDays(10);
                    Console.WriteLine(5);
                    break;
            }
            return respu;
        }
        #endregion

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pprocesa.Visible = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            pprocesa.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String mensaje = string.Empty;
            button2.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            for (DateTime d = dti.Value; d <= dtal.Value; d = d.AddDays(1))
            {
                foreach (string item in parti.Items)
                {
                    part.Text = item;
                    //Hacer algo con la fecha
                    DateTime xfecha = Convert.ToDateTime(d.ToString());
                    string anio = xfecha.ToString("yyyyMMdd");
                    String mesa = xfecha.ToString("MM");
                    string files = String.Empty;
                    mensaje = "";

                    s_wspci.wspciSoapClient asd = new s_wspci.wspciSoapClient();
                    string zsale = asd.getEstadoDeCuenta("SAP", "Pci123$", "SIN", part.Text.ToString(), anio);
                    if (zsale == "Index was outside the bounds of the array.")
                    {
                        mensaje = "Index was outside the bounds of the array.";
                        string sql = string.Empty;
                        sql = "Insert into fuecd(fuecd,fecha,estatus) ";
                        sql += "VALUES(@fuecd,@fecha,@estatus) ";

                        try
                        {
                            string dfuecd = anio + part.Text;
                            SqlConnection cadena = new SqlConnection(zcadena);
                            cadena.Open();
                            SqlCommand cmd = cadena.CreateCommand();
                            //cmd.CommandTimeout .CommandTimeout = 500000;
                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("@fuecd", dfuecd);
                            cmd.Parameters.AddWithValue("@fecha", xfecha.ToString("yyyy/MM/dd"));
                            cmd.Parameters.AddWithValue("@estatus", 0);
                            cmd.ExecuteNonQuery();
                            cadena.Close();
                        }
                        catch (Exception ex)
                        {
                            string era = ex.Message;
                        }
                    }
                    else
                    {
                        acceso aces = new acceso();
                        DataTable dta = new DataTable();
                        dta = aces.Conectar("Select ruta from facturador where rfc = '" + trfc.Text+"'");
                        string xruta = dta.Rows[0].Field<string>("ruta");
                        files = @xruta + "\\" + anio + part.Text.ToString() + ".xml";
                        File.WriteAllBytes(@files, Convert.FromBase64String(zsale));

                        acceso acesr = new acceso();
                        DataTable dtr = new DataTable();
                        string dfuecd = anio + part.Text;
                        dtr = acesr.Conectar("Select * from fuecd where fuecd ='" + dfuecd + "'");
                        if (dtr.Rows.Count > 0)
                        {
                            string sql = string.Empty;
                            sql = "Update fuecd set fecha = @fecha,estatus=@estatus where fuecd=@fuecd";

                            SqlConnection cadena = new SqlConnection(zcadena);
                            cadena.Open();
                            SqlCommand cmd = cadena.CreateCommand();
                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("@fecha", xfecha.ToString("yyyy/MM/dd"));
                            cmd.Parameters.AddWithValue("@estatus", 0);
                            cmd.Parameters.AddWithValue("@fuecd", dfuecd);
                            cmd.ExecuteNonQuery();
                            cadena.Close();
                        }
                        else
                        {
                            string sql = string.Empty;
                            sql = "Insert into fuecd(fuecd,fecha,estatus) ";
                            sql += "VALUES(@fuecd,@fecha,@estatus) ";

                            SqlConnection cadena = new SqlConnection(zcadena);
                            cadena.Open();
                            SqlCommand cmd = cadena.CreateCommand();
                            //cmd.CommandTimeout .CommandTimeout = 500000;
                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("@fuecd", dfuecd);
                            cmd.Parameters.AddWithValue("@fecha", xfecha.ToString("yyyy/MM/dd"));
                            cmd.Parameters.AddWithValue("@estatus", 0);
                            cmd.ExecuteNonQuery();
                            cadena.Close();
                        }


                        Boolean siguardo = obtenerestado(files);
                        Application.DoEvents();
                        if (siguardo)
                        {
                            mensaje = "Proceso terminado";

                        }
                        else
                        {
                            mensaje = "Este documento ya fue procesado";
                        }
                    }

                }
                Application.DoEvents();
                button2.Enabled = true;
            }
            Cursor.Current = Cursors.Default;
            pprocesa.Visible = false;
            button2.Enabled = true;
            MessageBox.Show(mensaje);
        }

        private void pprocesa_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) PanelMouseDownLocation = e.Location;
        }

        private void pprocesa_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)

            {

                pprocesa.Left += e.X - PanelMouseDownLocation.X;

                pprocesa.Top += e.Y - PanelMouseDownLocation.Y;

            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;

            NotificaIcon.Visible = false;
        }

        private void NotificaIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;

            NotificaIcon.Visible = false;
        }

        private void NotificaIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;

            NotificaIcon.Visible = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                NotificaIcon.Visible = true; ;
                NotificaIcon.BalloonTipText = "El proceso continua en segundo plano";
                NotificaIcon.BalloonTipTitle = "Ammper EOSOL";
                NotificaIcon.BalloonTipIcon = ToolTipIcon.Info;
                NotificaIcon.ShowBalloonTip(3000);
            }
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {
            about aba = new about();
            aba.ShowDialog();
        }

        private void mparti_SelectedValueChanged(object sender, EventArgs e)
        {
            trfc.Text = mparti.Text;
        }

        private void mparti_SelectedIndexChanged(object sender, EventArgs e)
        {
            trfc.Text = mparti.Text;
            cargasub(trfc.Text);
        }

        //public static void databaseFilePut(string varFilePath,zcadena)
        //{
        //    byte[] file;
        //    using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
        //    {
        //        using (var reader = new BinaryReader(stream))
        //        {
        //            file = reader.ReadBytes((int)stream.Length);
        //        }
        //    }
        //    //using (var varConnection = Locale.sqlConnectOneTime(Locale.sqlDataConnectionDetails))
        //    //using (var sqlWrite = new SqlCommand("INSERT INTO Raporty (RaportPlik) Values(@File)", varConnection))
        //    //{
        //    //    sqlWrite.Parameters.Add("@File", SqlDbType.VarBinary, file.Length).Value = file;
        //    //    sqlWrite.ExecuteNonQuery();
        //    //}


        //    //string sql = string.Empty;
        //    //sql = "Insert into fuecd(fuecd,fecha,estatus) ";
        //    //sql += "VALUES(@fuecd,@fecha,@estatus) ";
        //    //string dfuecd = anio + item;
        //    //SqlConnection cadena = new SqlConnection(zcadena);
        //    //cadena.Open();
        //    //SqlCommand cmd = cadena.CreateCommand();
        //    ////cmd.CommandTimeout .CommandTimeout = 500000;
        //    //cmd.CommandText = sql;
        //    //cmd.Parameters.AddWithValue("@fuecd", dfuecd);
        //    //cmd.Parameters.AddWithValue("@fecha", xfecha.ToString("yyyy/MM/dd"));
        //    //cmd.Parameters.AddWithValue("@estatus", 0);
        //    //cmd.ExecuteNonQuery();
        //    //cadena.Close();
        //}
    }


}

public class respuestaWs
{
    public string exito { get; set; }
    public string uuid { get; set; }
    public string pdf { get; set; }
    public string xml { get; set; }
}


public class ValOriMod
{
    public string importeOri { get; set; }
    public string importeMod { get; set; }
    public string montoAjuste { get; set; }
}

public class ValSerFol
{
    public string serie { get; set; }
    public string folio { get; set; }
}
