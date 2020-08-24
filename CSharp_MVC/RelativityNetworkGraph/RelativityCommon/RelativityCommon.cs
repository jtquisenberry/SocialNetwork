using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



namespace RelativityCommon
{
    public class RelativityCommon
    {
                

        #region " - Enums - "
        public enum enmMessageType
        {
            NotSet = 0,
            Err = 1,
            Warning = 2
        };
        #endregion


        #region " - Private Variables - "

            private const int DEFAULT_MESSAGE_LEVEL = 10;

            private string _Name;
            private int _AgentID;
            //private kCura.Agent.AgentBase _CallingClass;


            private int _WorkspaceArtifactID;

        #endregion

        #region Public Fields
        public String CustomLastError = "";

        public String WorkspaceDatabaseName = "";
        public String WorkspaceIsMasterDatabase = "false";
        public String WorkspaceServerName = "";

        public String MasterDatabaseName = "";
        public String MasterIsMasterDatabase = "false";
        public String MasterServerName = "";

        public String AuthenticationToken = "";
        public String Proxy_AuthType = "";
        public String Proxy_Uri = "";

        public String CaseName = "";

        public int ActiveUserArtifactID = 0;
        public int ActiveUserAuditArtifactID = 0;
        public int ActiveUserWorkspaceArtifactID = 0;


        #endregion  


        #region " - Properties - "



        public int WorkspaceArtifactID
        {
            set
            { 
                _WorkspaceArtifactID = value;
            }
        }



        public string Name 
        {
            set
            {
                _Name = value;
            }
        }

        public int AgentID 
        {
            set
            {
                _AgentID = value;
            }
        }
        


        #endregion


        #region constructor
        public RelativityCommon()
        {

        }
        #endregion


        #region " - Methods - "

        public int RunCustomPage() 
        {
            return RunCustomPageSteps();
        }
        
        #endregion

        #region " - Main Routines - "

        private int RunCustomPageSteps()
        {
            try
            {

                return 0;
            }
            catch (Exception ex)
            {
                this.CustomLastError = ex.Message;
                return 0;
            }


        }
        


        #endregion

    } // end class
} // end namespace
