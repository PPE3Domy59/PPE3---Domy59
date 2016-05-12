using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LibDao;
using Dispatcher.refWsSms;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.Configuration;

namespace Dispatcher
{
    public partial class EnvoiSMSForm : Form
    {
        private List<Technicien> listTechnicien = null;
        private List<Materiel> listMateriel = null; // nécessaire pour récupérer le matériel affecté au technicien (téléphone)
        Technicien technicienSelectionne = null;

        WebServiceSms proxy = null; // référence du proxy (gestion de la communication réseau vers le webService)
        Sms sms = null; // réference sur un objet sms à envoyer
        String clePublicDuCerticatChiffrement = ""; // cle sera lue dans le fichier de configuration de l'application
        String passwordEnvoiSMS = ""; // mot de passe commun à tous et lu dans le fichier de configuration
        //**************************************************************************************************
        // le constructeur
        public EnvoiSMSForm()
        {
            InitializeComponent();
            btnEnvoyerMessage.Enabled = false; // bouton envoi sms désactivé
            // récupération des paramètres de connexion dans app.config
            clePublicDuCerticatChiffrement = ConfigurationManager.AppSettings["CLE_PUBLIC"].ToString();
            passwordEnvoiSMS = ConfigurationManager.AppSettings["PASS_SMS"].ToString();
            InitialiserDGV(); 
        }
        //**************************************************************************************************
        private void InitialiserDGV()
        {
            using (TechnicienManager technicienManager = new TechnicienManager())
            {
                using (MaterielManager materielManager = new MaterielManager(technicienManager.getConnexion()))
                {
                    // récupère les techniciens et les matériels stockés en BDD
                    listTechnicien = technicienManager.getListeTechnicien();
                    listMateriel = materielManager.getListeMateriel();
                }
            }

            foreach (Technicien chaqueTechnicien in listTechnicien)
            {
                dgvTechnicien.Rows.Add(chaqueTechnicien.Nom,
                    chaqueTechnicien.Prenom,
                    chaqueTechnicien.LoginT);
            }
            // trie pae Nom
            dgvTechnicien.Sort(dgvTechnicien.Columns[0], ListSortDirection.Ascending);
        }
        //***********************************************************************************************
        private void viderChamps()
        {
            // vide les champs
            textBoxNom.ResetText();
            textBoxPrenom.ResetText();
            mTxtBoxNumtel.ResetText();
            btnEnvoyerMessage.Enabled = false;
        }
        //***********************************************************************************************
       
        // Cette methode est appelée par RemoteCertificateValidationDelegate
        // pour vérifier la validité du certificat type X509
        bool demandeDeValidationDuCertificat(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            // vérification par rapport au contenu de la clé publique du certificat
            // le certificat a été installé sur le serveur web
            if (certificate.GetPublicKeyString() == clePublicDuCerticatChiffrement)
            {
                return true; // certificat valide, le site distant est bien celui voulu
            }
            else
            {
                return false; // certificat de la transaction non valide
            }
        }

        //**************************************************************************************************
        // envoi de la requête vers le web service en https https://domy59efficom.ddns.net/WebServiceSms.asmx?WSDL
        // l'utilisateur avec son login est envoyé dans l'entête
        // le corps du sms est envoyé dans le corps de la requête
        // En production le WebSeervice ne devra pas fournir ses metadonnées par sécurité
        private void btnEnvoyerMessage_Click(object sender, EventArgs e)
        {
            try
            {
                if ((UtilisateurConnecte.Login != String.Empty) &&
                    (mTxtBoxNumtel.Text != String.Empty))// && (txtBoxMdpEnvoi.Text != String.Empty))
                {
                    // On demande la validation du certificats via la méthode demandeDeValidationDuCertificat
                    ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(demandeDeValidationDuCertificat);
                    // Création du proxy
                    proxy = new WebServiceSms();

                    // On prépare l'objet utilisateur transmis via l'entete soap
                    AuthentificationEnteteSoap authentication = new AuthentificationEnteteSoap();
                    Utilisateur utilisateur = new Utilisateur();
                    utilisateur.Login = UtilisateurConnecte.Login;
                    utilisateur.Password = passwordEnvoiSMS; // mot de passe standard pour l'envoi sms
                    authentication.user = utilisateur; // On passe l'utilisateur dans l'entête soap
                    proxy.AuthentificationEnteteSoapValue = authentication; // on transmet l'authentification via l'entete http
                    // Envoi du SMS
                    sms = new Sms();
                    sms.NumDestinataire = mTxtBoxNumtel.Text;
                    sms.TextMessage = richTextBoxMessage.Text;
                    MessageBox.Show(proxy.envoyerSms(sms));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Impossible de joindre le serveur d'envoi SMS"); 
            }
        }
       
        //***********************************************************************************************
        private void dgvTechnicien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int IdxLigneActuelle = e.RowIndex;
            Materiel materiel = new Materiel();
            viderChamps();

            if (IdxLigneActuelle >= 0)
            {
                string loginTechnicien = (string)dgvTechnicien.Rows[IdxLigneActuelle].Cells[2].Value;
                int indiceDansListTechnicien = listTechnicien.FindIndex(item => item.LoginT == loginTechnicien);
                technicienSelectionne = listTechnicien[indiceDansListTechnicien];
                // recupère les données du Technicien
                textBoxNom.Text = technicienSelectionne.Nom;
                textBoxPrenom.Text = technicienSelectionne.Prenom;
                // recherche du matériel affecté au technicien pour récupérer son numéro de téléphone
                int indiceDansListMateriel = listMateriel.FindIndex(materielRecherché => materielRecherché.IdMateriel == technicienSelectionne.FkIdMateriel);
                if (indiceDansListMateriel>=0) // matériel trouvé si >=0
                {
                    materiel = listMateriel[indiceDansListMateriel];
                    mTxtBoxNumtel.Text = materiel.NumeroTel;
                }
                else
                {
                    mTxtBoxNumtel.ResetText();
                    MessageBox.Show("Ce technicien n'a pas de matériel affecté, rentrer manuellement un numéro");
                }
            }
        }
        //**************************************************************************************************
        // permet de placer le curseur de saisie a gauche de la maskTextBox
        private void mTxtBoxNumtel_MouseClick(object sender, MouseEventArgs e)
        {
            mTxtBoxNumtel.SelectionStart = 0;
        }
        //**************************************************************************************************
        // Au chargement de la page on déselectionne la première cellule du dataGridView
        // Un try catch permet d'éviter attrape une exception rarissime si la table technicien est vide (Row null)
        private void EnvoiSMSForm_Load(object sender, EventArgs e)
        {
            try
            {
                dgvTechnicien.Rows[0].Selected = false;
            }
            catch { }
        }
        //**************************************************************************************************
        // validation du bouton envoyer sms s'il y a du texte qui est entré dans le textbox
        private void richTextBoxMessage_TextChanged(object sender, EventArgs e)
        {
            if (mTxtBoxNumtel.Text != String.Empty)
            {
                btnEnvoyerMessage.Enabled = true;
            }
        }
        //**************************************************************************************************
    }
}

