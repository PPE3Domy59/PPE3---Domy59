using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibDao;
using System.IO;
using Dispatcher;

namespace PeuplerTables
{
    class ClassePeuplementTables
    {
        // Jeux de test
        // Les données à mettre en BDD sont provisoirement placées dans un tableau de String au format csv
        // Chaque ligne représente un enregistrement 

        // liste des valeurs pour peupler la table des clients
        private String[] listeClients = new String[]{
            "NOVERRE#Mme#Bruno#Zieuvair#21 rue Lepelletier#Un complément d'adresse#LILLE#59000#0320674321#brunozieuvair@noverre.com#..\\..\\..\\PhotosClients\\pasDePhoto.jpg#50.638396#3.063018#valide#phenri#",
            "JOBROUSSE#Mr#Ali#Gator#24 Place De La Gare#Zoo de la citadelle#LILLE#59800#0620647389#agator@jobrousse.fr#..\\..\\..\\PhotosClients\\pasDePhoto.jpg#50.636225#3.069698#valide#phenri#",
            "WAULTOC#Mme#Hélène#Udanleau#17 Quai Du Wault#Zone du site interdit#LILLE#59027#0320674802#helenedansleau@waultoc.eu#..\\..\\..\\PhotosClients\\quais_du_vault.jpg#50.638136#3.054332#valide#phenri#",
            "ENVELO#Mr#Ferdinand#Dinar-Havélo#Boulevard De Turin##LILLE#59000#0320008877#fdinard@envelo.com#..\\..\\..\\PhotosClients\\Usine-gazelle-dernier-cri.jpg#50.640310#3.076884#valide#phenri#",
            "TOPLOUSE#Mr#Gaston#Laplouz#6 Avenue Halley#Zone industrielle de la pilaterie#Villeneuve d'Ascq#59650#0798764512#glaplouz@toplouse#..\\..\\..\\PhotosClients\\laplouze.jpg#50.605771#3.149680#valide#phenri#",
            "LES TICAHIER#Mme#Claire#Fontaine#Avenue Paul Langevin#ZAC des fontaines d'eau vive#Villeneuve d'Ascq#59650#0320891245#cfontaine@ticahier.fr#..\\..\\..\\PhotosClients\\ticahier.jpg#50.610585#3.137068#valide#phenri#",
            "EFFICOM#Mme#Cécile#Ancieux#146 Rue Nationale#Zonedes étudiants studieux#LILLE#59000#0320151677#cancieux@efficom-lille.com#..\\..\\..\\PhotosClients\\efficom.jpg#50.633822#3.053970#valide#phenri#"
    };
        // Liste des valeurs pour peupler la table employés (inutile dès que l'application est sur l'active directory
        private String[] listeEmployes = new String[]{
            "phenri#Pierre#Henri#dispatcher#",
            "asespoir#Aude#Sespoir#dispatcher#",
            "rlasire#Remi#Lasiré#comptabilité#"
    };
        // La variable de session est calculée aléatoirement lors 
        // de la première connecion journalière du technicien, elle est valable jusqu'au soir
        private String[] listeSessionTechnicien = new String[]{
            "session1#eroberts#",
            "session2#tberners#", 
            "session3#lpage#",
            "session4#jgosling#"
        };

        // liste des valeurs pour peupler la table des techniciens
        // les techniciens ne sont pas enregistrés sur l'actice directory
        // login,mdp,prenom,nom,fk matériel affecté
        private String[] listeETechniciens = new String[]{
            "eroberts#password#Edward#Roberts##",
            "tberners#password#Timothy#Berners##",
            "jgosling#password#James#Gosling##",
            "lpage#password#Lawrence#Page##"
    };
        // liste des valeurs pour initialiser une table des positions techniciens
        private String[] listePositionTechnicien = new String[]{
            "50.640866#3.027394#eroberts#",
            "50.623988#3.067550#tberners#", 
            "50.633658#3.054022#jgosling#",
            "50.614766#3.012540#lpage#"
        };

        // liste des valeurs pour peupler la table matériel
        // TypeMateriel,NumeroSerie,NumeroTel,Imei,IdGoogle,FkLoginE,EtatMateriel
        private String[] listeMateriels = new String[]{
            "Smartphone LG#12345678910#0645342312#358950050955750#hggkhghhjbxhjqslhqudchlchdslchldshlshclsh#phenri#stock#",
            "Smartphone Archos#009876543212##000000000000000##phenri#stock#",
            "Smartphone Asus#4567890123456##000000000000000##phenri#stock#"
    };

