using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetworkGraph.Models;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace NetworkGraph.Controllers
{
    public class HomeController : Controller
    {
        RelativityCommon.RelativityCommon relativity = new RelativityCommon.RelativityCommon();

        WorkspaceInfo workspaceInfo;

        public ActionResult Index(Int32 ID = 0)
        {
            
            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);


            ViewBag.Message = "ViewBag Placeholder";

            if (workspaceInfo.ArtifactID <= 0)
            {
                workspaceInfo.SocialNetworkDatabaseName = "SocialNetwork";
                ViewBag.Message = "My ViewBag";
            }

            return View("SocialNetwork001", workspaceInfo);
        }

        public ActionResult SocialNetwork001()
        {
            return Index();
        }


        public ActionResult DisplayReports()
        {
            return View("DisplayReports");
        }


        public ActionResult DisplayReport()
        {
            return View("DisplayReport");
        }


        public ActionResult RunHelper()
        {

            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            ViewBag.Message = "GetWorkspaceInfo";
            string dtNow = DateTime.Now.ToString("yyyyMMdd-hhmmss");

            return View(workspaceInfo);
        }

        [HttpGet]
        public JsonResult GetGraphGeneration10(int SenderEntityID = -1, int RecipientEntityID = -1)
        {
            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            CytoscapeAndScopeData cytoscapeAndScopeData;

            using (SqlConnection connection = workspaceInfo.GetAndOpenSQLConnection())
            {
                cytoscapeAndScopeData = workspaceInfo.GetNodesAndEdgesTopCommunicatorsGeneration10(SenderEntityID, RecipientEntityID, connection, workspaceInfo.SocialNetworkDatabaseName);
            }

            return Json(cytoscapeAndScopeData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGraphGeneration15(int SenderEntityID = -1, int RecipientEntityID = -1)
        {
            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            CytoscapeAndScopeData cytoscapeAndScopeData;

            using (SqlConnection connection = workspaceInfo.GetAndOpenSQLConnection())
            {
                cytoscapeAndScopeData = workspaceInfo.GetNodesAndEdgesTopCommunicatorsGeneration15(SenderEntityID, RecipientEntityID, connection, workspaceInfo.SocialNetworkDatabaseName);
            }

            return Json(cytoscapeAndScopeData, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult GetGraphGeneration20(int Levels = 2, int SenderEntityID = -1, int RecipientEntityID = -1)
        {
            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            CytoscapeAndScopeData cytoscapeAndScopeData;

            using (SqlConnection connection = workspaceInfo.GetAndOpenSQLConnection())
            {
                cytoscapeAndScopeData = workspaceInfo.GetNodesAndEdgesTopCommunicatorsGeneration20(Levels, SenderEntityID, RecipientEntityID, connection, workspaceInfo.SocialNetworkDatabaseName);
            }

            return Json(cytoscapeAndScopeData, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetGraphGeneration10Case(int SenderEntityID = -1, int RecipientEntityID = -1)
        {
            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            CytoscapeAndScopeData cytoscapeAndScopeData;

            using (SqlConnection connection = workspaceInfo.GetAndOpenSQLConnection())
            {
                cytoscapeAndScopeData = workspaceInfo.GetNodesAndEdgesTopCommunicatorsGeneration10Case(SenderEntityID, RecipientEntityID, connection, workspaceInfo.SocialNetworkDatabaseName);
            }

            return Json(cytoscapeAndScopeData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGraphGeneration15Case(int SenderEntityID = -1, int RecipientEntityID = -1)
        {
            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            CytoscapeAndScopeData cytoscapeAndScopeData;

            using (SqlConnection connection = workspaceInfo.GetAndOpenSQLConnection())
            {
                cytoscapeAndScopeData = workspaceInfo.GetNodesAndEdgesTopCommunicatorsGeneration15Case(SenderEntityID, RecipientEntityID, connection, workspaceInfo.SocialNetworkDatabaseName);
            }

            return Json(cytoscapeAndScopeData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGraphGeneration20Case(int Levels = 2, int SenderEntityID = -1, int RecipientEntityID = -1)
        {
            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            CytoscapeAndScopeData cytoscapeAndScopeData;

            using (SqlConnection connection = workspaceInfo.GetAndOpenSQLConnection())
            {
                cytoscapeAndScopeData = workspaceInfo.GetNodesAndEdgesTopCommunicatorsGeneration20Case(Levels, SenderEntityID, RecipientEntityID, connection, workspaceInfo.SocialNetworkDatabaseName);
            }

            return Json(cytoscapeAndScopeData, JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
        public JsonResult GetGraphGeneration10Filter(int SenderEntityID = -1, int RecipientEntityID = -1, int MaximumParticipants = 2000000000, int SavedSearch = -1)
        {
            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            CytoscapeAndScopeData cytoscapeAndScopeData;

            using (SqlConnection connection = workspaceInfo.GetAndOpenSQLConnection())
            {
                cytoscapeAndScopeData = workspaceInfo.GetNodesAndEdgesTopCommunicatorsGeneration10Filter(
                    SenderEntityID, RecipientEntityID, connection, workspaceInfo.SocialNetworkDatabaseName, MaximumParticipants, SavedSearch);
            }

            return Json(cytoscapeAndScopeData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGraphGeneration15Filter(int SenderEntityID = -1, int RecipientEntityID = -1, int MaximumParticipants = 2000000000, int SavedSearch = -1)
        {
            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            CytoscapeAndScopeData cytoscapeAndScopeData;

            using (SqlConnection connection = workspaceInfo.GetAndOpenSQLConnection())
            {
                cytoscapeAndScopeData = workspaceInfo.GetNodesAndEdgesTopCommunicatorsGeneration15Filter(
                    SenderEntityID, RecipientEntityID, connection, workspaceInfo.SocialNetworkDatabaseName, MaximumParticipants, SavedSearch);
            }

            return Json(cytoscapeAndScopeData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGraphGeneration20Filter(int Levels = 2, int SenderEntityID = -1, int RecipientEntityID = -1, int MaximumParticipants = 2000000000, int SavedSearch = -1)
        {
            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            CytoscapeAndScopeData cytoscapeAndScopeData;

            using (SqlConnection connection = workspaceInfo.GetAndOpenSQLConnection())
            {

                cytoscapeAndScopeData = workspaceInfo.GetNodesAndEdgesTopCommunicatorsGeneration20Filter(
                    Levels, SenderEntityID, RecipientEntityID, connection, workspaceInfo.SocialNetworkDatabaseName, MaximumParticipants, SavedSearch);
            }

            return Json(cytoscapeAndScopeData, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult EdgeClicked1(string EdgeID = "")
        {

            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            string uri = "mmm";
            
            using (SqlConnection connection = workspaceInfo.GetAndOpenSQLConnection())
            {
                uri = workspaceInfo.DoEdgeClicked(EdgeID, connection, workspaceInfo.SocialNetworkDatabaseName);
            }

            if (workspaceInfo.IsInRelativity == 0)
            {
                uri = "#";
            }


            return Json(uri, JsonRequestBehavior.AllowGet);
            
        }



        [HttpGet]
        public JsonResult UpdateScopeOnInitial(Int64 SenderEntityID = 1, Int64 RecipientEntityID = 1)
        {

            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            ScopeResult scopeResult = new ScopeResult();

            using (SqlConnection connection = workspaceInfo.GetAndOpenSQLConnection())
            {
                scopeResult.Senders = workspaceInfo.GetTopSenders(connection, workspaceInfo.SocialNetworkDatabaseName);
                scopeResult.Recipients = workspaceInfo.GetTopRecipients(connection, workspaceInfo.SocialNetworkDatabaseName);
                scopeResult.Pairs = workspaceInfo.GetTopSenderRecipientPairs(connection, workspaceInfo.SocialNetworkDatabaseName);
            }

            return Json(scopeResult, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetSavedSearches(Int64 SenderEntityID = 1, Int64 RecipientEntityID = 1)
        {

            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            List<SavedSearch> SavedSearches;

            using (SqlConnection connection = workspaceInfo.GetAndOpenSQLConnection())
            {
                SavedSearches = workspaceInfo.GetSavedSearches(connection);
            }

            return Json(SavedSearches, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RunSavedSearch(Int32 SavedSearchArtifactID, double Depth = 1.0)
        {

            workspaceInfo = new WorkspaceInfo();
            workspaceInfo.GetWorkspaceInformation();
            workspaceInfo.SetSession(HttpContext.Session);

            CytoscapeAndScopeData cytoscapeAndScopeData;

            using (SqlConnection connection = workspaceInfo.GetAndOpenSQLConnection())
            {
                //cytoscapeAndScopeData = workspaceInfo.RunSavedSearch(connection, workspaceInfo.SocialNetworkDatabaseName, SavedSearchArtifactID, Depth);
                cytoscapeAndScopeData = null;
            }

            return Json(cytoscapeAndScopeData, JsonRequestBehavior.AllowGet);
        }



    } // End Home Controller


} // NetworkGraph.Controllers