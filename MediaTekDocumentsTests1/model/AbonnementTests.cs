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
    public class AbonnementTests
    {
        [TestMethod()]
        public void AbonnementTest()
        {
            Abonnement abonnement = new Abonnement("A001", new DateTime(2026, 1, 1), 120.50, new DateTime(2026, 12, 31), "R001");

            Assert.AreEqual("A001", abonnement.Id);
            Assert.AreEqual(new DateTime(2026, 1, 1), abonnement.DateCommande);
            Assert.AreEqual(120.50, abonnement.Montant);
            Assert.AreEqual(new DateTime(2026, 12, 31), abonnement.DateFinAbonnement);
            Assert.AreEqual("R001", abonnement.IdRevue);
        }
    }
}