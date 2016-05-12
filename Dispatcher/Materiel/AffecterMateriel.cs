using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LibDao;

namespace Dispatcher
{
    public partial class AffecterMaterielFormulaire : Form
    {
        private List<Materiel> listMateriel = null;
        private List<Technicien> listTechniciens = null;
        Materiel materielSelectionne = null;
        Technicien technicienSelectionne = null;
        public AffecterMaterielFormulaire()
        {
            InitializeComponent();
            InitialiserDGVListBox();
        }

        //**************************************************************************************************
        private void InitialiserDGVListBox()
        {
            this.btnAttribuerMateriel.Enabled = false;
            materielSelectionne = null;
            technicienSelectionne = null;
            List<int> listIdMaterielDispo = new List<int>();
            dgvListeTechniciens.Rows.Clear();
            // Récupération de la liste des techniciens et des matériels
            using (MaterielManager materielManager = new MaterielManager())
            {
                // connexion déjà ouverte on peut la passer en paramètre
                using (TechnicienManager technicienManager = new TechnicienManager(materielManager.getConnexion()))
                {
                    // Récuperation de la liste des matériels 
                    listMateriel = materielManager.getListeMateriel();
                    // Récuperation de la liste des techniciens
                    listTechniciens = technicienManager.getListeTechnicien();
                }
            }
            // Création de la liste des Id des matériels dispo (au départ on suppose tout est disponible)
            if (listMateriel.Count > 0)
            {
                foreach (Materiel chaqueMateriel in listMateriel)
                {
                    listIdMaterielDispo.Add(chaqueMateriel.IdMateriel);
                }
            }
            // On rempli le dataGridView des Techniciens sans matériels et
            // on retire de la liste des Id des matériels dispo 
            // les id des matériels affectés à des techniciens
            foreach (Technicien chaqueTechnicien in listTechniciens)
            {
                // On ne prend que les techniciens sans matériel attribué
                if (chaqueTechnicien.FkIdMateriel == 0)
                {
                    // c'est un technicien sans matériel
                    dgvListeTechniciens.Rows.Add(chaqueTechnicien.LoginT,
                        chaqueTechnicien.Prenom,
                        chaqueTechnicien.Nom);
                }
                else
                {
                    // on a trouvé un matériel attribué,
                    // on enleve de la liste des materiels disponibles
                    listIdMaterielDispo.Remove(chaqueTechnicien.FkIdMateriel);
                }
            }
            // Trier par ordre alphabétique des noms le dataGridView
            dgvListeTechniciens.Sort(dgvListeTechniciens.Columns[2], ListSortDirection.Ascending);
            // clear de la listBox et des textBox
            listBoxSelectionMateriels.SelectedItem = null;
            listBoxSelectionMateriels.Items.Clear();
            textBoxTypeMateriel.ResetText();
            textBoxNumSerie.ResetText();
            mTxtBoxNumtel.ResetText();
            // On affiche dans la listBox que les matériels trouvé dispo
            if (listIdMaterielDispo.Count > 0)
            {
                foreach (int chaqueIdMateriel in listIdMaterielDispo)
                {
                    SelectItem sBoxItem = new SelectItem();
                    sBoxItem.Value = chaqueIdMateriel; // valeur de l'indice sera l'id du matériel
                    // On recherche la corresponadnce entre l'indice dans la liste matériel et l'Id du matériel
                    int indiceDansListMateriel = listMateriel.FindIndex(s => s.IdMateriel == chaqueIdMateriel);
                    sBoxItem.Text = listMateriel[indiceDansListMateriel].TypeMateriel + " - " + listMateriel[indiceDansListMateriel].NumeroSerie;
                    listBoxSelectionMateriels.Items.Add(sBoxItem);
                }
            }
        }
        //**************************************************************************************************
        private void listBoxSelectionMateriels_Click(object sender, EventArgs e)
        {
            if (listBoxSelectionMateriels.SelectedItem != null)
            {
                materielSelectionne = new Materiel();
                // récupération du client contenue dans l'item de la SelectBox
                SelectItem sBoxItem = (SelectItem)listBoxSelectionMateriels.SelectedItem;
                int idMateriel = (int)sBoxItem.Value;  // On a enregistré idMateriel dans  sBoxItem.Value
                int indiceDansListMateriel = listMateriel.FindIndex(s => s.IdMateriel == idMateriel);
                materielSelectionne = listMateriel[indiceDansListMateriel];
                // On affiche les attributs principaux du matériel
                textBoxTypeMateriel.Text = materielSelectionne.TypeMateriel;
                textBoxNumSerie.Text = materielSelectionne.NumeroSerie;
                mTxtBoxNumtel.Text = materielSelectionne.NumeroTel;
            }
            if ((materielSelectionne != null) && (technicienSelectionne != null))
            {
                this.btnAttribuerMateriel.Enabled = true;
            }
        }
        //**************************************************************************************************
        private void dgvListeTechniciens_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int IdxLigneActuelle = e.RowIndex; // indice de la ligne sélectionnée

            if (IdxLigneActuelle >= 0)
            {
                // loginT est caché dans la première colonne du dgv
                String logingT = (String)dgvListeTechniciens.Rows[IdxLigneActuelle].Cells[0].Value;
                // on récupère l'indice dans la liste des techniciend de celui qui est sélectionnée
                int indiceDansListTechnicien = listTechniciens.FindIndex(s => s.LoginT == logingT);
                // recupère le technicien sélectionnée
                technicienSelectionne = listTechniciens[indiceDansListTechnicien];
            }
            if ((materielSelectionne != null) && (technicienSelectionne != null))
            {
                this.btnAttribuerMateriel.Enabled = true;
            }
        }
        //**************************************************************************************************
        private void btnAttribuerMateriel_Click(object sender, EventArgs e)
        {
            if ((materielSelectionne != null) && (technicienSelectionne != null))
            {
                using (MaterielManager materielManager = new MaterielManager())
                {
                    materielSelectionne.EtatMateriel = "enService";
                    materielManager.affectationMaterielTechnicien(ref materielSelectionne,ref technicienSelectionne);
                }
                MessageBox.Show("Materiel affecté au technicien");
                InitialiserDGVListBox();
            }
            else
            {
                MessageBox.Show("Sélectionner un technicien et un matériel");
            }
        }
        //**************************************************************************************************
        // Au chargement de la page on déselectionne la première cellule du dataGridView
        private void AffecterMaterielFormulaire_Load(object sender, EventArgs e)
        {
            try
            {
                dgvListeTechniciens.Rows[0].Selected = false;
            }
            catch { }
        }
        //**************************************************************************************************
        // permet de placer le curseur de saisie a gauche de la maskTextBox
        private void mTxtBoxNumtel_MouseClick(object sender, MouseEventArgs e)
        {
            mTxtBoxNumtel.SelectionStart = 0;
        }
        //**************************************************************************************************
    }
}
