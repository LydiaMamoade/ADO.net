using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2.Pages
{
    public class PageProduits : MenuPage
    {
        public PageProduits() : base("Produits", false)
        {

            Menu.AddOption("1", "Afficher la liste des categories", () => AfficheProduits());
            Menu.AddOption("2", "Créer un nouveau produit ", CreerProduit);
            Menu.AddOption("3", "Modifier un produit ", ModifierProduit);
            Menu.AddOption("4", "Supprimer un produit ", SupprimerProduit);
        }

        private void SupprimerProduit()
        {
            var idcat = AfficheProduits();

            Product produit = new Product();
            var saisieIDProd = Input.Read<int>("Entrer l'Id du produit à modifier ");
            produit.ProductId = saisieIDProd;
            try
            {
                Northwind2App.DataContext.SuppresionProduit(saisieIDProd);
                Output.WriteLine(ConsoleColor.Magenta, "Produit supprimer avec succès");
            }
            catch (SqlException e)
            {
                GérerErreursql(e);
            }
        }

        private void GérerErreursql(SqlException ex)
        {
            if (ex.Number == 547) Output.WriteLine(ConsoleColor.Red, "Le produit ne peut pas être supprimé car il est référencé par une commande");
            else throw ex;
        }

        private void ModifierProduit()
        {

            var idcat = AfficheProduits();


            Product modifProduit = new Product();

            var saisieIDProd = Input.Read<int>("Entrer l'Id du produit à modifier ");
            modifProduit.ProductId = saisieIDProd;
            var produit = Northwind2App.DataContext.GetProduit(saisieIDProd);

            var saisieNom = Input.Read<string>("Entrer le nom du produit ", produit.Name);
            modifProduit.Name = saisieNom;

            var saisieIdcat = Input.Read<Guid>("Entrer l'Id de catégorie ", produit.CategoryId);
            modifProduit.CategoryId = saisieIdcat;

            var saisieIdFour = Input.Read<int>("Entrer l'Id du fournisseur ", produit.SupplierId);
            modifProduit.SupplierId = saisieIdFour;

            var saisieUprix = Input.Read<decimal>("Entrer le prix unitaire ", produit.UnitPrice);
            modifProduit.UnitPrice = saisieUprix;

            var saisieUstock = Input.Read<Int16>("Entrer l'unité en stock ", produit.UnitsInStock);
            modifProduit.UnitsInStock = saisieUstock;

            Northwind2App.DataContext.AjouterModifierProduit(modifProduit, typeOperation.Modification);
            Output.WriteLine(ConsoleColor.Magenta, "Produit modifié avec succès");

        }

        private void CreerProduit()
        {

            IList<Categorie> ListeCat = new List<Categorie>();
            ListeCat = Northwind2App.DataContext.GetCategories();
            ConsoleTable.From(ListeCat, "Catégorie").Display("Liste de categorie");

            Product nouveauProduit = new Product();
            var saisieIdcat = Input.Read<Guid>("Entrer l'Id de catégorie ");
            nouveauProduit.CategoryId = saisieIdcat;

            var saisieIdFour = Input.Read<int>("Entrer l'Id du fournisseur ");
            nouveauProduit.SupplierId = saisieIdFour;

            var saisieNom = Input.Read<string>("Entrer le nom du produit ");
            nouveauProduit.Name = saisieNom;

            var saisieUprix = Input.Read<decimal>("Entrer le prix unitaire ");
            nouveauProduit.UnitPrice = saisieUprix;

            var saisieUstock = Input.Read<Int16>("Entrer l'unité en stock ");
            nouveauProduit.UnitsInStock = saisieUstock;

            Northwind2App.DataContext.AjouterModifierProduit(nouveauProduit, typeOperation.Ajout);
            Output.WriteLine(ConsoleColor.Magenta, "Produit crée avec succès ");

        }

        private Guid AfficheProduits()
        {

            //List<Categorie> ListeCat = new List<Categorie>();
            var ListeCat = Northwind2App.DataContext.GetCategories();
            ConsoleTable.From(ListeCat, "Catégorie").Display("Liste de categorie");

            var saisieIdcat = Input.Read<Guid>("Entrer l'Id de catégorie ");
            IList<Product> ListeProduit = new List<Product>();
            ListeProduit = Northwind2App.DataContext.GetProduits(saisieIdcat);
            ConsoleTable.From(ListeProduit, "Produits").Display("Liste de produits");
            return saisieIdcat;
        }
    }


}
