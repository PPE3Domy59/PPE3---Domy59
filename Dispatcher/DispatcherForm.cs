using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LibDao;
using System.Globalization;

namespace Dispatcher
{
    public partial class DispatcherForm : Form
    {
        const bool VIA_ACTIVE_DIRECTORY = false; // true si application fonctionne sur AD,ATTENTION modification non automatique de la connexion à la BDD
        String VersionProg = "1.2";
        String VersionSql = "1.0";

        bool connexionBddValide = false; // pour tester si on a détecté une connexion bdd au lancement de l'application

        GMapOverlay overlayOne;
        // Réferences d'objet utilisés pour la gestion des marqueurs de techniciens
        List<VTechnicienItinerant> listTechnicienItinerant = null;
        Dictionary<String, GMapMarker> dicTechMark = null;
        Dictionary<String, GMapMarker> dicTechMarkPre = null;              
        VTechnicienItinerant technicienItinerant = null;
        // Réferences d'objet utilisés pour la gestion des marqueurs de clients
        Dictionary<int, GMapMarker> dicCliMark = null;
        List<Client> listClients = null;
        Client clientSelectionne = null;
        //**************************************************************************************************
        // Constructeur
        public DispatcherForm()
        {
            InitializeComponent();
            testValideConnexionBdd();
            recupererUtilisateurConnecte();

            // initialisation des élements de gestion de la carte de position des techniciens et des clients
            // listTechnicienItinerant contiendra les techniciens en déplacement
            listTechnicienItinerant = new List<VTechnicienItinerant>();
            // Les dictionnaires qui permettont d'associer un technicien à son marqueur
            dicTechMark = new Dictionary<String, GMapMarker>();
            dicTechMarkPre = new Dictionary<String, GMapMarker>();
            // contient les clients dont on souhaite visualiser la position
            listClients = new List<Client>();
            // Le dictionnaire qui permet d'associer un client à son marqueur
            dicCliMark = new Dictionary<int, GMapMarker>();

            menuStripDispatcher.Enabled = true;
            this.Text = this.Text + "  " + "Version Prog : " + VersionProg + "  Version SQL : " + VersionSql;
            //--------------------------------------------------------------------------------------------
        }
        //**************************************************************************************************
        // vérifie si la base de donnée répond
         void testValideConnexionBdd()
        {
            // test connection BDD
            try
            {
                ConnexionSqlServer connexionSqlServer = new ConnexionSqlServer();
                if (connexionSqlServer.Connexion != null)
                {
                    connexionBddValide = true;
                    timerRafraichissementPositionTechnicien.Enabled = true;
                }
            }
            catch
            {
                connexionBddValide = false;
            }
        }

