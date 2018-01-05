using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2
{
    public interface IDataContext
    {
        IList<string> GetPaysFournisseurs();
        IList<Supplier> GetFournisseurs(string saisiePays);
        int GetNbProduits(string PaysChosie);
        IList<Categorie> GetCategories();
        IList<Product> GetProduits(Guid categorieID);
        Product GetProduit(int id);
        void AjouterModifierProduit(Product produit, typeOperation operation);
        void SuppresionProduit(int IdProd);
        int EnregistrerModifsProduits();
    }
}
