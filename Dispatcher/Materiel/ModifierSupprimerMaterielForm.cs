using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LibDao;

namespace Dispatcher
{
    public partial class ModifierSupprimerMaterielForm : Form
    {
        private List<Materiel> listMateriel = null;
        Materiel materielSelectionne = null;
        public ModifierSupprimerMaterielForm()
        {
            InitializeComponent();
            if (InitialiserSelectBox())
            {
                listBoxSelectionMateriels.Enabled = true;
                btnModifierMateriel.Enabled = false;
                btnSupprimerMateriel.Enabled = false;
            }
        }
        //**************************************************************************************************
        private bool InitialiserSelectBox()
        {
            bool bRequete = false; // vrai si des matériels ont été récupérés
            // vide les items de la selectBox
            listBoxSelectionMateriels.SelectedItem = null;
            listBoxSelectionMateriels.Items.Clear();
            using (MaterielManager materielManager = new MaterielManager())
            {
                // Récupère la liste des matériels
                listMateriel = materielManager.getListeMateriel();
                if (listMateriel.Count > 0)
                {
                    bRequete = true;
                    foreach (Materiel chaqueMateriel in listMateriel)
                    {
                        SelectItem sBoxItem = new SelectItem();
                        sBoxItem.Value = chaqueMateriel.IdMateriel; // valeur de l'indice sera l'id du matériel
                        sBoxItem.Text = chaqueMateriel.TypeMateriel + " - " + chaqueMateriel.NumeroSerie;
                        listBoxSelectionMateriels.Items.Add(sBoxItem);
                    }
                }
            }
            return bRequete;
        }
        //**************************************************************************************************
        private void viderChamps()
        {
            // vide les champs
            textBoxNumSerie.ResetText();
            textBoxTypeMateriel.ResetText();
            mTxtBoxNumtel.ResetText();
            textBoxCodeIMEI.ResetText();
            textBoxIdGoogle.ResetText();
            txtBoxAffectationMat.ResetText();
            comBoxEtatMatériel.SelectedItem = "";
        }
        //**************************************************************************************************
        private void RafraichirIHM()
        {
            viderChamps();
            InitialiserSelectBox();
        }
        //**************************************************************************************************
        private void btnModifierMateriel_Click(object sender, EventArgs e)
        {
            // On récupère Tous les attributs du matériel
            using (MaterielManager materielManager = new MaterielManager())
            {
                materielSelectionne.TypeMateriel = textBoxTypeMateriel.Text.Trim();
                materielSelectionne.NumeroTel = mTxtBoxNumtel.Text.Trim();
                materielSelectionne.Imei = textBoxCodeIMEI.Text.Trim();
                materielSelectionne.IdGoogle = textBoxIdGoogle.Text.Trim();     
                // il faut chercher si un technicien a en usage le matériel
                // si oui et si etatMatériel n'est pas égale à enService il faut 
                // l'enlever de l'affectation du technicien                  
                using (TechnicienManager technicienManager = new TechnicienManager())
                {
                    Technicien technicien = new Technicien();
                    technicien.FkIdMateriel = materielSelectionne.IdMateriel;
                    // on recherche le technicien qui possédait le matériel 
                    technicien = technicienManager.getTechnicien(technicien);
                    if ((materielSelectionne.EtatMateriel == "enService")&&((string)comBoxEtatMatériel.SelectedItem!="enService"))
                    {
                        // il faut retirer l'affectation du matériel au technicien
                        technicien.FkIdMateriel = 0;
                        technicienManager.ajoutModifTechnicien(ref technicien);
                    }
                }
                materielSelectionne.EtatMateriel = comBoxEtatMatériel.SelectedItem.ToString();
                materielSelectionne.FkLoginE = UtilisateurConnecte.Login;
                // On persiste les modifications
                materielManager.insertUpdateMateriel(ref materielSelectionne);
                MessageBox.Show("Matériel modifié avec succès");
                RafraichirIHM();
            }
        }
        //**************************************************************************************************
        private void btnSupprimerMateriel_Click(object sender, EventArgs e)
        {
            // A CODER
        }
        //**************************************************************************************************
        private void btnResetSelectionMateriel_Click(object sender, EventArgs e)
        {
            RafraichirIHM();
        }
        //**************************************************************************************************
        private void listBoxSelectionMateriel_Click(object sender, EventArgs e)
        {
            viderChamps();
            if (listBoxSelectionMateriels.SelectedItem != null)
            {
                materielSelectionne = new Materiel();
                // récupération du matériel contenue dans l'item de la SelectBox
                SelectItem sBoxItem = (SelectItem)listBoxSelectionMateriels.SelectedItem;
                int idMateriel = (int)sBoxItem.Value;  // On a enregistré idMateriel dans sBoxItem.Value
                int indiceDansListMateriel = listMateriel.FindIndex(s => s.IdMateriel == idMateriel);
                materielSelectionne = listMateriel[indiceDansListMateriel];
                // On a récupéré l'objet correspondant à la sélection, on rempli les différents champs
                textBoxTypeMateriel.Text = materielSelectionne.TypeMateriel;
                textBoxNumSerie.Text = materielSelectionne.NumeroSerie;
                mTxtBoxNumtel.Text = materielSelectionne.NumeroTel;
                textBoxCodeIMEI.Text = materielSelectionne.Imei;
                textBoxIdGoogle.Text = materielSelectionne.IdGoogle;
                comBoxEtatMatériel.SelectedItem = materielSelectionne.EtatMateriel;
                lblDateEnregistrementMateriel.Text = materielSelectionne.DateEnregistrement.ToString("dd/MM/yyyy");
                if (materielSelectionne.DateAffectation != DateTime.MinValue)
                {
                    lblValDateAffectation.Text = materielSelectionne.DateAffectation.ToString("dd/MM/yyyy");
                }
                else
                {
                    lblValDateAffectation.Text = "jamais affecté";
                }

                using (TechnicienManager technicienManager = new TechnicienManager())
                {
                    Technicien technicien = new Technicien();
                    technicien.FkIdMateriel = materielSelectionne.IdMateriel;
                    technicien = technicienManager.getTechnicien(technicien);
                    txtBoxAffectationMat.Text = technicien.Prenom + "  " + technicien.Nom;
                }
                btnModifierMateriel.Enabled = true;
            }
        }
        //**************************************************************************************************
        // permet de placer le curseur de saisie a gauche de la maskTextBox
        private void mTxtBoxNumtel_MouseClick(object sender, MouseEventArgs e)
        {
            mTxtBoxNumtel.SelectionStart = 0;
        }
    }
}
