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
    public class CommandeDocumentSuiviTests
    {
        [TestMethod()]
        public void CommandeDocumentSuiviTest()
        {
            CommandeDocumentSuivi commande = new CommandeDocumentSuivi("C003", new DateTime(2026, 4, 1), 59.99, 2, "DVD001", "0002", "Livrée");

            Assert.AreEqual("C003", commande.Id);
            Assert.AreEqual(new DateTime(2026, 4, 1), commande.DateCommande);
            Assert.AreEqual(59.99, commande.Montant);
            Assert.AreEqual(2, commande.NbExemplaire);
            Assert.AreEqual("DVD001", commande.IdLivreDvd);
            Assert.AreEqual("0002", commande.IdSuivi);
            Assert.AreEqual("Livrée", commande.LibelleSuivi);
        }
    }
}