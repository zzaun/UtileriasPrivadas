using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace UtileriasSSMS;

public class ConexionSSMS {
    /*
        Parametros para conexion fija
    */
    /// <summary>
    /// Parametos para una conexion fija
    /// </summary>
    public string? strServer;
    public string? strDataBase;
    public string? strNameUser;
    public string? strPassword;
    /*
        Conexiones
    */
    /// <summary>
    /// Conexion fija, que nesesita los parametos del usuario
    /// </summary>
    /// <returns>Regresa una conexion</returns>
    public SqlConnection GetConexion(){
        string strConexion = $"Server={strServer};Database={strDataBase};User Id={strNameUser};Password={strPassword};";
        SqlConnection conexionSSMS = new SqlConnection(strConexion);
        return conexionSSMS;
    }
    /// <summary>
    /// Conexion Fija, que no nesesita parametros de usuario
    /// </summary>
    /// <param name="sinNameUser">Confirmar que no nesesita paramatros de usuario</param>
    /// <returns>Regresar una conexion</returns>
    public SqlConnection GetConexion(bool sinNameUser){
        string strConexion = $"Server={strServer};Database={strDataBase};Integrated Security=True;";
        SqlConnection conexionSSMS = new SqlConnection(strConexion);
        return conexionSSMS;
    }
    /// <summary>
    /// Conexion independiente y acoplabe
    /// </summary>
    /// <param name="notUser">Se nesesita parametros de usuario</param>
    /// <param name="strServer">nombre del servidor</param>
    /// <param name="strDataBase">nombre de la base de datos</param>
    /// <param name="strNameUser">nombre del usuario administrador</param>
    /// <param name="strPassword">contraseña del usuario</param>
    /// <returns>Regresar un conexion, rapida</returns>
    public static SqlConnection GetConexion(bool notUser = false, string strServer = "host", string strDataBase = "DB", string strNameUser = "root", string strPassword = "1234"){
        if(notUser){
            return new SqlConnection($"Server={strServer};Database={strDataBase};Integrated Security=True;");
        }
        else{
            return new SqlConnection($"Server={strServer};Database={strDataBase};User Id={strNameUser};Password={strPassword};");
        }
    }
    /*
        Funciones CMD
    */
    /// <summary>
    /// Ejecuta un comando, y regresa los resultados de la consulta y los datos de la ejecucion (clsResultados)
    /// </summary>
    /// <param name="results">Revisa los resultados de la ejecucion y notifica si ubo algun error</param>
    /// <param name="strComando">Es el comando que se quiere ejecutar</param>
    /// <param name="conexion">Es la conexion a Host y DataBase que se quiere utilizar</param>
    /// <returns>Regresar un DataTable con la consulta y un clsResultados con informacion</returns>
    public static DataTable EjecutarCMD(ref clsResultados results, string strComando, SqlConnection conexion) {
        DataTable dt = new DataTable();
        try {
            conexion.Open();
            results.booError = false;
            results.intNoErrores = 0;
            results.strMensaje = "Conexion Exitosa";
            results.intFilasAfectadas = 0;
        } 
        catch (SqlException e) {
            conexion.Close();
            results.booError = true;
            results.intNoErrores = e.ErrorCode;
            results.strMensaje = e.Message;
            results.intFilasAfectadas = 0;
            return dt;
        }
        SqlDataAdapter adapter = new SqlDataAdapter(strComando, conexion);
        try {
            results.intFilasAfectadas = adapter.Fill(dt);
            results.booError = false;
            results.intNoErrores = 0;
            results.strMensaje = "Comando Ejecutado con exito";
        }
        catch (SqlException e) {
            conexion.Close();
            results.booError = true;
            results.intNoErrores = e.ErrorCode;
            results.strMensaje = e.Message;
            results.intFilasAfectadas += 0;
            return dt;
        }
        conexion.Close();
        return dt;
    }
}
// string connectionString = "Server=localhost;Database=MiBaseDeDatos;Integrated Security=True;";