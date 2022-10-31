/***************************************************************************************************************************************************\
 * Déscription: Cette Class Programm est le programme principal.
 * 
 * Auteur: Fernandes David
 * 
 * Début du projet: 14.03.2022
 * Fin du projet:   07.11.2022
 * 
\***************************************************************************************************************************************************/


using System;


namespace Gr4_Module3_ver_DF
{
    class Program
    {
        /// <summary>
        /// Le Main est le debut de mon programme.
        /// A l'intérieur il contient le gestionaire de mes menus
        /// </summary>
        static void Main(string[] args)
        {
            int choixMenu = 0;
            DBManager.Connexion();
            do
            {
                switch (choixMenu)
                {
                    case 1: Menu.CreationLot(); choixMenu = 0; break;
                    case 2: Menu.CreationRecette(); choixMenu = 0; break;
                    case 3: Menu.EditerLot(); choixMenu = 0; break;
                    case 4: Menu.InformationRecette(); choixMenu = 0; break;
                    case 5: Menu.ListLot(); choixMenu = 0; break;
                    case 6: Menu.HistoriqueLot(); choixMenu = 0; break;
                    default: choixMenu = Menu.Principal(); break;
                }

            } while (choixMenu < 7);
            DBManager.Deconnexion();

            
        }

    }
}
