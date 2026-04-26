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
    public class ServiceTests
    {
        [TestMethod()]
        public void ServiceTest()
        {
            Service service = new Service("S1", "Administratif");

            Assert.AreEqual("S1", service.Id);
            Assert.AreEqual("Administratif", service.Nom);
        }
    }
}