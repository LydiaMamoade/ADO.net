using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
    class Contexte3 : DbContext, IDataContext

    {
        public Contexte3() : base("name =Exercies.Settings.Northwind2Connect") { }
        //public DbSet<Produit> produit { get; set; }
        public DbSet<Product> produit { get; set; }
        public DbSet<Supplier> fournisseurs { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public void AjouterModifierProduit(Product produit, typeOperation operation)
        {
            throw new NotImplementedException();
        }

        public int EnregistrerModifsProduits()
        {
            throw new NotImplementedException();
        }

        public IList<Categorie> GetCategories()
        {
            throw new NotImplementedException();
            //  var fournis = _contexte.Suppliers.Include(s => s.Products).ThenInclude(p => p.Category)
        }

        public IList<Supplier> GetFournisseurs(string pays)
        {
            return fournisseurs.Where(f => f.Address.Country == pays).ToList();

        }

        public int GetNbProduits(string PaysChosie)
        {
            
           var  Param= new SqlParameter
            {
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@pays",
                Value = PaysChosie
            };

            return Database.SqlQuery<int>(@"select Count(ProductId) NbProduits
                                from Product P
                                inner join Supplier S on (S.SupplierId = P.SupplierId)
                                inner  join Address A  on (A.AddressId = S.AddressId)
                                where A.Country = @pays
                           ", Param).Single();

        }

        public IList<string> GetPaysFournisseurs()
        {
            return fournisseurs.Select(f => f.Address.Country).Distinct().ToList();
        }

        public Product GetProduit(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Product> GetProduits(Guid categorieID)
        {
            throw new NotImplementedException();
        }

        public void SuppresionProduit(int IdProd)
        {
            throw new NotImplementedException();
        }
    }
}
