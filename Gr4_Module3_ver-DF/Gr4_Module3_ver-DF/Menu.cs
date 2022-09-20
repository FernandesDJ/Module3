using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace Gr4_Module3_ver_DF
{
    class Menu
    {

        private const int POS_CURSOR_X = 57;

        private const int SET_CURSOR_Y1 = 5;
        private const int SET_CURSOR_Y2 = 6;
        private const int SET_CURSOR_Y3 = 7;
        private const int SET_CURSOR_Y4 = 8;
        private const int SET_CURSOR_Y5 = 9;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int Principal()
        {

            Titre("Menu Principal");
            Console.WriteLine("");
            Console.WriteLine("".PadRight(40) + "1) Créer un lot");
            Console.WriteLine("".PadRight(40) + "2) Créer une recette");
            Console.WriteLine("".PadRight(40) + "3) Editer un lot");
            Console.WriteLine("".PadRight(40) + "4) Editer une recette");
            Console.WriteLine("".PadRight(40) + "5) Liste de lot");
            Console.WriteLine("".PadRight(40) + "6) Historique d'un lot précis");
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


        /// <summary>
        /// 
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

        /// <summary>
        /// 
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

        public static void EditerLot()
        {
            Titre("3) Editer un lot");

            Console.Write("\n".PadRight(10) + "Introduire le nom du lot: ");

            Console.WriteLine("\n\n"+" ".PadRight(101, '_'));

            Console.SetCursorPosition(POS_CURSOR_X, SET_CURSOR_Y1);
            string nomLot = Console.ReadLine();

            DBManager.informationLot[] lotRechercher = DBManager.GetLotInformation(nomLot);



            Titre($"3.1 Editer le lot: {nomLot}");

            Console.WriteLine("\n".PadRight(15) + "Choix d'édition".PadRight(50) + "Information du lot");

            Console.Write("\n".PadRight(5) + "1) Editer la date Limite de Production".PadRight(45) + "| Date de création:".PadRight(30) + $"{lotRechercher[0].dateDeCreation}");
            Console.Write("\n".PadRight(5) + "2) Editer la quantiter à produire".PadRight(45) + $"| Date Limite de Production:".PadRight(30) + $"{lotRechercher[0].dateLimiteProd.ToShortDateString()}");
            Console.Write("\n".PadRight(5) + "3) Changer de recette".PadRight(45) + $"| Quantiter à produire:".PadRight(30) + $"{lotRechercher[0].quantiterAProduire}");
            Console.Write("\n".PadRight(5) + "4) Supprimer le lot".PadRight(45) + $"| Nom de la recette associée:".PadRight(30) + $"{lotRechercher[0].nomRecette}");
            Console.Write("\n".PadRight(50) + "| Status du lot:".PadRight(30) + $"{lotRechercher[0].statusLot}");

            Console.WriteLine("\n\n".PadRight(101, '_'));





        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void ErrorMessage(string message)
        {
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nERREUR: {message}\nPour recommencer appuyer sur la touche [Enter]");
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombreOpération"></param>
        /// <returns></returns>
        private static DBManager.oppration[] AjouterOpération(int nombreOpération)
        {
            DBManager.oppration[] opération = new DBManager.oppration[nombreOpération];

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


                } while (posValide && tempsValide && cycleValide && quittanceValide == false);


                opération[nombreOpération - nbre_opération].nomOpération = nomOp;
                opération[nombreOpération - nbre_opération].position = position;
                opération[nombreOpération - nbre_opération].temps = temps;
                opération[nombreOpération - nbre_opération].cycleVerin = cycle;
                opération[nombreOpération - nbre_opération].quittance = quittance;

            }

            return opération;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="titreMenu"></param>
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
