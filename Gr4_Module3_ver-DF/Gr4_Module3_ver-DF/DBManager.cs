
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
        private static MySqlCommand cmd = connexion.CreateCommand();

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
 
                
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO lot VALUES (NULL,@lotNom,@dateCreation,@dateLimite,@quantité,@etatLot,@idrecette);";
                    
                cmd.Parameters.AddWithValue("@lotNom", nomLot);
                cmd.Parameters.AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy’-‘MM’-‘dd’ HH’:’mm’:’ss"));
                cmd.Parameters.AddWithValue("@etatLot", etatlot);
                cmd.Parameters.AddWithValue("@dateLimite", dateLimite);
                cmd.Parameters.AddWithValue("@quantité", quantiter);
                cmd.Parameters.AddWithValue("@idrecette", recette);

                cmd.ExecuteNonQuery();

            }
            catch(MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomRecette"></param>
        public static void CreationRecette(string nomRecette)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO recette VALUES (NULL,@recetteNom,@dateCreation);";

                cmd.Parameters.AddWithValue("@recetteNom", nomRecette);
                cmd.Parameters.AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy’-‘MM’-‘dd’ HH’:’mm’:’ss"));

                cmd.ExecuteNonQuery();



            }
            catch(MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Recette créer");
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
            try
            {
                foreach(oppration value in opération)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "INSERT INTO operation VALUES(NULL,@nom,@position,@temps,@cycleVerin,@quitance,@idRecette);";

                    cmd.Parameters.AddWithValue("@nom", value.nomOpération);
                    cmd.Parameters.AddWithValue("@position",value.position);
                    cmd.Parameters.AddWithValue("@temps", value.temps);
                    cmd.Parameters.AddWithValue("@cycleVerin", value.cycleVerin);
                    cmd.Parameters.AddWithValue("@quitance", value.quittance);
                    cmd.Parameters.AddWithValue("@idRecette", idRecette);

                    cmd.ExecuteNonQuery();

                    
                }
            }
            catch(MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
 
        }

        /// <summary>
        /// 
        /// </summary>
        public static informationLot[] GetLotInformation(string nomLot)
        {
            informationLot[] lot = new informationLot[1];

            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT LOT_CREATION,LOT_DATE_LIMITE,LOT_QUANTITER,REC_NOM,ET_LIBELLE " +
                                  "FROM `lot` Join recette ON lot.ID_RECETTE = recette.ID_RECETTE " +
                                  "JOIN etat ON lot.ID_ETAT = etat.ID_ETAT WHERE LOT_NOM = @NomLot;";
                
                cmd.Parameters.AddWithValue("@NomLot",nomLot);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        lot[0].dateDeCreation = reader.GetDateTime("LOT_CREATION");
                        lot[0].dateLimiteProd = reader.GetDateTime("LOT_DATE_LIMITE");
                        lot[0].quantiterAProduire = reader.GetInt32("LOT_QUANTITER");
                        lot[0].nomRecette = reader.GetString("REC_NOM");
                        lot[0].statusLot = reader.GetString("ET_LIBELLE");
                    }
                }


            }
            catch(MySqlException ex)
            {
                Console.WriteLine(ex.Message);
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
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT ID_RECETTE FROM recette WHERE REC_NOM = @RecetteNom;";
                cmd.Parameters.AddWithValue("@RecetteNom", nomRecette);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {

                    while(reader.Read())
                    {
                        idRecette = reader.GetInt32(0);
                    }

                }
                    
                
            }
            catch(MySqlException ex)
            {
                Menu.ErrorMessage(ex.Message);
            }

            return idRecette;


        }



    }
}
