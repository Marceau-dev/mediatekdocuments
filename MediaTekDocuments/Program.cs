using MediaTekDocuments.model;
using MediaTekDocuments.view;
using System;
using System.Windows.Forms;

namespace MediaTekDocuments
{
    /// <summary>
    /// Point d'entrée principal de l'application.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FrmAuthentification frmAuthentification = new FrmAuthentification();

            if (frmAuthentification.ShowDialog() == DialogResult.OK)
            {
                Utilisateur utilisateur = frmAuthentification.UtilisateurConnecte;

                if (utilisateur == null)
                {
                    return;
                }

                if ("S3".Equals(utilisateur.IdService))
                {
                    MessageBox.Show(
                        "Vos droits ne sont pas suffisants pour accéder à cette application.",
                        "Accès refusé",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }

                Application.Run(new FrmMediatek(utilisateur));
            }


        }
    }
}
