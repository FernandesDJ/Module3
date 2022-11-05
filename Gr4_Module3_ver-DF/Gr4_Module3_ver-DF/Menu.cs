/***************************************************************************************************************************************************\
 * Déscription: Cette Class Menu gère tous se qui est de l'affichage. Tous les différents menus et leurs interactions se situent ici que se soit 
 *              Pour ajouter une recette, ajouter un lot, éditer un lot (modification des quantiters,des dates et de recette), de l'historique et 
 *              du listing des différents lots.
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
    class Menu
    {
        /// <summary>
        /// Cette constante me permet de placer le curseur 
        /// de la console à un endroit bien précis en axe X
        /// </summary>
        private const int POS_CURSOR_X = 57;

        /// <summary>
        /// Ces constantes me permettent de placer le curseur 
        /// de la console à un endroit bien précis en axe Y
        /// </summary>
        private const int SET_CURSOR_Y1 = 5;
        private const int SET_CURSOR_Y2 = 6;
        private const int SET_CURSOR_Y3 = 7;
        private const int SET_CURSOR_Y4 = 8;
        private const int SET_CURSOR_Y5 = 9;


        /***************************************************************************************************************************************************\

                                                                            Menu Principal

        \***************************************************************************************************************************************************/

        /// <summary>
        /// Cette fonction me permet d'afficher mon menu principal 
        /// Et de séléctioner le menu que l'utilisateur veux
        /// Consulter
        /// </summary>
        /// <returns> Retourne le choix du menu </returns>
        public static int Principal()
        {

            Titre("Menu Principal");
            Console.WriteLine("");
            Console.WriteLine("".PadRight(40) + "1) Créer un lot");
            Console.WriteLine("".PadRight(40) + "2) Créer une recette");
            Console.WriteLine("".PadRight(40) + "3) Editer un lot");
            Console.WriteLine("".PadRight(40) + "4) Information d'une recette");
            Console.WriteLine("".PadRight(40) + "5) Liste de lot");
            Console.WriteLine("".PadRight(40) + "6) Historique d'un lot");
            Console.WriteLine("".PadRight(40) + "7) Quitter");
            Console.WriteLine(" ".PadRight(101, '_'));


            int choix = 0;
            do
            {
                Console.Write("\nQuelle est votre choix? ");
                try
                {
                    choix = int.Parse(Console.ReadLine());

                    if (choix < 1 || choix > 7)
                    {
                        ErrorMessage("Veuiller entrer un nombre comprit entre 1 et 7");
                    }

                }
                catch (FormatException ex)
                {
                    ErrorMessage("Veuiller entrer un nombre comprit entre 1 et 7");
                }
                catch (OverflowException ex)
                {
                    ErrorMessage("Veuiller entrer un nombre comprit entre 1 et 7");
                }
            } while(choix < 1 || choix > 7);


            return choix;    

        }


        /***************************************************************************************************************************************************\

                                                                        Menu de création de lot

        \***************************************************************************************************************************************************/

        /// <summary>
        /// Cette fonction affiche le menu de création de lot et effectue 
        /// les différentes actions nécessaire pour créer un lot
        /// </summary>
        public static void CreationLot()
        {
            string nomLot,dateLimiteBrut, nomRecette;
            int idRecette = 0;
            DateTime dateLimitTraiter;
            bool donnéesOK;
            
            int etatLot = 1;

            do
            {
                Titre("1) Créer un lot");

                Console.Write("\n".PadRight(10) + "Nom: ");
                Console.Write("\n".PadRight(10) + "Date limite de fabrication (DD/MM/YYYY) : ");
                Console.Write("\n".PadRight(10) + "Nom de la recette affiliée (peut être vide): ");
                Console.Write("\n".PadRight(10) + "Quantitée à produire:");

                Console.WriteLine("\n\n".PadRight(101, '_'));

                bool quantiterValide;

                Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y1);
                nomLot = Console.ReadLine();
                Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y2);
                dateLimiteBrut = Console.ReadLine();
                Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y3);
                nomRecette = Console.ReadLine();
                Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y4);

                quantiterValide = int.TryParse(Console.ReadLine(),out int quantiter);

                if (quantiterValide == false)
                    quantiter = 0;

                try
                {
                    if (nomRecette == "")
                    {
                        nomRecette = null;
                        etatLot = 4;
                        quantiter = 0;
                    }
                    else
                    {
                        idRecette = DBManager.GetIDFromNameRecette(nomRecette);
                    }

                    Console.WriteLine("");
                    dateLimitTraiter = DateTime.Parse(dateLimiteBrut);

                    DBManager.CreationLot(nomLot, dateLimitTraiter, quantiter, etatLot, idRecette);

                    donnéesOK = true;
                }
                catch (FormatException ex)
                {
                    ErrorMessage(ex.Message);
                    donnéesOK = false;
                }
                catch (OverflowException ex)
                {
                    ErrorMessage(ex.Message);
                    donnéesOK = false;
                }
                catch (MySqlException ex)
                {
                    ErrorMessage(ex.Message);
                    donnéesOK = false;
                }

            } while (!donnéesOK);
        }


        /***************************************************************************************************************************************************\

                                                                    Menu de création de recette

        \***************************************************************************************************************************************************/

        /************************************************************\

                                    Public

        \************************************************************/


        /// <summary>
        /// Cette fonction affiche le menu de création de recette et effectue 
        /// les différentes actions nécessaire pour créer une recette
        /// </summary>
        public static void CreationRecette()
        {
            string nomRecette;
            bool nbreOpValide;

            Titre("2) Créer une Recette");

            Console.Write("\n".PadRight(10) + "Nom de recette: ");
            Console.Write("\n".PadRight(10) + "Nombre d'opérations: ");

            Console.WriteLine("\n\n".PadRight(101, '_'));

            Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y1);
            nomRecette = Console.ReadLine();
            Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y2);
            nbreOpValide = int.TryParse(Console.ReadLine(), out int nbresOp);

            if(nbreOpValide == false || nbresOp > 10 || nbresOp < 1)
            {
                ErrorMessage("Veuillez introduire un nombre compris entre 1 et 10");
            }

            DBManager.CreationRecette(nomRecette);

            int idRecette = DBManager.GetIDFromNameRecette(nomRecette);

            DBManager.AddOpperation(idRecette, AjouterOpération(nbresOp));
        }

        /************************************************************\

                                Private

        \************************************************************/

        /// <summary>
        /// Cette fonction permet d'afficher un sous-menu du menu de création de recette
        /// pour ajouter un nombre prédéfinis précédemment d'opérations
        /// </summary>
        /// <param name="nombreOpération"> Le nombre d'opérations à créer </param>
        /// <returns> Retourne un tableau contenant toutes les informations des opérations créer </returns>
        private static DBManager.Oppration[] AjouterOpération(int nombreOpération)
        {
            DBManager.Oppration[] opération = new DBManager.Oppration[nombreOpération];

            string nomOp;

            int position, temps;
            bool posValide, tempsValide, cycleValide, quittanceValide, cycle, quittance;

            for (int nbre_opération = nombreOpération; nbre_opération > 0; nbre_opération--)
            {
                do
                {
                    Titre($"2.1) Ajouter {nbre_opération} opération");
                    Console.Write("\n".PadRight(10) + "Nom de l'opération: ");
                    Console.Write("\n".PadRight(10) + "Position de déplacement (1 à 5): ");
                    Console.Write("\n".PadRight(10) + "Temps d'attente (0 à 5 s): ");
                    Console.Write("\n".PadRight(10) + "Cycle des vérins (true/false): ");
                    Console.Write("\n".PadRight(10) + "Quittance demander (true/false): ");

                    Console.WriteLine("\n\n".PadRight(101, '_'));

                    Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y1);                     //Nom de l'opération:
                    nomOp = Console.ReadLine();

                    Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y2);                     //Position de déplacement (1 à 5):
                    posValide = int.TryParse(Console.ReadLine(), out position);

                    Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y3);                     //Temps d'attente (0 à 5 s):
                    tempsValide = int.TryParse(Console.ReadLine(), out temps);

                    Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y4);                     //Cycle des vérins (t/f):
                    cycleValide = bool.TryParse(Console.ReadLine(), out cycle);

                    Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y5);                     //Quittance demander (t/f): 
                    quittanceValide = bool.TryParse(Console.ReadLine(), out quittance);

                    if (position < 1 || position > 5)
                        posValide = false;

                    if (temps < 0 || temps > 5)
                        tempsValide = false;

                    if (quittanceValide && cycleValide != true)
                        ErrorMessage("Veuillez introduire true pour OUI ou false pour NON");

                    if (tempsValide && posValide != true)
                        ErrorMessage("Veuillez introduire un nombre compris entre 0 et 5");

                    if (temps > 0 && cycle == true)
                    {
                        ErrorMessage("Le cycle des vérins ne sera pas effectuer car nous avons un temps d'attente");
                        cycle = false;
                    }

                    if ((posValide && tempsValide && cycleValide && quittanceValide) == false)
                    {
                        ErrorMessage($"De multiple erreurs de saisi ont été détecter.\nVeuillez recommencer la saisi de l'étape {nbre_opération}");
                    }

                } while ((posValide && tempsValide && cycleValide && quittanceValide) == false);

                opération[nombreOpération - nbre_opération].nomOpération = nomOp;
                opération[nombreOpération - nbre_opération].position = position;
                opération[nombreOpération - nbre_opération].temps = temps;
                opération[nombreOpération - nbre_opération].cycleVerin = cycle;
                opération[nombreOpération - nbre_opération].quittance = quittance;
            }
            return opération;
        }


        /***************************************************************************************************************************************************\

                                                                      Menu modification de lot

        \***************************************************************************************************************************************************/

        /************************************************************\

                                Public

        \************************************************************/


        /// <summary>
        /// Cette fonction affiche le menu de modification de lot et propose 
        /// les différentes modification possible
        /// </summary>
        public static void EditerLot()
        {
            Titre("3) Editer un lot");

            Console.Write("\n".PadRight(10) + "Introduire le nom du lot: ");

            Console.WriteLine("\n\n"+" ".PadRight(101, '_'));

            Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y1);
            string nomLot = Console.ReadLine();

            DBManager.InformationLot[] lotRechercher = DBManager.GetLotInformation(nomLot);

            bool choixValide;
            int choix;

            do
            {
                Titre($"3) Editer le lot: {nomLot}");

                Console.WriteLine("\n".PadRight(15) + "Choix d'édition".PadRight(50) + "Information du lot");

                Console.Write("\n".PadRight(5) + "1) Editer la date Limite de Production".PadRight(45) + "| Date de création:".PadRight(30) + $"{lotRechercher[0].dateDeCreation}");
                Console.Write("\n".PadRight(5) + "2) Editer la quantiter à produire".PadRight(45) + $"| Date Limite de Production:".PadRight(30) + $"{lotRechercher[0].dateLimiteProd.ToShortDateString()}");
                Console.Write("\n".PadRight(5) + "3) Changer de recette".PadRight(45) + $"| Quantiter à produire:".PadRight(30) + $"{lotRechercher[0].quantiterAProduire}");
                Console.Write("\n".PadRight(5) + "4) Supprimer le lot".PadRight(45) + $"| Nom de la recette associée:".PadRight(30) + $"{DBManager.GetNameRecetteFromId(lotRechercher[0].idRecette)}");
                Console.Write("\n".PadRight(5) + "5) Quitter le menu".PadRight(45) + $"| Status du lot:".PadRight(30) + $"{lotRechercher[0].statusLot}");

                Console.WriteLine("\n\n".PadRight(101, '_'));

                Console.Write("\nVeuillez introduire votre choix: ");

                choixValide = int.TryParse(Console.ReadLine(), out choix);

                if(choixValide != true || choix < 1 || choix > 5)
                {

                    ErrorMessage("Veuillez introduire un chiffre allant de 1 à 4");
                    choixValide = false;
                }

            } while (choixValide != true);

            switch(choix)
            {
                case 1: EditerLot_DateLimite(nomLot);   break;
                case 2: EditerLot_Quantiter(nomLot);    break;
                case 3: EditerLot_Recette(nomLot);      break;
                case 4: EditerLot_Supprimer(nomLot);    break;
                case 5: break;
            }
        }

        /************************************************************\

                                 Private

        \************************************************************/


        /// <summary>
        /// Cette fonction permet d'afficher le sous-menu du menu de modification de lot
        /// et d'éditer la date limite de production
        /// </summary>
        /// <param name="nomLot"> Nom du lot à éditer la date limite de production </param>
        private static void EditerLot_DateLimite(string nomLot)
        {
            bool nouvelleDateLimiteValide;
            DateTime nouvelleDate;
            do
            {
                Titre($"3.1) Modifier la date limite de production du lot: {nomLot}");
                Console.Write("\n".PadRight(10) + "Nouvelle date limite de fabrication (DD/MM/YYYY) : ");
                Console.WriteLine("\n\n".PadRight(101, '_'));

                Console.SetCursorPosition(POS_CURSOR_X+3, SET_CURSOR_Y1);
                nouvelleDateLimiteValide = DateTime.TryParse(Console.ReadLine(), out nouvelleDate);

                if (nouvelleDateLimiteValide != true)
                    ErrorMessage("Veuillez introduire une date avec la syntaxe indiquer");

            } while (nouvelleDateLimiteValide != true);

            DBManager.UpdateLotDateLimite(nomLot, nouvelleDate);

        }

        /// <summary>
        /// Cette fonction permet d'afficher le sous-menu du menu de modification de lot
        /// et d'éditer la quantiter de pièce à produire
        /// </summary>
        /// <param name="nomLot"> Nom du lot à éditer la quantiter de pièce à produire </param>
        private static void EditerLot_Quantiter(string nomLot)
        {
            bool nouvelleQuantiterValide;
            int nouvelleQuantiter;

            do
            {
                Titre($"3.2) Modifier la quantiter de production du lot: {nomLot}");
                Console.Write("\n".PadRight(10) + "Nouvelle quantiter à produire : ");
                Console.WriteLine("\n\n".PadRight(101, '_'));

                Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y1);
                nouvelleQuantiterValide = int.TryParse(Console.ReadLine(), out nouvelleQuantiter);

                if (nouvelleQuantiterValide != true)
                    ErrorMessage("Veuillez introduitre une quantiter valide");

            } while (nouvelleQuantiterValide != true);

            DBManager.UpdateLotQuantiter(nomLot, nouvelleQuantiter);
        }

        /// <summary>
        /// Cette fonction permet d'afficher le sous-menu du menu de modification de lot
        /// et d'éditer la recette associé au lot
        /// </summary>
        /// <param name="nomLot"> Nom du lot à éditer la recette associé au lot </param>
        private static void EditerLot_Recette(string nomLot)
        {

            Titre($"3.3) Modifier la recette du lot: {nomLot}");

           
            Console.Write("\n".PadRight(10) + "Nouvelle Recette : ");
            Console.WriteLine("\n\n".PadRight(101, '_'));
            Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y1);
            string nouvelleRecette = Console.ReadLine();

            DBManager.UpdateLotRecette(nomLot, nouvelleRecette);
        }

        /// <summary>
        /// Cette fonction permet d'afficher le sous-menu du menu de modification de lot
        /// et de supprimer celui-ci
        /// </summary>
        /// <param name="nomLot"> Nom du lot à supprimer </param>
        private static void EditerLot_Supprimer(string nomLot)
        {
            bool choixValide;
            do
            {
                Titre($"3.4) Supprimer le lot: {nomLot}");
                Console.Write("\n".PadRight(10) + $"Etes-vous sur de vouloir supprimer le lot {nomLot} : ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\nPour supprimer, introduisez en toutes lettre le mot << Autoriser >> ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Pour annuler la suppression, introduisez en toutes lettre le mot << Annuler >> ");
                Console.ResetColor();
                Console.WriteLine("\n".PadRight(101, '_'));

                Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y1);
                string choixUtilisateur = Console.ReadLine();

                if (choixUtilisateur.ToLower() == "autoriser")
                {
                    DBManager.DeleteLot(nomLot);
                    choixValide = true;
                }
                else if (choixUtilisateur.ToLower() == "annuler")
                {
                    choixValide = true;
                }
                else
                {
                    ErrorMessage("Veuillez introduire lettre par lettre l'une des deux commandes qui vous sont proposer");
                    choixValide = false;
                }

            } while (choixValide != true);
        }


        /***************************************************************************************************************************************************\

                                                              Menu d'affichage d'une recette

        \***************************************************************************************************************************************************/


        /// <summary>
        /// Cette fonction affiche les différentes information contenue dans 
        /// une recette sous forme d'un tableau.
        /// </summary>
        public static void InformationRecette()
        {
            Titre("4) Information d'une recette");

            Console.Write("\n".PadRight(10) + "Introduire le nom de la recette: ");

            Console.WriteLine("\n\n" + " ".PadRight(101, '_'));

            Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y1);
            string nomRecette = Console.ReadLine();
            string dateCreation = DBManager.GetRecetteDateCréation(nomRecette);

            Titre($"4) Information pour la recette: {nomRecette}");
            Console.Write("\n".PadRight(15) + $"Recette créer le: {DBManager.GetRecetteDateCréation(nomRecette)} \n");
            DBManager.Oppration[] opperationRecette = DBManager.GetOpprationsFromRecette(DBManager.GetIDFromNameRecette(nomRecette)).ToArray();

            Console.Write("\n".PadRight(7));

            Console.BackgroundColor = ConsoleColor.DarkGray;

            Console.Write("N° d'Opération".PadRight(20) +
                          "Nom".PadRight(10) +
                          "Position".PadRight(11) +
                          "Temps d'attente".PadRight(19) +
                          "Cycle des vérins?".PadRight(20) +
                          "Quittance?");

            Console.ResetColor();

            for (int nbreOperation = 0; nbreOperation < DBManager.NombreOperationRecette(nomRecette);nbreOperation++)
            {
                Console.Write("\n".PadRight(7) + $"Opération N° {nbreOperation+1}".PadRight(20) + $"{opperationRecette[nbreOperation].nomOpération}".PadRight(13) +
                                                                                                  $"{opperationRecette[nbreOperation].position}".PadRight(14) + 
                                                                                                  $"{opperationRecette[nbreOperation].temps}".PadRight(19) +
                                                                                                  $"{opperationRecette[nbreOperation].cycleVerin}".PadRight(17) +
                                                                                                  $"{opperationRecette[nbreOperation].quittance}");
            }

            Console.WriteLine("\n\n" + " ".PadRight(101, '_'));

            Console.WriteLine("Pour quitter appuyez sur la touche [ENTER]");

            bool visualisationOk;
            do
            {
                if (Console.ReadKey(true).Key != ConsoleKey.Enter)
                {
                    visualisationOk = true;
                }
                else
                {
                    visualisationOk = false;
                }
            } while (visualisationOk);
        }


        /***************************************************************************************************************************************************\

                                                              Menu d'affichage de tous les lots

        \***************************************************************************************************************************************************/


        /// <summary>
        /// Cette fonction affiche les différentes informations 
        /// de tous les lots présent dans la base de données
        /// </summary>
        public static void ListLot()
        {
            Titre("5) Liste de lot");

            Console.Write("\n");
            Console.BackgroundColor = ConsoleColor.DarkGray;

            Console.Write("Nom du Lot".PadRight(12) +
                          "Date de création".PadRight(18) +
                          "Date limite de Prod.".PadRight(22) +
                          "Quantiter".PadRight(11) +
                          "Nom de la recette".PadRight(19) +
                          "Status".PadRight(18) + "\n");

            Console.ResetColor();

            DBManager.InformationLot[] lots = DBManager.ListingLot();
            for (int nbreLots = 0; nbreLots < lots.Length; nbreLots++)
            {
                Console.Write($"\n{lots[nbreLots].nomLot}".PadRight(15) +
                              $"{lots[nbreLots].dateDeCreation.ToShortDateString()}".PadRight(18) +
                              $"{lots[nbreLots].dateLimiteProd.ToShortDateString()}".PadRight(22) +
                              $"{lots[nbreLots].quantiterAProduire}".PadRight(11) +
                              $"{DBManager.GetNameRecetteFromId(lots[nbreLots].idRecette)}".PadRight(23) +
                              $"{lots[nbreLots].statusLot}");
            }
            Console.WriteLine("\n\n" + " ".PadRight(101, '_'));

            Console.WriteLine("Pour quitter appuyez sur la touche [ENTER]");

            bool visualisationOk;
            do
            {
                if (Console.ReadKey(true).Key != ConsoleKey.Enter)
                {
                    visualisationOk = true;
                }
                else
                {
                    visualisationOk = false;
                }
            } while (visualisationOk);
        }


        /***************************************************************************************************************************************************\

                                                              Menu d'affichage de l'historique d'un lot

        \***************************************************************************************************************************************************/


        /// <summary>
        /// Cette fonction affiche tous les message lié à un lot 
        /// de sa création à la fin de ça production.
        /// </summary>
        public static void HistoriqueLot()
        {
            Titre("6) Historique d'un lot");

            Console.Write("\n".PadRight(10) + "Introduire le nom du lot: ");
            Console.WriteLine("\n\n" + " ".PadRight(101, '_'));

            Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y1);
            string nomLot = Console.ReadLine();

            Titre($"6) Historique du lot: {nomLot}");

            Console.Write("\n");
            Console.BackgroundColor = ConsoleColor.DarkGray;

            Console.Write("Date du message".PadRight(21) + "|".PadRight(41) + "Message".PadRight(39));
            Console.ResetColor();
            Console.WriteLine("");

            DBManager.Message[] TabMessage = DBManager.Historique(nomLot);

            for(int index = 0; index < TabMessage.Length; index++)
            {
                Console.WriteLine($"{TabMessage[index].dateMessage}".PadRight(25) + 
                                  $"{TabMessage[index].textMessage}");
            }
            Console.WriteLine("\n" + " ".PadRight(101, '_'));

            Console.WriteLine("Pour quitter appuyez sur la touche [ENTER]");

            bool visualisationOk;
            do
            {
                if (Console.ReadKey(true).Key != ConsoleKey.Enter)
                {
                    visualisationOk = true;
                }
                else
                {
                    visualisationOk = false;
                }
            } while (visualisationOk);
        }


        /***************************************************************************************************************************************************\

                                                              Affichage + validation des messages d'erreur

        \***************************************************************************************************************************************************/

        /// <summary>
        /// Cette fonction affiche les message d'erreur en rouge 
        /// et attend une action de l'utilisateur pour valider les messages 
        /// d'erreur
        /// </summary>
        /// <param name="message"> Message d'erreur à afficher </param>
        public static void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nERREUR: {message}\nPour continuer appuyer sur la touche [Enter]");
            bool erreurValider;
            do
            {
                if(Console.ReadKey(true).Key != ConsoleKey.Enter)
                {
                    erreurValider = true;
                }
                else
                {
                    erreurValider = false;
                }
            } while (erreurValider);
           
            Console.ResetColor();
        }


        /***************************************************************************************************************************************************\

                                                      Fonction de génération de titre automatique

        \***************************************************************************************************************************************************/


        /// <summary>
        /// Fonction permetant de générer le bloc "titre" de mes menus avec le nom
        /// du menu automatiquement centrer
        /// </summary>
        /// <param name="title"> Nom du menu </param>
        private static void Titre(string titreMenu)
        {
            Console.Clear();

            Console.Title = $"Module3/ {titreMenu}";

            int calcule = 101 - titreMenu.Length;
            int reductorForString = 3;
            int reductorForEndTable = 2;

            if (calcule % 2 == 1)
            {
                reductorForString = 2;
                reductorForEndTable = 1;
            }

            int before = (calcule - reductorForString) / 2;
            int after = before + reductorForEndTable;

            Console.WriteLine(" ".PadRight(100, '_'));
            Console.WriteLine("| ".PadRight(100) + '|');
            Console.WriteLine("|".PadRight(before, '-') + "{" + titreMenu + "}".PadRight(after, '-') + '|');
            Console.WriteLine("|".PadRight(100, '_') + '|');
        }
    }
}
