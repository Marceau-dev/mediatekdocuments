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
    public class RevueTests
    {
        [TestMethod()]
        public void RevueTest()
        {
            Revue revue = new Revue("R001", "Science et Vie", "image.jpg", "G1", "Science", "P1", "Tout public", "R1", "Presse", "Mensuel", 30);

            Assert.AreEqual("R001", revue.Id);
            Assert.AreEqual("Science et Vie", revue.Titre);
            Assert.AreEqual("Mensuel", revue.Periodicite);
            Assert.AreEqual(30, revue.DelaiMiseADispo);
        }
    }
}