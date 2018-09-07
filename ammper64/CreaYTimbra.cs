using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ammper64
{

    class CreaYTimbra
    {
        private string Folio = "";
        public string CadenaDatosCFDI(DateTime dtahora, decimal tneto, decimal total, string metododepago,String Tcomprobante,String serie,String folios,string lexped,string uuid,string rela)
        {
            string respCadena = "";
            string trelacion = "";
            try
            {
                string lg_cp = lexped;
                int _folio = 0;

                if (rela != "")
                {
                    trelacion = "%" + rela + "/" + uuid;
                }
                else {
                    trelacion = "";
                }
                _folio = Convert.ToInt32(folios);

                if (_folio > 0)
                {
                    Folio = _folio.ToString();

                    respCadena = "#01@|" +
                         "|99|" //2.- Forma de Pago
                        + string.Format("{0:0.00}", tneto) // 3.- Subtotal
                        + "|||MXN|" // 4.- Descuento, 5.-Condiciones de Pago, 6.- Moneda
                        + string.Format("{0:0.00}", total) // 7.- Total
                        + "|"+ Tcomprobante +"| " // 8.- Tipo de Comprobante
                        + metododepago // 9.- Metodo de Pago
                        + "|" + lg_cp // 10.- Lugar de Expedicion CP
                        + "|"+serie+"|" // 11.- Serie
                        + Folio //12.- Folio
                        + "|1||"+trelacion+"| "; // 13.- Tipo de Cambio, 14.- Codigo de Confirmación, 15.- Cadena CfdiRelacionados
                }
            }
            catch (Exception ex)
            {
                //insertaLog("btnFacturar", "CadenaDatosCFDI", "Error", ex.Message, true);
            }

            return respCadena;
        }

        public string CadenaDatosReceptorLayOut(string rfc, string nombre_razons, string Usoc)
        {
            string respCadena = "";
            string rsoc = "";

            rsoc = nombre_razons;

            respCadena = "#02@|" + rfc // 1.- RFC
                + "|" + rsoc // 2.- Nombre
                + "|||" + Usoc + "| "; // 3.- Resisdencia Fiscal, 4.- NumRegIdTrib, 5.- Uso CFDI
            return respCadena;
        }

        public string cadenaconceptos(decimal neto,decimal impuesto,String descripcion,string NoIdenti,string tfactura)
        {
            
            decimal ximpuesto = 0;
            decimal xneto = 0;
            string claveproducto = "83101800";
            string ClaveUnidad = "ZZ";
            string Cunidad = "ZZ";
            string NoIdentificacion = NoIdenti;
            string tra_ = "%TRA/";
            string ret_ = "%RET/";
            String Concepto = "";

            // busco la cantidad con el NoIdenti en la tabla anexos
            acceso aces = new acceso();
            DataTable dt = new DataTable();
            String sql = "Select sum(potencia) as potencia, sum(potencia_mtr) as mtr, sum(potencia_mda) as mda from wsanexos where ful = '" + NoIdenti + "'";
            dt = aces.Conectar(sql);

            Decimal pot = dt.Rows[0].Field<decimal>("potencia");
            Decimal mtr = dt.Rows[0].Field<decimal>("mtr");
            Decimal mda = dt.Rows[0].Field<decimal>("mda");

            Decimal totcant = 0;

            if (pot > 0)
            {
                totcant = pot;
            }

            if (mtr > 0)
            {
                totcant = mtr - mda;
            }
            else {
                totcant = mda;
            }

            totcant = Convert.ToDecimal(totcant.ToString("#.##"));

            
            Decimal totprecunidad = Decimal.Round((neto / totcant),2);

            String totcantS = String.Format("{0:0.00}", totprecunidad);

            if (descripcion.Contains("Compra de la energía"))
            {
                ClaveUnidad = "MWH";
                Cunidad = "MEGAWATT HORA";
            }
            else {
                if (descripcion.Contains("Venta de energía generada"))
                {
                    ClaveUnidad = "MWH";
                    Cunidad = "MEGAWATT HORA";
                }
                else
                {
                    ClaveUnidad = "ZZ";
                    Cunidad = "Mutuamente definido";
                }
            }

            ximpuesto = impuesto;
            xneto = neto;

            decimal rneto = 0;
            decimal rimpuesto = 0;


            tra_ = tra_ + string.Format("{0:0.00}",xneto) + "/002/Tasa/0.160000/" + string.Format("{0:0.00}", ximpuesto);
            //tra_ = tra_ + "16.00/002/Tasa/0.160000/" + string.Format("{0:0.00}", ximpuesto);

            //ret_ = ret_ + string.Format("{0:0.00}",rneto) + "/002/Tasa/0.160000/" + string.Format("{0:0.00}", rimpuesto);
            //tra_ = tra_ + "%RET/"+ string.Format("{0:0.00}", rneto) +"/002/Tasa/0.16/"+ string.Format("{0:0.00}", rneto);
            switch (tfactura)
            {
                case "P0":
                    Concepto = Concepto + "#03@|" + totcant + "| " + Cunidad + "|" + descripcion + "|"
                                 + totprecunidad + "|" + string.Format("{0:0.00}", xneto) + "|" + NoIdentificacion
                      + "|" + claveproducto + "|" + ClaveUnidad + "|| "
                      + tra_ + "||||";
                    break;
                case "PD":
                    Concepto = Concepto + "#03@|" + "1.00" + "| " + Cunidad + "|" + descripcion + "|"
                               + string.Format("{0:0.00}", xneto) + "|" + string.Format("{0:0.00}", xneto) + "|" + NoIdentificacion
                    + "|" + claveproducto + "|" + ClaveUnidad + "|| "
                    + tra_ + "||||";
                    break;
                case "PC":
                    Concepto = Concepto + "#03@|" + "1.00" + "| " + Cunidad + "|" + descripcion + "|"
                                + string.Format("{0:0.00}", xneto) + "|" + string.Format("{0:0.00}", xneto) + "|" + NoIdentificacion
                     + "|" + claveproducto + "|" + ClaveUnidad + "|| "
                     + tra_ + "||||";
                    break;            
            }
            //Concepto = Concepto + "#03@|"+ totcant +"| "+Cunidad+"|"+ descripcion+ "|"
            //                     + totprecunidad + "|" + string.Format("{0:0.00}", xneto) + "|" + NoIdentificacion
            //          + "|" + claveproducto + "|" + ClaveUnidad + "|| "
            //          + tra_ + "||||";

            return Concepto;
        }

        public string Cadena04A(decimal importe)
        {
            decimal impuesto = 0;
            decimal Rimpuesto = 0;
            decimal neto = 0;
            //string _04A = "#04@|002|"+ string.Format("{0:0.00}", Rimpuesto) +"|#04A@|0.160000|002|";
            string _04A = "#04A@|0.160000|002|";

            if (importe > 0)
            {
                neto = importe / Convert.ToDecimal("1.16");
                impuesto = importe - neto;
            }

            string _importe_tras = (importe / Convert.ToDecimal("1.16")).ToString();
              
            return _04A + string.Format("{0:0.00}", importe) + "|Tasa|#04TI@|" + string.Format("{0:0.00}", importe) + "|"+ string.Format("{0:0.00}", Rimpuesto) +"| ";
        }

        //public string Cadena04A(decimal importe)
        //{
        //    decimal impuesto = 0;
        //    decimal neto = 0;
        //    string _04A = "#04A@|0.160000|002|";

        //    if (importe > 0)
        //    {
        //        neto = importe / Convert.ToDecimal("1.16");
        //        impuesto = importe - neto;
        //    }

        //    string _importe_tras = (importe / Convert.ToDecimal("1.16")).ToString();


        //    return _04A + string.Format("{0:0.00}", importe) + "|Tasa|#04TI@|0.00|";
        //}
    }
}
