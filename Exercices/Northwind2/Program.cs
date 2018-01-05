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
           
       }

    }

    /*
      1.3	Insertion, mise à jour et suppression de données
      Lorsqu’on tente de supprimer un produit qui est référencé par une ligne de commande, l’application plante. Pour y remédier :
•	Retrouver le numéro de l’erreur SQL correspondante en exécutant la requête dans SSMS
•	Dans la méthode SupprimerProduit de PageProduit, entourer l’appel à la méthode du contexte d’un bloc try – catch,
et à l’intérieur du bloc catch, appeler une méthode interne GérerErreurSql 
•	Dans GérerErreurSql, intercepter les erreurs de type System.Data.SqlClient.SqlException, et afficher en rouge 
un message spécifique pour l’erreur précédente. Les autres erreurs doivent être renvoyées telles quelles, pour faire planter l’application

     1.2	Affichage de données
     Etape 6 : Récupération des clients et commandes en une seule requête
Dans le contexte, créer une méthode GetClientsCommandes permettant de récupérer en une seule requête 
la liste des clients (Id et nom de société) et de leurs commandes associées (Id, date de commande, date d’envoi, frais,
nombre d’articles différents et montant total).
Créer une nouvelle page PageClientsCommandes dérivant de Page, et ouverte par une entrée de menu « 2. Clients et commandes » dans la page d’accueil.
Redéfinir sa méthode Display en affichant la liste des clients obtenue par la méthode de contexte GetClientsCommandes
Demander la saisie d’un id de client
Au moyen d’une requête Linq, extraire la liste de commandes de ce client, puis l’afficher en tableau.
Tester avec le client « CACTU ». Comment s’affiche la date de livraison de la commande lorsqu’elle est nulle dans la base ? Comment mieux gérer ce cas ?

     */

}
