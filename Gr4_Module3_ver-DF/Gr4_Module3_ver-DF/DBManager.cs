
/***************************************************************************************************************************************************\
 * Déscription: Cette Class DBManager contients tous le code C# qui me permet d'effectuer des actions avec la base de données telle que des actions
 *              de créations, d'insertions, de connexion, de déconexxion, de récupération d'informations ainsi que de suppréssions.
 * 
 * Auteur: Fernandes David
 * 
 * Début du projet: 14.03.2022
 * Fin du projet:   07.11.2022
 * 
\***************************************************************************************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;


namespace Gr4_Module3_ver_DF
{
    class DBManager
    {

        /// <summary>
        /// Cette structure me permet d'acquerir toutes les données d'une 
        /// opération qui est composer de plusieurs types de variables.
        /// </summary>
        public struct Oppration
        {
            public string nomOpération;
            public int position,temps;
            public bool cycleVerin, quittance;
        }

        /// <summary>
        /// Cette structure me permet d'acquerir toutes les données d'un 
        /// lot qui est composer de plusieurs types de variables.
        /// </summary>
        public struct InformationLot
        {
            public string statusLot,nomLot;
            public DateTime dateDeCreation,dateLimiteProd;
            public int quantiterAProduire;
            public int idRecette;
        }

        /// <summary>
        /// Cette structure me permet d'acquerir toutes les données d'un 
        /// message qui est composer de plusieurs types de variables
        /// </summary>
        public struct Message
        {
            public DateTime dateMessage;
            public string textMessage;
        }


        /// <summary>
        /// Cette chaîne de caractère contients les différentes 
        /// informations nécessaires pour se connecter à la base de données. 
        /// </summary>
        private static string connectionString = "server=Localhost;port=3306;database=module_3;user=root;password=";

        /// <summary>
        /// Création d'un objet MySqlConnection qui contient 
        /// les informations nécessaire pour se connecter à la base de données. 
        /// </summary>
        private static MySqlConnection connexion = new MySqlConnection(connectionString);



    /***************************************************************************************************************************************************\
      
                                                                   Login/Logout

    \***************************************************************************************************************************************************/


        /// <summary>
        /// Cette fonction me permet de me connecter à la base de données
        /// </summary>
        public static void Connexion()
        {
            if(connexion.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    connexion.Open();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        /// <summary>
        /// Cette fonction me permet de me déconnecter de la base de données
        /// </summary>
        public static void Deconnexion()
        {
            if(connexion.State == System.Data.ConnectionState.Open )
            {
                try
                {
                    connexion.Close();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        /***************************************************************************************************************************************************\

                                                                      Fonctions liées aux Lots 

        \***************************************************************************************************************************************************/

        /************************************************************\

                                    Public

        \************************************************************/


        /// <summary>
        /// Cette fonction envois une requête MySQL pour créer un lot
        /// </summary>
        /// <param name="nomLot">       Nom du lot                            </param>
        /// <param name="dateLimite">   Date Limite de Production             </param>
        /// <param name="quantiter">    Quantiter de pièces à produire        </param>
        /// <param name="idEtatlot">    Identification de l'état du lot       </param>
        /// <param name="idRecette">    Identification de la recette associés </param>
        public static void CreationLot(string nomLot,DateTime dateLimite,int quantiter,int idEtatlot, int idRecette)
        {

            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {

                    cmd.CommandText = "INSERT INTO lot VALUES (NULL,@lotNom,@dateCreation,@dateLimite,@quantité,@etatLot,@idrecette);";

                    cmd.Parameters.AddWithValue("@lotNom",          nomLot);
                    cmd.Parameters.AddWithValue("@dateCreation",    DateTime.Now.ToString("yyyy’-‘MM’-‘dd’ HH’:’mm’:’ss"));
                    cmd.Parameters.AddWithValue("@etatLot",         idEtatlot);
                    cmd.Parameters.AddWithValue("@dateLimite",      dateLimite);
                    cmd.Parameters.AddWithValue("@quantité",        quantiter);
                    cmd.Parameters.AddWithValue("@idrecette",       idRecette);

                    cmd.ExecuteNonQuery();

                    //Message de création du lot envoyer à la base de données
                    InsertMessage($"[CREAT] Création du lot {nomLot}", GetIDFromLot(nomLot));

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Cette fonction permet d'acquérir toutes 
        /// les informations concernant un lot.
        /// </summary>
        /// <param name="nomLot"> Nom du lot que nous voulons connaitre les informations </param>
        /// <returns> Retourne un tableau contenant toutes les informations du lot </returns>
        public static InformationLot[] GetLotInformation(string nomLot)
        {
            InformationLot[] lot = new InformationLot[1];
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {

                    cmd.CommandText = "SELECT LOT_CREATION,LOT_DATE_LIMITE,LOT_QUANTITER,ID_RECETTE,ET_LIBELLE " +
                                      "FROM `lot` JOIN etat ON lot.ID_ETAT = etat.ID_ETAT WHERE LOT_NOM = @NomLot;";

                    cmd.Parameters.AddWithValue("@NomLot", nomLot);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lot[0].dateDeCreation = reader.GetDateTime("LOT_CREATION");
                            lot[0].dateLimiteProd = reader.GetDateTime("LOT_DATE_LIMITE");
                            lot[0].quantiterAProduire = reader.GetInt32("LOT_QUANTITER");

                            bool validationRecette = reader.IsDBNull(3);
                            if (!validationRecette)
                            {
                                lot[0].idRecette = reader.GetInt32("ID_RECETTE");
                            }

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
        /// Cette fonction permet de mettre à jour la date limite de production.
        /// </summary>
        /// <param name="nomLot">             Nom du lot à modifier la date limite de production    </param>
        /// <param name="nouvelleDateLimite"> Nouvelle date limite de production                    </param>
        public static void UpdateLotDateLimite(string nomLot, DateTime nouvelleDateLimite)
        {
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "UPDATE lot SET LOT_DATE_LIMITE = @NouvelleDateLimite WHERE LOT_NOM = @NomLot;";
                    cmd.Parameters.AddWithValue("@NouvelleDateLimite", nouvelleDateLimite);
                    cmd.Parameters.AddWithValue("@NomLot", nomLot);

                    cmd.ExecuteNonQuery();

                    InsertMessage("[UPDATE] Mise à jour de la date limite de production", GetIDFromLot(nomLot));

                }
                catch (MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }
        }

        /// <summary>
        /// Cette fonction permet de mettre à jour la quantiter de pièces à produire.
        /// </summary>
        /// <param name="nomLot">               Nom du lot à modifier la date limite de production  </param>
        /// <param name="nouvelleQuantiter">    Nouvelle quantitée de production                    </param>
        public static void UpdateLotQuantiter(string nomLot, int nouvelleQuantiter)
        {
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "UPDATE lot SET LOT_QUANTITER = @NouvelleQuantiter WHERE LOT_NOM = @NomLot;";
                    cmd.Parameters.AddWithValue("@NouvelleQuantiter", nouvelleQuantiter);
                    cmd.Parameters.AddWithValue("@NomLot", nomLot);

                    cmd.ExecuteNonQuery();

                    InsertMessage($"[UPDATE] Mise à jour de la quantiter de pièce à produire", GetIDFromLot(nomLot));

                }
                catch (MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }
        }

        /// <summary>
        /// Cette fonction permet de mettre à jour la recette associée.
        /// </summary>
        /// <param name="nomLot">           Nom du lot à modifier la date limite de production  </param>
        /// <param name="nouvelleRecette">  Nom de la nouvelle recette associée                 </param>        
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

                    InsertMessage($"[UPDATE] Mise à jour de la recette de production", GetIDFromLot(nomLot));

                }
                catch (MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }
        }

        /// <summary>
        /// Cette fonction permet de supprimer un lot.
        /// </summary>
        /// <param name="nomLot"> Nom du lot à supprimer </param>
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

        /// <summary>
        /// Cette fonction nous permet de lister tous les lots avec leurs informations 
        /// </summary>
        /// <returns> Retourne un tableau contenant tous les lots et toute leurs informations </returns>
        public static InformationLot[] ListingLot()
        {
            InformationLot[] TabDesLots = new InformationLot[NombreTotalDeLot()];

            using (MySqlCommand cmd = connexion.CreateCommand())
            {

                try
                {
                    cmd.CommandText = "SELECT LOT_NOM,LOT_CREATION,LOT_DATE_LIMITE,LOT_QUANTITER,ID_RECETTE,ET_LIBELLE " +
                                      "FROM `lot` JOIN etat ON lot.ID_ETAT = etat.ID_ETAT;";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        int compteur = 0;

                        while (reader.Read())
                        {
                            TabDesLots[compteur].nomLot = reader.GetString("LOT_NOM");
                            TabDesLots[compteur].dateDeCreation = reader.GetDateTime("LOT_CREATION");
                            TabDesLots[compteur].dateLimiteProd = reader.GetDateTime("LOT_DATE_LIMITE");
                            TabDesLots[compteur].quantiterAProduire = reader.GetInt32("LOT_QUANTITER");

                            // Si l'id de la recette est NULL,
                            // le programme ne va pas lire l'ID de la recette.
                            bool validationRecette = reader.IsDBNull(4);
                            if (!validationRecette)
                            {
                                TabDesLots[compteur].idRecette = reader.GetInt32("ID_RECETTE");
                            }

                            TabDesLots[compteur].statusLot = reader.GetString("ET_LIBELLE");
                            compteur++;
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return TabDesLots;
        }

        /************************************************************\

                                    Private

        \************************************************************/


        /// <summary>
        /// Cette fonction me permet de connaitre l'ID d'un lot uniquement avec son nom. 
        /// </summary>
        /// <param name="nomLot"> Nom du lot </param>
        /// <returns> Retourne l'ID du lot </returns>
        private static int GetIDFromLot(string nomLot)
        {

            int idLot = -1;
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {

                    cmd.CommandText = "SELECT ID_LOT FROM lot WHERE LOT_NOM = @LotNom;";
                    cmd.Parameters.AddWithValue("@LotNom", nomLot);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            idLot = reader.GetInt32(0);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }
            return idLot;
        }

        /// <summary>
        /// Cette fonction me permet de connaitre le nombre total de lot dans la base de données.
        /// </summary>
        /// <returns> Retourne le nombre total de lot contenue dans la base de données </returns>
        private static int NombreTotalDeLot()
        {
            int nbreDeLot = 0;

            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM `lot`; ";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nbreDeLot = reader.GetInt32("COUNT(*)");
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return nbreDeLot;
        }


        /***************************************************************************************************************************************************\

                                                                     Fonctions liées aux Recette 

        \***************************************************************************************************************************************************/

        /************************************************************\

                            Public

        \************************************************************/


        /// <summary>
        ///  Cette fonction me permet de créer une recette dans la base de données.
        /// </summary>
        /// <param name="nomRecette"> Nom de la recette </param>
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
        /// Cette fonction me permet d'inserer des oppérations dans une recette.
        /// </summary>
        /// <param name="idRecette"> ID de la recette que nous voulons ajouter des opérations                 </param>
        /// <param name="opération"> Tableau contenant toutes les oppérations a envoyer à la base de données  </param>
        public static void AddOpperation(int idRecette, Oppration[] opération)
        {
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    foreach (Oppration value in opération)
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
        /// Cette fonction me permet de connaitre l'ID d'une recette uniquement avec son nom.  
        /// </summary>
        /// <param name="nomRecette"> Nom de la recette </param>
        /// <returns> Retourne l'ID de la recette  </returns>
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
        /// Cette fonction me permet de connaitre le nom d'une recette avec son ID
        /// </summary>
        /// <param name="idRecette"> ID de la recette </param>
        /// <returns> Retourne le nom de la recette </returns>
        public static string GetNameRecetteFromId(int idRecette)
        {

            string nomRecette = "";
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {

                    cmd.CommandText = "SELECT REC_NOM FROM recette WHERE ID_RECETTE = @IDRecette;";
                    cmd.Parameters.AddWithValue("@IDRecette", idRecette);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            nomRecette = reader.GetString(0);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }
            return nomRecette;
        }

        /// <summary>
        /// Cette fonction me permet de récupérer toute les oppérations lié a une recette.
        /// </summary>
        /// <param name="idRecette"> ID de la recette </param>
        /// <returns> Retourne une liste contenant toute les informations des opération lié à une recette </returns>
        public static List<Oppration> GetOpprationsFromRecette(int idRecette)
        {
            List<Oppration> opération = new List<Oppration>();
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
                            opération.Add(new Oppration{ nomOpération = reader.GetString("OP_NOM"),
                                                         position     = reader.GetInt32("OP_POSITION"),
                                                         temps        = reader.GetInt32("OP_TEMPS"),
                                                         cycleVerin   = reader.GetBoolean("OP_CYCLE_VERIN"),
                                                         quittance    = reader.GetBoolean("OP_QUITTANCE")}); ;
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

        /// <summary>
        /// Cette fonction me permet de récupérer la date de création d'une recette.
        /// </summary>
        /// <param name="nomRecette"> Nom de la recette </param>
        /// <returns> Retourne la date de création sous forme de chaîne de caractére </returns>
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

        /// <summary>
        /// Cette fonction me permet de connaitre le nombre total d'oppération contenue dans une recette.
        /// </summary>
        /// <param name="nomRecette"> Nom de la recette </param>
        /// <returns> Retourne le nombre total d'opération </returns>
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

        /***************************************************************************************************************************************************\

                                                             Fonctions liées aux Messages

        \***************************************************************************************************************************************************/

        /************************************************************\

                                   Public

        \************************************************************/


        /// <summary>
        /// Cette fonction me permet de récupérer tous l'historique d'un lot.
        /// </summary>
        /// <param name="nomLot"> Nom du lot </param>
        /// <returns> Retourne un tableau contenant tous l'historique d'un lot </returns>
        public static Message[] Historique(string nomLot)
        {
            Message[] TabMessage = new Message[NombreTotalMessage(nomLot)];

            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "SELECT MESS_DATE,MESS_TEXT FROM lot JOIN message ON " +
                                       "lot.ID_LOT = message.ID_LOT WHERE LOT_NOM = @nomLot ;";
                    cmd.Parameters.AddWithValue("@nomLot", nomLot);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int compteur = 0;
                        while (reader.Read())
                        {
                            TabMessage[compteur].dateMessage = reader.GetDateTime("MESS_DATE");
                            TabMessage[compteur].textMessage = reader.GetString("MESS_TEXT");
                            compteur++;
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }
            return TabMessage;
        }


        /************************************************************\

                                  Private

        \************************************************************/


        /// <summary>
        /// Cette fonction me permet de savoir le nombre total de message lié à un lot.
        /// </summary>
        /// <param name="nomLot"> Nom du lot </param>
        /// <returns> Retourne le nombre de message lié au lot </returns>
        private static int NombreTotalMessage(string nomLot)
        {
            int nbreDeMessage = 0;
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM lot JOIN message ON " +
                                       "lot.ID_LOT = message.ID_LOT WHERE LOT_NOM = @nomLot ;";
                    cmd.Parameters.AddWithValue("@nomLot", nomLot);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            nbreDeMessage = reader.GetInt32("COUNT(*)");
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }
            return nbreDeMessage;
        }

        /// <summary>
        /// Cette fonction me permet d'inserer un message et de le lié à un lot.
        /// </summary>
        /// <param name="message">  Message à envoyer                          </param>
        /// <param name="idLot">    ID du lot auxquelle le message viendra lié </param>
        private static void InsertMessage(string message, int idLot)
        {
            using (MySqlCommand cmd = connexion.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "INSERT INTO message VALUES(NULL,@message,@dateCreation,@idLot);";

                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters.AddWithValue("@dateCreation", DateTime.Now);
                    cmd.Parameters.AddWithValue("@idLot", idLot);

                    cmd.ExecuteNonQuery();
                }
                catch(MySqlException ex)
                {
                    Menu.ErrorMessage(ex.Message);
                }
            }
        }


    }
}
