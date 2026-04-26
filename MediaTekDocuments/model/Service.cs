using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Service : représente le service auquel appartient un utilisateur.
    /// </summary>
    public class Service
    {
        public string Id { get; set; }
        public string Nom { get; set; }

        public Service(string id, string nom)
        {
            this.Id = id;
            this.Nom = nom;
        }
    }
}
