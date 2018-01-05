using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2.Pages
{
    public class PageFournisseur : MenuPage
    {
        public PageFournisseur() : base("Fournisseur", false)
        {
            Menu.AddOption("1", "Afficher la liste des pays", AfficheListePays);
            Menu.AddOption("2", "Fournisseur d'un pays", AfficheListeFournisseurs);
            Menu.AddOption("3", "Nombre de produits d’un pays", AfficheListeNbProduits);
        }

        private void AfficheListeNbProduits()
        {
            string saisiePays = Input.Read<string>("Entrer un pays pour lequel vous voulez connaitre le nombre de produit:");
            var NbProduits = Northwind2App.DataContext.GetNbProduits(saisiePays);
            Output.WriteLine(ConsoleColor.Magenta, NbProduits.ToString() + " produits");
        }

        public void AfficheListeFournisseurs()
        {
            string saisiePays = Input.Read<string>("Entrer un pays pour lequel vous voulez connaitre les fournisseur");
            var listFournisseur = Northwind2App.DataContext.GetFournisseurs(saisiePays);
            ConsoleTable.From(listFournisseur, "Fournisseur").Display("Liste des fournisseur");
        }

        public void AfficheListePays()
        {
           // IList<string> ListePays = new List<string>();
            var ListePays = Northwind2App.DataContext.GetPaysFournisseurs();
            ConsoleTable.From(ListePays, "Pays").Display("Liste de Pays fournisseur");

        }
    }
}
