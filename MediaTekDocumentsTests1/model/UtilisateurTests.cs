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
    public class UtilisateurTests
    {
        [TestMethod()]
        public void UtilisateurTest()
        {
            Utilisateur utilisateur = new Utilisateur("U1", "admin", "admin123", "Dupont", "Alice", "S1", "Administratif");

            Assert.AreEqual("U1", utilisateur.Id);
            Assert.AreEqual("admin", utilisateur.Login);
            Assert.AreEqual("admin123", utilisateur.Pwd);
            Assert.AreEqual("Dupont", utilisateur.Nom);
            Assert.AreEqual("Alice", utilisateur.Prenom);
            Assert.AreEqual("S1", utilisateur.IdService);
            Assert.AreEqual("Administratif", utilisateur.Service);
        }
    }
}