using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de acceso
/// </summary>
public class acceso
{
    //string conectastring = "Data Source=DESKTOP-8II2M49;Initial Catalog=ppesc;User ID=sa;Password=sa";
   public string conectastring = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
    public acceso()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public DataTable Conectar(string sql)
    {
        try
        {
            SqlConnection cadena = new SqlConnection(conectastring);
            SqlCommand cmd = new SqlCommand(sql, cadena);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            cadena.Open();
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            cadena.Close();
            return dt;
        }
        catch (Exception ex)
        {
            String script = "<script>alert('" + ex.Message + "')</script>";
            //System.Web.HttpContext.Current.Response.Write(script);
            return null;
        }
    }

    public bool Actualiza(string sql)
    {
        bool resul = false;
        try
        {
            SqlConnection cadena = new SqlConnection(conectastring);

            cadena.Open();
            SqlCommand cmd = cadena.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            resul = true;
        }
        catch
        {
            resul = false;
        }
        return resul;
    }

    public bool delete(string sql)
    {
        bool resul = false;
        try
        {
            SqlConnection cadena = new SqlConnection(conectastring);

            cadena.Open();
            SqlCommand cmd = cadena.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            resul = true;
        }
        catch
        {
            resul = false;
        }
        return resul;
    }
}