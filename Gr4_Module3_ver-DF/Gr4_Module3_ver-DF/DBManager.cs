
/*
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;


namespace Gr4_Module3_ver_DF
{
    class DBManager
    {
        public struct oppration
        {
            public string nomOpération;
            public int position,temps;
            public bool cycleVerin, quittance;
        }

        public struct informationLot
        {
            public string nomRecette,statusLot;
            public DateTime dateDeCreation,dateLimiteProd;
            public int quantiterAProduire;
        }


        /// <summary>
        /// 
        /// </summary>
        private static string connectionString = "server=Localhost;port=3306;database=module_3;user=root;password=";

        /// <summary>
        /// 
        /// </summary>
        private static MySqlConnection connexion = new MySqlConnection(connectionString);

        /// <summary>
        /// 
        /// </summary>
        public static void Connexion()
        {
            if(connexion.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connexion.Open();
                    Console.WriteLine("Connection OK");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
 
        }
        /// <summary>
        /// 
        /// </summary>
        public static void Deconnexion()
        {
            if(connexion.State == System.Data.ConnectionState.Open )
            {
                try
                {
                    connexion.Close();
                    Console.WriteLine("\nDéconnection OK");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomLot"></param>
        /// <param name="dateLimite"></param>
        /// <param name="recette"></param>
        public static void CreationLot(string nomLot,DateTime dateLimite,int quantiter,int etatlot, int recette)
        {

            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    
                    cmd.CommandText = "INSERT INTO lot VALUES (NULL,@lotNom,@dateCreation,@dateLimite,@quantité,@etatLot,@idrecette);";

                    cmd.Parameters.AddWithValue("@lotNom", nomLot);
                    cmd.Parameters.AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy’-‘MM’-‘dd’ HH’:’mm’:’ss"));
                    cmd.Parameters.AddWithValue("@etatLot", etatlot);
                    cmd.Parameters.AddWithValue("@dateLimite", dateLimite);
                    cmd.Parameters.AddWithValue("@quantité", quantiter);
                    cmd.Parameters.AddWithValue("@idrecette", recette);

                    cmd.ExecuteNonQuery();

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomRecette"></param>
        public static void CreationRecette(string nomRecette)
        {
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    
                    cmd.CommandText = "INSERT INTO recette VALUES (NULL,@recetteNom,@dateCreation);";

                    cmd.Parameters.AddWithValue("@recetteNom", nomRecette);
                    cmd.Parameters.AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy’-‘MM’-‘dd’ HH’:’mm’:’ss"));

                    cmd.ExecuteNonQuery();



                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Historique()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idRecette"></param>
        /// <param name="opération"></param>
        public static void AddOpperation(int idRecette, oppration[] opération)
        {
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    foreach (oppration value in opération)
                    {
                        
                        cmd.CommandText = "INSERT INTO operation VALUES(NULL,@nom,@position,@temps,@cycleVerin,@quitance,@idRecette);";

                        cmd.Parameters.AddWithValue("@nom", value.nomOpération);
                        cmd.Parameters.AddWithValue("@position", value.position);
                        cmd.Parameters.AddWithValue("@temps", value.temps);
                        cmd.Parameters.AddWithValue("@cycleVerin", value.cycleVerin);
                        cmd.Parameters.AddWithValue("@quitance", value.quittance);
                        cmd.Parameters.AddWithValue("@idRecette", idRecette);

                        cmd.ExecuteNonQuery();


                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
 
        }

        /// <summary>
        /// 
        /// </summary>
        public static informationLot[] GetLotInformation(string nomLot)
        {
            informationLot[] lot = new informationLot[1];
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                   
                    cmd.CommandText = "SELECT LOT_CREATION,LOT_DATE_LIMITE,LOT_QUANTITER,REC_NOM,ET_LIBELLE " +
                                      "FROM `lot` Join recette ON lot.ID_RECETTE = recette.ID_RECETTE " +
                                      "JOIN etat ON lot.ID_ETAT = etat.ID_ETAT WHERE LOT_NOM = @NomLot;";

                    cmd.Parameters.AddWithValue("@NomLot", nomLot);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lot[0].dateDeCreation = reader.GetDateTime("LOT_CREATION");
                            lot[0].dateLimiteProd = reader.GetDateTime("LOT_DATE_LIMITE");
                            lot[0].quantiterAProduire = reader.GetInt32("LOT_QUANTITER");
                            lot[0].nomRecette = reader.GetString("REC_NOM");
                            lot[0].statusLot = reader.GetString("ET_LIBELLE");
                        }
                    }


                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return lot;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomTable"></param>
        /// <returns></returns>
        public static int GetIDFromNameRecette(string nomRecette)
        {

            int idRecette = -1;
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    
                    cmd.CommandText = "SELECT ID_RECETTE FROM recette WHERE REC_NOM = @RecetteNom;";
                    cmd.Parameters.AddWithValue("@RecetteNom", nomRecette);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            idRecette = reader.GetInt32(0);
                        }

                    }


                }
                catch (MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }
            return idRecette;


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomLot"></param>
        /// <param name="nouvelleDateLimite"></param>
        public static void UpdateLotDateLimite(string nomLot,DateTime nouvelleDateLimite)
        {
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "UPDATE lot SET LOT_DATE_LIMITE = @NouvelleDateLimite WHERE LOT_NOM = @NomLot;";
                    cmd.Parameters.AddWithValue("@NouvelleDateLimite", nouvelleDateLimite);
                    cmd.Parameters.AddWithValue("@NomLot", nomLot);

                    cmd.ExecuteNonQuery();

                }
                catch (MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomLot"></param>
        /// <param name="nouvelleQuantiter"></param>
        public static void UpdateLotQuantiter(string nomLot,int nouvelleQuantiter)
        {
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "UPDATE lot SET LOT_QUANTITER = @NouvelleQuantiter WHERE LOT_NOM = @NomLot;";
                    cmd.Parameters.AddWithValue("@NouvelleQuantiter", nouvelleQuantiter);
                    cmd.Parameters.AddWithValue("@NomLot", nomLot);

                    cmd.ExecuteNonQuery();

                }
                catch (MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomLot"></param>
        /// <param name="nouvelleQuantiter"></param>
        public static void UpdateLotRecette(string nomLot, string nouvelleRecette)
        {
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "UPDATE lot SET ID_RECETTE = @NouvelleRecette WHERE LOT_NOM = @NomLot;";
                    cmd.Parameters.AddWithValue("@NouvelleRecette", GetIDFromNameRecette(nouvelleRecette));
                    cmd.Parameters.AddWithValue("@NomLot", nomLot);

                    cmd.ExecuteNonQuery();

                }
                catch (MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }
        }



        public static void DeleteLot(string nomLot)
        {
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "DELETE FROM lot WHERE LOT_NOM = @NomLotASupprimer;";
                    cmd.Parameters.AddWithValue("@NomLotASupprimer", nomLot);

                    cmd.ExecuteNonQuery();

                }
                catch (MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }
        }


        public static List<oppration> GetOpprationsFromRecette(int idRecette)
        {
            List<oppration> opération = new List<oppration>();
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "SELECT OP_NOM,OP_POSITION,OP_TEMPS, OP_CYCLE_VERIN, OP_QUITTANCE FROM operation WHERE ID_RECETTE = @ID_Recette; ";

                    cmd.Parameters.AddWithValue("@ID_Recette", idRecette);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            opération.Add(new oppration
                            {
                                nomOpération = reader.GetString("OP_NOM"),
                                position = reader.GetInt32("OP_POSITION"),
                                temps = reader.GetInt32("OP_TEMPS"),
                                cycleVerin = reader.GetBoolean("OP_CYCLE_VERIN"),
                                quittance = reader.GetBoolean("OP_QUITTANCE")
                            }); ;

                        }
                    }

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return opération;
        }


        public static string GetRecetteDateCréation(string nomRecette)
        {
            DateTime dateCreation = new DateTime();

            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "SELECT * FROM recette WHERE ID_RECETTE = @ID_Recette;";

                    cmd.Parameters.AddWithValue("@ID_Recette", GetIDFromNameRecette(nomRecette));



                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            dateCreation = reader.GetDateTime("REC_DATE_CREATION");
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }


            return dateCreation.ToString();
        }


        public static int NombreOperationRecette(string nomRecette)
        {
            int nombreOperation = 0;

            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {

                    cmd.CommandText = "SELECT COUNT(*) FROM operation WHERE ID_RECETTE = @ID_Recette";

                    cmd.Parameters.AddWithValue("@ID_Recette", GetIDFromNameRecette(nomRecette));

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            nombreOperation = reader.GetInt32("COUNT(*)");
                        }
                    }

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return nombreOperation;
        }

        public static List<lo>

    }
}
