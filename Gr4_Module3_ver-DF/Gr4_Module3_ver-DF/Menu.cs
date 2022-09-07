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
            int quantiter,idRecette = 0;
            DateTime dateLimitTraiter;
            bool donnéesOK = true;
            
            int etatLot = 1;

            

            do
            {
                
                Titre("1) Créer un lot");

                Console.Write("\n".PadRight(10)  +  "Nom: ");
                Console.Write("\n".PadRight(10)  +  "Date limite de fabrication : ");
                Console.Write("\n".PadRight(10)  +  "Nom de la recette affiliée (peut être vide): ");
                Console.Write("\n".PadRight(10)  +  "Quantitée à produire:");
                

                Console.WriteLine("\n\n".PadRight(101, '_'));

                
                Console.SetCursorPosition(POS_CURSOR_LOT_CREA_X, SET_CURSOR_LOT_CREA_NAME_Y);
                nomLot = Console.ReadLine();
                Console.SetCursorPosition(POS_CURSOR_LOT_CREA_X, SET_CURSOR_LOT_CREA_DATELIMITE_Y);
                dateLimiteBrut = Console.ReadLine();
                Console.SetCursorPosition(POS_CURSOR_LOT_CREA_X, SET_CURSOR_LOT_CREA_RECETTE_Y);
                nomRecette = Console.ReadLine();
                Console.SetCursorPosition(POS_CURSOR_LOT_CREA_X, SET_CURSOR_LOT_CREA_QUANTITER_Y);

                

                try
                {
                    
                    if (nomRecette == "")
                    {
                        nomRecette = null;
                        etatLot = 4;
                    }
                    else
                    {
                        quantiter = int.Parse(Console.ReadLine());
                        idRecette = DBManager.GetIDFromNameRecette(nomRecette);
                    }
                    

                    
                    Console.WriteLine("");
                    dateLimitTraiter = DateTime.Parse(dateLimiteBrut);

      
                }
                catch(FormatException ex)
                {
                    ErrorMessage(ex.Message);
                    donnéesOK = false;
                    
                }
                catch(OverflowException ex)
                {
                    ErrorMessage(ex.Message);
                    donnéesOK = false;
                    
                }
                /*catch(MySqlException ex)
                {
                    ErrorMessage(ex.Message);
                    donnéesOK = false;
                    Console.ReadLine();
                }*/

                


            } while (!donnéesOK);

            

            //DBManager.CreationLot(nomLot, dateLimite, quantiter, nomRecette);
            
            

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private static void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERREUR: {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        private static void Titre(string title)
        {
            Console.Clear();

            Console.Title = $"Module3/ {title}";

            int calcule = 101 - title.Length;
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
            Console.WriteLine("|".PadRight(before, '-') + "{" + title + "}".PadRight(after, '-') + '|');
            Console.WriteLine("|".PadRight(100, '_') + '|');

        }

    }
}
