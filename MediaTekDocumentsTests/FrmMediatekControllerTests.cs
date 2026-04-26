using System;
using System.Collections.Generic;
using MediaTekDocuments.controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocumentsTests
{
    [TestClass]
    public class FrmMediatekControllerTests
    {
        [TestMethod]
        public void ParutionDansAbonnement_RetourneTrue_SiDateDansPeriode()
        {
            FrmMediatekController controller = new FrmMediatekController();

            bool resultat = controller.ParutionDansAbonnement(
                new DateTime(2026, 5, 1),
                new DateTime(2026, 5, 31),
                new DateTime(2026, 5, 15)
            );

            Assert.IsTrue(resultat);
        }

        [TestMethod]
        public void ParutionDansAbonnement_RetourneTrue_SiDateEgaleDateDebut()
        {
            FrmMediatekController controller = new FrmMediatekController();

            bool resultat = controller.ParutionDansAbonnement(
                new DateTime(2026, 5, 1),
                new DateTime(2026, 5, 31),
                new DateTime(2026, 5, 1)
            );

            Assert.IsTrue(resultat);
        }

        [TestMethod]
        public void ParutionDansAbonnement_RetourneFalse_SiDateHorsPeriode()
        {
            FrmMediatekController controller = new FrmMediatekController();

            bool resultat = controller.ParutionDansAbonnement(
                new DateTime(2026, 5, 1),
                new DateTime(2026, 5, 31),
                new DateTime(2026, 6, 1)
            );

            Assert.IsFalse(resultat);
        }
    }
}
