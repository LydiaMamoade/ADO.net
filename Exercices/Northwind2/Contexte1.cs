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

    public class Contexte1 : IDataContext
    {
        public IList<string> GetPaysFournisseurs()
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

        public void SuppresionProduit(int IdProd)
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
        
        IList<Supplier> IDataContext.GetFournisseurs(string saisiePays)
        {
            List<Supplier> listFournisseur = new List<Supplier>();
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
                        var four = new Supplier();
                        four.SupplierId = (int)sdr["SupplierId"];
                        four.CompanyName = (string)sdr["CompanyName"];

                        listFournisseur.Add(four);

                    }
                }
            }
            return listFournisseur;
        }
        
        public int GetNbProduits(string PaysChosie)
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

        public IList<Categorie> GetCategories()
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

        public IList<Product> GetProduits(Guid categorieID)
        {
            var listProduit = new List<Product>();

            var cmd = new SqlCommand();
            cmd.CommandText = @"select P.ProductId, P.UnitPrice, P.UnitsInStock
                                    from Product p
                                    inner join Category S on (S.CategoryId =P.CategoryId)
                                    where S.CategoryId = @catId 
                                    order by P.ProductId
                                    ";
            cmd.Parameters.Add(new SqlParameter
            {
                SqlDbType = SqlDbType.UniqueIdentifier,
                ParameterName = "@catId",
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
                        var prod = new Product();
                        prod.ProductId = (int)sdr["ProductId"];
                        prod.UnitPrice = (decimal)sdr["UnitPrice"];
                        prod.UnitsInStock = (Int16)sdr["UnitsInStock"];
                        listProduit.Add(prod);
                    }
                }
            }
            return listProduit;
        }

        public Product GetProduit(int id)
        {
            var com = new SqlCommand();
            var prod = new Product();
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

                        prod.Name = (string)sdr["Name"];
                        prod.CategoryId = (Guid)sdr["CategoryId"];
                        prod.SupplierId = (int)sdr["SupplierId"];
                        prod.UnitPrice = (decimal)sdr["UnitPrice"];


                    }
                }
            }
            return prod;
        }

        public void AjouterModifierProduit(Product produit, typeOperation operation)
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
                    Value = produit.CategoryId
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    ParameterName = "@IdFour",
                    Value = produit.SupplierId
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    ParameterName = "@nom",
                    Value = produit.Name
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
                    Value = produit.ProductId,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    ParameterName = "@nom",
                    Value = produit.Name
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    ParameterName = "@IdCat",
                    Value = produit.CategoryId
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    ParameterName = "@IdFour",
                    Value = produit.SupplierId
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

        public int EnregistrerModifsProduits()
        {
            return 0;
        }

    }

}