using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;

public abstract class LogItem
{
    private string _mensaje;

    public string Mensaje
    {
        get { return _mensaje; }
        set { _mensaje = value; }
    }

    private int _tipo;

    public int Tipo
    {
        get { return _tipo; }
        set { _tipo = value; }
    }


    public abstract string SaveLog();

}

/// <summary>
/// Clase concreta para el almacenamiento el base de datos
/// </summary>
public class SaveDataBase : LogItem
{
    public override string SaveLog()
    {
        /*Se guarda en base de datos*/
        SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringLogDB"].ConnectionString);
        SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = "UP_INSERT_LOG";
        command.CommandType = System.Data.CommandType.StoredProcedure;

        SqlParameter parMsg = new SqlParameter();
        parMsg.ParameterName = "@MESSAGE";
        parMsg.Value = Mensaje;

        SqlParameter parType = new SqlParameter();
        parType.ParameterName = "@TYPE";
        parType.Value = Tipo;

        command.Parameters.Add(parMsg);
        command.Parameters.Add(parType);

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        catch (Exception ex)
        {
            StringBuilder sbError = new StringBuilder();

            sbError.Append("Error: ");
            sbError.Append(ex.Message);
            return sbError.ToString();

        }
        finally
        {
            if (connection.State != System.Data.ConnectionState.Closed)
                connection.Close();
        }
        return "Ok";
    }
}

/// <summary>
/// Clase concreta para el almacenamiento el archivo
/// </summary>
public class SaveFile : LogItem
{
    public override string SaveLog()
    {
        /*Se guarda en archivo*/
        StringBuilder logText = new StringBuilder();
        StringBuilder sbFileName = new StringBuilder();
        sbFileName.Append(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"]);
        sbFileName.Append("LogFile-");
        sbFileName.Append(DateTime.Now.ToShortDateString().Replace("/", "-"));
        sbFileName.Append(".txt");

        logText.Append(DateTime.Now.ToShortDateString());
        logText.Append("TYPE: ");
        logText.Append(Tipo.ToString());
        logText.Append(Mensaje);

        try
        {
            if (!File.Exists(sbFileName.ToString()))
                File.Create(sbFileName.ToString()).Dispose();

            using (var sw = new StreamWriter(sbFileName.ToString(), true))
            {
                sw.WriteLine(logText.ToString());
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return "Ok";
    }
}

/// <summary>
/// Clase factoria para los objetos LogItem
/// </summary>
public class LogFactory
{
    public static LogItem getLogItem(string tipo)
    {
        if (tipo.Equals("BD"))
            return new SaveDataBase();
        else
            return new SaveFile();
    }
}