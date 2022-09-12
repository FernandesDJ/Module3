using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace Gr4_Module3_ver_DF
{
    class Menu
    {

        private const int POS_CURSOR_LOT_CREA_X = 57;

        private const int SET_CURSOR_LOT_CREA_NAME_Y = 5;
        private const int SET_CURSOR_LOT_CREA_DATELIMITE_Y = 6;
        private const int SET_CURSOR_LOT_CREA_RECETTE_Y = 7;
        private const int SET_CURSOR_LOT_CREA_QUANTITER_Y = 8;


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

                Console.SetCursorPosition(POS_CURSOR_LOT_CREA_X, SET_CURSOR_LOT_CREA_NAME_Y);
                nomLot = Console.ReadLine();
                Console.SetCursorPosition(POS_CURSOR_LOT_CREA_X, SET_CURSOR_LOT_CREA_DATELIMITE_Y);
                dateLimiteBrut = Console.ReadLine();
                Console.SetCursorPosition(POS_CURSOR_LOT_CREA_X, SET_CURSOR_LOT_CREA_RECETTE_Y);
                nomRecette = Console.ReadLine();
                Console.SetCursorPosition(POS_CURSOR_LOT_CREA_X, SET_CURSOR_LOT_CREA_QUANTITER_Y);
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

            Console.SetCursorPosition(POS_CURSOR_LOT_CREA_X, SET_CURSOR_LOT_CREA_NAME_Y);
            nomRecette = Console.ReadLine();
            Console.SetCursorPosition(POS_CURSOR_LOT_CREA_X, SET_CURSOR_LOT_CREA_DATELIMITE_Y);
            nbreOpValide = int.TryParse(Console.ReadLine(), out int nbresOp);

            if(nbreOpValide == false || nbresOp > 10 || nbresOp < 1)
            {
                ErrorMessage("Veuillez introduire un nombre compris entre 1 et 10");
            }

            AjouterOpération(nbresOp);



        }




        private static DBManager.oppration[] AjouterOpération(int nombreOpération)
        {
            DBManager.oppration[] opération = new DBManager.oppration[nombreOpération];

            Titre($"2.1) Ajouter {nombreOpération} opération");






            return opération;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private static void ErrorMessage(string message)
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
