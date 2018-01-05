using Microsoft.VisualStudio.TestTools.UnitTesting;
using Northwind2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind2.Tests
{
    [TestClass()]
    public class TestsContexte
    {
        //TestGetPaysFournisseurs	Vérifier qu’on récupère 16 pays et que le dernier est les USA
        [TestMethod()]
        public void TestGetPaysFournisseurs()
        {
            var list = Northwind2App.DataContext.GetPaysFournisseurs();
            Assert.AreEqual(16, list.Count);
            Assert.AreEqual("USA", list[list.Count - 1]);
        }
        
        //Vérifier que les fournisseurs du Japon sont ceux d’id 6 et 4
        [TestMethod()]
        public void TestGetFournisseurs()
        {
            var list = Northwind2App.DataContext.GetFournisseurs("Japan");

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].CompanyName == "Japan")   Assert.IsTrue(list[i].SupplierId == 6 | list[i].SupplierId== 4 );
              
            }

        }
        
        //Vérifier que le Royaume Uni propose 7 produits
        [TestMethod()]
        public void TestGetNbProduits()
        {
            Assert.AreEqual(5, Northwind2App.DataContext.GetNbProduits("UK"));
        }

        
        //Vérifier qu’on récupère 8 catégories de produits et que la dernière est nommée « Seafood »
        [TestMethod()]
        public void TestGetCategories()
        {
            var list = Northwind2App.DataContext.GetCategories();
            Assert.AreEqual(8, list.Count);
            Assert.AreEqual("Seafood", list[list.Count - 1].Nom);
        }

        
        //Vérifier qu’il y a 12 produits dans la catégorie Seafood et que le 7ème est le N° 40
        [TestMethod()]
        public void TestGetProduits()
        {
            var listC = Northwind2App.DataContext.GetCategories();

            for (int i = 0; i < listC.Count; i++)
            {
                if (listC[i].Nom == "Seafood")
                {
                    var listF = Northwind2App.DataContext.GetProduits(listC[i].Id);
                    Assert.AreEqual(12, listF.Count);
                    Assert.AreEqual(40, listF[6].ProductId);
                }

            }
        }
        
        //
        //[TestMethod()]
        //public void TestGetProduit()
        //{
        //    Assert.Fail();
        //}
        
        //Ajouter un nouveau produit dans la catégorie Cheeses et vérifier qu’il y a désormais 11 produits dans cette catégorie
        [TestMethod()]
        public void TestAjouterModifierProduit()
        {
            Product p = new Product();
            var listC = Northwind2App.DataContext.GetCategories();
            for (int i = 0; i < listC.Count; i++)
            {
                if (listC[i].Description == "Cheeses")
                {
                    p.CategoryId = listC[i].Id;
                    p.SupplierId = 5;
                    p.Name = "Nouveau Produit";
                    p.UnitPrice = 5.5m;
                    p.UnitsInStock = 5;

                    Northwind2App.DataContext.AjouterModifierProduit(p, typeOperation.Ajout);

                    var listF = Northwind2App.DataContext.GetProduits(listC[i].Id);
                    Assert.AreEqual(11, listF.Count);
                }
            }
        }
        
        //Supprimer le produit créé précédemment et vérifier qu’il y a de nouveau 10 produits dans la catégorie
        [TestMethod()]
       public void TestSuppresionProduit()
        {
            Product p = new Product();
            var listC = Northwind2App.DataContext.GetCategories();
            for (int i = 0; i < listC.Count; i++)
            {
                if (listC[i].Description == "Cheeses")
                {
                    var listF = Northwind2App.DataContext.GetProduits(listC[i].Id);
                    for (int iter = 0; iter < listF.Count; iter++)
                        if (listF[i].UnitPrice == 5.5m && listF[i].UnitsInStock == 5)
                        {
                            Northwind2App.DataContext.SuppresionProduit(listF[i].ProductId);
                            Assert.AreEqual(10, listF.Count);
                        }
                }

            }

            //var listF = Contexte.GetProduits(Guid.Parse("323734f8-a4ac-4d92-b4e5-a4e896fc32a2"));
            //Contexte.SuppresionProduit(listF.Count - 1);
            //Assert.AreEqual(11, listF.Count);
        }

    }
}