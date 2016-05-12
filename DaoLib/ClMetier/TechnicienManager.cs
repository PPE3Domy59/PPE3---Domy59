using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Data.Sql;

namespace LibDao
{
    public class TechnicienManager : IDisposable
    {
        ConnexionSqlServer connexionSqlServer = null;
        SqlConnection sqlConnexion = null;
        //*****************************************************************************************************************
        public TechnicienManager()
        {
            connexionSqlServer = new ConnexionSqlServer();
            sqlConnexion = connexionSqlServer.Connexion;
        }

        public TechnicienManager(ConnexionSqlServer connexionSqlServer)
        {
            this.connexionSqlServer = connexionSqlServer;
            sqlConnexion = connexionSqlServer.Connexion;
        }

        public ConnexionSqlServer getConnexion()
        {
            return connexionSqlServer;
        }
        //*****************************************************************************************************************
        public Technicien getTechnicien(Technicien technicien)
        {
            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;

            // Type de commande de commande et nom de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = @"spGetTechnicien";

            // paramètres passées à la procédure stockée

            sqlCmd.Parameters.Add("@pLoginT", SqlDbType.NVarChar, 25).Value = technicien.LoginT;
            sqlCmd.Parameters.Add("@pPrenom", SqlDbType.NVarChar, 20).Value = technicien.Prenom;
            sqlCmd.Parameters.Add("@pNom", SqlDbType.NVarChar, 30).Value = technicien.Nom;
            sqlCmd.Parameters.Add("@pIdMateriel", SqlDbType.Int).Value = technicien.FkIdMateriel;

            try
            {
                // On se connecte
                if (sqlConnexion.State != ConnectionState.Open)
                {
                    sqlConnexion.Open();
                }
                SqlDataReader dataReader = sqlCmd.ExecuteReader();
                // Lecture d'un enregistrements contenus dans le DataRead
                if (dataReader.Read() == true) // une objet technicien trouvée
                {
                    technicien.LoginT = (String)dataReader["loginT"];
                    technicien.Prenom = (String)dataReader["prenom"];
                    technicien.Nom = (String)dataReader["nom"];
                    technicien.FkIdMateriel = (dataReader["fkIdMateriel"]) == DBNull.Value ? 0 : ((int)dataReader["fkIdMateriel"]);
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                Dispose();
                throw new Exception("Erreur recherche technicien \n");
            }
            return technicien;
        }

        //*****************************************************************************************************************
        public bool ajoutModifTechnicien(ref Technicien technicien)
        {
            bool retour = false;

            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;

            // Type de commande de commande et nom de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = @"spInsUpdateTechnicien";

            // paramètres passés à la procédure stockée
            sqlCmd.Parameters.Add("@pLoginT", SqlDbType.NVarChar, 25).Value = technicien.LoginT;
            sqlCmd.Parameters.Add("@pPrenom", SqlDbType.NVarChar, 20).Value = technicien.Prenom;
            sqlCmd.Parameters.Add("@pNom", SqlDbType.NVarChar, 30).Value = technicien.Nom;
            sqlCmd.Parameters.Add("@pPasswdT", SqlDbType.NVarChar, 32).Value = technicien.PasswdT;
            if (technicien.FkIdMateriel == 0)
            {
                sqlCmd.Parameters.Add("@pFkIdMateriel", SqlDbType.Int).Value = DBNull.Value;
            }
            else
            {
                sqlCmd.Parameters.Add("@pFkIdMateriel", SqlDbType.Int).Value = technicien.FkIdMateriel;
            }

            // On persiste les data
            try
            {
                // On se connecte
                if (sqlConnexion.State != ConnectionState.Open)
                {
                    sqlConnexion.Open();
                }
                // On appelle la procédure stockée
                if ((int)sqlCmd.ExecuteNonQuery() == -1)
                {
                    retour = true; // Une ligne a été modifiée dans la BDD tvb
                }
            }
            catch (Exception ex)
            {
                Dispose();
                throw new Exception("Erreur lors de l'ajout ou modification d'un technicien");
            }
            return retour;
        }
        //*****************************************************************************************************************
        // On passe en paramètre un utilisateur qui se voit supprimer
        // on retourne True si tout s'est bien passé
        public bool supprimerTechnicien(Technicien prmTechnicien)
        {
            Technicien technicien = getTechnicien(prmTechnicien); // On récupère un objet complet
            bool retour = false;

            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;

            // Type de commande de commande et nom de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;

            if (technicien.LoginT != "") // il y a un Technicien à supprimer
            {
                sqlCmd.CommandText = @"spTechnicienDelete";
                // paramètres passées à la procédure stockée
                sqlCmd.Parameters.Add("@pLoginT", SqlDbType.NVarChar, 25).Value = technicien.LoginT;
                try
                {
                    // On ouvre la connexion
                    if (sqlConnexion.State != ConnectionState.Open)
                    {
                        sqlConnexion.Open();
                    }
                    // On appelle la procédure stockée
                    if ((int)sqlCmd.ExecuteNonQuery() == -1)
                    {
                        retour = true;
                    }
                }
                catch (Exception ex)
                {
                    Dispose();
                    throw new Exception("Erreur lors de la suppression d'un Technicien \n" + ex.Message);
                }
            }
            return retour;
        }

        //*****************************************************************************************************************
        public List<Technicien> getListeTechnicien()
        {
            // pour une commande "select * " on utilise pas de procédure stockée
            List<Technicien> listTechniciens = new List<Technicien>();
            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand("select * from technicien", sqlConnexion);
            try
            {
                // On se connecte
                if (sqlConnexion.State != ConnectionState.Open)
                {
                    sqlConnexion.Open();
                }
                SqlDataReader dataReader = sqlCmd.ExecuteReader();
                // Lecture de tous les enregistrements contenus dans le DataRead
                while (dataReader.Read())
                {
                    listTechniciens.Add(new Technicien()
                    {
                        LoginT = (String)dataReader["loginT"],
                        Prenom = (String)dataReader["prenom"],
                        Nom = (String)dataReader["nom"],
                        FkIdMateriel = (dataReader["fkIdMateriel"]) == DBNull.Value ? 0 : ((int)dataReader["fkIdMateriel"])
                    });
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                Dispose();
                throw new Exception("Erreur lors de la récupération liste des Techniciens \n" + ex.Message);
            }
            return listTechniciens;
        }
        //*****************************************************************************************************************
        public bool insUpdatePosTechnicien(ref PositionTechnicien positionTechnicien)
        {
            bool retour = false;

            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;

            // Type de commande de commande et nom de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = @"spInsUpdatePosTechnicien";

            // paramètres passés à la procédure stockée
            sqlCmd.Parameters.Add("@pLatitude", SqlDbType.NVarChar, 15).Value = positionTechnicien.Latitude;
            sqlCmd.Parameters.Add("@pLongitude", SqlDbType.NVarChar, 15).Value = positionTechnicien.Longitude;
            sqlCmd.Parameters.Add("@pLoginT", SqlDbType.NVarChar, 25).Value = positionTechnicien.FkLoginT;
            // On persiste les data
            try
            {
                // On se connecte
                if (sqlConnexion.State != ConnectionState.Open)
                {
                    sqlConnexion.Open();
                }
                // On appelle la procédure stockée
                if ((int)sqlCmd.ExecuteNonQuery() == -1)
                {
                    retour = true; // Une ligne a été modifiée dans la BDD tvb
                }
            }
            catch (Exception ex)
            {
                Dispose();
                throw new Exception("Erreur lors de la modification d'une position technicien");
            }
            return retour;
        }
        //*****************************************************************************************************************
        public bool insUpdateSessionTechnicien(ref SessionTechnicien sessionTechnicien)
        {
            bool retour = false;

            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;

            // Type de commande de commande et nom de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = @"spInsUpdateSessionTechnicien";

            // paramètres passés à la procédure stockée
            sqlCmd.Parameters.Add("@pJeton", SqlDbType.NVarChar, 255).Value = sessionTechnicien.Jeton;
            sqlCmd.Parameters.Add("@pLoginT", SqlDbType.NVarChar, 25).Value = sessionTechnicien.FkLoginT;
            // On persiste les data
            try
            {
                // On se connecte
                if (sqlConnexion.State != ConnectionState.Open)
                {
                    sqlConnexion.Open();
                }
                // On appelle la procédure stockée
                if ((int)sqlCmd.ExecuteNonQuery() == -1)
                {
                    retour = true; // Une ligne a été modifiée dans la BDD tvb
                }
            }
            catch (Exception ex)
            {
                Dispose();
                throw new Exception("Erreur lors de la modification session technicien");
            }
            return retour;
        }
        //*****************************************************************************************************************

        // On récupère tous les techniciens en déplacement qui ont un jeton de session non nul
        public List<VTechnicienItinerant> getListeTechniciensActif()
        {
            List<VTechnicienItinerant> listeTechnicienItinerants = new List<VTechnicienItinerant>();
            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand("select * from viewPosTechniciensActifs", sqlConnexion);
            try
            {
                // On se connecte
                if (sqlConnexion.State != ConnectionState.Open)
                {
                    sqlConnexion.Open();
                }
                SqlDataReader dataReader = sqlCmd.ExecuteReader();
                // Lecture de tous les enregistrements contenus dans le DataRead
                while (dataReader.Read())
                {
                    listeTechnicienItinerants.Add(new VTechnicienItinerant()
                    {
                        LoginT = (String)dataReader["loginT"],
                        Prenom = (String)dataReader["prenom"],
                        Nom = (String)dataReader["nom"],
                        Latitude = (String)dataReader["latitude"],
                        Longitude = (String)dataReader["longitude"],
                        Datepos = (DateTime)dataReader["datepos"]
                    });
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                Dispose();
                throw new Exception("Erreur lors de la récupération liste des Techniciens \n" + ex.Message);
            }
            return listeTechnicienItinerants;
        }
        //*****************************************************************************************************************
        // On récupère tous les techniciens et on ajoute à la liste renoyé ceux qui ont une Session non nulle
        public PositionTechnicien getPositionTechnicienActif(ref Technicien technicienActif)
        {
            PositionTechnicien positionTechnicien = new PositionTechnicien();
            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;
            // Type de commande de commande et nom de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = @"spGetPosTechnicien";
            // paramètres passées à la procédure stockée
            sqlCmd.Parameters.Add("@pLoginT", SqlDbType.NVarChar, 25).Value = technicienActif.LoginT;

            try
            {
                // On se connecte
                if (sqlConnexion.State != ConnectionState.Open)
                {
                    sqlConnexion.Open();
                }
                SqlDataReader dataReader = sqlCmd.ExecuteReader();
                // Lecture d'un enregistrement contenu dans le DataRead
                if (dataReader.Read() == true) // un objet position trouvé
                {
                    positionTechnicien.Latitude = (String)dataReader["latitude"];
                    positionTechnicien.Longitude = (String)dataReader["longitude"];
                    positionTechnicien.Datepos = (DateTime)dataReader["datepos"];
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                Dispose();
                throw new Exception("Erreur lecture position technicien");
            }
            return positionTechnicien;
        }
        //*****************************************************************************************************************   
        public void Dispose()
        {
            if (connexionSqlServer != null)
                connexionSqlServer.closeConnexion();
        }
    }
}
