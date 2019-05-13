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
                if (DameRecords().Count == 0)
                {
                    string insertRecords = "INSERT INTO PUNTUACIONES VALUES" +
                            "(0,'???',0), " +
                            "(1,'???',0), " +
                            "(2,'???',0), " +
                            "(3,'???',0), " +
                            "(4,'???',0), " +
                            "(5,'???',0), " +
                            "(6,'???',0), " +
                            "(7,'???',0), " +
                            "(8,'???',0), " +
                            "(9,'???',0) ";
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

        //PARA OBTENER DATOS
        public static List<String> DameDato(string consulta)
        {
            List<String> entries = new List<string>();

            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    (consulta, db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                }

                db.Close();
            }
            return entries;
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
        public static List<String> DameRecords()
        {
            List<String> entries = new List<string>();

            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT Id,Siglas,Puntuacion FROM PUNTUACIONES", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                    entries.Add(query.GetString(1));
                    entries.Add(query.GetString(2));
                }

                db.Close();
            }
            return entries;
        }
        }
}
