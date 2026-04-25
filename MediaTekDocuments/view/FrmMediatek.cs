using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();
        private string modeGestionLivre = "";

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);

            RemplirComboCategorie(controller.GetAllGenres(), bdgGestionLivreGenres, cbxGestionLivreGenre);
            RemplirComboCategorie(controller.GetAllPublics(), bdgGestionLivrePublics, cbxGestionLivrePublic);
            RemplirComboCategorie(controller.GetAllRayons(), bdgGestionLivreRayons, cbxGestionLivreRayon);

            RemplirLivresListeComplete();
            ActiverGestionLivre(false);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        private readonly BindingSource bdgGestionLivreGenres = new BindingSource();
        private readonly BindingSource bdgGestionLivrePublics = new BindingSource();
        private readonly BindingSource bdgGestionLivreRayons = new BindingSource();

        /// <summary>
        /// Active ou désactive la zone de gestion des livres
        /// </summary>
        /// <param name="actif"></param>
        private void ActiverGestionLivre(bool actif)
        {
            txbGestionLivreNumero.Enabled = actif;
            txbGestionLivreTitre.Enabled = actif;
            txbGestionLivreIsbn.Enabled = actif;
            txbGestionLivreAuteur.Enabled = actif;
            txbGestionLivreCollection.Enabled = actif;
            txbGestionLivreImage.Enabled = actif;
            cbxGestionLivreGenre.Enabled = actif;
            cbxGestionLivrePublic.Enabled = actif;
            cbxGestionLivreRayon.Enabled = actif;
            btnValiderLivre.Enabled = actif;
        }

        /// <summary>
        /// Vide la zone de gestion des livres
        /// </summary>
        private void ViderGestionLivre()
        {
            txbGestionLivreNumero.Text = "";
            txbGestionLivreTitre.Text = "";
            txbGestionLivreIsbn.Text = "";
            txbGestionLivreAuteur.Text = "";
            txbGestionLivreCollection.Text = "";
            txbGestionLivreImage.Text = "";
            cbxGestionLivreGenre.SelectedIndex = -1;
            cbxGestionLivrePublic.SelectedIndex = -1;
            cbxGestionLivreRayon.SelectedIndex = -1;
        }

        /// <summary>
        /// Charge un livre dans la zone de gestion
        /// </summary>
        /// <param name="livre">livre à charger</param>
        private void ChargerGestionLivre(Livre livre)
        {
            txbGestionLivreNumero.Text = livre.Id;
            txbGestionLivreTitre.Text = livre.Titre;
            txbGestionLivreIsbn.Text = livre.Isbn;
            txbGestionLivreAuteur.Text = livre.Auteur;
            txbGestionLivreCollection.Text = livre.Collection;
            txbGestionLivreImage.Text = livre.Image;

            cbxGestionLivreGenre.SelectedIndex = cbxGestionLivreGenre.FindStringExact(livre.Genre);
            cbxGestionLivrePublic.SelectedIndex = cbxGestionLivrePublic.FindStringExact(livre.Public);
            cbxGestionLivreRayon.SelectedIndex = cbxGestionLivreRayon.FindStringExact(livre.Rayon);
        }

        /// <summary>
        /// Prépare l'ajout d'un nouveau livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNouveauLivre_Click(object sender, EventArgs e)
        {
            modeGestionLivre = "ajout";
            txbGestionLivreNumero.Enabled = true;
            ViderGestionLivre();
            ActiverGestionLivre(true);
            txbGestionLivreNumero.Focus();
        }

        /// <summary>
        /// Prépare la modification d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifierLivre_Click(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell == null)
            {
                MessageBox.Show("Sélectionne un livre.", "Information");
                return;
            }

            Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
            modeGestionLivre = "modification";
            ChargerGestionLivre(livre);
            ActiverGestionLivre(true);
            txbGestionLivreNumero.Enabled = false;
        }


        /// <summary>
        /// Valide les modifications ou l'ajout d'un livre selon le mode de gestion (modification ou ajout)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValidationLivre_Click(object sender, EventArgs e)
        {
            if (txbGestionLivreNumero.Text.Trim().Equals("") ||
                txbGestionLivreTitre.Text.Trim().Equals("") ||
                cbxGestionLivreGenre.SelectedItem == null ||
                cbxGestionLivrePublic.SelectedItem == null ||
                cbxGestionLivreRayon.SelectedItem == null)
            {
                MessageBox.Show("Les champs obligatoires ne sont pas tous renseignés.", "Information");
                return;
            }

            Genre genre = (Genre)cbxGestionLivreGenre.SelectedItem;
            Public lePublic = (Public)cbxGestionLivrePublic.SelectedItem;
            Rayon rayon = (Rayon)cbxGestionLivreRayon.SelectedItem;

            Livre livre = new Livre(
                txbGestionLivreNumero.Text.Trim(),
                txbGestionLivreTitre.Text.Trim(),
                txbGestionLivreImage.Text.Trim(),
                txbGestionLivreIsbn.Text.Trim(),
                txbGestionLivreAuteur.Text.Trim(),
                txbGestionLivreCollection.Text.Trim(),
                genre.Id,
                genre.Libelle,
                lePublic.Id,
                lePublic.Libelle,
                rayon.Id,
                rayon.Libelle
            );

            bool ok = false;

            if (modeGestionLivre == "ajout")
            {
                ok = controller.CreerLivre(livre);
            }
            else if (modeGestionLivre == "modification")
            {
                ok = controller.ModifierLivre(txbGestionLivreNumero.Text.Trim(), livre);
            }
            else
            {
                MessageBox.Show("Aucune opération sélectionnée.", "Information");
                return;
            }

            if (ok)
            {
                MessageBox.Show("Opération réussie.", "Information");
                lesLivres = controller.GetAllLivres();
                RemplirLivresListeComplete();
                ViderGestionLivre();
                ActiverGestionLivre(false);
                txbGestionLivreNumero.Enabled = true;
                modeGestionLivre = "";
            }
            else
            {
                MessageBox.Show("Opération impossible.", "Erreur");
            }
        }


        /// <summary>
        /// Supprime un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerLivre_Click(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell == null)
            {
                MessageBox.Show("Sélectionne un livre.", "Information");
                return;
            }

            Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];

            DialogResult reponse = MessageBox.Show(
                "Voulez-vous vraiment supprimer ce livre ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (reponse == DialogResult.Yes)
            {
                if (controller.SupprimerLivre(livre.Id))
                {
                    MessageBox.Show("Livre supprimé.", "Information");
                    lesLivres = controller.GetAllLivres();
                    RemplirLivresListeComplete();
                    ViderGestionLivre();
                    ActiverGestionLivre(false);
                    modeGestionLivre = "";
                }
                else
                {
                    MessageBox.Show("Suppression impossible : le livre possède des exemplaires ou des commandes.", "Erreur");
                }
            }
        }


        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        private void ActiverGestionDvd(bool actif)
        {
            txbGestionDvdNumero.Enabled = actif;
            txbGestionDvdTitre.Enabled = actif;
            txbGestionDvdDuree.Enabled = actif;
            txbGestionDvdRealisateur.Enabled = actif;
            txbGestionDvdSynopsis.Enabled = actif;
            txbGestionDvdImage.Enabled = actif;
            cbxGestionDvdGenre.Enabled = actif;
            cbxGestionDvdPublic.Enabled = actif;
            cbxGestionDvdRayon.Enabled = actif;
            btnValiderDvd.Enabled = actif;
        }

        private void ViderGestionDvd()
        {
            txbGestionDvdNumero.Text = "";
            txbGestionDvdTitre.Text = "";
            txbGestionDvdDuree.Text = "";
            txbGestionDvdRealisateur.Text = "";
            txbGestionDvdSynopsis.Text = "";
            txbGestionDvdImage.Text = "";
            cbxGestionDvdGenre.SelectedIndex = -1;
            cbxGestionDvdPublic.SelectedIndex = -1;
            cbxGestionDvdRayon.SelectedIndex = -1;
        }


        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);

            RemplirComboCategorie(controller.GetAllGenres(), bdgGestionDvdGenres, cbxGestionDvdGenre);
            RemplirComboCategorie(controller.GetAllPublics(), bdgGestionDvdPublics, cbxGestionDvdPublic);
            RemplirComboCategorie(controller.GetAllRayons(), bdgGestionDvdRayons, cbxGestionDvdRayon);
            ActiverGestionDvd(false);

            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Charge les informations d'un dvd dans la zone de gestion
        /// </summary>
        /// <param name="dvd">dvd à charger</param>
        private void ChargerGestionDvd(Dvd dvd)
        {
            txbGestionDvdNumero.Text = dvd.Id;
            txbGestionDvdTitre.Text = dvd.Titre;
            txbGestionDvdDuree.Text = dvd.Duree.ToString();
            txbGestionDvdRealisateur.Text = dvd.Realisateur;
            txbGestionDvdSynopsis.Text = dvd.Synopsis;
            txbGestionDvdImage.Text = dvd.Image;

            cbxGestionDvdGenre.SelectedIndex = cbxGestionDvdGenre.FindStringExact(dvd.Genre);
            cbxGestionDvdPublic.SelectedIndex = cbxGestionDvdPublic.FindStringExact(dvd.Public);
            cbxGestionDvdRayon.SelectedIndex = cbxGestionDvdRayon.FindStringExact(dvd.Rayon);
        }


        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }

        private readonly BindingSource bdgGestionDvdGenres = new BindingSource();
        private readonly BindingSource bdgGestionDvdPublics = new BindingSource();
        private readonly BindingSource bdgGestionDvdRayons = new BindingSource();
        private string modeGestionDvd = "";


        private void btnNouveauDvd_Click(object sender, EventArgs e)
        {
            modeGestionDvd = "ajout";
            ViderGestionDvd();
            ActiverGestionDvd(true);
            txbGestionDvdNumero.Enabled = true;
            txbGestionDvdNumero.Focus();
        }

        private void btnValiderDvd_Click(object sender, EventArgs e)
        {
            if (txbGestionDvdNumero.Text.Trim().Equals("") ||
                txbGestionDvdTitre.Text.Trim().Equals("") ||
                txbGestionDvdDuree.Text.Trim().Equals("") ||
                cbxGestionDvdGenre.SelectedItem == null ||
                cbxGestionDvdPublic.SelectedItem == null ||
                cbxGestionDvdRayon.SelectedItem == null)
            {
                MessageBox.Show("Les champs obligatoires ne sont pas tous renseignés.", "Information");
                return;
            }

            int duree;
            if (!int.TryParse(txbGestionDvdDuree.Text.Trim(), out duree))
            {
                MessageBox.Show("La durée doit être numérique.", "Information");
                return;
            }

            Genre genre = (Genre)cbxGestionDvdGenre.SelectedItem;
            Public lePublic = (Public)cbxGestionDvdPublic.SelectedItem;
            Rayon rayon = (Rayon)cbxGestionDvdRayon.SelectedItem;

            Dvd dvd = new Dvd(
                txbGestionDvdNumero.Text.Trim(),
                txbGestionDvdTitre.Text.Trim(),
                txbGestionDvdImage.Text.Trim(),
                duree,
                txbGestionDvdRealisateur.Text.Trim(),
                txbGestionDvdSynopsis.Text.Trim(),
                genre.Id,
                genre.Libelle,
                lePublic.Id,
                lePublic.Libelle,
                rayon.Id,
                rayon.Libelle
            );

            bool ok = false;

            if (modeGestionDvd == "ajout")
            {
                ok = controller.CreerDvd(dvd);
            }
            else if (modeGestionDvd == "modification")
            {
                ok = controller.ModifierDvd(txbGestionDvdNumero.Text.Trim(), dvd);
            }
            else
            {
                MessageBox.Show("Aucune opération sélectionnée.", "Information");
                return;
            }

            if (ok)
            {
                MessageBox.Show("Opération réussie.", "Information");
                lesDvd = controller.GetAllDvd();
                RemplirDvdListeComplete();
                ViderGestionDvd();
                ActiverGestionDvd(false);
                txbGestionDvdNumero.Enabled = true;
                modeGestionDvd = "";
            }
            else
            {
                MessageBox.Show("Opération impossible.", "Erreur");
            }

        }

        /// <summary>
        /// Prépare la modification d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifierDvd_Click(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell == null)
            {
                MessageBox.Show("Sélectionne un dvd.", "Information");
                return;
            }

            Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
            modeGestionDvd = "modification";
            ChargerGestionDvd(dvd);
            ActiverGestionDvd(true);
            txbGestionDvdNumero.Enabled = false;
        }

        /// <summary>
        /// Supprime un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerDvd_Click(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell == null)
            {
                MessageBox.Show("Sélectionne un dvd.", "Information");
                return;
            }

            Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];

            DialogResult reponse = MessageBox.Show(
                "Voulez-vous vraiment supprimer ce dvd ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (reponse == DialogResult.Yes)
            {
                if (controller.SupprimerDvd(dvd.Id))
                {
                    MessageBox.Show("DVD supprimé.", "Information");
                    lesDvd = controller.GetAllDvd();
                    RemplirDvdListeComplete();
                    ViderGestionDvd();
                    ActiverGestionDvd(false);
                    modeGestionDvd = "";
                }
                else
                {
                    MessageBox.Show("Suppression impossible : le dvd possède des exemplaires ou des commandes.", "Erreur");
                }
            }
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);

            RemplirComboCategorie(controller.GetAllGenres(), bdgGestionRevueGenres, cbxGestionRevueGenre);
            RemplirComboCategorie(controller.GetAllPublics(), bdgGestionRevuePublics, cbxGestionRevuePublic);
            RemplirComboCategorie(controller.GetAllRayons(), bdgGestionRevueRayons, cbxGestionRevueRayon);

            RemplirComboPeriodicite();

            ActiverGestionRevue(false);

            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Active ou désactive la zone de gestion des revues
        /// </summary>
        /// <param name="actif"></param>
        private void ActiverGestionRevue(bool actif)
        {
            txbGestionRevueNumero.Enabled = actif;
            txbGestionRevueTitre.Enabled = actif;
            cbxGestionRevuePeriodicite.Enabled = actif;
            txbGestionRevueDelaiMiseADispo.Enabled = actif;
            txbGestionRevueImage.Enabled = actif;
            cbxGestionRevueGenre.Enabled = actif;
            cbxGestionRevuePublic.Enabled = actif;
            cbxGestionRevueRayon.Enabled = actif;
            btnValiderRevue.Enabled = actif;
        }

        /// <summary>
        /// Vide la zone de gestion des revues
        /// </summary>
        private void ViderGestionRevue()
        {
            txbGestionRevueNumero.Text = "";
            txbGestionRevueTitre.Text = "";
            cbxGestionRevuePeriodicite.SelectedIndex = -1;
            txbGestionRevueDelaiMiseADispo.Text = "";
            txbGestionRevueImage.Text = "";
            cbxGestionRevueGenre.SelectedIndex = -1;
            cbxGestionRevuePublic.SelectedIndex = -1;
            cbxGestionRevueRayon.SelectedIndex = -1;
        }

        /// <summary>
        /// Remplit la liste des périodicités
        /// </summary>
        private void RemplirComboPeriodicite()
        {
            cbxGestionRevuePeriodicite.Items.Clear();
            cbxGestionRevuePeriodicite.Items.Add("QT");
            cbxGestionRevuePeriodicite.Items.Add("HB");
            cbxGestionRevuePeriodicite.Items.Add("MS");
            cbxGestionRevuePeriodicite.SelectedIndex = -1;
        }

        /// <summary>
        /// Charge les informations d'une revue dans la zone de gestion
        /// </summary>
        /// <param name="revue">revue à charger</param>
        private void ChargerGestionRevue(Revue revue)
        {
            txbGestionRevueNumero.Text = revue.Id;
            txbGestionRevueTitre.Text = revue.Titre;
            txbGestionRevueImage.Text = revue.Image;
            txbGestionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();

            cbxGestionRevueGenre.SelectedIndex = cbxGestionRevueGenre.FindStringExact(revue.Genre);
            cbxGestionRevuePublic.SelectedIndex = cbxGestionRevuePublic.FindStringExact(revue.Public);
            cbxGestionRevueRayon.SelectedIndex = cbxGestionRevueRayon.FindStringExact(revue.Rayon);
            cbxGestionRevuePeriodicite.SelectedItem = revue.Periodicite;
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }

        private readonly BindingSource bdgGestionRevueGenres = new BindingSource();
        private readonly BindingSource bdgGestionRevuePublics = new BindingSource();
        private readonly BindingSource bdgGestionRevueRayons = new BindingSource();
        private string modeGestionRevue = "";
        /// <summary>
        /// Prépare l'ajout d'une nouvelle revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNouveauRevue_Click(object sender, EventArgs e)
        {

            modeGestionRevue = "ajout";
            ViderGestionRevue();
            ActiverGestionRevue(true);
            txbGestionRevueNumero.Enabled = true;
            txbGestionRevueNumero.Focus();
        }

        /// <summary>
        /// Valide l'ajout d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderRevue_Click(object sender, EventArgs e)
        {
            if (txbGestionRevueNumero.Text.Trim().Equals("") ||
                txbGestionRevueTitre.Text.Trim().Equals("") ||
                cbxGestionRevuePeriodicite.SelectedItem == null ||
                txbGestionRevueDelaiMiseADispo.Text.Trim().Equals("") ||
                cbxGestionRevueGenre.SelectedItem == null ||
                cbxGestionRevuePublic.SelectedItem == null ||
                cbxGestionRevueRayon.SelectedItem == null)
            {
                MessageBox.Show("Les champs obligatoires ne sont pas tous renseignés.", "Information");
                return;
            }

            int delai;
            if (!int.TryParse(txbGestionRevueDelaiMiseADispo.Text.Trim(), out delai))
            {
                MessageBox.Show("Le délai de mise à disposition doit être numérique.", "Information");
                return;
            }

            Genre genre = (Genre)cbxGestionRevueGenre.SelectedItem;
            Public lePublic = (Public)cbxGestionRevuePublic.SelectedItem;
            Rayon rayon = (Rayon)cbxGestionRevueRayon.SelectedItem;
            string periodicite = cbxGestionRevuePeriodicite.SelectedItem.ToString();

            Revue revue = new Revue(
                txbGestionRevueNumero.Text.Trim(),
                txbGestionRevueTitre.Text.Trim(),
                txbGestionRevueImage.Text.Trim(),
                genre.Id,
                genre.Libelle,
                lePublic.Id,
                lePublic.Libelle,
                rayon.Id,
                rayon.Libelle,
                periodicite,
                delai
            );

            bool ok = false;

            if (modeGestionRevue == "ajout")
            {
                ok = controller.CreerRevue(revue);
            }
            else if (modeGestionRevue == "modification")
            {
                ok = controller.ModifierRevue(txbGestionRevueNumero.Text.Trim(), revue);
            }
            else
            {
                MessageBox.Show("Aucune opération sélectionnée.", "Information");
                return;
            }

            if (ok)
            {
                MessageBox.Show("Opération réussie.", "Information");
                lesRevues = controller.GetAllRevues();
                RemplirRevuesListeComplete();
                ViderGestionRevue();
                ActiverGestionRevue(false);
                txbGestionRevueNumero.Enabled = true;
                modeGestionRevue = "";
            }
            else
            {
                MessageBox.Show("Opération impossible.", "Erreur");
            }

        }

        /// <summary>
        /// Prépare la modification d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifierRevue_Click(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell == null)
            {
                MessageBox.Show("Sélectionne une revue.", "Information");
                return;
            }

            Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
            modeGestionRevue = "modification";
            ChargerGestionRevue(revue);
            ActiverGestionRevue(true);
            txbGestionRevueNumero.Enabled = false;
        }

        /// <summary>
        /// Supprime une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerRevue_Click(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell == null)
            {
                MessageBox.Show("Sélectionne une revue.", "Information");
                return;
            }

            Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];

            DialogResult reponse = MessageBox.Show(
                "Voulez-vous vraiment supprimer cette revue ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (reponse == DialogResult.Yes)
            {
                if (controller.SupprimerRevue(revue.Id))
                {
                    MessageBox.Show("Revue supprimée.", "Information");
                    lesRevues = controller.GetAllRevues();
                    RemplirRevuesListeComplete();
                    ViderGestionRevue();
                    ActiverGestionRevue(false);
                    txbGestionRevueNumero.Enabled = true;
                    modeGestionRevue = "";
                }
                else
                {
                    MessageBox.Show("Suppression impossible.", "Erreur");
                }
            }
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }
        #endregion

        #region Onglet Commandes Livres
        private readonly BindingSource bdgCommandeLivresListe = new BindingSource();
        private readonly BindingSource bdgCommandeLivreSuivis = new BindingSource();
        private List<CommandeDocumentSuivi> lesCommandesLivres = new List<CommandeDocumentSuivi>();
        private List<Suivi> lesSuivis = new List<Suivi>();
        private Livre livreCommandeSelectionne = null;

        /// <summary>
        /// Ouverture de l'onglet Commandes Livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandesLivres_Enter(object sender, EventArgs e)
        {
            lesSuivis = controller.GetAllSuivis();
            bdgCommandeLivreSuivis.DataSource = lesSuivis;
            cbxCommandeLivreSuivi.DataSource = bdgCommandeLivreSuivis;
            cbxCommandeLivreSuivi.DisplayMember = "Libelle";
            cbxCommandeLivreSuivi.ValueMember = "Id";
            cbxCommandeLivreSuivi.SelectedIndex = -1;

            ViderCommandeLivreZones();
        }

        /// <summary>
        /// Vide les zones de l'onglet Commandes Livres
        /// </summary>
        private void ViderCommandeLivreZones()
        {
            txbCommandeLivreNumeroRecherche.Text = "";

            txbCommandeLivreNumero.Text = "";
            txbCommandeLivreTitre.Text = "";
            txbCommandeLivreAuteur.Text = "";
            txbCommandeLivreCollection.Text = "";
            txbCommandeLivreIsbn.Text = "";
            txbCommandeLivreGenre.Text = "";
            txbCommandeLivrePublic.Text = "";
            txbCommandeLivreRayon.Text = "";
            txbCommandeLivreImage.Text = "";
            pcbCommandeLivresImage.Image = null;

            txbCommandeLivreIdCommande.Text = "";
            txbCommandeLivreMontant.Text = "";
            txbCommandeLivreNbExemplaire.Text = "";
            cbxCommandeLivreSuivi.SelectedIndex = -1;

            bdgCommandeLivresListe.DataSource = null;
            dgvCommandeLivresListe.DataSource = bdgCommandeLivresListe;

            livreCommandeSelectionne = null;
            lesCommandesLivres = new List<CommandeDocumentSuivi>();

            ViderGestionCommandeLivre();
            ActiverGestionCommandeLivre(false);
            modeGestionCommandeLivre = "";
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné pour les commandes
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheCommandeLivreInfos(Livre livre)
        {
            txbCommandeLivreNumero.Text = livre.Id;
            txbCommandeLivreTitre.Text = livre.Titre;
            txbCommandeLivreAuteur.Text = livre.Auteur;
            txbCommandeLivreCollection.Text = livre.Collection;
            txbCommandeLivreIsbn.Text = livre.Isbn;
            txbCommandeLivreGenre.Text = livre.Genre;
            txbCommandeLivrePublic.Text = livre.Public;
            txbCommandeLivreRayon.Text = livre.Rayon;
            txbCommandeLivreImage.Text = livre.Image;

            string image = livre.Image;
            try
            {
                pcbCommandeLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbCommandeLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Remplit la liste des commandes d'un livre
        /// </summary>
        /// <param name="commandes">liste des commandes</param>
        private void RemplirCommandeLivresListe(List<CommandeDocumentSuivi> commandes)
        {
            bdgCommandeLivresListe.DataSource = commandes;
            dgvCommandeLivresListe.DataSource = bdgCommandeLivresListe;

            if (dgvCommandeLivresListe.Columns.Contains("Id"))
            {
                dgvCommandeLivresListe.Columns["Id"].Visible = false;
            }
            if (dgvCommandeLivresListe.Columns.Contains("IdLivreDvd"))
            {
                dgvCommandeLivresListe.Columns["IdLivreDvd"].Visible = false;
            }
            if (dgvCommandeLivresListe.Columns.Contains("IdSuivi"))
            {
                dgvCommandeLivresListe.Columns["IdSuivi"].Visible = false;
            }

            dgvCommandeLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Recherche un livre pour afficher ses commandes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommandeLivreRecherche_Click(object sender, EventArgs e)
        {
            if (txbCommandeLivreNumeroRecherche.Text.Trim().Equals(""))
            {
                MessageBox.Show("Saisis un numéro de document.", "Information");
                return;
            }

            List<Livre> lesLivres = controller.GetAllLivres();
            Livre livre = lesLivres.Find(x => x.Id.Equals(txbCommandeLivreNumeroRecherche.Text.Trim()));

            if (livre == null)
            {
                MessageBox.Show("Livre introuvable.", "Information");
                ViderCommandeLivreZones();
                return;
            }

            livreCommandeSelectionne = livre;
            AfficheCommandeLivreInfos(livre);

            lesCommandesLivres = controller.GetCommandesDocument(livre.Id);
            RemplirCommandeLivresListe(lesCommandesLivres);
        }

        private string modeGestionCommandeLivre = "";

        /// <summary>
        /// Active ou désactive la zone de gestion des commandes livres
        /// </summary>
        /// <param name="actif"></param>
        private void ActiverGestionCommandeLivre(bool actif)
        {
            txbCommandeLivreIdCommande.Enabled = actif;
            dtpCommandeLivreDateCommande.Enabled = actif;
            txbCommandeLivreMontant.Enabled = actif;
            txbCommandeLivreNbExemplaire.Enabled = actif;
            cbxCommandeLivreSuivi.Enabled = actif;
            btnValiderCommandeLivre.Enabled = actif;
        }

        /// <summary>
        /// Vide la zone de gestion des commandes livres
        /// </summary>
        private void ViderGestionCommandeLivre()
        {
            txbCommandeLivreIdCommande.Text = "";
            dtpCommandeLivreDateCommande.Value = DateTime.Today;
            txbCommandeLivreMontant.Text = "";
            txbCommandeLivreNbExemplaire.Text = "";
            cbxCommandeLivreSuivi.SelectedIndex = -1;
        }

        /// <summary>
        /// Prépare l'ajout d'une nouvelle commande de livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNouvelleCommandeLivre_Click(object sender, EventArgs e)
        {
            if (livreCommandeSelectionne == null)
            {
                MessageBox.Show("Recherche d'abord un livre.", "Information");
                return;
            }

            modeGestionCommandeLivre = "ajout";
            ViderGestionCommandeLivre();
            ActiverGestionCommandeLivre(true);
            txbCommandeLivreIdCommande.Enabled = true;
            cbxCommandeLivreSuivi.SelectedValue = "00001";
            txbCommandeLivreIdCommande.Focus();
        }

        /// <summary>
        /// Lance la recherche d'un livre avec la touche Entrée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbCommandeLivreNumeroRecherche_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnCommandeLivreRecherche_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Valide l'ajout d'une commande de livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderCommandeLivre_Click(object sender, EventArgs e)
        {
            if (livreCommandeSelectionne == null)
            {
                MessageBox.Show("Recherche d'abord un livre.", "Information");
                return;
            }

            if (txbCommandeLivreIdCommande.Text.Trim().Equals("") ||
                txbCommandeLivreMontant.Text.Trim().Equals("") ||
                txbCommandeLivreNbExemplaire.Text.Trim().Equals(""))
            {
                MessageBox.Show("Les champs obligatoires ne sont pas tous renseignés.", "Information");
                return;
            }

            double montant;
            if (!double.TryParse(txbCommandeLivreMontant.Text.Trim(), out montant))
            {
                MessageBox.Show("Le montant doit être numérique.", "Information");
                return;
            }

            int nbExemplaire;
            if (!int.TryParse(txbCommandeLivreNbExemplaire.Text.Trim(), out nbExemplaire))
            {
                MessageBox.Show("Le nombre d'exemplaires doit être numérique.", "Information");
                return;
            }

            bool ok = false;

            if (modeGestionCommandeLivre == "ajout")
            {
                CommandeDocumentSuivi commandeExistante = lesCommandesLivres.Find(x => x.Id.Equals(txbCommandeLivreIdCommande.Text.Trim()));
                if (commandeExistante != null)
                {
                    MessageBox.Show("Le numéro de commande existe déjà.", "Information");
                    return;
                }

                ok = controller.CreerCommandeDocument(
                    txbCommandeLivreIdCommande.Text.Trim(),
                    dtpCommandeLivreDateCommande.Value.Date,
                    montant,
                    nbExemplaire,
                    livreCommandeSelectionne.Id
                );
            }
            else if (modeGestionCommandeLivre == "modification")
            {
                if (cbxCommandeLivreSuivi.SelectedValue == null)
                {
                    MessageBox.Show("Sélectionne un suivi.", "Information");
                    return;
                }

                ok = controller.ModifierSuiviCommandeDocument(
                    txbCommandeLivreIdCommande.Text.Trim(),
                    cbxCommandeLivreSuivi.SelectedValue.ToString()
                );
            }
            else
            {
                MessageBox.Show("Aucune opération sélectionnée.", "Information");
                return;
            }

            if (ok)
            {
                MessageBox.Show("Opération réussie.", "Information");
                lesCommandesLivres = controller.GetCommandesDocument(livreCommandeSelectionne.Id);
                RemplirCommandeLivresListe(lesCommandesLivres);
                ViderGestionCommandeLivre();
                ActiverGestionCommandeLivre(false);
                txbCommandeLivreIdCommande.Enabled = true;
                dtpCommandeLivreDateCommande.Enabled = true;
                txbCommandeLivreMontant.Enabled = true;
                txbCommandeLivreNbExemplaire.Enabled = true;
                modeGestionCommandeLivre = "";
            }
            else
            {
                MessageBox.Show("Opération impossible.", "Erreur");
            }


        }

        /// <summary>
        /// Charge une commande de livre dans la zone de gestion
        /// </summary>
        /// <param name="commande">commande sélectionnée</param>
        private void ChargerGestionCommandeLivre(CommandeDocumentSuivi commande)
        {
            txbCommandeLivreIdCommande.Text = commande.Id;
            dtpCommandeLivreDateCommande.Value = commande.DateCommande;
            txbCommandeLivreMontant.Text = commande.Montant.ToString();
            txbCommandeLivreNbExemplaire.Text = commande.NbExemplaire.ToString();
            cbxCommandeLivreSuivi.SelectedValue = commande.IdSuivi;
        }

        /// <summary>
        /// Prépare la modification du suivi d'une commande de livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifierSuiviCommandeLivre_Click(object sender, EventArgs e)
        {
            if (dgvCommandeLivresListe.CurrentCell == null)
            {
                MessageBox.Show("Sélectionne une commande.", "Information");
                return;
            }

            CommandeDocumentSuivi commande = (CommandeDocumentSuivi)bdgCommandeLivresListe.List[bdgCommandeLivresListe.Position];
            modeGestionCommandeLivre = "modification";
            ChargerGestionCommandeLivre(commande);
            ActiverGestionCommandeLivre(true);

            txbCommandeLivreIdCommande.Enabled = false;
            dtpCommandeLivreDateCommande.Enabled = false;
            txbCommandeLivreMontant.Enabled = false;
            txbCommandeLivreNbExemplaire.Enabled = false;
        }

        #endregion

        /// <summary>
        /// Supprime une commande de livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerCommandeLivre_Click(object sender, EventArgs e)
        {
            if (dgvCommandeLivresListe.CurrentCell == null)
            {
                MessageBox.Show("Sélectionne une commande.", "Information");
                return;
            }

            CommandeDocumentSuivi commande = (CommandeDocumentSuivi)bdgCommandeLivresListe.List[bdgCommandeLivresListe.Position];

            DialogResult reponse = MessageBox.Show(
                "Voulez-vous vraiment supprimer cette commande ?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (reponse == DialogResult.Yes)
            {
                if (controller.SupprimerCommandeDocument(commande.Id))
                {
                    MessageBox.Show("Commande supprimée.", "Information");
                    lesCommandesLivres = controller.GetCommandesDocument(livreCommandeSelectionne.Id);
                    RemplirCommandeLivresListe(lesCommandesLivres);
                    ViderGestionCommandeLivre();
                    ActiverGestionCommandeLivre(false);
                    txbCommandeLivreIdCommande.Enabled = true;
                    dtpCommandeLivreDateCommande.Enabled = true;
                    txbCommandeLivreMontant.Enabled = true;
                    txbCommandeLivreNbExemplaire.Enabled = true;
                    modeGestionCommandeLivre = "";
                }
                else
                {
                    MessageBox.Show("Suppression impossible.", "Erreur");
                }
            }
        }


    }
}