         //**************************************************************************************************
        // récupération propriétés utilisateur connecté sur le pc
         void recupererUtilisateurConnecte()
         {
             if (connexionBddValide)
             {
                 Employe employe = new Employe();
                 // récupération du groupe de l'utilisateur
                 if (VIA_ACTIVE_DIRECTORY == false)
                 {
                     // on est en local
                     // Jeu de test
                     UtilisateurConnecte.Login = employe.LoginE = "phenri";
                     UtilisateurConnecte.Prenom = employe.Prenom = "Pierre";
                     UtilisateurConnecte.Nom = employe.Nom = "Henri";
                     //UtilisateurConnecte.Groupe = employe.Groupe = "Dispatcher";
                     //UtilisateurConnecte.Login = employe.LoginE = "mcardona";
                     //UtilisateurConnecte.Prenom = employe.Prenom= "Marie";
                     //UtilisateurConnecte.Nom=employe.Nom = "Cardona";
                     UtilisateurConnecte.Groupe = employe.Groupe = "Administration";
                     // UtilisateurConnecte.Groupe = employe.Groupe = "Commercial";
                     // UtilisateurConnecte.Groupe = employe.Groupe= "Informatique";
                 }
                 else
                 {
                     // on est sur l'AD
                     // récupération information employé connecté sur AD
                     InfoActiveDirectory infoActiveDirectory = new InfoActiveDirectory();
                     employe = infoActiveDirectory.getEmployeFromAD(Environment.UserName);
                 }
                 if (employe != null)
                 {
                     UtilisateurConnecte.Groupe = employe.Groupe;
                     UtilisateurConnecte.Login = employe.LoginE;
                     // on persiste cet employé en BDD local
                     try
                     {
                         using (EmployeManager employeManager = new EmployeManager())
                         {
                             employeManager.ajoutModifEmploye(ref employe);
                         }
                     }
                     catch (Exception ex)
                     {
                         MessageBox.Show(ex.Message);
                     }
                 }
                 //--------------------------------------------------------------------------------------------
                 // Gestion des droits au niveau des menus
                 // Principe : on interdit tout et on valide un a un (comme le filtrage réseau)
                 switch (UtilisateurConnecte.Groupe)
                 {
                     case ("Dispatcher"):
                         {
                             // menu client
                             toolStripMenuItemClient.Enabled = true;
                             // menu intervention
                             toolStripMenuItemIntervention.Enabled = true;
                             ajouterToolStripMenuItemIntervention.Enabled = true;
                             supprimerToolStripMenuItemIntervention.Enabled = true;
                             modifierToolStripMenuItem.Enabled = true;
                             aperçuToolStripMenuItem.Enabled = true;
                             // menu SMS
                             envoiSMSToolStripMenuItem.Enabled = true;
                         }
                         break;
                     case ("Commercial"):
                         {
                             toolStripMenuItemClient.Enabled = true;
                             toolStripMenuItemIntervention.Enabled = true;
                             aperçuToolStripMenuItem.Enabled = true;
                         }
                         break;
                     case ("Informatique"):
                         {
                             gestionMatérielToolStripMenuItem.Enabled = true;
                         }
                         break;
                     case ("Administration"):
                         {
                             // menu client
                             toolStripMenuItemClient.Enabled = true;
                             // menu intervention
                             toolStripMenuItemIntervention.Enabled = true;
                             ajouterToolStripMenuItemIntervention.Enabled = true;
                             supprimerToolStripMenuItemIntervention.Enabled = true;
                             modifierToolStripMenuItem.Enabled = true;
                             aperçuToolStripMenuItem.Enabled = true;
                             // menu SMS
                             envoiSMSToolStripMenuItem.Enabled = true;
                             // menu Matériel
                             gestionMatérielToolStripMenuItem.Enabled = true;
                             // menu Technicien
                             TechnicienToolStripMenuItem.Enabled = true;
                         }
                         break;
                     default:
                         {
                             MessageBox.Show("Vous n'appartenez pas à un groupe autorisé à utiliser ce logiciel");
                         }
                         break;
                     //--------------------------------------------------------------------------------------------
                 }
             }
             else
             {
                 MessageBox.Show("Arrêt de l'application car pas de connexion BDD");
             }
         }

        //**************************************************************************************************
        // Chargement de la carte pour placer les techniciens et les clients
         private void Map_Load(object sender, EventArgs e) // code appelé au chargement de la carte
         {
             if (connexionBddValide)
             {
                 //utilisation de la map fourni par google
                 MapMain.MapProvider = GoogleMapProvider.Instance;
                 MapMain.MinZoom = 1;    //defini le zoom max et min
                 MapMain.MaxZoom = 20;

                 //prend dans les serveurs les infos non chargé et dans le cache les infos deja chargées
                 MapMain.Manager.Mode = AccessMode.ServerAndCache;
                 GMapProvider.WebProxy = null; // pas de proxy

                 overlayOne = new GMapOverlay(MapMain, "OverlayOne");//associe un overlay a la map
                 // Uniquement pour afficher la carte
                 MapMain.Overlays.Add(overlayOne);//ajoute l'overlay a la map
                 GMapMarker gMapMarker1 = new GMap.NET.WindowsForms.Markers.GMapMarkerGoogleGreen(new PointLatLng(
                             Convert.ToDouble("50.640866", new CultureInfo("en-Gb")),
                             Convert.ToDouble("3.027394", new CultureInfo("en-Gb"))));// nouveau marker avec la position d'un technicien

                 overlayOne.Markers.Add(gMapMarker1);

                 //ShowTechn();//ajoute les Markers par rapport aux technicienx connectés

                 MapMain.ZoomAndCenterMarkers(overlayOne.Id); //centre la map par rapport au markers
                 MapMain.Zoom = 10;
                 overlayOne.Markers.Clear();
                 timerRafraichissementPositionTechnicien.Start();//timer pour replacer les markers des techniciens en temp reel.
                 ShowCli();
                 timerRafraichissementPositionTechnicien_Tick(this, EventArgs.Empty);
             }
             else
             {
                 Application.Exit(); // on ferme l'application car pas de connexion BDD
             }
         }

