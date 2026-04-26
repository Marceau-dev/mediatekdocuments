using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    public class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les commandes d'un document
        /// </summary>
        /// <param name="idLivreDvd">id du document concerné</param>
        /// <returns>Liste d'objets CommandeDocumentSuivi</returns>
        public List<CommandeDocumentSuivi> GetCommandesDocument(string idLivreDvd)
        {
            return access.GetCommandesDocument(idLivreDvd);
        }


        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// getter sur la liste des suivis
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivis()
        {
            return access.GetAllSuivis();
        }

        /// <summary>
        /// Retourne les abonnements d'une revue
        /// </summary>
        /// <param name="idRevue">id de la revue concernée</param>
        /// <returns>Liste des abonnements</returns>
        public List<Abonnement> GetAbonnementsRevue(string idRevue)
        {
            return access.GetAbonnementsRevue(idRevue);
        }

        /// <summary>
        /// Retourne les abonnements qui se terminent dans moins de 30 jours
        /// </summary>
        /// <returns>Liste des abonnements concernés</returns>
        public List<Abonnement> GetFinAbonnements()
        {
            return access.GetFinAbonnements();
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// Crée un livre dans la bdd
        /// </summary>
        /// <param name="livre">L'objet Livre concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerLivre(Livre livre)
        {
            return access.CreerLivre(livre);
        }

        /// <summary>
        /// Crée un dvd dans la bdd
        /// </summary>
        /// <param name="dvd">L'objet Dvd concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerDvd(Dvd dvd)
        {
            return access.CreerDvd(dvd);
        }

        /// <summary>
        /// ecriture d'une revue en base de données
        /// </summary>
        /// <param name="revue">revue à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerRevue(Revue revue)
        {
            return access.CreerRevue(revue);
        }

        /// <summary>
        /// Crée une commande de livre ou dvd
        /// </summary>
        /// <param name="id">id de la commande</param>
        /// <param name="dateCommande">date</param>
        /// <param name="montant">montant</param>
        /// <param name="nbExemplaire">nombre d'exemplaires</param>
        /// <param name="idLivreDvd">id du document</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommandeDocument(string id, DateTime dateCommande, double montant, int nbExemplaire, string idLivreDvd)
        {
            return access.CreerCommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd);
        }

        /// <summary>
        /// Crée un abonnement
        /// </summary>
        /// <param name="id">id de la commande</param>
        /// <param name="dateCommande">date de commande</param>
        /// <param name="montant">montant</param>
        /// <param name="dateFinAbonnement">date de fin d'abonnement</param>
        /// <param name="idRevue">id de la revue</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerAbonnement(string id, DateTime dateCommande, double montant, DateTime dateFinAbonnement, string idRevue)
        {
            return access.CreerAbonnement(id, dateCommande, montant, dateFinAbonnement, idRevue);
        }

        /// <summary>
        /// Modifie un livre dans la bdd
        /// </summary>
        /// <param name="id">id du livre à modifier</param>
        /// <param name="livre">L'objet Livre contenant les nouvelles valeurs</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierLivre(string id, Livre livre)
        {
            return access.ModifierLivre(id, livre);
        }

        /// <summary>
        /// Modifie un dvd dans la bdd
        /// </summary>
        /// <param name="id">id du dvd à modifier</param>
        /// <param name="dvd">L'objet Dvd contenant les nouvelles valeurs</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierDvd(string id, Dvd dvd)
        {
            return access.ModifierDvd(id, dvd);
        }

        /// <summary>
        /// Modifie une revue dans la bdd
        /// </summary>
        /// <param name="id">id de la revue à modifier</param>
        /// <param name="revue">L'objet Revue contenant les nouvelles valeurs</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierRevue(string id, Revue revue)
        {
            return access.ModifierRevue(id, revue);
        }

        /// <summary>
        /// Modifie le suivi d'une commande de livre ou dvd
        /// </summary>
        /// <param name="id">id de la commande</param>
        /// <param name="idSuivi">nouvel id de suivi</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierSuiviCommandeDocument(string id, string idSuivi)
        {
            return access.ModifierSuiviCommandeDocument(id, idSuivi);
        }

        /// <summary>
        /// Supprime un livre dans la bdd
        /// </summary>
        /// <param name="id">id du livre à supprimer</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerLivre(string id)
        {
            return access.SupprimerLivre(id);
        }

        /// <summary>
        /// Supprime un dvd dans la bdd
        /// </summary>
        /// <param name="id">id du dvd à supprimer</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerDvd(string id)
        {
            return access.SupprimerDvd(id);
        }

        /// <summary>
        /// Supprime une revue dans la bdd
        /// </summary>
        /// <param name="id">id de la revue à supprimer</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerRevue(string id)
        {
            return access.SupprimerRevue(id);
        }

        /// <summary>
        /// Supprime une commande de livre ou dvd
        /// </summary>
        /// <param name="id">id de la commande</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerCommandeDocument(string id)
        {
            return access.SupprimerCommandeDocument(id);
        }

        /// <summary>
        /// Indique si une date de parution est comprise dans la période d'un abonnement
        /// </summary>
        /// <param name="dateCommande">date de début</param>
        /// <param name="dateFinAbonnement">date de fin</param>
        /// <param name="dateParution">date de parution</param>
        /// <returns>true si la parution est dans l'abonnement</returns>
        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFinAbonnement;
        }

        /// <summary>
        /// Supprime un abonnement
        /// </summary>
        /// <param name="id">id de l'abonnement</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerAbonnement(string id)
        {
            return access.SupprimerAbonnement(id);
        }


    }
}
