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
            var NbProduits = Contexte.GetNbProduits(saisiePays);
            Output.WriteLine(ConsoleColor.Magenta, NbProduits.ToString() + " produits");
        }

        public void AfficheListeFournisseurs()
        {
            string saisiePays = Input.Read<string>("Entrer un pays pour lequel vous voulez connaitre les fournisseur");

            var cmd = new SqlCommand();
            cmd.CommandText = @" SELECT S.SupplierId, S.CompanyName
                                 FROM Address A  
                                 inner join Supplier S on (A.AddressId = S.AddressId)
                                 where A.Country = @pays";
            cmd.Parameters.Add(new SqlParameter
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@pays",
                Value = saisiePays
            });

            var listFournisseur = new List<Fournisseur>();
            // On crée une connexion à partir de la chaîne de connexion stockée
            // dans les paramètres de l'appli
            using (var cnx = new SqlConnection(Settings.Default.Northwind2Connect))
            {
                // On affecte la connexion à la commande
                cmd.Connection = cnx;
                // On ouvre la connexion
                cnx.Open();

                // On exécute la commande en récupérant son résultat dans un objet SqlDataRedader
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    // On lit les lignes de résultat une par une
                    while (sdr.Read())
                    {
                        //...et pour chacune on crée un objet qu'on ajoute à la liste
                        var four = new Fournisseur();
                        four.Id = (int)sdr["SupplierId"];
                        four.Nom = (string)sdr["CompanyName"];

                        listFournisseur.Add(four);

                    }
                }
            }
            // Le fait d'avoir créé la connexion dans une instruction using
            // permet de fermer cette connexion automatiquement à la fin du bloc using




            //List<Fournisseur> List = new List<Fournisseur>();
            //List = Contexte.GetFournisseurs();
            ConsoleTable.From(listFournisseur, "Fournisseur").Display("Liste des fournisseur");
        }

        public void AfficheListePays()
        {
            List<string> ListePays = new List<string>();
            ListePays = Contexte.GetPaysFournisseurs();
            ConsoleTable.From(ListePays, "Pays").Display("Liste de Pays fournisseur");

        }
    }
}
