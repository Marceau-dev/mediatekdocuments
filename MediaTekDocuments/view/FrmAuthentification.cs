using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    public partial class FrmAuthentification : Form
    {
        public FrmAuthentification()
        {
            InitializeComponent();
            controller = new FrmMediatekController();
            UtilisateurConnecte = null;
        }

        /// <summary>
        /// Contrôleur utilisé pour accéder aux données.
        /// </summary>
        private readonly FrmMediatekController controller;

        /// <summary>
        /// Utilisateur authentifié.
        /// </summary>
        public Utilisateur UtilisateurConnecte { get; private set; }

        /// <summary>
        /// Vérifie les identifiants saisis.
        /// </summary>
        private void btnConnexion_Click(object sender, EventArgs e)
        {
            string login = txbLogin.Text.Trim();
            string pwd = txbPwd.Text.Trim();

            if (login.Equals("") || pwd.Equals(""))
            {
                MessageBox.Show("Tous les champs doivent être renseignés.", "Information");
                return;
            }

            Utilisateur utilisateur = controller.GetUtilisateur(login, pwd);

            if (utilisateur == null)
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Erreur");
                txbPwd.Text = "";
                txbPwd.Focus();
                return;
            }

            UtilisateurConnecte = utilisateur;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Annule l'authentification.
        /// </summary>
        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Validation avec la touche Entrée.
        /// </summary>
        private void txbPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnConnexion_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }
    }
}
