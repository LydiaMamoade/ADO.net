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

            Produit produit = new Produit();
            var saisieIDProd = Input.Read<int>("Entrer l'Id du produit à modifier ");
            produit.IdProduit = saisieIDProd;
            try
            {
                Contexte.SuppresionProduit(saisieIDProd);
                Output.WriteLine(ConsoleColor.Magenta, "Produit supprimer avec succès");
            }
            catch (SqlException e)
            {
                GérerErreursql( e);
            }
        }

        private void GérerErreursql( SqlException ex)
        {
            if (ex.Number == 547) Output.WriteLine(ConsoleColor.Red, "Le produit ne peut pas être supprimé car il est référencé par une commande");
            else throw ex;
        }

        private void ModifierProduit()
        {

            var idcat = AfficheProduits();


            Produits modifProduit = new Produits();

            var saisieIDProd = Input.Read<int>("Entrer l'Id du produit à modifier ");
            modifProduit.IdProduit = saisieIDProd;
            var produit = Contexte.GetProduit(saisieIDProd);

            var saisieNom = Input.Read<string>("Entrer le nom du produit ", produit.Nom);
            modifProduit.Nom = saisieNom;

            var saisieIdcat = Input.Read<Guid>("Entrer l'Id de catégorie ", produit.Idcategorie);
            modifProduit.Idcategorie = saisieIdcat;

            var saisieIdFour = Input.Read<int>("Entrer l'Id du fournisseur ", produit.Idfournisseur);
            modifProduit.Idfournisseur = saisieIdFour;

            var saisieUprix = Input.Read<decimal>("Entrer le prix unitaire ", produit.UnitPrice);
            modifProduit.UnitPrice = saisieUprix;

            var saisieUstock = Input.Read<Int16>("Entrer l'unité en stock ", produit.UnitsInStock);
            modifProduit.UnitsInStock = saisieUstock;

            Contexte.AjouterModifierProduit(modifProduit, typeOperation.Modification);
            Output.WriteLine(ConsoleColor.Magenta, "Produit modifié avec succès");

        }

        private void CreerProduit()
        {

            List<Categorie> ListeCat = new List<Categorie>();
            ListeCat = Contexte.GetCategories();
            ConsoleTable.From(ListeCat, "Catégorie").Display("Liste de categorie");

            Produits nouveauProduit = new Produits();
            var saisieIdcat = Input.Read<Guid>("Entrer l'Id de catégorie ");
            nouveauProduit.Idcategorie = saisieIdcat;

            var saisieIdFour = Input.Read<int>("Entrer l'Id du fournisseur ");
            nouveauProduit.Idfournisseur = saisieIdFour;

            var saisieNom = Input.Read<string>("Entrer le nom du produit ");
            nouveauProduit.Nom = saisieNom;

            var saisieUprix = Input.Read<decimal>("Entrer le prix unitaire ");
            nouveauProduit.UnitPrice = saisieUprix;

            var saisieUstock = Input.Read<Int16>("Entrer l'unité en stock ");
            nouveauProduit.UnitsInStock = saisieUstock;

            Contexte.AjouterModifierProduit(nouveauProduit, typeOperation.Ajout);
            Output.WriteLine(ConsoleColor.Magenta, "Produit crée avec succès ");

        }

        private Guid AfficheProduits()
        {

            List<Categorie> ListeCat = new List<Categorie>();
            ListeCat = Contexte.GetCategories();
            ConsoleTable.From(ListeCat, "Catégorie").Display("Liste de categorie");

            var saisieIdcat = Input.Read<Guid>("Entrer l'Id de catégorie ");
            List<Produit> ListeProduit = new List<Produit>();
            ListeProduit = Contexte.GetProduits(saisieIdcat);
            ConsoleTable.From(ListeProduit, "Produits").Display("Liste de produits");
            return saisieIdcat;
        }
    }


}