        // liste des valeurs pour peupler la table Intervention
        //CompteRendu,nbrJourAprèsAujourdui,debutIntervention,finIntervention,objetVisite,Photolieu,
        //NomContact,prenomContact,telContact,EtatVisite,FkloginE,FkIdClient,FkloginT
        private String[] listeInterventions = new String[]{
            "#0#0#2#Changement telephone IP et paramétrage#..\\..\\..\\PhotosIntervention\\GXV3140-9.jpg#Turacrosh#Théophile#0621346589#planifiée#phenri#1#eroberts#",
            "#0#3#4#Installation nouvelle ligne telephonique#..\\..\\..\\PhotosIntervention\\electriciteparticuliers.jpg#Barre#Laurent#8522369685#planifiée#asespoir#2#lpage#",
            "#0#6#8#Changement Box Internet#..\\..\\..\\PhotosIntervention\\internetBox.jpg#Perret#Inès#7412589636#planifiée#rlasire#4#eroberts#",
            "#0#8#9#Coupure dans armoire éléctrique#..\\..\\..\\PhotosIntervention\\armoireElecrique.jpg#Dabloc#Elmer#85296341#planifiée#asespoir#6#jgosling#",
            "#1#0#1#Ajout additif temporisation#..\\..\\..\\PhotosIntervention\\artisanelectricien.jpg#Coptaire#Elie#0621346589#planifiée#phenri#5#eroberts#",
            "#1#1#5#Changement moteur électrique#..\\..\\..\\PhotosIntervention\\MOTEURELECTRIQUE.jpg#Gaize#Elmer#0722369685#planifiée#asespoir#2#eroberts#",
            "#1#7#8#Changement relais commande chauffage#..\\..\\..\\PhotosIntervention\\pict0010.jpg#Tassion#Félicie#0612589636#planifiée#rlasire#4#eroberts#",
            "#2#8#9#Coupure fibre internet#..\\..\\..\\PhotosIntervention\\photo-012.jpg#Oposte#Fidel#05296341#planifiée#asespoir#6#jgosling#"
    };
        //*****************************************************************************************************************
        // pour chaque employé il faut :
        // faire un split du string csv représentant l'employé
        // peupler une entité employé et l'inscrire en BDD
        private void peuplerTableEmploye()
        {
            Employe employe = new Employe();
            using (EmployeManager employeManager = new EmployeManager()) // appel automatique de la methode dispose qui ferme la connexion
            {
                foreach (var item in listeEmployes) // pour tous les clients du jeux d'essais
                {
                    String[] str = item.Split('#'); // on a choisi # comme séparateur csv
                    employe.LoginE = str[0];                
                    employe.Prenom = str[1];
                    employe.Nom = str[2];
                    employe.Groupe = str[3];
                    // On persiste l'entité en BDD
                    employeManager.ajoutModifEmploye(ref employe);
                }
            }
        }

        //*****************************************************************************************************************
        // pour chaque technicien il faut :
        // faire un split du string csv représentant un enregistrement technicien
        // peupler une entité technicien et l'inscrire en BDD
        private void peuplerTableTechnicien()
        {
            Technicien technicien = new Technicien();
            using (TechnicienManager technicienManager = new TechnicienManager())
            {
                Utils utils = new Utils();
                foreach (var item in listeETechniciens) // pour tous les techniciens du jeux d'essais
                {
                    String[] str = item.Split('#'); // on a choisi # comme séparateur csv
                    technicien.LoginT = str[0];
                    technicien.PasswdT = utils.getMd5Hash(str[1]);
                    technicien.Prenom = str[2];
                    technicien.Nom = str[3];
                    if (str[4] != "")
                    {
                        technicien.FkIdMateriel = Convert.ToInt32(str[4]); // on pourra affecté d'office un matériel
                    }
                    // On persiste l'entité en BDD
                    technicienManager.ajoutModifTechnicien(ref technicien);
                }
            }
        }

        //*****************************************************************************************************************
        private void peuplerTablePositionTechnicien()
        {
            PositionTechnicien positionTechnicien = new PositionTechnicien();
            using (TechnicienManager technicienManager = new TechnicienManager())
            {
                foreach (var item in listePositionTechnicien) 
                {
                    String[] str = item.Split('#'); // on a choisi # comme séparateur csv
                    positionTechnicien.Latitude = str[0];
                    positionTechnicien.Longitude = str[1];
                    positionTechnicien.FkLoginT = str[2];
                    // On persiste l'entité en BDD
                    technicienManager.insUpdatePosTechnicien(ref positionTechnicien);
                }
            }
        }

