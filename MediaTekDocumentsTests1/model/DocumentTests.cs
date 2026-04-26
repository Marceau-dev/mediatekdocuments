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
    public class DocumentTests
    {
        [TestMethod()]
        public void DocumentTest()
        {
            Document document = new Document("D001", "Titre", "image.jpg", "G1", "Roman", "P1", "Adulte", "R1", "Littérature");

            Assert.AreEqual("D001", document.Id);
            Assert.AreEqual("Titre", document.Titre);
            Assert.AreEqual("image.jpg", document.Image);
        }
    }
}