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
    public class DvdTests
    {
        [TestMethod()]
        public void DvdTest()
        {
            Dvd dvd = new Dvd("DVD001", "Inception", "image.jpg", 148, "Christopher Nolan", "Science-fiction", "G1", "SF", "P1", "Adulte", "R1", "Cinéma");

            Assert.AreEqual("DVD001", dvd.Id);
            Assert.AreEqual("Inception", dvd.Titre);
            Assert.AreEqual(148, dvd.Duree);
            Assert.AreEqual("Christopher Nolan", dvd.Realisateur);
            Assert.AreEqual("Science-fiction", dvd.Synopsis);
        }
    }
}