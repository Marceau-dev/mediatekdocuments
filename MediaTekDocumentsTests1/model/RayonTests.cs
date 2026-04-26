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
    public class RayonTests
    {
        [TestMethod()]
        public void RayonTest()
        {
            Rayon rayon = new Rayon("0004", "Presse");

            Assert.AreEqual("0004", rayon.Id);
            Assert.AreEqual("Presse", rayon.Libelle);
        }
    }
}