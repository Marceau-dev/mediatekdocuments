using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Utilisateur : représente une personne autorisée à se connecter à l'application.
    /// </summary>
    public class Utilisateur
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Pwd { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string IdService { get; set; }
        public string Service { get; set; }

        public Utilisateur(string id, string login, string pwd, string nom, string prenom, string idService, string service)
        {
            this.Id = id;
            this.Login = login;
            this.Pwd = pwd;
            this.Nom = nom;
            this.Prenom = prenom;
            this.IdService = idService;
            this.Service = service;
        }
    }
}
