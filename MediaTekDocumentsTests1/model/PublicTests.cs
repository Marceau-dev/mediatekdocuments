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
    public class PublicTests
    {
        [TestMethod()]
        public void PublicTest()
        {
            Public lePublic = new Public("0003", "Jeunesse");

            Assert.AreEqual("0003", lePublic.Id);
            Assert.AreEqual("Jeunesse", lePublic.Libelle);
        }
    }
}