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
    public class MaterielManager : IDisposable
    {
        ConnexionSqlServer connexionSqlServer = null;
        SqlConnection sqlConnexion = null;
        //*****************************************************************************************************************
        public MaterielManager()
        {
            connexionSqlServer = new ConnexionSqlServer();
            sqlConnexion = connexionSqlServer.Connexion;
        }

        public MaterielManager(ConnexionSqlServer connexionSqlServer)
        {
            this.connexionSqlServer = connexionSqlServer;
            sqlConnexion = connexionSqlServer.Connexion;
        }

        public ConnexionSqlServer getConnexion()
        {
            return connexionSqlServer;
        }
        //*****************************************************************************************************************

        public Materiel getMateriel(ref Materiel prmMateriel)
        {
            Materiel materiel = new Materiel();
            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;

            // Type de commande de commande et DateRemise de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = @"spGetMateriel";

            // paramètres passées à la procédure stockée
            sqlCmd.Parameters.Add("@pIdMateriel", SqlDbType.Int).Value = prmMateriel.IdMateriel;
            try
            {
                // On se connecte
                if (sqlConnexion.State != ConnectionState.Open)
                {
                    sqlConnexion.Open();
                }
                SqlDataReader dataReader = sqlCmd.ExecuteReader();
                // Lecture d'un enregistrements contenus dans le DataRead
                if (dataReader.Read() == true) // un Materiel trouvé
                {
                    materiel.IdMateriel = (int)dataReader["IdMateriel"];
                    materiel.TypeMateriel = (String)dataReader["TypeMateriel"];
                    materiel.DateAffectation = (DateTime)dataReader["DateAffectation"];
                    materiel.NumeroTel = (String)dataReader["NumeroTel"];
                    materiel.NumeroSerie = (String)dataReader["NumeroSerie"];
                    materiel.Imei = (String)dataReader["Imei"];
                    materiel.IdGoogle = (String)dataReader["IdGoogle"];
                    materiel.EtatMateriel = (String)dataReader["EtatMateriel"];
                    materiel.FkLoginE = (String)dataReader["fkLoginE"];
                }
                dataReader.Close();
            }
            catch (Exception)
            {
                Dispose();
                throw new Exception("Erreur recherche Materiel \n");
            }
            return materiel;
        }

        //*****************************************************************************************************************
        // On passe en paramètre un matériel a supprimer
        // on retourne True si tout s'est bien passé
        public bool supprimerMateriel(Materiel prmMateriel)
        {
            prmMateriel = getMateriel(ref prmMateriel); // On récupère un objet complet
            bool retour = false;

            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;

            // Type de commande de commande et DateRemise de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;

            if (prmMateriel.IdMateriel != 0) // il y a une EntityMateriel à supprimer
            {
                sqlCmd.CommandText = @"spMaterielDelete";
                // paramètres passées à la procédure stockée
                sqlCmd.Parameters.Add("@pIdMateriel", SqlDbType.Int).Value = prmMateriel.IdMateriel;

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
                    throw new Exception("Erreur lors de la suppression d'une Materiel \n" + ex.Message);
                }
            }
            return retour;
        }

        //*****************************************************************************************************************
        // Le matériel est connu par son Id
        public bool insertUpdateMateriel(ref Materiel prmMateriel)
        {
            bool retour = false;

            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;

            // Type de commande de commande et DateRemise de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = @"spInsUpdateMateriel";
            // paramètres passés à la procédure stockée
            sqlCmd.Parameters.Add("@pTypeMateriel", SqlDbType.NVarChar, 100).Value = prmMateriel.TypeMateriel;
            sqlCmd.Parameters.Add("@pNumeroSerie", SqlDbType.NVarChar, 50).Value = prmMateriel.NumeroSerie;
            sqlCmd.Parameters.Add("@pNumeroTel", SqlDbType.NVarChar, 12).Value = prmMateriel.NumeroTel;
            sqlCmd.Parameters.Add("@pImei", SqlDbType.NVarChar, 17).Value = prmMateriel.Imei;
            sqlCmd.Parameters.Add("@pIdGoogle", SqlDbType.Text).Value = prmMateriel.IdGoogle;
            sqlCmd.Parameters.Add("@pLoginE", SqlDbType.NVarChar, 25).Value = prmMateriel.FkLoginE;
            sqlCmd.Parameters.Add("@pEtatMateriel", SqlDbType.NVarChar, 15).Value = prmMateriel.EtatMateriel;

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
                throw new Exception("Erreur insertion ou modification d'un Materiel \n" + ex.Message);
            }
            return retour;
        }

        //*****************************************************************************************************************
        public List<Materiel> getListeMateriel()
        {
            // pour une commande "select * " on utilise pas de procédure stockée
            List<Materiel> listMateriels = new List<Materiel>();

            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand("select * from Materiel", sqlConnexion);

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
                    listMateriels.Add(new Materiel()
                    {
                        IdMateriel = (int)dataReader["idMateriel"],
                        TypeMateriel = (String)dataReader["typeMateriel"],
                        NumeroSerie = (String)dataReader["numeroSerie"],
                        NumeroTel = (dataReader["numeroTel"]) == DBNull.Value ? String.Empty : ((String)dataReader["numeroTel"]),
                        Imei = (dataReader["imei"]) == DBNull.Value ? String.Empty : ((String)dataReader["imei"]),
                        IdGoogle = (dataReader["idGoogle"]) == DBNull.Value ? String.Empty : ((String)dataReader["idGoogle"]),
                        DateEnregistrement = (DateTime)dataReader["dateEnregistrement"], 
                        DateAffectation = (dataReader["dateAffectation"] == DBNull.Value ? new DateTime(0) : (DateTime)(dataReader["dateAffectation"])),
                        FkLoginE = (String)dataReader["fkLoginE"],
                        EtatMateriel = (String)dataReader["etatMateriel"]   
                    });
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                Dispose();
                throw new Exception("Erreur lors de la récupération liste des Materiels \n" + ex.Message);
            }
            return listMateriels;
        }

        //*****************************************************************************************************************
        // Le matériel est connu par son Id
        public bool affectationMaterielTechnicien(ref Materiel materiel,ref Technicien technicien)
        {
            bool retour = false;

            // Initialisation de la commande associée à la connexion en cours
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConnexion;

            // Type de commande de commande et DateRemise de la procédure appelée
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = @"spAffectationMatériel";
            // paramètres passés à la procédure stockée
            sqlCmd.Parameters.Add("@pIdMateriel", SqlDbType.Int).Value = materiel.IdMateriel;
            sqlCmd.Parameters.Add("@pNumeroTel", SqlDbType.NVarChar, 12).Value = materiel.NumeroTel;
            sqlCmd.Parameters.Add("@pImei", SqlDbType.NVarChar, 17).Value = materiel.Imei;
            sqlCmd.Parameters.Add("@pIdGoogle", SqlDbType.Text).Value = materiel.IdGoogle;
            sqlCmd.Parameters.Add("@pLoginT", SqlDbType.NVarChar, 25).Value = technicien.LoginT;
            sqlCmd.Parameters.Add("@pLoginE", SqlDbType.NVarChar, 25).Value = materiel.FkLoginE;
            sqlCmd.Parameters.Add("@pEtatMateriel", SqlDbType.NVarChar, 15).Value = materiel.EtatMateriel;


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
                throw new Exception("Erreur affectation d'un Materiel \n" + ex.Message);
            }
            return retour;
        }

        //*****************************************************************************************************************
        public void Dispose()
        {
            if (connexionSqlServer != null)
                connexionSqlServer.closeConnexion();
        }
    }
}
