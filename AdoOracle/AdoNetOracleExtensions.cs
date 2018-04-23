using AdoNetTemplate;
using AdoNetTemplate.Database.Core;
using AdoNetTemplate.Database.Support;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdoOracle
{


    /// <summary>
    /// NOT COMPLETE 
    /// TODO
    /// </summary>

    public class AdoNetOracleExtensions
    {
        static OracleConnection conn; 
        static public void Executequery(string ConnectionString)
        {
            var qry = "select ROWID , col_id from DUMMY_TABLE";
            using (conn = new OracleConnection(ConnectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(qry, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.AddRowid = true;

                    // create the dependency object
                    // note this does not perform the registration
                    // it only defines the relationship
                    OracleDependency dep = new OracleDependency(cmd);

                    // Set the port number for the listener to listen for the notification
                    // request
                    OracleDependency.Port = 1005;

                    // set notification to persist after first notification is received
                    // this will allow all notifications to be received instead of just
                    // a single notification
                    cmd.Notification.IsNotifiedOnce = false;
                    cmd.NotificationAutoEnlist = true;

                    // define the event handler to invoke when the change notification
                    // is received
                    dep.OnChange += new OnChangeEventHandler(OnDatabaseNotification);

                    // execute the command (ignore the actual result set here)
                    // this performs the registration that was defined when
                    // the dependency object was created
                    cmd.ExecuteNonQuery();

                    // simply loop forever waiting for the notification from the database
                    // you need to ctrl+c from the console
                    // or Stop Debugging if running from within Visual Studio
                    while (true)
                    {
                        Console.WriteLine("Waiting for notification...");
                        Thread.Sleep(2000);
                    }
                }
            }
        }
        

        public static void OnDatabaseNotification(object src, OracleNotificationEventArgs args)
        {
            // this method is invoked each time a change notification
            // is received from the database

            // sql statement to retrieve changed data using the rowid
            // including the rowid here is not required but is
            // informational in nature
            string sql = "select ROWID , col_id from DUMMY_TABLE where rowid = :1";

            // create parameter object to hold the rowid
            // get the rowid from the OracleNotificationEventArgs
            // parameter to this method
            // this assumes there is a single row updated which is the
            // case in this sample
            OracleParameter p_rowid = new OracleParameter() ;
            p_rowid.Value = args.Details.Rows[0]["rowid"];

            // command to retrieve new data
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(p_rowid);

            // execute the command
            OracleDataReader dr = cmd.ExecuteReader();

            // get the data
            dr.Read();

            // output a simple message with the resource name
            Console.WriteLine();
            Console.WriteLine("Database Change Notification received!");
            DataTable changeDetails = args.Details;
            Console.WriteLine("Resource {0} has changed.", changeDetails.Rows[0]["ResourceName"]);

            // display the new data
            Console.WriteLine();
            Console.WriteLine("  New Data:");
            Console.WriteLine("     Rowid: {0}", dr.GetString(0));
            Console.WriteLine("First Name: {0}", dr.GetString(1));
            Console.WriteLine(" Last Name: {0}", dr.GetString(2));
            Console.WriteLine("    Salary: {0}", dr.GetDecimal(3).ToString());
            Console.WriteLine();

            // clean-up
            dr.Dispose();
            cmd.Dispose();
            p_rowid.Dispose();
        }
    }
}
