using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;

namespace NetworkGraph.Models
{
    public class WorkspaceInfo
    {

        public int ArtifactID = 0;
        public HttpSessionStateBase Session;
        public NameObjectCollectionBase.KeysCollection Keys;
        public String SQLStatement;
        public String CustomLastError;

        public String WorkspaceDatabaseName = "";
        public String WorkspaceIsMasterDatabase = "";
        public String WorkspaceServerName = "";

        public String MasterDatabaseName = "";
        public String MasterIsMasterDatabase = "";
        public String MasterServerName = "";

        public String CaseName = "";
        public String SocialNetworkDatabaseName = "";

        public Int64 RootCommunicator = 0;
        public String Uri = "";
        public String BaseUri = "";


        public String AuthenticationToken = "";
        public int IsInRelativity = 0;

        public List<Int64> EdgeDocuments;

        public int ActiveUserArtifactID = 0;
        public int ActiveUserAuditArtifactID = 0;
        public int ActiveUserWorkspaceArtifactID = 0;

        List<MyNode> nodes = new List<MyNode>();
        List<MyEdge> edges = new List<MyEdge>();
        Dictionary<long, string> dictNodes = new Dictionary<long, string>();

        public WorkspaceInfo()
        {

        }



        public void GetWorkspaceInformation()
        {
            int WorkspaceArtifactID = -1;
            
            try
            {
                WorkspaceArtifactID = -1;
            }
            catch (Exception e)
            {
                string eMessage = e.Message;
            }

            if (WorkspaceArtifactID == -1)
            {

                this.ArtifactID = 7777777;
                this.CaseName = "Social Network Demo";
                this.SocialNetworkDatabaseName = "SocialNetwork";
                this.WorkspaceServerName = "localhost";
                this.MasterServerName = "localhost";
                this.WorkspaceDatabaseName = "EDDS6666666";
                this.CustomLastError = "No Error";
                this.WorkspaceIsMasterDatabase = "false";
                this.MasterDatabaseName = "EDDS";
                this.MasterIsMasterDatabase = "true";
                this.AuthenticationToken = "xxx-xxx-xxx-xxx-xxx";
                this.RootCommunicator = 55555;
                this.IsInRelativity = 0;
                this.BaseUri = "http://localhost.com/Relativity/External.aspx";
                this.ActiveUserArtifactID = 7777778;
                this.ActiveUserAuditArtifactID = 7777778;
                this.ActiveUserWorkspaceArtifactID = 7777778;

                string connString =
                String.Format("Data Source={0};" +
                "Initial Catalog={1};" +
                "User id=service_user;" +
                "Password=service_password;", this.WorkspaceServerName, this.WorkspaceDatabaseName);

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch
                    {

                    }
                    this.RootCommunicator = 55555;
                }
                                
                return;
            }

            else
            {

                RelativityCommon.RelativityCommon relativity = new RelativityCommon.RelativityCommon();
                relativity.WorkspaceArtifactID = WorkspaceArtifactID;
                int intResults = relativity.RunCustomPage();

                //Dynamically retrieved workspace variables.
                this.ArtifactID = WorkspaceArtifactID;
                this.SocialNetworkDatabaseName = "SocialNetwork_EDDS" + WorkspaceArtifactID.ToString();
                // Eventually program flow will get to JavaScript. Ensure that there is a backslash between the server and instance
                // if needed.
                //workspaceInfo.AuthenticationToken = ConnectionHelper.Helper().GetAuthenticationManager().GetAuthenticationToken();
                this.AuthenticationToken = relativity.AuthenticationToken;


                //workspaceInfo.WorkspaceServerName = ConnectionHelper.Helper().GetDBContext(WorkspaceArtifactID).ServerName.Replace("\\", "\\\\");
                this.WorkspaceServerName = relativity.WorkspaceServerName.Replace("\\", "\\\\");

                //workspaceInfo.MasterServerName = ConnectionHelper.Helper().GetDBContext(-1).ServerName.Replace("\\", "\\\\");
                this.MasterServerName = relativity.MasterServerName.Replace("\\", "\\\\");

                //workspaceInfo.WorkspaceDatabaseName = ConnectionHelper.Helper().GetDBContext(WorkspaceArtifactID).Database.Replace("\\", "\\\\");
                this.WorkspaceDatabaseName = relativity.WorkspaceDatabaseName;

                //workspaceInfo.CaseName = WorkspaceInfo.GetRootCommunicator(ConnectionHelper.Helper().GetDBContext(WorkspaceArtifactID).GetConnection(), workspaceInfo.SocialNetworkDatabaseName).ToString();
                //this.Keys = HttpContext.Session.Keys;

                this.CustomLastError = relativity.CustomLastError;
                this.WorkspaceIsMasterDatabase = relativity.WorkspaceIsMasterDatabase;
                this.MasterDatabaseName = relativity.MasterDatabaseName;
                this.MasterIsMasterDatabase = relativity.MasterIsMasterDatabase;
                this.CaseName = relativity.CaseName;

                this.ActiveUserArtifactID = relativity.ActiveUserArtifactID;
                this.ActiveUserAuditArtifactID = relativity.ActiveUserAuditArtifactID;
                this.ActiveUserWorkspaceArtifactID = relativity.ActiveUserWorkspaceArtifactID;

                //this.RootCommunicator = WorkspaceInfo.GetRootCommunicator(ConnectionHelper.Helper().GetDBContext(WorkspaceArtifactID).GetConnection(), this.SocialNetworkDatabaseName);
                this.RootCommunicator = 55555;

                this.IsInRelativity = 0;
                // this.BaseUri = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("CustomPages/")) + "External.aspx";
                this.BaseUri = HttpContext.Current.Request.Url.AbsoluteUri;

            }
        }

        public void SetSession(System.Web.HttpSessionStateBase session)
        {
            this.Session = session;
            this.Keys = session.Keys;
        }

        public static Int64 GetRootCommunicator(SqlConnection connection, string SocialNetworkDatabase)
        {

            connection.ChangeDatabase(SocialNetworkDatabase);

            string cmdString = "SELECT TOP 1 EntityID FROM simple_SocialNetwork WHERE EntityType = 1 GROUP BY EntityID  ORDER BY COUNT(*) DESC";
            //string cmdString = "SELECT DB_NAME()";

            Int64 output = 666;

            using (SqlCommand cmd = new SqlCommand(cmdString, connection))
            {

                try
                {

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {

                        while (rdr.Read())
                        {
                            output = rdr.GetInt64(0); ;
                        }
                    }
                }
                catch (Exception e)
                {
                    string eMessage = e.Message;
                }

            }

            return output;
        } //GetRootCommunicator

        public SqlConnection GetAndOpenSQLConnection()
        {
            SqlConnection connection = null;

            int WorkspaceArtifactID = -1;

            try
            {
                WorkspaceArtifactID = -1;
            }
            catch (Exception e)
            {
                string eMessage = e.Message;
            }

            if (WorkspaceArtifactID == -1)
            {
                // Not in Relativity
                string connString =
                    String.Format("Data Source={0};" +
                    "Initial Catalog={1};" +
                    "User id=service_user;" +
                    "Password=service_password;", this.WorkspaceServerName, this.WorkspaceDatabaseName);

                try
                {
                    connection = new SqlConnection(connString);
                    if (connection.State != System.Data.ConnectionState.Open)
                    {
                        connection.Open();
                    }
                }
                catch (Exception e)
                {
                    string eMessage = e.Message;
                }

            }
            else
            {
                // In Relativity
                /*
                connection = ConnectionHelper.Helper().GetDBContext(WorkspaceArtifactID).GetConnection();
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                */
            }

            return connection;

        } // GetAndOpenSQLConnection



