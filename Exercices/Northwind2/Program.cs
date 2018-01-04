using Northwind2;
using Northwind2.Pages;
using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercies
{
    class Program
    {
        static void Main(string[] args)
        {
            Northwind2App app = Northwind2App.Instance;
            app.Title = "Northwind2";
      

            // Ajout des pages
            Page accueil = new PageAccueil();
            app.AddPage(accueil);
            app.AddPage(new PageFournisseur());
            app.AddPage(new PageProduits());
            // app.AddPage(new PageTestTableaux());

            // Affichage de la page d'accueil
            app.NavigateTo(accueil);
            app.Run();
       }

    }

    /*Lorsqu’on tente de supprimer un produit qui est référencé par une ligne de commande, l’application plante. Pour y remédier :
•	Retrouver le numéro de l’erreur SQL correspondante en exécutant la requête dans SSMS
•	Dans la méthode SupprimerProduit de PageProduit, entourer l’appel à la méthode du contexte d’un bloc try – catch,
et à l’intérieur du bloc catch, appeler une méthode interne GérerErreurSql 
•	Dans GérerErreurSql, intercepter les erreurs de type System.Data.SqlClient.SqlException, et afficher en rouge 
un message spécifique pour l’erreur précédente. Les autres erreurs doivent être renvoyées telles quelles, pour faire planter l’application

     
     */

}
