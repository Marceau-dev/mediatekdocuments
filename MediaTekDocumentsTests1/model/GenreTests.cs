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
    public class GenreTests
    {
        [TestMethod()]
        public void GenreTest()
        {
            Genre genre = new Genre("0002", "Roman");

            Assert.AreEqual("0002", genre.Id);
            Assert.AreEqual("Roman", genre.Libelle);
        }
    }
}