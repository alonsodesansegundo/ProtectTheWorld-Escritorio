using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace DataAccessLibrary
{
    public static class DataAcess
    {
        public static void InitializeDatabase()
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();
                EjecutaQuery("DROP TABLE PUNTUACIONES");
                String tablaOpciones = "CREATE TABLE IF NOT " +
                    "EXISTS OPCIONES (Nave INTEGER, " +
                    "Musica INTEGER)";

                String tablaRecords = "CREATE TABLE IF NOT " +
                    "EXISTS PUNTUACIONES (Id INTEGER, " +
                    "Siglas VARCHAR(3), " +
                    "Puntuacion INTEGER)";

                //CREO LAS TABLAS
                SqliteCommand creaTablaOpciones = new SqliteCommand(tablaOpciones, db);
                SqliteCommand creaTablaRecords = new SqliteCommand(tablaRecords, db);

                creaTablaOpciones.ExecuteReader();
                creaTablaRecords.ExecuteReader();

                //INSERTO DATOS EN ELLAS
                //records iniciales
                if (DameSiglas().Count == 0 || DamePuntuaciones().Count == 0)
                {
                    string insertRecords = "INSERT INTO PUNTUACIONES VALUES" +
                            "(0,'???',10), " +
                            "(1,'???',10), " +
                            "(2,'???',10), " +
                            "(3,'???',10), " +
                            "(4,'???',10), " +
                            "(5,'???',10), " +
                            "(6,'???',10), " +
                            "(7,'???',10), " +
                            "(8,'???',10), " +
                            "(9,'???',10) ";
                    EjecutaQuery(insertRecords);
                }
                if (DameOpciones().Count == 0)
                {
                    //opciones iniciales
                    string insertOpciones = "INSERT INTO OPCIONES VALUES" +
                        "(1,1)";
                    EjecutaQuery(insertOpciones);
                }
            }
        }
        //PARA HACER INSERTS
        public static void EjecutaQuery(string query)
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = query;

                insertCommand.ExecuteReader();

                db.Close();
            }


        }
        public static List<String> DameOpciones()
        {
            List<String> entries = new List<string>();

            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT Nave,Musica FROM OPCIONES", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                    entries.Add(query.GetString(1));
                }

                db.Close();
            }
            return entries;
        }
        public static List<String> DamePuntuaciones()
        {
            List<String> entries = new List<string>();

            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT siglas,id,puntuacion FROM PUNTUACIONES ORDER BY 3 desc, 2 ", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(2));
                }

                db.Close();
            }
            return entries;
        }
        public static List<String> DameSiglas()
        {
            List<String> entries = new List<string>();

            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT siglas,id,puntuacion FROM PUNTUACIONES ORDER BY 3 desc, 2 ", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));

                }

                db.Close();
            }
            return entries;
        }
        public static List<String> PeorPuntuacion()
        {
            List<String> entries = new List<string>();

            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT id,siglas,puntuacion FROM PUNTUACIONES ORDER BY 3 , 1 DESC LIMIT 1", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                    entries.Add(query.GetString(2));
                }
                db.Close();
            }
            return entries;
        }
        public static int MaximoId()
        {
            int max = 0;
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT max(id) FROM PUNTUACIONES", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    max=query.GetInt32(0);
                }
                db.Close();
            }
            return max;
        }
    }

}
