using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LibDao;

namespace Dispatcher
{
    public partial class ApercuInterventionForm : Form
    {
        private List<Technicien> listTechniciens = null;
        //Technicien technicienSelectionne = null;
        private List<Intervention> listInterventions = null;
        Intervention interventionSelectionne = null;
        //Utils utils = null;
        //**************************************************************************************************
        public ApercuInterventionForm()
        {
            InitializeComponent();
            InitialiserDGV();
        }
        //**************************************************************************************************
        private void InitialiserDGV()
        {
            dgvIntervention.Rows.Clear();
            try
            {
                using (InterventionManager interventionManager = new InterventionManager())
                {
                    // créer un liste d'interventions et récupère les interventions de la BDD
                    listInterventions = interventionManager.listeInterventions();
                    using (TechnicienManager technicienManager = new TechnicienManager(interventionManager.getConnexion()))
                    {
                        // Récuperation de la liste des techniciens
                        listTechniciens = technicienManager.getListeTechnicien();
                    }
                }
                foreach (Intervention chaqueIntervention in listInterventions)
                {

                    foreach (Technicien chaqueTechnicien in listTechniciens)
                    {
                        if (chaqueIntervention.FkLoginT == chaqueTechnicien.LoginT)
                        {
                            dgvIntervention.Rows.Add(
                                chaqueIntervention.IdIntervention,
                                chaqueIntervention.NomContact,
                                chaqueIntervention.PrenomContact,
                                chaqueIntervention.DebutIntervention,
                                chaqueIntervention.EtatVisite.Replace("aRenouveler", "à renouveler"),
                                chaqueTechnicien.Nom,
                                chaqueTechnicien.Prenom);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }           
            txtBoxPrenomContact.ResetText();
            txtBoxNomContact.ResetText();
            txtBoxTelContact.ResetText();
            rTxtBoxObjectif.ResetText();
            rTxTBoxCompteR.ResetText();
            cBoxEtatVisite.ResetText();
        }
        //**************************************************************************************************
        private void dgvIntervention_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int IdxLigneActuelle = e.RowIndex;

            if (IdxLigneActuelle >= 0)
            {
                int idIntervention = (int)dgvIntervention.Rows[IdxLigneActuelle].Cells[0].Value;
                // recherche de l'intervention sélectionnée dans la liste des interventions (linq)
                // explication requête : https://msdn.microsoft.com/fr-fr/library/bb397676.aspx
                int indiceDansListIntervention = listInterventions.FindIndex(intervention => intervention.IdIntervention == idIntervention);
                // on récupère l'intervention sélectionnée dans la liste des intervention via l'index dans la liste
                interventionSelectionne = listInterventions[indiceDansListIntervention];
                txtBoxNomContact.Text = interventionSelectionne.NomContact;
                txtBoxPrenomContact.Text = interventionSelectionne.PrenomContact;
                txtBoxTelContact.Text = interventionSelectionne.TelContact;
                rTxtBoxObjectif.Text = interventionSelectionne.ObjectifVisite;
                rTxTBoxCompteR.Text = interventionSelectionne.CompteRendu.Replace("''", "'");
                cBoxEtatVisite.SelectedItem = interventionSelectionne.EtatVisite.Replace("aRenouveler","à renouveler");
            }
        }
        //**************************************************************************************************
        private void btnValider_Click(object sender, EventArgs e)
        {
            if (interventionSelectionne.CompteRendu != rTxTBoxCompteR.Text || interventionSelectionne.EtatVisite != cBoxEtatVisite.SelectedItem.ToString())
            {
                interventionSelectionne.CompteRendu = rTxTBoxCompteR.Text.Replace("'", "''");
                interventionSelectionne.EtatVisite = cBoxEtatVisite.SelectedItem.ToString().Replace("à renouveler","aRenouveler");
                try
                {
                    using (InterventionManager interventionManager = new InterventionManager())
                    {
                        // On persiste l'entité en BDD
                        interventionManager.updateIntervention(interventionSelectionne);
                        InitialiserDGV();
                        interventionSelectionne = null;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //**************************************************************************************************
        // Au chargement de la page on déselectionne la première cellule du dataGridView
        // Un try catch permet d'éviter attrape une exception rarissime si la table intervention est vide (Row null)
        private void ApercuInterventionForm_Load(object sender, EventArgs e)
        {
            try
            {
                dgvIntervention.Rows[0].Selected = false;
            }
            catch { }
        }
        //**************************************************************************************************
    }
}