        //**************************************************************************************************
        // méthode qui replace tous les marqeurs de techniciens sur la carte et qui reconstruit la listBox
        private void ShowTechn()
        {  
            try
            {
                using (TechnicienManager technicienManager = new TechnicienManager())
                {
                    listTechnicienItinerant = technicienManager.getListeTechniciensActif();//récuère tous les techniciens connectés)
                }

                foreach (VTechnicienItinerant unTechnicienItinerant in listTechnicienItinerant)// ajoute tout les markers pour tout les techniciens
                {
                    if (unTechnicienItinerant.Latitude != String.Empty && unTechnicienItinerant.Longitude != String.Empty)
                    {
                        // Obliger de préciser la culture lors de la conversion car les valeurs sont inscrites 
                        // en BDD avec un point au lieu d'une virgule
                        // 3 types de marqueurs sont possibles (rouge, vert et croix)
                        GMapMarker gMapMarker = new GMap.NET.WindowsForms.Markers.GMapMarkerGoogleGreen(new PointLatLng(
                                        Convert.ToDouble(unTechnicienItinerant.Latitude, new CultureInfo("en-Gb")),
                                        Convert.ToDouble(unTechnicienItinerant.Longitude, new CultureInfo("en-Gb"))));// nouveau marker avec la position d'un technicien
                        // Chaque marqueur est placé dans un dictionnaire dont la clé est le login du technicien
                        gMapMarker.ToolTipText = unTechnicienItinerant.Nom + " " + unTechnicienItinerant.Prenom;
                        dicTechMark[unTechnicienItinerant.LoginT] = gMapMarker;
                        overlayOne.Markers.Add(gMapMarker);

                        SelectItem sBoxItem = new SelectItem();
                        sBoxItem.Value = unTechnicienItinerant.LoginT;
                        sBoxItem.Text = unTechnicienItinerant.Nom + " " + unTechnicienItinerant.Prenom;
                        ListBoxTechniciens.Items.Add(sBoxItem);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }       
        }
        //**************************************************************************************************
        private void ShowCli()
        {
            try
            {
                using (ClientManager clientManager = new ClientManager())
                {
                    listClients = clientManager.listeClient();//récupère tous les clients avec un intervention planifiée)
                }

                foreach (Client unClientDemandeur in listClients)// ajoute tout les markers pour tout les clients
                {
                    if (unClientDemandeur.Latitude != String.Empty && unClientDemandeur.Longitude != String.Empty)
                    {
                        SelectItem sBoxItem = new SelectItem();
                        sBoxItem.Value = unClientDemandeur.IdClient;
                        sBoxItem.Text = unClientDemandeur.Nom + " " + unClientDemandeur.Prenom;
                        ListBoxClients.Items.Add(sBoxItem);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //**************************************************************************************************
        private void timerRafraichissementPositionTechnicien_Tick(object sender, EventArgs e)
        {
            ListBoxTechniciens.SelectedItem = null;
            ListBoxTechniciens.Items.Clear();
            try
            {
                using (TechnicienManager technicienManager = new TechnicienManager())
                {
                    listTechnicienItinerant = technicienManager.getListeTechniciensActif();//récuère tous les techniciens connectés)
                }
                dicTechMarkPre = new Dictionary<String, GMapMarker>(dicTechMark);
                dicTechMark.Clear();
                foreach (VTechnicienItinerant technicienItinerant in listTechnicienItinerant)
                {
                    if (dicTechMarkPre.ContainsKey(technicienItinerant.LoginT))
                    {
                        // le technicien existait déjà, on se contente de rafraichir sa position
                        dicTechMark.Add(technicienItinerant.LoginT, dicTechMarkPre[technicienItinerant.LoginT]);
                        dicTechMarkPre.Remove(technicienItinerant.LoginT); // on le retire du dictionnaire précedent
                        SelectItem sBoxItem = new SelectItem();
                        sBoxItem.Value = technicienItinerant.LoginT;
                        sBoxItem.Text = technicienItinerant.Nom + " " + technicienItinerant.Prenom;
                        ListBoxTechniciens.Items.Add(sBoxItem);
                    }
                    else
                    {
                        // c'est un nouveau technicien qui vient de se connecter
                        // nouveau marker avec la position du technicien
                        GMapMarker gMapMarker = new GMap.NET.WindowsForms.Markers.GMapMarkerGoogleGreen(new PointLatLng(
                                      Convert.ToDouble(technicienItinerant.Latitude, new CultureInfo("en-Gb")),
                                      Convert.ToDouble(technicienItinerant.Longitude, new CultureInfo("en-Gb"))));
                        gMapMarker.ToolTipText = technicienItinerant.Nom + " " + technicienItinerant.Prenom;
                        overlayOne.Markers.Add(gMapMarker);
                        dicTechMark.Add(technicienItinerant.LoginT, gMapMarker);
                        SelectItem sBoxItem = new SelectItem();
                        sBoxItem.Value = technicienItinerant.LoginT;
                        sBoxItem.Text = technicienItinerant.Nom + " " + technicienItinerant.Prenom;
                        ListBoxTechniciens.Items.Add(sBoxItem);
                    }
                }
                // on retire les marqueurs des techniciens qui ne sont plus connectés
                foreach (KeyValuePair<String, GMapMarker> item in dicTechMarkPre)
                {
                    overlayOne.Markers.Remove(item.Value); // on supprime le marqueur sur l'overlay
                }
            }
            catch (Exception ex)
            {
                timerRafraichissementPositionTechnicien.Stop();
                timerRafraichissementPositionTechnicien.Enabled = false;
                toolStripMenuItemClient.Visible = false;
                toolStripMenuItemIntervention.Visible = false;
                gestionMatérielToolStripMenuItem.Visible = false;
                TechnicienToolStripMenuItem.Visible = false;
                MessageBox.Show(ex.Message);
            }
            MapMain.Refresh();// replace les markers sur la map
        }
        //**************************************************************************************************
        private void ListBoxTechniciens_MouseClick(object sender, MouseEventArgs e)
        {
            // il faut récupérer le technicien sélectionné
            if (ListBoxTechniciens.SelectedItem != null)
            {
                technicienItinerant = new VTechnicienItinerant();
                // récupération du technicien sélectionné dans l'item de la SelectBox
                SelectItem sBoxItem = (SelectItem)ListBoxTechniciens.SelectedItem;
                String  loginT = (String)sBoxItem.Value;
                int indiceDansListTechnicien = listTechnicienItinerant.FindIndex(s => s.LoginT == loginT);
                technicienItinerant = listTechnicienItinerant[indiceDansListTechnicien];
                // On a sélectionné un technicien on modifie le type de marqueur
                if (dicTechMark[technicienItinerant.LoginT].GetType() == typeof(GMap.NET.WindowsForms.Markers.GMapMarkerGoogleGreen))
                {
                    btnRefresh_Click(this, EventArgs.Empty); // on raffraichi l'affichage pour éviter d'avoir 2 marqueurs rouges
                    // On remplace le marqueur vert par le rouge
                    if (technicienItinerant.Latitude != String.Empty && technicienItinerant.Longitude != String.Empty)
                    {
                        overlayOne.Markers.Remove(dicTechMark[technicienItinerant.LoginT]);// suppr le marker VERT
                        //<--
                        GMapMarker markersNew = new GMap.NET.WindowsForms.Markers.GMapMarkerGoogleRed(new PointLatLng(
                                        Convert.ToDouble(technicienItinerant.Latitude, new CultureInfo("en-Gb")),
                                        Convert.ToDouble(technicienItinerant.Longitude, new CultureInfo("en-Gb"))));
                        dicTechMark[technicienItinerant.LoginT] = markersNew;
                        dicTechMark[technicienItinerant.LoginT].ToolTipText =
                            technicienItinerant.Nom + " " + technicienItinerant.Prenom;
                        overlayOne.Markers.Add(dicTechMark[technicienItinerant.LoginT]);//--> rajoute le markers ROUGE avec les infos de celui qui a été supp
                    }
                }
                else
                {
                    if (technicienItinerant.Latitude != String.Empty && technicienItinerant.Longitude != String.Empty)
                    {
                        // On remplace le marqueur rouge par le vert
                        overlayOne.Markers.Remove(dicTechMark[technicienItinerant.LoginT]);//suppr le marker ROUGE
                        //<--
                        GMapMarker markersOld = new GMap.NET.WindowsForms.Markers.GMapMarkerGoogleGreen(new PointLatLng(
                                        Convert.ToDouble(technicienItinerant.Latitude, new CultureInfo("en-Gb")),
                                        Convert.ToDouble(technicienItinerant.Longitude, new CultureInfo("en-Gb"))));
                        dicTechMark[technicienItinerant.LoginT] = markersOld;
                        dicTechMark[technicienItinerant.LoginT].ToolTipText =
                            technicienItinerant.Nom + " " + technicienItinerant.Prenom;
                        overlayOne.Markers.Add(dicTechMark[technicienItinerant.LoginT]); //--> rajoute le markers VERT avec les infos de celui qui a été supp
                    }
                }
            }   
        }
        //**************************************************************************************************
        private void ListBoxClients_MouseClick(object sender, MouseEventArgs e)
        {
            if (ListBoxClients.SelectedItem != null)
            {
                clientSelectionne = new Client();
                // récupération du client sélectionné dans l'item de la SelectBox
                SelectItem sBoxItem = (SelectItem)ListBoxClients.SelectedItem;
                int idClient = (int)sBoxItem.Value;
                int indiceDansListClient = listClients.FindIndex(s => s.IdClient == idClient);
                clientSelectionne = listClients[indiceDansListClient];
                // On a sélectionné un client on lui attrbut le marqueur
                if (clientSelectionne.Latitude != String.Empty && clientSelectionne.Longitude != String.Empty)
                    {
                        btnRefresh_Click(this, EventArgs.Empty);                       
                        GMapMarker markerClient = new GMap.NET.WindowsForms.Markers.GMapMarkerCross(new PointLatLng(
                                        Convert.ToDouble(clientSelectionne.Latitude, new CultureInfo("en-Gb")),
                                        Convert.ToDouble(clientSelectionne.Longitude, new CultureInfo("en-Gb"))));
                        dicCliMark[clientSelectionne.IdClient] = markerClient;
                        overlayOne.Markers.Add(dicCliMark[clientSelectionne.IdClient]); 
                    }               
            }
        }
        //**************************************************************************************************
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (connexionBddValide)
            {
                overlayOne.Markers.Clear();
                ListBoxTechniciens.Items.Clear();//--> vide  la listBox    
                ShowTechn();
                ListBoxClients.Items.Clear();//--> vide  la listBox    
                ShowCli();
            }
        }
        //**************************************************************************************************
        // GESTION DES MENUS 
        //**************************************************************************************************
        private void ajouterMaterielMenuItem_Click(object sender, EventArgs e)
        {
            AjouterMaterielForm formulaireAjouterMateriel = new AjouterMaterielForm();
            formulaireAjouterMateriel.ShowDialog();
        }
        //**************************************************************************************************
        private void modifierSupprimerMaterielMenuItem_Click(object sender, EventArgs e)
        {
            ModifierSupprimerMaterielForm formulaireModifierSupprimerMateriel = new ModifierSupprimerMaterielForm();
            formulaireModifierSupprimerMateriel.ShowDialog();
        }
        //**************************************************************************************************
        //**************************************************************************************************
        // Actions Menu Item Technicien
        //**************************************************************************************************
        //**************************************************************************************************
        private void mAjoutTechnicienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AjouterTechnicienForm formulaireAjouterTechnicien = new AjouterTechnicienForm();
            formulaireAjouterTechnicien.ShowDialog();
        }
        //**************************************************************************************************
        private void modifierTechnicienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModifierSupprimerTechnicienForm formulaireModifierSupprimerTechnicien = new ModifierSupprimerTechnicienForm();
            formulaireModifierSupprimerTechnicien.ShowDialog();
        }
        //**************************************************************************************************
        //**************************************************************************************************
        // Actions Menu Item SMS
        //**************************************************************************************************
       //**************************************************************************************************
        private void envoiSMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnvoiSMSForm formulaireEnvoiSms = new EnvoiSMSForm();
            formulaireEnvoiSms.ShowDialog();
        }
        //**************************************************************************************************
        //**************************************************************************************************
        // Actions Menu Item Client
        //**************************************************************************************************
        //**************************************************************************************************
        private void ajouterClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AjouterClientForm formulaireAjouterClient = new AjouterClientForm();
            //formulaire modal
            formulaireAjouterClient.ShowDialog();
        }
        //**************************************************************************************************
        private void modifierClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModifierSupprimerClientForm formulaireModifierSupprimerClient = new ModifierSupprimerClientForm();
            formulaireModifierSupprimerClient.ShowDialog();
        }
       
       
        //**************************************************************************************************
        //**************************************************************************************************
        // Actions Menu Item Matériel
        //**************************************************************************************************
        //**************************************************************************************************
        private void ajouterMaterielToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AjouterMaterielForm formulaireAjouterMateriel = new AjouterMaterielForm();
            formulaireAjouterMateriel.ShowDialog();
        }
        //**************************************************************************************************
        private void modifierMatérielToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModifierSupprimerMaterielForm formulaireModifierSupprimerMateriel = new ModifierSupprimerMaterielForm();
            formulaireModifierSupprimerMateriel.ShowDialog();
        }
        //**************************************************************************************************
        private void affecterMaterielAUnTechnicienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AffecterMaterielFormulaire affecterMaterielFormulaire = new AffecterMaterielFormulaire();
            affecterMaterielFormulaire.ShowDialog();
        }
        //**************************************************************************************************
        //**************************************************************************************************
        // Actions Menu Item Intervention
        //**************************************************************************************************
        //**************************************************************************************************
        private void ajouterToolStripMenuItemIntervention_Click(object sender, EventArgs e)
        {
            AjouterPlanningForm formulaireAjouterPlanning = new AjouterPlanningForm();
            formulaireAjouterPlanning.ShowDialog();
        }
        //**************************************************************************************************
        private void supprimerToolStripMenuItemIntervention_Click(object sender, EventArgs e)
        {
            SupprimerInterventionForm formulaireSupprimerPlanning = new SupprimerInterventionForm();
            formulaireSupprimerPlanning.ShowDialog();
        }
        //**************************************************************************************************
        private void modifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModifierInterventionForm modifierInterventionForm = new ModifierInterventionForm();
            modifierInterventionForm.ShowDialog();
        }
        //**************************************************************************************************
        private void aperçuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApercuInterventionForm apercuInterventionForm = new ApercuInterventionForm();
            apercuInterventionForm.ShowDialog();
        }
        //**************************************************************************************************
    }
}
