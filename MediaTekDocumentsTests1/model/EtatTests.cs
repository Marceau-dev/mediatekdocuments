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
    public class EtatTests
    {
        [TestMethod()]
        public void EtatTest()
        {
            Etat etat = new Etat("0001", "Neuf");

            Assert.AreEqual("0001", etat.Id);
            Assert.AreEqual("Neuf", etat.Libelle);
        }
    }
}