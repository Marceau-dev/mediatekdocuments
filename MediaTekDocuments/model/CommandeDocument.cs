using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier CommandeDocument
    /// </summary>
    internal class CommandeDocument
    {
        public string Id { get; set; }
        public int NbExemplaire { get; set; }
        public string IdLivreDvd { get; set; }
        public string IdSuivi { get; set; }

        public CommandeDocument(string id, int nbExemplaire, string idLivreDvd, string idSuivi)
        {
            this.Id = id;
            this.NbExemplaire = nbExemplaire;
            this.IdLivreDvd = idLivreDvd;
            this.IdSuivi = idSuivi;
        }
    }
}
