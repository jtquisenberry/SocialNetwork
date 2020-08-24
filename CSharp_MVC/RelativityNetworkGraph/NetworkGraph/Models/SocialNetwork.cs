using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace NetworkGraph.Models
{

    


    public class SocialNetwork
    {
        public NameCount nameCount;
        public List<NameCount> nameCounts;
        public String CaseName;

        public List<CorrespondingRecipient> correspondingRecipients;
        public List<CorrespondingSender> correspondingSenders;
        public CorrespondingRecipient correspondingRecipient;
        public CorrespondingSender correspondingSender;



        public SocialNetworkNode socialNetworkNode;
        public List<SocialNetworkNode> socialNetworkNodes;

        public SocialNetworkEdge socialNetworkEdge;
        public List<SocialNetworkEdge> socialNetworkEdges;

        public String SelectedEntityName;


        public SocialNetwork()
        {
            CaseName = "My Case";
            nameCounts = new List<NameCount>();
            correspondingRecipients = new List<CorrespondingRecipient>();
            correspondingSenders = new List<CorrespondingSender>();
            socialNetworkNodes = new List<SocialNetworkNode>();
            socialNetworkEdges = new List<SocialNetworkEdge>();
        }

        public void GetTopEntities(String SocialNetworkDatabaseName)
        {

            String cs = ConfigurationManager.ConnectionStrings["SocialNetworking"].ConnectionString;
            //String query = "SELECT TOP 10 EntityName, EL_ID FROM simple_EntityList";
            String procedure = "[Util].dbo.usp_SN_GetTopParticipants";

            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(procedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@DatabaseName", SocialNetworkDatabaseName));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Int32 i = 0;

                            while (reader.Read())
                            {
                                nameCount = new NameCount();
                                nameCount.ID = i;
                                nameCount.Name = reader["EntityName"].ToString();
                                nameCount.Display = reader["EntityDisplay"].ToString();
                                nameCount.Count = reader.GetInt32(reader.GetOrdinal("DocumentCount"));
                                nameCounts.Add(nameCount);
                                i++;
                            }

                        }

                    }


                }


            }



        } //GetTopEntities

        public void GetSelectedEntityName(Int32 ID = 0)
        {
            SelectedEntityName = nameCounts[ID].Name;
        }


        public void GetCorrespondingEntities(String SocialNetworkDatabaseName)
        {

            String cs = ConfigurationManager.ConnectionStrings["SocialNetworking"].ConnectionString;
            String procedure = "[Util].dbo.usp_SN_GetCorrespondingEntities";
            Console.WriteLine(cs);

            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(procedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@DatabaseName", SocialNetworkDatabaseName));
                    command.Parameters.Add(new SqlParameter("@EntityName", SelectedEntityName));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                String EntityRole = reader["EntityRole"].ToString();

                                if (EntityRole == "Sender")
                                {
                                    correspondingRecipient = new CorrespondingRecipient();
                                    correspondingRecipient.Name = reader["EntityDisplay"].ToString();
                                    correspondingRecipient.Count = reader.GetInt32(reader.GetOrdinal("Instances"));
                                    correspondingRecipients.Add(correspondingRecipient);
                                }
                                else if (EntityRole == "Recipient")
                                {
                                    correspondingSender = new CorrespondingSender();
                                    correspondingSender.Name = reader["EntityDisplay"].ToString();
                                    correspondingSender.Count = reader.GetInt32(reader.GetOrdinal("Instances"));
                                    correspondingSenders.Add(correspondingSender);
                                }
                            }
                        }
                    }
                }
            }
        } // GetCorrespondingEntities

        public void GetSocialNetworkNodes(String SocialNetworkDatabaseName)
        {

            String cs = ConfigurationManager.ConnectionStrings["SocialNetworking"].ConnectionString;
            String procedure = "[Util].dbo.usp_SN_GetCorrespondingEntities";
            Console.WriteLine(cs);

            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(procedure, connection))
                {

                    String ExcludedEntity = SelectedEntityName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@DatabaseName", SocialNetworkDatabaseName));
                    command.Parameters.Add(new SqlParameter("@EntityName", ExcludedEntity));

                    Dictionary<String, Int32> ids = new Dictionary<String, Int32>();
                    

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Int32 StringLength = 0;
                        Int32 id = 1;
                        socialNetworkNode = new SocialNetworkNode();
                        socialNetworkNode.id = id;
                        StringLength = ExcludedEntity.Length >= 20 ? 20 : ExcludedEntity.Length;
                        socialNetworkNode.label = ExcludedEntity.Substring(0, StringLength);
                        socialNetworkNode.group = 1;
                        socialNetworkNodes.Add(socialNetworkNode);

                        Boolean check = ids.ContainsKey(ExcludedEntity);

                        if (check == false)
                        {
                            ids.Add(ExcludedEntity, id);
                        }


                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                String EntityRole = reader["EntityRole"].ToString();
                                String EntityName = reader["EntityName"].ToString();

                                socialNetworkNode = new SocialNetworkNode();
                                if (ids.ContainsKey(EntityName) == false)
                                {
                                    id++;
                                    ids.Add(EntityName, id);
                                    Int32 CurrentID = ids[EntityName];
                                    socialNetworkNode.id = CurrentID;
                                    StringLength = reader["EntityDisplay"].ToString().Length >= 20 ? 20 : reader["EntityDisplay"].ToString().Length;
                                    socialNetworkNode.label = reader["EntityDisplay"].ToString().Substring(0, StringLength);
                                    socialNetworkNode.group = 2;
                                    socialNetworkNodes.Add(socialNetworkNode);

                                    socialNetworkEdge = new SocialNetworkEdge();
                                    if (EntityRole == "Sender")
                                    {
                                        socialNetworkEdge.from = 1;
                                        socialNetworkEdge.to = CurrentID;
                                    }
                                    else
                                    {
                                        socialNetworkEdge.from = CurrentID;
                                        socialNetworkEdge.to = 1;
                                    }
                                    socialNetworkEdges.Add(socialNetworkEdge);
                                }
                                else
                                {
                                    id++;
                                    Int32 CurrentID = ids[EntityName];

                                    socialNetworkEdge = new SocialNetworkEdge();
                                    if (EntityRole == "Sender")
                                    {
                                        socialNetworkEdge.from = 1;
                                        socialNetworkEdge.to = CurrentID;
                                    }
                                    else
                                    {
                                        socialNetworkEdge.from = CurrentID;
                                        socialNetworkEdge.to = 1;
                                    }
                                    socialNetworkEdges.Add(socialNetworkEdge);

                                }



                                
                                
                                


                            }
                        }
                    }
                }
            }
        } // GetSocialNetworkNodes




    }

    public class NameCount
    {

        public Int32 ID;
        public String Name;
        public String Display;
        public Int32 Count; 
    }

    public class CorrespondingRecipient
    {
        public Int32 ID;
        public String Name;
        public Int32 Count;
    }

    public class CorrespondingSender
    {
        public Int32 ID;
        public String Name;
        public Int32 Count;
    }

    public class SocialNetworkNode
    {
        public Int32 id;
        public String label;
        public Int32 group;
    }

    public class SocialNetworkEdge
    {
        public Int32 from;
        public Int32 to;

    }





}
