﻿/*
 * 
 * 
 * 
 */

using System;


namespace Gr4_Module3_ver_DF
{
    class Program
    {
        static void Main(string[] args)
        {
            DBManager.Connexion();


            Menu.CreationLot();




            DBManager.Deconnexion();

            
        }



    }
}
