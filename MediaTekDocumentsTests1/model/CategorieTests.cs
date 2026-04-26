using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model.Tests
{
    [TestClass()]
    public class CategorieTests
    {
        [TestMethod()]
        public void CategorieTest()
        {
            Categorie categorie = new Categorie("0001", "Horreur");

            Assert.AreEqual("0001", categorie.Id);
            Assert.AreEqual("Horreur", categorie.Libelle);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Categorie categorie = new Categorie("0001", "Horreur");

            Assert.AreEqual("Horreur", categorie.ToString());
        }
    }
}