using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using LibDao;

namespace Dispatcher
{
    public partial class AjouterTechnicienForm : Form
    {
        public AjouterTechnicienForm()
        {
            InitializeComponent();
        }

        //**************************************************************************************************
        private void btnAjouterTechnicien_Click(object sender, EventArgs e)
        {
             Utils utilMdp = new Utils();
            // créer un technicien et lui affecte les champs remplis 
            Technicien monTechnicien = new Technicien();
            if ((textBoxNom.Text!=String.Empty)&&(textBoxPrenom.Text!=String.Empty)&&
                (textBoxLoginT.Text!=String.Empty)&&(txtBoxMdp.Text!=String.Empty))
            {
                using (TechnicienManager technicienManager = new TechnicienManager())
                {
                    monTechnicien.Nom = textBoxNom.Text.Trim();
                    monTechnicien.Prenom = textBoxPrenom.Text.Trim();
                    monTechnicien.LoginT = textBoxLoginT.Text.Trim();
                    monTechnicien.PasswdT = utilMdp.getMd5Hash(txtBoxMdp.Text.Trim());
                    bool resultat = technicienManager.ajoutModifTechnicien(ref monTechnicien);
                    // On ajoute le technicien en BDD

                    if (resultat)  // si l'ajout s'est bien passé
                    {
                        MessageBox.Show("Technicien ajouté avec succès");
                    }
                    else
                    {
                        MessageBox.Show("Les champs remplis sont incorrectes");
                    }
                }
            }
            else
            {
                MessageBox.Show("Merci de remplir tous les champs");
            }    
        }
        //**************************************************************************************************
        private void btnViderChamps_Click(object sender, EventArgs e)
        {
            textBoxNom.ResetText();
            textBoxPrenom.ResetText();
            textBoxLoginT.ResetText();
            txtBoxMdp.ResetText();
        }
        //**************************************************************************************************
    }
}
