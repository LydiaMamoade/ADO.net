using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
    class Entites
    {
    }
  
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string CompanyName { get; set; }

        public virtual Address Address { get; set; }
        public Guid AddressId{ get; set; }
    }
    public class Categorie
    {
        public Guid Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
    }
  
    public class  Product
    { 
        public Guid CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public Int16 UnitsInStock { get; set; }

    }

    //public class Produits
    //{
    //    public int IdProduit { get; set; }
    //    public decimal UnitPrice { get; set; }
    //    public Int16 UnitsInStock { get; set; }
    //}

    public class Address
    {
        public Guid AddressId { get; set; }
        public string Country { get; set; }
    }
}
