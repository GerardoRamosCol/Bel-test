using System;
using System.Text;
using System.IO;
using System.Data.SqlClient;

public class JobLoggerCorrected
{
    private static bool _logToFile;
    private static bool _logToConsole;
    private static bool _logMessage;
    private static bool _logWarning;
    private static bool _logError;
    private static bool _logToDatabase;

    enum LogType { Message = 1, Warning, Error };


    public JobLoggerCorrected(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessage, bool logWarning, bool logError)
    {
        _logError = logError;
        _logMessage = logMessage;
        _logWarning = logWarning;
        _logToDatabase = logToDatabase;
        _logToFile = logToFile;
        _logToConsole = logToConsole;
    }

    public string LogMessage(string message)
    {
        message = message.Trim();
        if (message == null || message.Length == 0)
            return "Message could not be null or empty";

        if (!_logToConsole && !_logToFile && !_logToDatabase)
            return "Invalid configuration";

        if (!_logError && !_logMessage && !_logWarning)
            return "Error or Warning or Message must be specified";

        int type = 0;
        if (_logMessage)
        {
            type = (int)LogType.Message;
        }
        if (_logError)
        {
            type = (int)LogType.Error;
        }
        if (_logWarning)
        {
            type = (int)LogType.Warning;
        }

        string state = string.Empty;

        if (_logToDatabase)
        {
            state = SaveOnDatabase(message, type);

            if (state != "Ok")
                return state;
        }

        if (_logToConsole)
            OnConsole(message);

        if (_logToFile)
        {
            state = SaveOnFile(message, type);

            if (state != "Ok")
                return state;
        }

        return "Ok";
    }

    /// <summary>
    /// Funciotn to show og messasge on Console
    /// </summary>
    /// <param name="message">Message to show</param>
    private void OnConsole(string message)
    {
        if (_logError)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        if (_logWarning)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        if (_logMessage)
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        StringBuilder sbtext = new StringBuilder();
        sbtext.Append(DateTime.Now.ToShortDateString());
        sbtext.Append(": ");
        sbtext.Append(message);

        Console.WriteLine(sbtext.ToString());
        Console.ReadLine();
    }

    /// <summary>
    /// Function so save the message to file
    /// </summary>
    /// <param name="message">Message to save</param>
    /// <param name="type">int with the value associate to the log type</param>
    private string SaveOnFile(string message, int type)
    {
        StringBuilder logText = new StringBuilder();
        StringBuilder sbFileName = new StringBuilder();
        sbFileName.Append(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"]);
        sbFileName.Append("LogFile-");
        sbFileName.Append(DateTime.Now.ToShortDateString().Replace("/","-"));
        sbFileName.Append(".txt");

        logText.Append(DateTime.Now.ToShortDateString());
        logText.Append("TYPE: ");
        logText.Append(type.ToString());
        logText.Append(message);

        try
        {
            if (!File.Exists(sbFileName.ToString()))
                File.Create(sbFileName.ToString()).Dispose();

            using (var sw = new StreamWriter(sbFileName.ToString(),true))
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

    /// <summary>
    /// Function to save the message on database
    /// </summary>
    /// <param name="message">Message to save</param>
    /// <param name="type">int with the value associate to the log type</param>
    private string SaveOnDatabase(string message, int type)
    {
        SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringLogDB"].ConnectionString);
        SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = "UP_INSERT_LOG";
        command.CommandType = System.Data.CommandType.StoredProcedure;

        SqlParameter parMsg = new SqlParameter();
        parMsg.ParameterName = "@MESSAGE";
        parMsg.Value = message;

        SqlParameter parType = new SqlParameter();
        parType.ParameterName = "@TYPE";
        parType.Value = type;

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

            Console.WriteLine(sbError.ToString());
            Console.ReadLine();

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

