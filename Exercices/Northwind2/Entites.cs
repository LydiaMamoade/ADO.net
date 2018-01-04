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
    public class Fournisseur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
    }
    public class Categorie
    {
        public Guid Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
    }

    public class Produits : Produit
    {
        public Guid Idcategorie { get; set; }
        public int Idfournisseur { get; set; }
        public string Nom { get; set; }

    }

    public class Produit
    {
        public int IdProduit { get; set; }
        public decimal UnitPrice { get; set; }
        public Int16 UnitsInStock { get; set; }
    }

}