        public string DoEdgeClicked(string EdgeID = "", SqlConnection connection = null, string SocialNetworkDatabaseName = null)
        {
            //Populates EdgeDocuments List<Int64>
            GetDocumentsFromEdge(EdgeID, connection, SocialNetworkDatabaseName);
            HandleEdgeDocumentsInRelativity(this.EdgeDocuments);
            return GetViewURL() + "@@" + this.CustomLastError;
        }
                

        public List<Int64> GetDocumentsFromEdge(string EdgeID = "", SqlConnection connection = null, string SocialNetworkDatabaseName = null)
        {
            
            EdgeDocuments = new List<Int64>();

            string[] EdgeParts = EdgeID.Split('-');
            int SenderEntityID = Int32.Parse(EdgeParts[0]);
            int RecipientEntityID = Int32.Parse(EdgeParts[1]);

            string cmdString =
                "usp_SN_GetDocumentsFromEdge";

            // The database starts as the workspace database. Change to the social networking database.
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            using (connection)
            {

                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SenderEntityID", SqlDbType.Int).Value = SenderEntityID;
                    cmd.Parameters.Add("@RecipientEntityID", SqlDbType.Int).Value = RecipientEntityID;

                    try
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {

                                Int64 ArtifactID = rdr.GetInt64(rdr.GetOrdinal("ArtifactID"));
                                EdgeDocuments.Add(ArtifactID);
                            }

                        }
                    }

