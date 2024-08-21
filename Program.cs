using Microsoft.Data.Sqlite;

namespace TODOCSHARP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists("todo.db"))
            {
                File.Create("todo.db").Close();
            }

            var connexion = new SqliteConnection("Filename=todo.db");
            try
            {
                connexion.Open();
                var createTableCommand = "CREATE TABLE IF NOT EXISTS " + 
                                         "Todos (Id INTEGER PRIMARY KEY AUTOINCREMENT, " + 
                                         "Value NVARCHAR (2048) NOT NULL, " + "Completed BOOLEAN)";
                new SqliteCommand(createTableCommand,connexion).ExecuteNonQuery();

                string choix = "";

                while (choix != "q")
                {
                    Console.Clear();
                    Console.WriteLine("--------------MENU-----------");
                    Console.WriteLine("1. Lire les Todos");
                    Console.WriteLine("2. Creer un Todos");
                    Console.WriteLine("3. Marquer un Todos comme terminer");
                    Console.WriteLine("Faite Q pour quitter");

                    choix=Console.ReadLine();

                    if (choix.Equals("q", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                    else if (choix=="1")
                    {
                        SqliteCommand command = new SqliteCommand("SELECT * FROM Todos", connexion);
                        SqliteDataReader reader= command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var id = reader.GetInt32(reader.GetOrdinal("Id"));
                                var value = reader.GetString(reader.GetOrdinal("Value"));
                                var completed = reader.GetBoolean(reader.GetOrdinal("Completed"));
                                Console.WriteLine($"{id} {value} (terminé? {(completed ? "V" : "N") })");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Aucun TODO a afficher");
                        }
                        Console.WriteLine("Appuyer sur entrer pour retourner au menu");
                        Console.ReadLine();
                    }
                    else if(choix=="2")
                    {
                        Console.WriteLine("Saisir la tache a realiser: ");
                        var value= Console.ReadLine();

                        SqliteCommand insert = new SqliteCommand("INSERT INTO Todos(Value, Completed) VALUES (@Value, 0)", connexion);
                        insert.Parameters.AddWithValue("@Value", value);
                        insert.ExecuteNonQuery();
                        Console.WriteLine("Insertion effectuer avec suces");
                        Console.WriteLine("Appuyer sur entrer pour retourner au menu");
                        Console.ReadLine();
                    }
                    else if (choix=="3")
                    {
                        Console.WriteLine("Saisir l'id du todo a valider");
                        var id= int.Parse(Console.ReadLine());

                        SqliteCommand update = new SqliteCommand("UPDATE Todos SET Completed=1 WHERE Id=@Id", connexion);
                        update.Parameters.AddWithValue("@Id", id);
                        update.ExecuteNonQuery();
                        Console.WriteLine("Modification effectuer avec succes");
                        Console.WriteLine("Appuyer sur entrer pour retourner au menu");
                        Console.ReadLine();
                    }
                }
            }
            finally
            {
                connexion.Close();
            }
        }
    }
}