        //*****************************************************************************************************************
        private void peuplerTableSessionTechnicien()
        {
            SessionTechnicien sessionTechnicien = new SessionTechnicien();
            using (TechnicienManager technicienManager = new TechnicienManager())
            {
                foreach (var item in listeSessionTechnicien)
                {
                    String[] str = item.Split('#'); // on a choisi # comme séparateur csv
                    sessionTechnicien.Jeton = str[0];
                    sessionTechnicien.FkLoginT = str[1];
                    // On persiste l'entité en BDD
                    technicienManager.insUpdateSessionTechnicien(ref sessionTechnicien);
                }
            }
        }
        //*****************************************************************************************************************
        private void recupererListeDesTechniciensActifs()
        {
            using (TechnicienManager technicienManager = new TechnicienManager())
            {
                List<VTechnicienItinerant> listeTechniciensActifs = new List<VTechnicienItinerant>();
                listeTechniciensActifs = technicienManager.getListeTechniciensActif();
                foreach (var item in listeTechniciensActifs)
                {
                    Console.WriteLine(item.Prenom + "   " + item.Nom);
                }
            }
        }
        //*****************************************************************************************************************
        // pour chaque client il faut :
        // faire un split du string csv représentatnt le client et éventuellment récupérer et convertir l'image du lieu
        // peupler une entité client et l'inscrire en BDD
        private void peuplerTableClient()
        {
            Client client = new Client();
            using (ClientManager clientManager = new ClientManager()) // appel automatique de la methode dispose qui ferme la connexion
            {
                foreach (var item in listeClients) // pour tous les clients du jeux d'essais
                {
                    String[] str = item.Split('#'); // on a choisi # comme séparateur csv
                    client.Entreprise = str[0];            
                    client.Civilite = str[1];
                    client.Prenom = str[2];
                    client.Nom = str[3];
                    client.Adresse = str[4];
                    client.CompAdresse = str[5];
                    client.Ville = str[6];
                    client.CodePostal = str[7];
                    client.NumeroTel = str[8];
                    client.Email = str[9];
                    if (str[10] != "") // y a t il une image correspondant à l'adresse fournie par le client
                    {
                        // il faut charger et convertir l'image
                        // Conversion fichier photo en tableau de byte pour enregistrement en BDD
                        FileStream fs = new FileStream(str[10], FileMode.OpenOrCreate, FileAccess.Read); // on ouvre le fichier de la photo
                        byte[] imageBytes = new byte[fs.Length]; // tableau de byte pour recevoir le contenu des octets de la photo
                        fs.Read(imageBytes, 0, Convert.ToInt32(fs.Length)); // on place le contenu des octes de la photo dans le tableau
                        client.Photoent = imageBytes; // on enregistre dans l'entité
                    }
                    else
                    {
                        client.Photoent = new Byte[0];
                    }
                    client.Latitude = str[11];
                    client.Longitude = str[12];
                    client.EtatClient = str[13];
                    client.FkLoginE = str[14];
                    // On persiste l'entité en BDD
                    clientManager.insUpdateClient(client);
                }
            } // fermeture de la connexion et destruction de l'objet dao

        }
        //*****************************************************************************************************************
        void listerLesClients()
        {
            try
            {
                using (ClientManager clientManager = new ClientManager())
                {
                    List<Client> listClient = clientManager.listeClient();
                    foreach (Client chaqueClient in listClient)
                    {
                        Console.Write(chaqueClient.Prenom + "  " + chaqueClient.Nom
                            + "  " + chaqueClient.DateCreation.ToString()
                            + "  " + chaqueClient.DateModification.ToString());
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //*****************************************************************************************************************
        // pour chaque materiel il faut :
        // faire un split du string csv représentant le materiel
        // peupler une entité materiel et l'inscrire en BDD
        private void peuplerTableMateriel()
        {
            Materiel materiel = new Materiel();
            using (MaterielManager materielManager = new MaterielManager()) // appel automatique de la methode dispose qui ferme la connexion
            {
                foreach (var item in listeMateriels) // pour tous les clients du jeux d'essais
                {
                    String[] str = item.Split('#'); // on a choisi # comme séparateur csv
                    materiel.TypeMateriel = str[0];
                    materiel.NumeroSerie = str[1];
                    materiel.NumeroTel = str[2];
                    materiel.Imei = str[3];
                    materiel.IdGoogle = str[4];
                    materiel.FkLoginE = str[5];
                    materiel.EtatMateriel = str[6];
                    // On persiste l'entité en BDD
                    materielManager.insertUpdateMateriel(ref materiel);
                }
            }
        }

        //*****************************************************************************************************************
        void listerLesMateriels()
        {
            try
            {
                using (MaterielManager materielManager = new MaterielManager())
                {
                    List<Materiel> listMateriel = materielManager.getListeMateriel();
                    foreach (Materiel chaqueMateriel in listMateriel)
                    {
                        Console.Write(chaqueMateriel.TypeMateriel + "  " + chaqueMateriel.NumeroSerie
                            + "  " + chaqueMateriel.DateEnregistrement.ToString()
                            + "  " + chaqueMateriel.DateAffectation.ToString());
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //*****************************************************************************************************************
        void peuplerTableIntervention()
        {
            DateTime dateDuJour = DateTime.Now.Date.AddHours(8); // la journée débute à 8 heures

            Intervention intervention = new Intervention();
            using (InterventionManager interventionManager = new InterventionManager()) // appel automatique de la methode dispose qui ferme la connexion
            {
                foreach (var item in listeInterventions) // pour tous les clients du jeux d'essais
                {
                    String[] str = item.Split('#'); // on a choisi # comme séparateur csv
                    intervention.CompteRendu = str[0];
                    intervention.DebutIntervention = dateDuJour.AddDays(Convert.ToDouble(str[1]));
                    intervention.DebutIntervention = intervention.DebutIntervention.AddHours(Convert.ToDouble(str[2]));
                    intervention.FinIntervention = dateDuJour.AddDays(Convert.ToDouble(str[1]));
                    intervention.FinIntervention = intervention.FinIntervention.AddHours(Convert.ToDouble(str[3]));
                    intervention.ObjectifVisite = str[4];
                    if (str[5] != "") // y a t il une image correspondant à l'intervention
                    {
                        // il faut charger et convertir l'image
                        // Conversion fichier photo en tableau de byte pour enregistrement en BDD
                        FileStream fs = new FileStream(str[5], FileMode.OpenOrCreate, FileAccess.Read); // on ouvre le fichier de la photo
                        byte[] imageBytes = new byte[fs.Length]; // tableau de byte pour recevoir le contenu des octets de la photo
                        fs.Read(imageBytes, 0, Convert.ToInt32(fs.Length)); // on place le contenu des octes de la photo dans le tableau
                        intervention.PhotoLieu = imageBytes; // on enregistre dans l'entité
                    }
                    else
                    {
                        intervention.PhotoLieu = new Byte[0];
                    }
                    intervention.NomContact = str[6];
                    intervention.PrenomContact = str[7];
                    intervention.TelContact = str[8];
                    intervention.EtatVisite = str[9];
                    intervention.FkLoginE = str[10];
                    if (str[11] != "")
                    {
                        intervention.FkIdClient = Convert.ToInt32(str[11]); // on doit affecter d'office un client à l'intervention
                    }
                    intervention.FkLoginT = str[12];

                    // On persiste l'entité en BDD
                    interventionManager.ajouterIntervention(intervention);
                }
            }
        }
        //*****************************************************************************************************************
        void listerInterventions()
        {
            try
            {
                using (InterventionManager interventionManager = new InterventionManager())
                {
                    List<Intervention> listIntervention = interventionManager.listeInterventions();
                    foreach (Intervention chaqueIntervention in listIntervention)
                    {
                        Console.Write(chaqueIntervention.NomContact + "  " + chaqueIntervention.FkLoginT
                            + "  " + chaqueIntervention.DebutIntervention.ToString()
                            + "  " + chaqueIntervention.FinIntervention.ToString());
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //*****************************************************************************************************************
        void listerInterventionsTechnicienDate()
        {
            Intervention uneIntervention = new Intervention();
            uneIntervention.DebutIntervention = DateTime.Now.Date;
            uneIntervention.FkLoginT = "eroberts";
            try
            {
                using (InterventionManager interventionManager = new InterventionManager())
                {
                    List<Intervention> listIntervention = interventionManager.listeInterventionsTechnicien(uneIntervention);
                    foreach (Intervention chaqueIntervention in listIntervention)
                    {
                        Console.Write(chaqueIntervention.NomContact + "  " + chaqueIntervention.FkLoginT
                            + "  " + chaqueIntervention.DebutIntervention.ToString()
                            + "  " + chaqueIntervention.FinIntervention.ToString());
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //*****************************************************************************************************************
        ClassePeuplementTables()
        {
            // DIFFERENTS APPELS DE METHODES POSSIBLES
            //peuplerTableEmploye();
            //peuplerTableClient();
            //listerLesClients();
            peuplerTableMateriel();
            //listerLesMateriels();
            //peuplerTableTechnicien();
            //peuplerTablePositionTechnicien();
            //peuplerTableSessionTechnicien();
            //recupererListeDesTechniciensActifs();
            //peuplerTableIntervention();
            //listerInterventions();
            //listerInterventionsTechnicienDate();
        }
        //*****************************************************************************************************************
        static void Main(string[] args)
        {
            new ClassePeuplementTables();
        }
    }
}
