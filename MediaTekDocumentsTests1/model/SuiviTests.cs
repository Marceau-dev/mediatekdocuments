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
    public class SuiviTests
    {
        [TestMethod()]
        public void SuiviTest()
        {
            Suivi suivi = new Suivi("0001", "En cours");

            Assert.AreEqual("0001", suivi.Id);
            Assert.AreEqual("En cours", suivi.Libelle);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Suivi suivi = new Suivi("0001", "En cours");

            Assert.AreEqual("En cours", suivi.ToString());
        }
    }
}