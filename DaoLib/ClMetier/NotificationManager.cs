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
    public class NotificationManager : IDisposable
    {
        ConnexionSqlServer connexionSqlServer = null;
        SqlConnection sqlConnexion = null;
        //*****************************************************************************************************************
        public NotificationManager()
        {
            connexionSqlServer = new ConnexionSqlServer();
            sqlConnexion = connexionSqlServer.Connexion;
        }

        public NotificationManager(ConnexionSqlServer connexionSqlServer)
        {
            this.connexionSqlServer = connexionSqlServer;
            sqlConnexion = connexionSqlServer.Connexion;
        }
        public ConnexionSqlServer getConnexion()
        {
            return connexionSqlServer;
        }
        //*****************************************************************************************************************


        public Notification getEntityNotification(Notification prmEntityNotification)
        {
            Notification EntityNotification = new Notification();
            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;

            // Type de commande de commande et nom de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = @"appelNotification";

            // paramètres passées à la procédure stockée
            sqlCmd.Parameters.Add("@pIdNotification", SqlDbType.Int).Value = prmEntityNotification.IdNotification;
            sqlCmd.Parameters.Add("@pDateEnvoi", SqlDbType.DateTime).Value = prmEntityNotification.DateEnvoi;
            sqlCmd.Parameters.Add("@pContenu", SqlDbType.Text).Value = prmEntityNotification.Contenu;
            sqlCmd.Parameters.Add("@pIdTechnicien", SqlDbType.Int).Value = prmEntityNotification.FkIdTechnicien;


            try
            {
                // On se connecte
                if (sqlConnexion.State != ConnectionState.Open)
                {
                    sqlConnexion.Open();
                }
                SqlDataReader dataReader = sqlCmd.ExecuteReader();
                // Lecture d'un enregistrements contenus dans le DataRead
                if (dataReader.Read() == true) // une EntityNotification trouvée
                {
                    EntityNotification.IdNotification = (int)dataReader["IdNotification"];
                    EntityNotification.DateEnvoi = (DateTime)dataReader["DateEnvoi"];
                    EntityNotification.Contenu = (String)dataReader["Contenu"];
                    EntityNotification.FkIdTechnicien = (int)dataReader["IdTechnicien"];
                    


                }
                dataReader.Close();
            }
            catch (Exception)
            {
                Dispose();
                throw new Exception("Erreur recherche EntityNotification \n");
            }
            return EntityNotification;
        }

        //*****************************************************************************************************************
        public bool ajouterEntityNotification(Notification EntityNotification)
        {
            bool retour = false;

            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;

            // Type de commande de commande et nom de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = @"creerNotification";

            // paramètres passés à la procédure stockée
            sqlCmd.Parameters.Add("@pContenu", SqlDbType.VarChar, 50).Value = EntityNotification.Contenu;
            sqlCmd.Parameters.Add("@pIdTechnicien", SqlDbType.Int).Value = EntityNotification.FkIdTechnicien;

            



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
                throw new Exception("Erreur lors de l'ajout d'une EntityNotification \n" + ex.Message);
            }
            return retour;
        }

        //*****************************************************************************************************************
        // On passe en paramètre un utilisateur qui se voit supprimer
        // on retourne True si tout s'est bien passé
        public bool supprimerEntityNotification(Notification EntityNotification)
        {
            EntityNotification = getEntityNotification(EntityNotification); // On récupère un objet complet
            bool retour = false;

            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;

            // Type de commande de commande et nom de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;

            if (EntityNotification.IdNotification != 0) // il y a une EntityNotification à supprimer
            {
                sqlCmd.CommandText = @"suppressionNotification";
                // paramètres passées à la procédure stockée
                sqlCmd.Parameters.Add("@IdNotification", SqlDbType.Int).Value = EntityNotification.IdNotification;

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
                    throw new Exception("Erreur lors de la suppression d'une EntityNotification \n" + ex.Message);
                }
            }
            return retour;
        }

        //*****************************************************************************************************************
        // L'utilisateur est connu par son Id
        public bool modifierEntityNotification(Notification EntityNotification)
        {
            // il faut récupérer l'Id de la EntityNotification qui ne peut être null
            EntityNotification.IdNotification = getEntityNotification(EntityNotification).IdNotification;

            bool retour = false;

            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;

            // Type de commande de commande et nom de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = @"modifNotification";

            // paramètres passés à la procédure stockée
            sqlCmd.Parameters.Add("@pIdNotification", SqlDbType.Int).Value = EntityNotification.IdNotification;
            sqlCmd.Parameters.Add("@pDateEnvoi", SqlDbType.DateTime).Value = EntityNotification.DateEnvoi;
            sqlCmd.Parameters.Add("@pContenu", SqlDbType.VarChar, 50).Value = EntityNotification.Contenu;
            sqlCmd.Parameters.Add("@pIdTechnicien", SqlDbType.Int).Value = EntityNotification.FkIdTechnicien;


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
                throw new Exception("Erreur lors de la modification d'une EntityNotification \n" + ex.Message);
            }
            return retour;
        }

        //*****************************************************************************************************************
        public List<Notification> listeEntityNotification()
        {
            // pour une commande "select * " on utilise pas de procédure stockée
            List<Notification> listEntityNotifications = new List<Notification>();

            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand("select * from Notification", sqlConnexion);

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
                    listEntityNotifications.Add(new Notification()
                    {
                        IdNotification = (int)dataReader["IdNotification"],
                        Contenu = (String)dataReader["Contenu"],
                        DateEnvoi = (DateTime)dataReader["DateEnvoi"],
                        FkIdTechnicien = (int)dataReader["Idtechnicien"]

                    });
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                Dispose();
                throw new Exception("Erreur lors de la récupération liste des Notifications \n" + ex.Message);
            }
            return listEntityNotifications;
        }


        //*****************************************************************************************************************
        public void Dispose()
        {
            if (connexionSqlServer != null)
                connexionSqlServer.closeConnexion();
        }
    }
}
