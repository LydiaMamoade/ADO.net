using Outils.TConsole;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
    public enum typeOperation { Ajout, Modification }

    public class Contexte
    {
        public static List<string> GetPaysFournisseurs()
        {
            List<string> ListePaysFournisseur = new List<string>();
            var psf = new SqlCommand();
            psf.CommandText = @"SELECT distinct Country
                                 FROM Address A
                                 inner join Supplier S on(A.AddressId = S.AddressId)
                                 order by 1 ";
            using (var cnx = new SqlConnection(Settings.Default.Northwind2Connect))
            {
                // On affecte la connexion à la commande
                psf.Connection = cnx;
                // On ouvre la connexion
                cnx.Open();

                // On exécute la commande en récupérant son résultat dans un objet SqlDataRedader
                using (SqlDataReader sdr = psf.ExecuteReader())
                {
                    // On lit les lignes de résultat une par une
                    while (sdr.Read())
                    {
                        //...et pour chacune on crée un objet qu'on ajoute à la liste
                        string pays;
                        pays = (string)sdr["Country"];

                        ListePaysFournisseur.Add(pays);
                    }
                }
            }
            return ListePaysFournisseur;
        }

        //public static void GérerErreursql(object sqlException, object e)
        //{

        //}

        internal static void SuppresionProduit(int IdProd)
        {
            var cmd = new SqlCommand();
            cmd.CommandText = @"delete from Product  where ProductId = @idprod ";
            cmd.Parameters.Add(new SqlParameter
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@idprod",
                Value = IdProd
            });
            using (var cnx = new SqlConnection(Settings.Default.Northwind2Connect))
            {
                cmd.Connection = cnx;
                cnx.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Fournisseur> GetFournisseurs()
        {
            var listFournisseur = new List<Fournisseur>();

            // On créé une commande et on définit le code sql à exécuter
            var fr = new SqlCommand();
            fr.CommandText = "Select SupplierId, CompanyName  from Supplier";

            // On crée une connexion à partir de la chaîne de connexion stockée
            // dans les paramètres de l'appli
            using (var cnx = new SqlConnection(Settings.Default.Northwind2Connect))
            {
                // On affecte la connexion à la commande
                fr.Connection = cnx;
                // On ouvre la connexion
                cnx.Open();

                // On exécute la commande en récupérant son résultat dans un objet SqlDataRedader
                using (SqlDataReader sdr = fr.ExecuteReader())
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
            return listFournisseur;
        }

        public static int GetNbProduits(string PaysChosie)
        {
            // On créé une commande et on définit le code sql à exécuter
            var cmd = new SqlCommand();
            cmd.CommandText = @"select dbo.ufn_GetNbProduits(@Pays)";
            //@"select Count(ProductId) NbProduits
            //                    from Product P
            //                    inner join Supplier S on (S.SupplierId = P.SupplierId)
            //                    inner join Address A  on (A.AddressId = S.AddressId)
            //                    where A.Country = @pays
            //               ";
            cmd.Parameters.Add(new SqlParameter
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@pays",
                Value = PaysChosie
            });

            using (var cnx = new SqlConnection(Settings.Default.Northwind2Connect))
            {
                cmd.Connection = cnx;
                cnx.Open();

                return (int)cmd.ExecuteScalar();
            }

        }

        public static List<Categorie> GetCategories()
        {
            var listCategorie = new List<Categorie>();

            var cmd = new SqlCommand();
            cmd.CommandText = @"select CategoryId, Name, Description
                               from Category";

            using (var cnx = new SqlConnection(Settings.Default.Northwind2Connect))
            {
                cmd.Connection = cnx;
                cnx.Open();

                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        var cat = new Categorie();
                        cat.Id = (Guid)sdr["CategoryId"];
                        cat.Nom = (string)sdr["Name"];
                        cat.Description = (string)sdr["Description"];
                        listCategorie.Add(cat);
                    }
                }
            }
            return listCategorie;
        }

        public static List<Produit> GetProduits(string categorieID)
        {
            var listProduit = new List<Produit>();

            var cmd = new SqlCommand();
            cmd.CommandText = @"select P.ProductId, P.UnitPrice, P.UnitsInStock
                                    from Product p
                                    inner join Category S on (S.CategoryId =P.CategoryId)
                                    where S.CategoryId = @catId 
                                    order by P.ProductId
                                    ";
            cmd.Parameters.Add(new SqlParameter
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@catId ",
                Value = categorieID
            });

            using (var cnx = new SqlConnection(Settings.Default.Northwind2Connect))
            {
                cmd.Connection = cnx;
                cnx.Open();

                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        var prod = new Produit();
                        prod.IdProduit = (int)sdr["ProductId"];
                        prod.UnitPrice = (decimal)sdr["UnitPrice"];
                        prod.UnitsInStock = (Int16)sdr["UnitsInStock"];
                        listProduit.Add(prod);
                    }
                }
            }
            return listProduit;
        }

       public static Produits GetProduit( int id)
        {
            var com = new SqlCommand();
            var prod = new Produits();
            com.CommandText = @"select Name, CategoryId, SupplierId, UnitPrice
                   from Product where ProductID = @Id";
            com.Parameters.Add(new SqlParameter
            {
                SqlDbType = SqlDbType.Int,
                ParameterName = "@Id",
                Value = id,
            });
            using (var cnx = new SqlConnection(Settings.Default.Northwind2Connect))
            {
                com.Connection = cnx;
                cnx.Open();
                using (SqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        
                        prod.Nom = (string)sdr["Name"];
                        prod.Idcategorie = (Guid)sdr["CategoryId"];
                        prod.Idfournisseur = (int)sdr["SupplierId"];
                        prod.UnitPrice = (decimal)sdr["UnitPrice"];


                    }
                }
            }
            return prod;
        }

        public static void AjouterModifierProduit(Produits produit, typeOperation operation)
        {
            var cmd = new SqlCommand();
           
            if (operation == typeOperation.Ajout)
            {

                cmd.CommandText = @"Insert  Product ( CategoryId, SupplierId, Name, UnitPrice, UnitsInStock)
                                Values ( @IdCat, @IdFour, @nom, @prix, @stock)
                                    ";
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    ParameterName = "@IdCat",
                    Value = produit.Idcategorie
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    ParameterName = "@IdFour",
                    Value = produit.Idfournisseur
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    ParameterName = "@nom",
                    Value = produit.Nom
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.Money,
                    ParameterName = "@prix",
                    Value = produit.UnitPrice
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.SmallInt,
                    ParameterName = "@stock",
                    Value = produit.UnitsInStock
                });
            }

            else if (operation == typeOperation.Modification)
            {

                cmd.CommandText = @"Update Product set Name = @nom, CategoryId =  @IdCat, SupplierId = @IdFour, UnitPrice = @prix, UnitsInStock = @stock
                                     where ProductId =  @Id
                                   ";
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    ParameterName = "@Id",
                    Value = produit.IdProduit,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    ParameterName = "@nom",
                    Value = produit.Nom
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    ParameterName = "@IdCat",
                    Value = produit.Idcategorie
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    ParameterName = "@IdFour",
                    Value = produit.Idfournisseur
                });
                
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.Money,
                    ParameterName = "@prix",
                    Value = produit.UnitPrice
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.SmallInt,
                    ParameterName = "@stock",
                    Value = produit.UnitsInStock
                });
            }
            
            using (var cnx = new SqlConnection(Settings.Default.Northwind2Connect))
            {
                cmd.Connection = cnx;
                cnx.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

}