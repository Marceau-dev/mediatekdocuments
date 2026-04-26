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
    public class LivreTests
    {
        [TestMethod()]
        public void LivreTest()
        {
            Livre livre = new Livre("L001", "1984", "image.jpg", "9781234567890", "Orwell", "Folio", "G1", "Roman", "P1", "Adulte", "R1", "Littérature");

            Assert.AreEqual("L001", livre.Id);
            Assert.AreEqual("1984", livre.Titre);
            Assert.AreEqual("9781234567890", livre.Isbn);
            Assert.AreEqual("Orwell", livre.Auteur);
            Assert.AreEqual("Folio", livre.Collection);
        }
    }
}