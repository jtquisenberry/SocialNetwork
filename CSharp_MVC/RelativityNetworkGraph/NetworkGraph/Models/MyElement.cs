using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.IO;

namespace NetworkGraph.Models
{
    public class MyElement
    {

        public List<MyNode> nodes;
        public List<MyEdge> edges;
        private Dictionary<long, string> dictNodes;

        public MyElement()
        {
            nodes = new List<MyNode>();
            edges = new List<MyEdge>();
            dictNodes = new Dictionary<long, string>();
        }



        public void GetNodesAndEdgesTopCommunicatorsGeneration10(int EntityID = 1, SqlConnection connection = null, string SocialNetworkDatabaseName = null)
        {

            string cmdString =
                "usp_SN_GetGraphGeneration10";

            // The database starts as the workspace database. Change to the social networking database.
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            using (connection)
            {

                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@EntityID", SqlDbType.Int).Value = EntityID;

                    try
                    {

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {

                                //Console.WriteLine(++i);

                                MyEdge edge = new MyEdge();
                                Int32 s_EntityID = rdr.GetInt32(rdr.GetOrdinal("s_EntityID"));
                                Int32 r_EntityID = rdr.GetInt32(rdr.GetOrdinal("r_EntityID"));
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
        } // GetNodesAndEdgesGen1



        public void GetNodesAndEdgesTopCommunicatorsGeneration15(int Levels = 1, int EntityID = 1, SqlConnection connection = null, string SocialNetworkDatabaseName = null)
        {

            string cmdString =
                "usp_SN_GetGraphGeneration15";

            // The database starts as the workspace database. Change to the social networking database.
            connection.ChangeDatabase(SocialNetworkDatabaseName);

            using (connection)
            {

                using (SqlCommand cmd = new SqlCommand(cmdString, connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Levels", SqlDbType.Int).Value = Levels;
                    cmd.Parameters.Add("@EntityID", SqlDbType.Int).Value = EntityID;

                    try
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {

                                MyEdge edge = new MyEdge();
                                Int32 s_EntityID = rdr.GetInt32(rdr.GetOrdinal("s_EntityID"));
                                Int32 r_EntityID = rdr.GetInt32(rdr.GetOrdinal("r_EntityID"));
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

        } // GetNodesGen15



        public void GetNodesAndEdgesTopCommunicatorsGeneration20(int Levels = 1, int EntityID = 1, SqlConnection connection = null, string SocialNetworkDatabaseName = null)
        {

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
                    cmd.Parameters.Add("@EntityID", SqlDbType.Int).Value = EntityID;

                    try
                    {

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {
                                
                                MyEdge edge = new MyEdge();
                                Int32 s_EntityID = rdr.GetInt32(rdr.GetOrdinal("s_EntityID"));
                                Int32 r_EntityID = rdr.GetInt32(rdr.GetOrdinal("r_EntityID"));
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

                        }

                    }

                    catch (SqlException e)
                    {

                        string eMessage = e.Message;
                    }

                }

            }

        } // GetNodesAndEdges


        








    }
}