                    catch (SqlException e)
                    {

                        this.CustomLastError += "@@" + e.Message;
                        throw e;
                        //string eMessage = e.Message;
                        // do something with the exception
                        // don't hide it
                    }

                }
            }

            return EdgeDocuments;

        } // GetDocumentsFromEdge


        private void HandleEdgeDocumentsInRelativity(List<Int64> ArtifactIDs)
        {

            int RdoArtifactID = 0;
            string Name = "";
            int User = 0;
            bool RdoExists = false;

            //SqlConnection connection = ConnectionHelper.Helper().GetDBContext(workspaceInfo.ArtifactID).GetConnection();
            using (SqlConnection connection = this.GetAndOpenSQLConnection())
            {
                if (!(connection.State == System.Data.ConnectionState.Open))
                {
                    connection.Open();
                }

                string cmdString =
                    String.Format(@"SELECT TOP 1 ArtifactID, Name, ISNULL([User],-1) as TheUser FROM EDDSDBO.EdgeDocuments WHERE [User] = {0}", this.ActiveUserWorkspaceArtifactID);

                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    try
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows == true)
                            {
                                RdoExists = true;
                            }

                            while (rdr.Read())
                            {
                                RdoArtifactID = rdr.GetInt32(rdr.GetOrdinal("ArtifactID"));
                                Name = rdr.GetString(rdr.GetOrdinal("Name"));
                                User = rdr.GetInt32(rdr.GetOrdinal("TheUser"));
                            }
                        }

                        this.CustomLastError += RdoExists.ToString();
                    }

                    catch (SqlException e)
                    {
                                                
                        this.CustomLastError += "@@" + e.Message;
                        throw e;
                        // do something with the exception
                        // don't hide it
                    }

                }

            } //SqlConnection

            if (this.IsInRelativity == 1)
            {

                if (RdoExists)
                {

                }
                else
                {

                    RdoArtifactID = 0;

                }

                /*At this point, ArtifactID is set, either to the ArtifactID of an existing RDO or to a newly created RDO.*/


                // Do SQL Work

                //SqlConnection connection = ConnectionHelper.Helper().GetDBContext(this.ArtifactID).GetConnection();
                SqlConnection connection = this.GetAndOpenSQLConnection();

                string tableName = "";
                string docFieldID = "";
                string rdoFieldID = "";

                using (connection)
                {

                    if (!(connection.State == System.Data.ConnectionState.Open))
                    {
                        connection.Open();
                    }

                    string cmdString =
                        @"SELECT 
                            [tableName] =  [RelationalTableSchemaName]
                        , [docFieldId] = case when f.ArtifactID = r.[FieldArtifactId1] then[RelationalTableFieldColumnName1] else [RelationalTableFieldColumnName2]
                            end
	                    , [rdoFieldID] = case when f.ArtifactID = r.[FieldArtifactId1] then [RelationalTableFieldColumnName2] else [RelationalTableFieldColumnName1] end
                    FROM [EDDSDBO].[ObjectsFieldRelation] r with(nolock)
                    INNER JOIN[EDDSDBO].[ExtendedField]
                            f with(nolock) on f.ArtifactID = r.[FieldArtifactId1] or f.ArtifactID = r.[FieldArtifactId2]
                    WHERE f.FieldTypeId = 13 and f.DisplayName = 'Documents' and f.FieldArtifactTypeName = 'Document'";


                    using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                    {

                        try
                        {
                            using (SqlDataReader rdr = cmd.ExecuteReader())
                            {

                                while (rdr.Read())
                                {
                                    tableName = rdr.GetString(rdr.GetOrdinal("tableName"));
                                    docFieldID = rdr.GetString(rdr.GetOrdinal("docFieldID"));
                                    rdoFieldID = rdr.GetString(rdr.GetOrdinal("rdoFieldID"));
                                }
                            }
                        }

                        catch (SqlException e)
                        {
                                                        
                            this.CustomLastError += "@@" + e.Message;
                            throw e;
                            // do something with the exception
                            // don't hide it
                        }

                    }


                    cmdString = String.Format("DELETE FROM {0} WHERE {1} = {2}", tableName, rdoFieldID, RdoArtifactID);

                    using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                    {

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }

                        catch (SqlException e)
                        {

                            this.CustomLastError += "@@" + e.Message;
                            throw e;
                            // do something with the exception
                            // don't hide it
                        }

                    }


                    DataTable table = new DataTable();
                    table.Columns.Add(rdoFieldID, typeof(int));
                    table.Columns.Add(docFieldID, typeof(int));

                    foreach (int ArtifactID in ArtifactIDs)
                    {
                        table.Rows.Add(RdoArtifactID, ArtifactID);
                    }

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock |
                            SqlBulkCopyOptions.FireTriggers |
                            SqlBulkCopyOptions.UseInternalTransaction,
                            null))
                    {

                        bulkCopy.ColumnMappings.Add(rdoFieldID, rdoFieldID);
                        bulkCopy.ColumnMappings.Add(docFieldID, docFieldID);

                        // set the destination table name
                        bulkCopy.DestinationTableName = tableName;

                        // write the data in the "table"
                        bulkCopy.WriteToServer(table);
                    }


                } //using (connection)

            } // if IsInRelativity == 1

        } // WorkInRelativity



        private string GetViewURL()
        {


            


            string BaseURL = this.BaseUri;
            int AppID = this.ArtifactID;
            string CustomPageGuid = "";
            int SelectedTab = 0;
            int ViewID = 0;

            //SqlConnection connection = ConnectionHelper.Helper().GetDBContext(workspaceInfo.ArtifactID).GetConnection();
            using (SqlConnection connection = this.GetAndOpenSQLConnection())
            {
                if (!(connection.State == System.Data.ConnectionState.Open))
                {
                    connection.Open();
                }

                string cmdString =
                    @"    SELECT CAST(ag.ArtifactGuid as nvarchar(48)) as theGuid FROM EDDSDBO.ExtendedArtifact ea
                          JOIN [EDDSDBO].[ArtifactGuid] ag ON ea.ArtifactID = ag.ArtifactID
                          WHERE ArtifactTypeID = 1000014 AND TextIdentifier = 'Relativity List Page'
                        ";

                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    try
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                CustomPageGuid = rdr.GetString(rdr.GetOrdinal("theGuid"));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }

                cmdString = @"
                    SELECT ArtifactID FROM EDDSDBO.Artifact
                    WHERE ArtifactTypeID = 23
                    AND TextIdentifier = 'Document'
                ";

                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    try
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                SelectedTab = rdr.GetInt32(rdr.GetOrdinal("ArtifactID"));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }

                cmdString = @"
                    SELECT ArtifactID FROM EDDSDBO.Artifact
                    WHERE ArtifactTypeID = 4
                    AND TextIdentifier = 'Edge Documents'
                ";

                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    try
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                ViewID = rdr.GetInt32(rdr.GetOrdinal("ArtifactID"));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                                               

                return String.Format("{0}?AppID={1}&ArtifactTypeID=10&navigateListPage=true&DirectTo=%25applicationpath%25%2fCustomPages%2f{2}%2findex.html%3f%25appid%25%26%25artifacttypeid%25%26%25systemid%25&SelectedTab={3}%2fViewId#/ViewId={4}",
                    BaseURL, AppID, CustomPageGuid, SelectedTab, ViewID);
            }


        }

        public List<SenderRecipientPair> GetTopSenderRecipientPairs(SqlConnection connection =  null, string SocialNetworkDatabaseName = "")
        {
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            List<SenderRecipientPair> Pairs = new List<SenderRecipientPair>();


            string cmdString =
                    @"    SELECT TOP 10 s_EntityID, s_EntityDisplay, r_EntityID, r_EntityDisplay, PairsCount 
                          FROM SN_Pairs ORDER BY PairsCount DESC
                        ";

            using (SqlCommand cmd = new SqlCommand(cmdString, connection))
            {

                try
                {
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {

                        while (rdr.Read())
                        {
                            SenderRecipientPair Pair = new SenderRecipientPair();



                            Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                            Pairs.Add(Pair);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }


            return Pairs;



        }

        

        public List<Sender> GetTopSenders(SqlConnection connection = null, string SocialNetworkDatabaseName = "")
        {
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            List<Sender> Senders = new List<Sender>();


            string cmdString =
                    @"      SELECT sn.EntityID as s_EntityID, el.EntityDisplay as s_EntityDisplay, sn.theCount
                            FROM 
                            (
	                            SELECT TOP 10 EntityID, COUNT(*) as theCount FROM simple_SocialNetwork 
	                            WHERE EntityType = 1.0
	                            GROUP BY EntityID
	                            ORDER BY COUNT(*) DESC
                            ) sn
                            JOIN simple_EntityList el
                            ON sn.EntityID = el.EL_ID
                        ";

            using (SqlCommand cmd = new SqlCommand(cmdString, connection))
            {

                try
                {
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {

                        while (rdr.Read())
                        {
                            Sender Sender = new Sender();



                            Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            Senders.Add(Sender);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }


            return Senders;



        }

        public List<Recipient> GetTopRecipients(SqlConnection connection = null, string SocialNetworkDatabaseName = "")
        {
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            List<Recipient> Recipients = new List<Recipient>();


            string cmdString =
                    @"      SELECT sn.EntityID as r_EntityID, ISNULL(el.EntityDisplay, N'empty') as r_EntityDisplay, sn.theCount
                            FROM 
                            (
	                            SELECT TOP 10 EntityID, COUNT(*) as theCount FROM simple_SocialNetwork 
	                            WHERE EntityType in (2.1, 2.2, 2.3, 4.0)
	                            GROUP BY EntityID
	                            ORDER BY COUNT(*) DESC
                            ) sn
                            JOIN simple_EntityList el
                            ON sn.EntityID = el.EL_ID
                        ";

            using (SqlCommand cmd = new SqlCommand(cmdString, connection))
            {

                try
                {
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {

                        while (rdr.Read())
                        {
                            Recipient Recipient = new Recipient();


                            Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            Recipients.Add(Recipient);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }


            return Recipients;



        }


        public ScopeResult ScopeOnSender(SqlConnection connection = null, string SocialNetworkDatabaseName = "", Int64 SenderEntityID = 1)
        {
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            ScopeResult scopeResult = new ScopeResult();

            string cmdString =
                    @"usp_SN_Scope_On_Sender";

            using (SqlCommand cmd = new SqlCommand(cmdString, connection))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SenderEntityID", SqlDbType.BigInt).Value = SenderEntityID;

                try
                {

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {


                        while (rdr.Read())
                        {
                            Sender Sender = new Sender();


                            Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            scopeResult.Senders.Add(Sender);
                        }

                        rdr.NextResult();


                        while (rdr.Read())
                        {
                            Recipient Recipient = new Recipient();


                            Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            scopeResult.Recipients.Add(Recipient);
                        }

                        rdr.NextResult();

                        while (rdr.Read())
                        {
                            SenderRecipientPair Pair = new SenderRecipientPair();



                            Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                            scopeResult.Pairs.Add(Pair);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return scopeResult;

        }

        public ScopeResult ScopeOnRecipient(SqlConnection connection = null, string SocialNetworkDatabaseName = "", Int64 RecipientEntityID = 1)
        {
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            ScopeResult scopeResult = new ScopeResult();

            string cmdString =
                    @"usp_SN_Scope_On_Recipient";

            using (SqlCommand cmd = new SqlCommand(cmdString, connection))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RecipientEntityID", SqlDbType.BigInt).Value = RecipientEntityID;
                cmd.CommandTimeout = 300;

                try
                {

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {


                        while (rdr.Read())
                        {
                            Sender Sender = new Sender();


                            Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            scopeResult.Senders.Add(Sender);
                        }

                        rdr.NextResult();


                        while (rdr.Read())
                        {
                            Recipient Recipient = new Recipient();


                            Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            scopeResult.Recipients.Add(Recipient);
                        }

                        rdr.NextResult();

                        while (rdr.Read())
                        {
                            SenderRecipientPair Pair = new SenderRecipientPair();



                            Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                            scopeResult.Pairs.Add(Pair);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return scopeResult;

        }

        public ScopeResult ScopeOnPair(SqlConnection connection = null, string SocialNetworkDatabaseName = "", Int64 SenderEntityID = 1, Int64 RecipientEntityID = 1)
        {
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            ScopeResult scopeResult = new ScopeResult();

            string cmdString =
                    @"usp_SN_Scope_On_Pair";

            using (SqlCommand cmd = new SqlCommand(cmdString, connection))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SenderEntityID", SqlDbType.BigInt).Value = SenderEntityID;
                cmd.Parameters.Add("@RecipientEntityID", SqlDbType.BigInt).Value = RecipientEntityID;

                try
                {

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {


                        while (rdr.Read())
                        {
                            Sender Sender = new Sender();


                            Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            scopeResult.Senders.Add(Sender);
                        }

                        rdr.NextResult();


                        while (rdr.Read())
                        {
                            Recipient Recipient = new Recipient();


                            Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            scopeResult.Recipients.Add(Recipient);
                        }

                        rdr.NextResult();

                        while (rdr.Read())
                        {
                            SenderRecipientPair Pair = new SenderRecipientPair();



                            Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                            scopeResult.Pairs.Add(Pair);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return scopeResult;

        }


        public List<SavedSearch> GetSavedSearches(SqlConnection connection)
        {

            List<SavedSearch> SavedSearches = new List<SavedSearch>();

            if (!(connection.State == System.Data.ConnectionState.Open))
            {
                try
                {
                    connection.Open();
                }
                catch
                {

                }
            }

            string cmdString =
                @"      SELECT ArtifactID, TextIdentifier FROM EDDSDBO.ExtendedArtifact a
                        JOIN EDDSDBO.ArtifactType at ON a.ArtifactTypeID = at.ArtifactTypeID
                        WHERE at.ArtifactType = N'Search'
                    ";

            using (SqlCommand cmd = new SqlCommand(cmdString, connection))
            {
                try
                {
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SavedSearch savedSearch = new SavedSearch();
                            savedSearch.ArtifactID = rdr.GetInt32(rdr.GetOrdinal("ArtifactID"));
                            savedSearch.TextIdentifier = rdr.GetString(rdr.GetOrdinal("TextIdentifier"));

                            SavedSearches.Add(savedSearch);
                        }
                    }
                }
                catch (Exception e)
                {
                    //throw e;
                    Console.WriteLine(e);
                }
            }


            return SavedSearches;



        }


        private void RunSavedSearchGeneration10(SqlConnection connection, CytoscapeAndScopeData cytoscapeAndScopeData)
        {
            string cmdString = "usp_SN_Scope_On_Saved_Search_Generation10";

            using (SqlCommand cmd = new SqlCommand(cmdString, connection))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                try
                {

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {

                        while (rdr.Read())
                        {
                            Sender Sender = new Sender();


                            Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            cytoscapeAndScopeData.scopeResult.Senders.Add(Sender);
                        }

                        rdr.NextResult();

                        while (rdr.Read())
                        {
                            Recipient Recipient = new Recipient();


                            Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            cytoscapeAndScopeData.scopeResult.Recipients.Add(Recipient);
                        }

                        rdr.NextResult();

                        while (rdr.Read())
                        {
                            SenderRecipientPair Pair = new SenderRecipientPair();

                            Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                            cytoscapeAndScopeData.scopeResult.Pairs.Add(Pair);
                        }

                        rdr.NextResult();
                        Dictionary<long, string> dictNodes = new Dictionary<long, string>();
                        MyElement element = new MyElement();

                        while (rdr.Read())
                        {

                            //Console.WriteLine(++i);

                            MyEdge edge = new MyEdge();
                            Int64 s_EntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Int64 r_EntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            string s_EntityDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            string r_EntityDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Int32 PairsCount = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));
                            string theRole = rdr.GetString(rdr.GetOrdinal("theRole"));

                            edge.style.label = PairsCount.ToString();

                            if (theRole == "sender")
                            {
                                edge.data.source = s_EntityID;
                                edge.data.target = r_EntityID;
                                edge.data.group = "1";
                                edge.data.id = s_EntityID + "-" + r_EntityID;
                            }
                            else
                            {
                                edge.data.source = r_EntityID;
                                edge.data.target = s_EntityID;
                                edge.data.group = "2";
                                edge.data.id = r_EntityID + "-" + s_EntityID;
                            }

                            //edge.data.group = "1";


                            element.edges.Add(edge);

                            string x = edge.data.id;



                            if (dictNodes.ContainsKey(s_EntityID))
                            {

                            }
                            else
                            {
                                dictNodes.Add(s_EntityID, s_EntityDisplay);
                            }
                            if (dictNodes.ContainsKey(r_EntityID))
                            {

                            }
                            else
                            {
                                dictNodes.Add(r_EntityID, r_EntityDisplay);
                            }

                        }


                        MyNode node = new MyNode();
                        foreach (var x in dictNodes)
                        {
                            node = new MyNode();
                            node.data.id = x.Key.ToString();
                            node.data.label = x.Value;
                            element.nodes.Add(node);
                        }

                        cytoscapeAndScopeData.cytoscapeOptions.elements.edges = element.edges;
                        cytoscapeAndScopeData.cytoscapeOptions.elements.nodes = element.nodes;



                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        } //RunSavedSearchGeneration10

        private void RunSavedSearchGeneration15(SqlConnection connection, CytoscapeAndScopeData cytoscapeAndScopeData)
        {
            string cmdString = "usp_SN_Scope_On_Saved_Search_Generation15";

            using (SqlCommand cmd = new SqlCommand(cmdString, connection))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                try
                {

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {

                        while (rdr.Read())
                        {
                            Sender Sender = new Sender();


                            Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            cytoscapeAndScopeData.scopeResult.Senders.Add(Sender);
                        }

                        rdr.NextResult();

                        while (rdr.Read())
                        {
                            Recipient Recipient = new Recipient();


                            Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            cytoscapeAndScopeData.scopeResult.Recipients.Add(Recipient);
                        }

                        rdr.NextResult();

                        while (rdr.Read())
                        {
                            SenderRecipientPair Pair = new SenderRecipientPair();

                            Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                            cytoscapeAndScopeData.scopeResult.Pairs.Add(Pair);
                        }

                        rdr.NextResult();
                        Dictionary<long, string> dictNodes = new Dictionary<long, string>();
                        MyElement element = new MyElement();

                        while (rdr.Read())
                        {

                            //Console.WriteLine(++i);

                            MyEdge edge = new MyEdge();
                            Int64 s_EntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Int64 r_EntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            string s_EntityDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            string r_EntityDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Int32 PairsCount = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));
                            string theRole = rdr.GetString(rdr.GetOrdinal("theRole"));

                            edge.style.label = PairsCount.ToString();

                            if (theRole == "sender")
                            {
                                edge.data.source = s_EntityID;
                                edge.data.target = r_EntityID;
                                edge.data.group = "1";
                                edge.data.id = s_EntityID + "-" + r_EntityID;
                            }
                            else
                            {
                                edge.data.source = r_EntityID;
                                edge.data.target = s_EntityID;
                                edge.data.group = "2";
                                edge.data.id = r_EntityID + "-" + s_EntityID;
                            }

                            //edge.data.group = "1";


                            element.edges.Add(edge);

                            string x = edge.data.id;



                            if (dictNodes.ContainsKey(s_EntityID))
                            {

                            }
                            else
                            {
                                dictNodes.Add(s_EntityID, s_EntityDisplay);
                            }
                            if (dictNodes.ContainsKey(r_EntityID))
                            {

                            }
                            else
                            {
                                dictNodes.Add(r_EntityID, r_EntityDisplay);
                            }

                        }


                        MyNode node = new MyNode();
                        foreach (var x in dictNodes)
                        {
                            node = new MyNode();
                            node.data.id = x.Key.ToString();
                            node.data.label = x.Value;
                            element.nodes.Add(node);
                        }

                        cytoscapeAndScopeData.cytoscapeOptions.elements.edges = element.edges;
                        cytoscapeAndScopeData.cytoscapeOptions.elements.nodes = element.nodes;



                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        } //RunSavedSearchGeneration15


        private void RunSavedSearchGeneration20(SqlConnection connection, CytoscapeAndScopeData cytoscapeAndScopeData)
        {
            string cmdString = "usp_SN_Scope_On_Saved_Search_Generation20";

            using (SqlCommand cmd = new SqlCommand(cmdString, connection))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                try
                {

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {

                        while (rdr.Read())
                        {
                            Sender Sender = new Sender();


                            Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            cytoscapeAndScopeData.scopeResult.Senders.Add(Sender);
                        }

                        rdr.NextResult();

                        while (rdr.Read())
                        {
                            Recipient Recipient = new Recipient();


                            Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                            cytoscapeAndScopeData.scopeResult.Recipients.Add(Recipient);
                        }

                        rdr.NextResult();

                        while (rdr.Read())
                        {
                            SenderRecipientPair Pair = new SenderRecipientPair();

                            Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                            cytoscapeAndScopeData.scopeResult.Pairs.Add(Pair);
                        }

                        rdr.NextResult();
                        Dictionary<long, string> dictNodes = new Dictionary<long, string>();
                        MyElement element = new MyElement();

                        while (rdr.Read())
                        {

                            //Console.WriteLine(++i);

                            MyEdge edge = new MyEdge();
                            Int64 s_EntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                            Int64 r_EntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                            string s_EntityDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                            string r_EntityDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                            Int32 PairsCount = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));
                            string theRole = "sender";

                            edge.style.label = PairsCount.ToString();

                            if (theRole == "sender")
                            {
                                edge.data.source = s_EntityID;
                                edge.data.target = r_EntityID;
                                edge.data.group = "1";
                                edge.data.id = s_EntityID + "-" + r_EntityID;
                            }
                            else
                            {
                                edge.data.source = r_EntityID;
                                edge.data.target = s_EntityID;
                                edge.data.group = "2";
                                edge.data.id = r_EntityID + "-" + s_EntityID;
                            }

                            //edge.data.group = "1";


                            element.edges.Add(edge);

                            string x = edge.data.id;



                            if (dictNodes.ContainsKey(s_EntityID))
                            {

                            }
                            else
                            {
                                dictNodes.Add(s_EntityID, s_EntityDisplay);
                            }
                            if (dictNodes.ContainsKey(r_EntityID))
                            {

                            }
                            else
                            {
                                dictNodes.Add(r_EntityID, r_EntityDisplay);
                            }

                        }


                        MyNode node = new MyNode();
                        foreach (var x in dictNodes)
                        {
                            node = new MyNode();
                            node.data.id = x.Key.ToString();
                            node.data.label = x.Value;
                            element.nodes.Add(node);
                        }

                        cytoscapeAndScopeData.cytoscapeOptions.elements.edges = element.edges;
                        cytoscapeAndScopeData.cytoscapeOptions.elements.nodes = element.nodes;



                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        } //RunSavedSearchGeneration20




        public CytoscapeAndScopeData GetNodesAndEdgesTopCommunicatorsGeneration10Filter(int SenderEntityID = -1, int RecipientEntityID = -1, SqlConnection connection = null, string SocialNetworkDatabaseName = null, int MaximumParticipants = 2000000000, int SavedSearch = -1)
        {

            CytoscapeAndScopeData cytoscapeAndScopeData = new CytoscapeAndScopeData();

            String cmdString = "";

            // The database starts as the workspace database. Change to the social networking database.
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            using (connection)
            {

                cmdString = "usp_SN_GetGraphGeneration10Filter";

                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SenderEntityID", SqlDbType.Int).Value = SenderEntityID;
                    cmd.Parameters.Add("@RecipientEntityID", SqlDbType.Int).Value = RecipientEntityID;
                    cmd.Parameters.Add("@MaximumParticipants", SqlDbType.Int).Value = MaximumParticipants;
                    cmd.Parameters.Add("@SavedSearch", SqlDbType.Int).Value = SavedSearch;

                    try
                    {

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {


                            while (rdr.Read())
                            {
                                Sender Sender = new Sender();


                                Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Senders.Add(Sender);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                Recipient Recipient = new Recipient();


                                Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Recipients.Add(Recipient);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                SenderRecipientPair Pair = new SenderRecipientPair();

                                Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                                cytoscapeAndScopeData.scopeResult.Pairs.Add(Pair);
                            }

                            rdr.NextResult();



                            while (rdr.Read())
                            {

                                //Console.WriteLine(++i);

                                MyEdge edge = new MyEdge();
                                Int64 s_EntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Int64 r_EntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                string s_EntityDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                string r_EntityDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Int32 PairsCount = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));
                                string theRole = rdr.GetString(rdr.GetOrdinal("theRole"));

                                edge.style.label = PairsCount.ToString();





                                if (theRole == "sender")
                                {
                                    edge.data.source = s_EntityID;
                                    edge.data.target = r_EntityID;
                                    edge.data.group = "1";
                                    edge.data.id = s_EntityID + "-" + r_EntityID;
                                }
                                else
                                {
                                    edge.data.source = r_EntityID;
                                    edge.data.target = s_EntityID;
                                    edge.data.group = "2";
                                    edge.data.id = r_EntityID + "-" + s_EntityID;
                                }

                                //edge.data.group = "1";

                                edges.Add(edge);

                                string x = edge.data.id;



                                if (dictNodes.ContainsKey(s_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(s_EntityID, s_EntityDisplay);
                                }
                                if (dictNodes.ContainsKey(r_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(r_EntityID, r_EntityDisplay);
                                }

                            }


                            MyNode node = new MyNode();
                            foreach (var x in dictNodes)
                            {
                                node = new MyNode();
                                node.data.id = x.Key.ToString();
                                node.data.label = x.Value;
                                nodes.Add(node);
                            }

                            cytoscapeAndScopeData.cytoscapeOptions.elements.edges = edges;
                            cytoscapeAndScopeData.cytoscapeOptions.elements.nodes = nodes;


                        }



                    }

                    catch (SqlException e)
                    {

                        string eMessage = e.Message;
                        //throw e;
                        // do something with the exception
                        // don't hide it
                    }

                }
            }


            return cytoscapeAndScopeData;


        } // GetNodesAndEdgesGen1



        public CytoscapeAndScopeData GetNodesAndEdgesTopCommunicatorsGeneration15Filter(int SenderEntityID = 0, int RecipientEntityID = 0, SqlConnection connection = null, string SocialNetworkDatabaseName = null, int MaximumParticipants = 2000000000, int SavedSearch = -1)
        {

            CytoscapeAndScopeData cytoscapeAndScopeData = new CytoscapeAndScopeData();



            string cmdString =
                "usp_SN_GetGraphGeneration15Filter";

            // The database starts as the workspace database. Change to the social networking database.
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            using (connection)
            {


                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SenderEntityID", SqlDbType.Int).Value = SenderEntityID;
                    cmd.Parameters.Add("@RecipientEntityID", SqlDbType.Int).Value = RecipientEntityID;
                    cmd.Parameters.Add("@MaximumParticipants", SqlDbType.Int).Value = MaximumParticipants;
                    cmd.Parameters.Add("@SavedSearch", SqlDbType.Int).Value = SavedSearch;

                    try
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                Sender Sender = new Sender();


                                Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Senders.Add(Sender);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                Recipient Recipient = new Recipient();


                                Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Recipients.Add(Recipient);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                SenderRecipientPair Pair = new SenderRecipientPair();

                                Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                                cytoscapeAndScopeData.scopeResult.Pairs.Add(Pair);
                            }

                            rdr.NextResult();






                            while (rdr.Read())
                            {

                                MyEdge edge = new MyEdge();
                                Int64 s_EntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Int64 r_EntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                string s_EntityDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                string r_EntityDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Int32 PairsCount = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));
                                string theRole = rdr.GetString(rdr.GetOrdinal("theRole"));

                                edge.style.label = PairsCount.ToString();


                                edge.data.source = s_EntityID;
                                edge.data.target = r_EntityID;
                                edge.data.id = s_EntityID + "-" + r_EntityID;


                                if (theRole == "sender")
                                {
                                   // edge.data.source = s_EntityID;
                                    //edge.data.target = r_EntityID;
                                    edge.data.group = "1";
                                    //edge.data.id = s_EntityID + "-" + r_EntityID;
                                }
                                else
                                {
                                    //edge.data.source = r_EntityID;
                                    //edge.data.target = s_EntityID;
                                    edge.data.group = "2";
                                    //edge.data.id = r_EntityID + "-" + s_EntityID;
                                }

                                //edge.data.group = "1";

                                edges.Add(edge);

                                string x = edge.data.id;



                                if (dictNodes.ContainsKey(s_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(s_EntityID, s_EntityDisplay);
                                }
                                if (dictNodes.ContainsKey(r_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(r_EntityID, r_EntityDisplay);
                                }
                            }


                            MyNode node = new MyNode();
                            foreach (var x in dictNodes)
                            {
                                node = new MyNode();
                                node.data.id = x.Key.ToString();
                                node.data.label = x.Value;
                                nodes.Add(node);
                            }

                            cytoscapeAndScopeData.cytoscapeOptions.elements.edges = edges;
                            cytoscapeAndScopeData.cytoscapeOptions.elements.nodes = nodes;


                        }
                    }

                    catch (SqlException e)
                    {

                        string eMessage = e.Message;
                        // do something with the exception
                        // don't hide it
                    } // Catch

                } // sqlcommand
            } // connection

            return cytoscapeAndScopeData;

        } // GetNodesGen15



        public CytoscapeAndScopeData GetNodesAndEdgesTopCommunicatorsGeneration20Filter(int Levels = 1, int SenderEntityID = 1, int RecipientEntityID = 1, SqlConnection connection = null, string SocialNetworkDatabaseName = null, int MaximumParticipants = 2000000000, int SavedSearch = -1)
        {

            CytoscapeAndScopeData cytoscapeAndScopeData = new CytoscapeAndScopeData();

            //Stored procedure
            string cmdString =
                "usp_SN_GetGraphGeneration20Filter";

            // The database starts as the workspace database. Change to the social networking database.
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            using (connection)
            {


                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Levels", SqlDbType.Int).Value = Levels;
                    cmd.Parameters.Add("@SenderEntityID", SqlDbType.Int).Value = SenderEntityID;
                    cmd.Parameters.Add("@RecipientEntityID", SqlDbType.Int).Value = RecipientEntityID;
                    cmd.Parameters.Add("@MaximumParticipants", SqlDbType.Int).Value = MaximumParticipants;
                    cmd.Parameters.Add("@SavedSearch", SqlDbType.Int).Value = SavedSearch;


                    try
                    {

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                Sender Sender = new Sender();


                                Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Senders.Add(Sender);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                Recipient Recipient = new Recipient();


                                Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Recipients.Add(Recipient);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                SenderRecipientPair Pair = new SenderRecipientPair();

                                Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                                cytoscapeAndScopeData.scopeResult.Pairs.Add(Pair);
                            }

                            rdr.NextResult();





                            while (rdr.Read())
                            {

                                MyEdge edge = new MyEdge();
                                Int64 s_EntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Int64 r_EntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                string s_EntityDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                string r_EntityDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Int32 PairsCount = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));
                                edge.data.id = s_EntityID + "-" + r_EntityID;
                                edge.style.label = PairsCount.ToString();
                                edge.data.source = s_EntityID;
                                edge.data.target = r_EntityID;
                                edge.data.group = "1";

                                edges.Add(edge);

                                if (dictNodes.ContainsKey(s_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(s_EntityID, s_EntityDisplay);
                                }
                                if (dictNodes.ContainsKey(r_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(r_EntityID, r_EntityDisplay);
                                }

                            }

                            MyNode node = new MyNode();
                            foreach (var x in dictNodes)
                            {
                                node = new MyNode();
                                node.data.id = x.Key.ToString();
                                node.data.label = x.Value;
                                nodes.Add(node);
                            }

                            cytoscapeAndScopeData.cytoscapeOptions.elements.edges = edges;
                            cytoscapeAndScopeData.cytoscapeOptions.elements.nodes = nodes;


                        }

                    }

                    catch (SqlException e)
                    {

                        string eMessage = e.Message;
                    }

                }

            }

            return cytoscapeAndScopeData;

        } // GetNodesAndEdges




        public CytoscapeAndScopeData GetNodesAndEdgesTopCommunicatorsGeneration10(int SenderEntityID = -1, int RecipientEntityID = -1, SqlConnection connection = null, string SocialNetworkDatabaseName = null)
        {

            CytoscapeAndScopeData cytoscapeAndScopeData = new CytoscapeAndScopeData();

            string cmdString =
                "usp_SN_GetGraphGeneration10";

            if (connection.State.ToString() != "Closed")
            {

                // The database starts as the workspace database. Change to the social networking database.
                connection.ChangeDatabase(SocialNetworkDatabaseName);

                using (connection)
                {

                    using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@SenderEntityID", SqlDbType.Int).Value = SenderEntityID;
                        cmd.Parameters.Add("@RecipientEntityID", SqlDbType.Int).Value = RecipientEntityID;

                        try
                        {

                            using (SqlDataReader rdr = cmd.ExecuteReader())
                            {


                                while (rdr.Read())
                                {
                                    Sender Sender = new Sender();


                                    Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                    Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                    Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                    cytoscapeAndScopeData.scopeResult.Senders.Add(Sender);
                                }

                                rdr.NextResult();

                                while (rdr.Read())
                                {
                                    Recipient Recipient = new Recipient();


                                    Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                    Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                    Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                    cytoscapeAndScopeData.scopeResult.Recipients.Add(Recipient);
                                }

                                rdr.NextResult();

                                while (rdr.Read())
                                {
                                    SenderRecipientPair Pair = new SenderRecipientPair();

                                    Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                    Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                    Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                    Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                    Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                                    cytoscapeAndScopeData.scopeResult.Pairs.Add(Pair);
                                }

                                rdr.NextResult();

                                int i = 0;


                                while (rdr.Read())
                                {

                                    //Console.WriteLine(++i);
                                    ++i;

                                    MyEdge edge = new MyEdge();
                                    Int64 s_EntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                    Int64 r_EntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                    string s_EntityDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                    string r_EntityDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                    Int32 PairsCount = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));
                                    string theRole = rdr.GetString(rdr.GetOrdinal("theRole"));

                                    edge.style.label = PairsCount.ToString();

                                    if (theRole == "sender")
                                    {
                                        edge.data.source = s_EntityID;
                                        edge.data.target = r_EntityID;
                                        edge.data.group = "1";
                                        edge.data.id = s_EntityID + "-" + r_EntityID;
                                    }
                                    else
                                    {
                                        edge.data.source = r_EntityID;
                                        edge.data.target = s_EntityID;
                                        edge.data.group = "2";
                                        edge.data.id = r_EntityID + "-" + s_EntityID;
                                    }

                                    //edge.data.group = "1";

                                    edges.Add(edge);

                                    string x = edge.data.id;



                                    if (dictNodes.ContainsKey(s_EntityID))
                                    {

                                    }
                                    else
                                    {
                                        dictNodes.Add(s_EntityID, s_EntityDisplay);
                                    }
                                    if (dictNodes.ContainsKey(r_EntityID))
                                    {

                                    }
                                    else
                                    {
                                        dictNodes.Add(r_EntityID, r_EntityDisplay);
                                    }

                                }


                                MyNode node = new MyNode();
                                foreach (var x in dictNodes)
                                {
                                    node = new MyNode();
                                    node.data.id = x.Key.ToString();
                                    node.data.label = x.Value;
                                    nodes.Add(node);
                                }

                                cytoscapeAndScopeData.cytoscapeOptions.elements.edges = edges;
                                cytoscapeAndScopeData.cytoscapeOptions.elements.nodes = nodes;


                            }



                        }

                        catch (SqlException e)
                        {

                            string eMessage = e.Message;
                            // do something with the exception
                            // don't hide it
                        }

                    }
                }

            }

            return cytoscapeAndScopeData;

        } // GetNodesAndEdgesGen1



        public CytoscapeAndScopeData GetNodesAndEdgesTopCommunicatorsGeneration15(int SenderEntityID = -1, int RecipientEntityID = -1, SqlConnection connection = null, string SocialNetworkDatabaseName = null)
        {

            CytoscapeAndScopeData cytoscapeAndScopeData = new CytoscapeAndScopeData();

            string cmdString =
                "usp_SN_GetGraphGeneration15";

            // The database starts as the workspace database. Change to the social networking database.
            connection.ChangeDatabase(SocialNetworkDatabaseName);
            

            using (connection)
            {

                

                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 120;

                    cmd.Parameters.Add("@SenderEntityID", SqlDbType.Int).Value = SenderEntityID;
                    cmd.Parameters.Add("@RecipientEntityID", SqlDbType.Int).Value = RecipientEntityID;

                    try
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                Sender Sender = new Sender();


                                Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Senders.Add(Sender);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                Recipient Recipient = new Recipient();


                                Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Recipients.Add(Recipient);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                SenderRecipientPair Pair = new SenderRecipientPair();

                                Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                                cytoscapeAndScopeData.scopeResult.Pairs.Add(Pair);
                            }

                            rdr.NextResult();



                            while (rdr.Read())
                            {

                                MyEdge edge = new MyEdge();
                                Int64 s_EntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Int64 r_EntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                string s_EntityDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                string r_EntityDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Int32 PairsCount = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));
                                string theRole = rdr.GetString(rdr.GetOrdinal("theRole"));

                                edge.style.label = PairsCount.ToString();

                                if (theRole == "sender")
                                {
                                    edge.data.source = s_EntityID;
                                    edge.data.target = r_EntityID;
                                    edge.data.group = "1";
                                    edge.data.id = s_EntityID + "-" + r_EntityID;
                                }
                                else
                                {
                                    //edge.data.source = r_EntityID;
                                    //edge.data.target = s_EntityID;
                                    edge.data.source = s_EntityID;
                                    edge.data.target = r_EntityID;
                                    edge.data.group = "2";
                                    edge.data.id = s_EntityID + "-" + r_EntityID;
                                    //edge.data.id = r_EntityID + "-" + s_EntityID;
                                }

                                //edge.data.group = "1";

                                edges.Add(edge);

                                string x = edge.data.id;



                                if (dictNodes.ContainsKey(s_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(s_EntityID, s_EntityDisplay);
                                }
                                if (dictNodes.ContainsKey(r_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(r_EntityID, r_EntityDisplay);
                                }
                            }


                            MyNode node = new MyNode();
                            foreach (var x in dictNodes)
                            {
                                node = new MyNode();
                                node.data.id = x.Key.ToString();
                                node.data.label = x.Value;
                                nodes.Add(node);
                            }

                            cytoscapeAndScopeData.cytoscapeOptions.elements.edges = edges;
                            cytoscapeAndScopeData.cytoscapeOptions.elements.nodes = nodes;


                        }
                    }

                    catch (SqlException e)
                    {

                        string eMessage = e.Message;
                        // do something with the exception
                        // don't hide it
                    }

                }
            }

            return cytoscapeAndScopeData;

        } // GetNodesGen15



        public CytoscapeAndScopeData GetNodesAndEdgesTopCommunicatorsGeneration20(int Levels = 1, int SenderEntityID = -1, int RecipientEntityID = -1, SqlConnection connection = null, string SocialNetworkDatabaseName = null)
        {

            CytoscapeAndScopeData cytoscapeAndScopeData = new CytoscapeAndScopeData();

            //Stored procedure
            string cmdString =
                "usp_SN_GetGraphGeneration20";

            // The database starts as the workspace database. Change to the social networking database.
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            using (connection)
            {

                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Levels", SqlDbType.Int).Value = Levels;
                    cmd.Parameters.Add("@SenderEntityID", SqlDbType.Int).Value = SenderEntityID;
                    cmd.Parameters.Add("@RecipientEntityID", SqlDbType.Int).Value = RecipientEntityID;

                    try
                    {

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                Sender Sender = new Sender();


                                Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Senders.Add(Sender);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                Recipient Recipient = new Recipient();


                                Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Recipients.Add(Recipient);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                SenderRecipientPair Pair = new SenderRecipientPair();

                                Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                                cytoscapeAndScopeData.scopeResult.Pairs.Add(Pair);
                            }

                            rdr.NextResult();



                            while (rdr.Read())
                            {

                                MyEdge edge = new MyEdge();
                                Int64 s_EntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Int64 r_EntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                string s_EntityDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                string r_EntityDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Int32 PairsCount = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));
                                string theRole = rdr.GetString(rdr.GetOrdinal("theRole"));
                                edge.style.label = PairsCount.ToString();



                                if (theRole == "sender")
                                {
                                    edge.data.source = s_EntityID;
                                    edge.data.target = r_EntityID;
                                    edge.data.group = "1";
                                    edge.data.id = s_EntityID + "-" + r_EntityID;
                                }
                                else
                                {
                                    //edge.data.source = r_EntityID;
                                    //edge.data.target = s_EntityID;
                                    edge.data.source = s_EntityID;
                                    edge.data.target = r_EntityID;
                                    edge.data.group = "2";
                                    edge.data.id = s_EntityID + "-" + r_EntityID;
                                    //edge.data.id = r_EntityID + "-" + s_EntityID;
                                }








                                edges.Add(edge);

                                if (dictNodes.ContainsKey(s_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(s_EntityID, s_EntityDisplay);
                                }
                                if (dictNodes.ContainsKey(r_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(r_EntityID, r_EntityDisplay);
                                }

                            }

                            MyNode node = new MyNode();
                            foreach (var x in dictNodes)
                            {
                                node = new MyNode();
                                node.data.id = x.Key.ToString();
                                node.data.label = x.Value;
                                nodes.Add(node);
                            }

                            cytoscapeAndScopeData.cytoscapeOptions.elements.edges = edges;
                            cytoscapeAndScopeData.cytoscapeOptions.elements.nodes = nodes;

                        }

                    }

                    catch (SqlException e)
                    {

                        string eMessage = e.Message;
                    }

                }

            }


            return cytoscapeAndScopeData;

        } // GetNodesAndEdges





        public CytoscapeAndScopeData GetNodesAndEdgesTopCommunicatorsGeneration10Case(int SenderEntityID = -1, int RecipientEntityID = -1, SqlConnection connection = null, string SocialNetworkDatabaseName = null)
        {

            CytoscapeAndScopeData cytoscapeAndScopeData = new CytoscapeAndScopeData();

            string cmdString =
                "usp_SN_GetGraphGeneration10_Case";

            // The database starts as the workspace database. Change to the social networking database.
            try
            {
                connection.ChangeDatabase(SocialNetworkDatabaseName);
            }
            catch
            {

            }

            using (connection)
            {


                if (connection.State.ToString() != "Closed")
                {


                    using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@SenderEntityID", SqlDbType.Int).Value = SenderEntityID;
                        cmd.Parameters.Add("@RecipientEntityID", SqlDbType.Int).Value = RecipientEntityID;

                        try
                        {

                            using (SqlDataReader rdr = cmd.ExecuteReader())
                            {


                                while (rdr.Read())
                                {
                                    Sender Sender = new Sender();


                                    Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                    Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                    Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                    cytoscapeAndScopeData.scopeResult.Senders.Add(Sender);
                                }

                                rdr.NextResult();

                                while (rdr.Read())
                                {
                                    Recipient Recipient = new Recipient();


                                    Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                    Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                    Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                    cytoscapeAndScopeData.scopeResult.Recipients.Add(Recipient);
                                }

                                rdr.NextResult();

                                while (rdr.Read())
                                {
                                    SenderRecipientPair Pair = new SenderRecipientPair();

                                    Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                    Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                    Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                    Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                    Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                                    cytoscapeAndScopeData.scopeResult.Pairs.Add(Pair);
                                }

                                rdr.NextResult();

                                int i = 0;


                                while (rdr.Read())
                                {

                                    //Console.WriteLine(++i);
                                    ++i;

                                    MyEdge edge = new MyEdge();
                                    Int64 s_EntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                    Int64 r_EntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                    string s_EntityDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                    string r_EntityDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                    Int32 PairsCount = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));
                                    string theRole = rdr.GetString(rdr.GetOrdinal("theRole"));

                                    edge.style.label = PairsCount.ToString();

                                    if (theRole == "sender")
                                    {
                                        edge.data.source = s_EntityID;
                                        edge.data.target = r_EntityID;
                                        edge.data.group = "1";
                                        edge.data.id = s_EntityID + "-" + r_EntityID;
                                    }
                                    else
                                    {
                                        edge.data.source = r_EntityID;
                                        edge.data.target = s_EntityID;
                                        edge.data.group = "2";
                                        edge.data.id = r_EntityID + "-" + s_EntityID;
                                    }

                                    //edge.data.group = "1";

                                    edges.Add(edge);

                                    string x = edge.data.id;



                                    if (dictNodes.ContainsKey(s_EntityID))
                                    {

                                    }
                                    else
                                    {
                                        dictNodes.Add(s_EntityID, s_EntityDisplay);
                                    }
                                    if (dictNodes.ContainsKey(r_EntityID))
                                    {

                                    }
                                    else
                                    {
                                        dictNodes.Add(r_EntityID, r_EntityDisplay);
                                    }

                                }


                                MyNode node = new MyNode();
                                foreach (var x in dictNodes)
                                {
                                    node = new MyNode();
                                    node.data.id = x.Key.ToString();
                                    node.data.label = x.Value;
                                    nodes.Add(node);
                                }

                                cytoscapeAndScopeData.cytoscapeOptions.elements.edges = edges;
                                cytoscapeAndScopeData.cytoscapeOptions.elements.nodes = nodes;


                            }



                        }

                        catch (SqlException e)
                        {

                            string eMessage = e.Message;
                            // do something with the exception
                            // don't hide it
                        }

                    }

                }
            }

            return cytoscapeAndScopeData;

        } // GetNodesAndEdgesGen1



        public CytoscapeAndScopeData GetNodesAndEdgesTopCommunicatorsGeneration15Case(int SenderEntityID = -1, int RecipientEntityID = -1, SqlConnection connection = null, string SocialNetworkDatabaseName = null)
        {

            CytoscapeAndScopeData cytoscapeAndScopeData = new CytoscapeAndScopeData();

            string cmdString =
                "usp_SN_GetGraphGeneration15_Case";

            // The database starts as the workspace database. Change to the social networking database.
            connection.ChangeDatabase(SocialNetworkDatabaseName);


            using (connection)
            {



                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 120;

                    cmd.Parameters.Add("@SenderEntityID", SqlDbType.Int).Value = SenderEntityID;
                    cmd.Parameters.Add("@RecipientEntityID", SqlDbType.Int).Value = RecipientEntityID;

                    try
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                Sender Sender = new Sender();


                                Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Senders.Add(Sender);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                Recipient Recipient = new Recipient();


                                Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Recipients.Add(Recipient);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                SenderRecipientPair Pair = new SenderRecipientPair();

                                Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                                cytoscapeAndScopeData.scopeResult.Pairs.Add(Pair);
                            }

                            rdr.NextResult();



                            while (rdr.Read())
                            {

                                MyEdge edge = new MyEdge();
                                Int64 s_EntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Int64 r_EntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                string s_EntityDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                string r_EntityDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Int32 PairsCount = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));
                                string theRole = rdr.GetString(rdr.GetOrdinal("theRole"));

                                edge.style.label = PairsCount.ToString();





                                if (theRole == "sender")
                                {
                                    edge.data.source = s_EntityID;
                                    edge.data.target = r_EntityID;
                                    edge.data.group = "1";
                                    edge.data.id = s_EntityID + "-" + r_EntityID;
                                }
                                else
                                {
                                    //edge.data.source = r_EntityID;
                                    //edge.data.target = s_EntityID;
                                    edge.data.source = s_EntityID;
                                    edge.data.target = r_EntityID;
                                    edge.data.group = "2";
                                    //edge.data.id = r_EntityID + "-" + s_EntityID;
                                    edge.data.id = s_EntityID + "-" + r_EntityID;
                                }

                                //edge.data.group = "1";

                                edges.Add(edge);

                                string x = edge.data.id;



                                if (dictNodes.ContainsKey(s_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(s_EntityID, s_EntityDisplay);
                                }
                                if (dictNodes.ContainsKey(r_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(r_EntityID, r_EntityDisplay);
                                }
                            }


                            MyNode node = new MyNode();
                            foreach (var x in dictNodes)
                            {
                                node = new MyNode();
                                node.data.id = x.Key.ToString();
                                node.data.label = x.Value;
                                nodes.Add(node);
                            }

                            cytoscapeAndScopeData.cytoscapeOptions.elements.edges = edges;
                            cytoscapeAndScopeData.cytoscapeOptions.elements.nodes = nodes;


                        }
                    }

                    catch (SqlException e)
                    {

                        string eMessage = e.Message;
                        // do something with the exception
                        // don't hide it
                    }

                }
            }

            return cytoscapeAndScopeData;

        } // GetNodesGen15



        public CytoscapeAndScopeData GetNodesAndEdgesTopCommunicatorsGeneration20Case(int Levels = 1, int SenderEntityID = -1, int RecipientEntityID = -1, SqlConnection connection = null, string SocialNetworkDatabaseName = null)
        {

            CytoscapeAndScopeData cytoscapeAndScopeData = new CytoscapeAndScopeData();

            //Stored procedure
            string cmdString =
                "usp_SN_GetGraphGeneration20_Case";

            // The database starts as the workspace database. Change to the social networking database.
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            using (connection)
            {

                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Levels", SqlDbType.Int).Value = Levels;
                    cmd.Parameters.Add("@SenderEntityID", SqlDbType.Int).Value = SenderEntityID;
                    cmd.Parameters.Add("@RecipientEntityID", SqlDbType.Int).Value = RecipientEntityID;

                    try
                    {

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                Sender Sender = new Sender();


                                Sender.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Sender.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Sender.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Senders.Add(Sender);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                Recipient Recipient = new Recipient();


                                Recipient.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Recipient.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Recipient.Count = rdr.GetInt32(rdr.GetOrdinal("theCount"));

                                cytoscapeAndScopeData.scopeResult.Recipients.Add(Recipient);
                            }

                            rdr.NextResult();

                            while (rdr.Read())
                            {
                                SenderRecipientPair Pair = new SenderRecipientPair();

                                Pair.SenderEntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Pair.SenderDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                Pair.RecipientEntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                Pair.RecipientDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Pair.Count = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));

                                cytoscapeAndScopeData.scopeResult.Pairs.Add(Pair);
                            }

                            rdr.NextResult();



                            while (rdr.Read())
                            {

                                MyEdge edge = new MyEdge();
                                Int64 s_EntityID = rdr.GetInt64(rdr.GetOrdinal("s_EntityID"));
                                Int64 r_EntityID = rdr.GetInt64(rdr.GetOrdinal("r_EntityID"));
                                string s_EntityDisplay = rdr.GetString(rdr.GetOrdinal("s_EntityDisplay"));
                                string r_EntityDisplay = rdr.GetString(rdr.GetOrdinal("r_EntityDisplay"));
                                Int32 PairsCount = rdr.GetInt32(rdr.GetOrdinal("PairsCount"));
                                string theRole = rdr.GetString(rdr.GetOrdinal("theRole"));

                                edge.style.label = PairsCount.ToString();





                                if (theRole == "sender")
                                {
                                    edge.data.source = s_EntityID;
                                    edge.data.target = r_EntityID;
                                    edge.data.group = "1";
                                    edge.data.id = s_EntityID + "-" + r_EntityID;
                                }
                                else
                                {
                                    //edge.data.source = r_EntityID;
                                    //edge.data.target = s_EntityID;
                                    edge.data.source = s_EntityID;
                                    edge.data.target = r_EntityID;
                                    edge.data.group = "2";
                                    //edge.data.id = r_EntityID + "-" + s_EntityID;
                                    edge.data.id = s_EntityID + "-" + r_EntityID;
                                }



                                edges.Add(edge);

                                if (dictNodes.ContainsKey(s_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(s_EntityID, s_EntityDisplay);
                                }
                                if (dictNodes.ContainsKey(r_EntityID))
                                {

                                }
                                else
                                {
                                    dictNodes.Add(r_EntityID, r_EntityDisplay);
                                }

                            }

                            MyNode node = new MyNode();
                            foreach (var x in dictNodes)
                            {
                                node = new MyNode();
                                node.data.id = x.Key.ToString();
                                node.data.label = x.Value;
                                nodes.Add(node);
                            }

                            cytoscapeAndScopeData.cytoscapeOptions.elements.edges = edges;
                            cytoscapeAndScopeData.cytoscapeOptions.elements.nodes = nodes;

                        }

                    }

                    catch (SqlException e)
                    {

                        string eMessage = e.Message;
                    }

                }

            }


            return cytoscapeAndScopeData;

        } // GetNodesAndEdgesTopCommunicatorsGeneration20Case
        
    } // class WorkspaceInfo
} // namespace NetworkGraph.Models