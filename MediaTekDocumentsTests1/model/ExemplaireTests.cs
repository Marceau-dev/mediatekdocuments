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
    public class ExemplaireTests
    {
        [TestMethod()]
        public void ExemplaireTest()
        {
            DateTime dateAchat = new DateTime(2026, 4, 26);
            Exemplaire exemplaire = new Exemplaire(1, dateAchat, "photo.jpg", "0001", "R001");

            Assert.AreEqual(1, exemplaire.Numero);
            Assert.AreEqual(dateAchat, exemplaire.DateAchat);
            Assert.AreEqual("photo.jpg", exemplaire.Photo);
            Assert.AreEqual("0001", exemplaire.IdEtat);
            Assert.AreEqual("R001", exemplaire.Id);
        }
    }
}