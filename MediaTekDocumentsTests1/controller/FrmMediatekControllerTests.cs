using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.controller.Tests
{
    [TestClass()]
    public class FrmMediatekControllerTests
    {
        private static readonly FrmMediatekController controller = new FrmMediatekController();

        [TestMethod()]
        public void FrmMediatekControllerTest()
        {

        }

        [TestMethod()]
        public void GetAllGenresTest()
        {

        }

        [TestMethod()]
        public void GetAllLivresTest()
        {

        }

        [TestMethod()]
        public void GetAllDvdTest()
        {

        }

        [TestMethod()]
        public void GetAllRevuesTest()
        {

        }

        [TestMethod()]
        public void GetCommandesDocumentTest()
        {

        }

        [TestMethod()]
        public void GetAllRayonsTest()
        {

        }

        [TestMethod()]
        public void GetAllPublicsTest()
        {

        }

        [TestMethod()]
        public void GetExemplairesRevueTest()
        {

        }

        [TestMethod()]
        public void GetAllSuivisTest()
        {

        }

        [TestMethod()]
        public void GetAbonnementsRevueTest()
        {

        }

        [TestMethod()]
        public void CreerExemplaireTest()
        {

        }

        [TestMethod()]
        public void CreerLivreTest()
        {

        }

        [TestMethod()]
        public void CreerDvdTest()
        {

        }

        [TestMethod()]
        public void CreerRevueTest()
        {

        }

        [TestMethod()]
        public void CreerCommandeDocumentTest()
        {

        }

        [TestMethod()]
        public void CreerAbonnementTest()
        {

        }

        [TestMethod()]
        public void ModifierLivreTest()
        {

        }

        [TestMethod()]
        public void ModifierDvdTest()
        {

        }

        [TestMethod()]
        public void ModifierRevueTest()
        {

        }

        [TestMethod()]
        public void ModifierSuiviCommandeDocumentTest()
        {

        }

        [TestMethod()]
        public void SupprimerLivreTest()
        {

        }

        [TestMethod()]
        public void SupprimerDvdTest()
        {

        }

        [TestMethod()]
        public void SupprimerRevueTest()
        {

        }

        [TestMethod()]
        public void SupprimerCommandeDocumentTest()
        {

        }

        [TestMethod()]
        public void ParutionDansAbonnementTest()
        {
            bool resultat = controller.ParutionDansAbonnement(
                new DateTime(2026, 5, 1),
                new DateTime(2026, 5, 31),
                new DateTime(2026, 5, 15)
            );

            Assert.IsTrue(resultat, "Echec du test : pour que le test réussie il faut que la date soit dans la période");
        }

        [TestMethod()]
        public void ParutionDansAbonnementHorsPeriodeTest()
        {
            bool resultat = controller.ParutionDansAbonnement(
                new DateTime(2026, 5, 1),
                new DateTime(2026, 5, 31),
                new DateTime(2026, 6, 1)
            );

            Assert.IsFalse(resultat, "Echec du test : pour que le test réussie il faut que la date soit hors période");
        }

        [TestMethod()]
        public void SupprimerAbonnementTest()
        {

        }
    }
}