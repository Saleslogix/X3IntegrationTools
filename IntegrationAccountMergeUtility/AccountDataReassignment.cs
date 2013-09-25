using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Windows.Forms;
using log4net;
using Sage.Platform.Data;
using Sage.Platform.Orm;

namespace IntegrationAccountMergeUtility
{
    public partial class AccountDataReassignment : Form
    {
        private static ConnectionStringDataService dataService;
        private static readonly ILog logger = LogManager.GetLogger("SDataSync");

        public AccountDataReassignment()
        {
            InitializeComponent();
            SetupApplicationContext();
        }

        private static void SetupApplicationContext()
        {
            try
            {
                Sage.Platform.Application.ApplicationContext.Initialize("SalesLogix");

                var conString = System.Configuration.ConfigurationManager.ConnectionStrings[0].ConnectionString;
                dataService = new ConnectionStringDataService(conString);
                var conn = dataService.GetConnection();
                
                conn.Open();
                conn.Close();
                Sage.Platform.Application.ApplicationContext.Current.Services.Add(typeof(IDataService), dataService);


            }
            catch (Exception e)
            {
                var message = e.Message;
                if (e is OleDbException)
                {
                    message = string.Format(Properties.Resources.Error_Invalid_Connection, dataService.Server,
                                            dataService.Database);
                    logger.Error(message);
                }
                else
                    logger.Error(e.Message, e);
                throw new ApplicationException(message);
            }
        }

        private void ExecuteCmd(IDbCommand cmd, string sql)
        {
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                logger.ErrorFormat("AccountDataReassignment - Statement failed: {0}  ERROR: {1}", sql, e.Message);
            }
        }

        private void MoveData(SessionScopeWrapper session, string source, string target)
        {
            var cmd = session.Connection.CreateCommand();
            var cmd2 = session.Connection.CreateCommand();
            var delCmd = session.Connection.CreateCommand();
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                ExecuteCmd(cmd,string.Format("UPDATE ACTIVITY SET ACCOUNTID = '{0}' WHERE ACCOUNTID = '{1}'", target, source));
                ExecuteCmd(cmd,cmd.CommandText = string.Format("UPDATE HISTORY SET ACCOUNTID = '{0}' WHERE ACCOUNTID = '{1}'", target, source));
                ExecuteCmd(cmd,cmd.CommandText = string.Format("UPDATE ATTACHMENT SET ACCOUNTID = '{0}' WHERE ACCOUNTID = '{1}'", target, source));

                cmd.CommandText = string.Format("SELECT CONTACTID, LASTNAME FROM CONTACT WHERE ACCOUNTID = '{0}'", target);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader != null && reader.Read())
                    {
                        try
                        {

                            cmd2.CommandText =
                                string.Format(
                                    "SELECT CONTACTID FROM CONTACT WHERE ACCOUNTID = '{0}' AND LASTNAME = '{1}'",
                                    source, reader.GetString(1));

                            using (var reader2 = cmd2.ExecuteReader())
                            {
                                while (reader2 != null && reader2.Read())
                                {
                                    MoveContactData(session, reader2.GetString(0), reader.GetString(0));
                                    ExecuteCmd(delCmd,string.Format("delete from contact where contactid='{0}'", reader2.GetString(0)));
                                }
                            }
                        }
                        catch (Exception innerError)
                        {
                            logger.Error("Moving contact data to X3 contact: " +innerError);
                        }
                    }
                }

                if (cbNewContact.Checked)
                {
                    ExecuteCmd(cmd,string.Format("UPDATE CONTACT SET ACCOUNTID = '{0}' WHERE ACCOUNTID = '{1}'", target, source));

                    //Assign UUIDs to new contacts that are not currently linked.
                    ExecuteCmd(cmd,string.Format("UPDATE CONTACT SET GLOBALSYNCID = CONVERT(varchar(36), NEWID()) WHERE ACCOUNTID = '{0}' AND GLOBALSYNCID IS NULL", target));
                }
                else
                {  // Contacts are not moved to X3 account, but are logged to a csv file to be reviewed.
                    cmd.CommandText = string.Format("SELECT FIRSTNAME,LASTNAME,ACCOUNT,TITLE,WORKPHONE,HOMEPHONE,FAX,MOBILE,EMAIL FROM CONTACT WHERE ACCOUNTID = '{0}'", source);
                    using (var reader = cmd.ExecuteReader())
                    {
                        var sb = new StringBuilder();
                        while (reader != null && reader.Read())
                        {
                            try
                            {
                                sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", GetFieldAsString(reader, 0),
                                                            GetFieldAsString(reader, 1), GetFieldAsString(reader, 2), GetFieldAsString(reader, 3),
                                                            GetFieldAsString(reader, 4), GetFieldAsString(reader, 5), GetFieldAsString(reader, 6),
                                                            GetFieldAsString(reader, 7), GetFieldAsString(reader, 8)));
                            }
                            catch (Exception innerError)
                            {
                                logger.Error("Logging New Contacts not moved: " + innerError);
                            }
                        }
                        File.AppendAllText(@"c:\temp\Contacts Not Moved.csv", sb.ToString());
                    }
                }

                if (cbDelete.Checked)
                {
                    ExecuteCmd(cmd,string.Format("DELETE FROM ACCOUNT WHERE ACCOUNTID = '{0}'", source));
                }
            }
            catch (Exception error)
            {
                logger.Error("Moving account data: " + error);
            }
        }

        private string GetFieldAsString(IDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return string.Empty;
            return reader.GetString(index);
        }

        private void MoveContactData(SessionScopeWrapper session, string source, string target)
        {
            var cmd = session.Connection.CreateCommand();
            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                ExecuteCmd(cmd,string.Format("UPDATE ACTIVITY SET CONTACTID = '{0}' WHERE CONTACTID = '{1}'", target, source));
                ExecuteCmd(cmd,string.Format("UPDATE HISTORY SET CONTACTID = '{0}' WHERE CONTACTID = '{1}'", target, source));
                ExecuteCmd(cmd,string.Format("UPDATE ATTACHMENT SET CONTACTID = '{0}' WHERE CONTACTID = '{1}'", target, source));


                if (cbDelete.Checked)
                {
                    ExecuteCmd(cmd,string.Format("DELETE FROM CONTACT WHERE CONTACTID = '{0}'", source));
                }
            }
            catch (Exception error)
            {
                logger.Error(error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            lblStatus.Text = Properties.Resources.Status_New_Session;
            Application.DoEvents();
            using (var session = new SessionScopeWrapper())
            {
                using (var cmd = session.Connection.CreateCommand())
                {
                    lblStatus.Text = Properties.Resources.Status_Account_Loading;
                    Application.DoEvents();
                    var sql = "SELECT ACCOUNTID,ACCOUNT FROM SYSDBA.ACCOUNT WHERE GLOBALSYNCID IS NOT NULL";
                    cmd.CommandText = sql;
                    var count = 0;
                    using (var reader = cmd.ExecuteReader())
                    {
                        using (var cmd2 = session.Connection.CreateCommand())
                        {
                            lblStatus.Text = Properties.Resources.Status_Account_Processing;
                            Application.DoEvents();
                            //foreach (IAccount account in accounts)
                            while (reader != null && reader.Read())
                            {
                                count++;
                                var status = 0;
                                Math.DivRem(count, 3, out status);
                                if (status == 0)
                                {
                                    txtAccountChanges.Text = count.ToString();
                                    Application.DoEvents();
                                }
                                var target = reader.GetString(0);
                                try
                                {

                                    cmd2.CommandText = string.Format("SELECT ACCOUNTID FROM ACCOUNT WHERE ACCOUNT = '{0}' and GLOBALSYNCID IS NULL",
                                            reader.GetString(1));
                                    using (var reader2 = cmd2.ExecuteReader())
                                    {
                                        var id = "";
                                        var innerCount = 0;
                                        while (reader2 != null && reader2.Read())
                                        {
                                            innerCount++;
                                            if (innerCount == 1)
                                            {id = reader2.GetString(0);}
                                        }
                                        if (innerCount == 1)
                                        {MoveData(session, id, target);}
                                    }
                                }
                                catch (Exception error)
                                {
                                    logger.Error(error);
                                }
                            }

                            txtAccountChanges.Text = count.ToString();
                            Application.DoEvents();
                        }
                    }


                }

            }
        }

    }
}
