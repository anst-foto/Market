using System.Data;
using Market.Lib.Models;
using Npgsql;

namespace Market.Lib;

public class TablePersons
{
    private const string ConnectionString = "Server=127.0.0.1;Port=5432;Database=market_db;User Id=postgres;Password=1234;SearchPath=test;";
    
    private readonly NpgsqlConnection _connection = new(ConnectionString);

    public IEnumerable<Person>? GetAllPersons()
    {
        var result = new List<Person>();
        _connection.Open();

        const string sql = """
                           SELECT id, 
                                  first_name, last_name, patronymic 
                           FROM table_persons
                           """;
        using var command = new NpgsqlCommand(sql, _connection);
        var reader = command.ExecuteReader();
        
        if (!reader.HasRows) return null;
        
        while (reader.Read())
        {
            result.Add(new Person()
            {
                Id = reader.GetInt32("id"),
                LastName = reader.GetString("last_name"),
                FirstName = reader.GetString("first_name"),
                Patronymic = reader.GetString("patronymic")
            });
        }
        
        _connection.Close();
        return result;
    }

    public Person? GetPersonById(int id)
    {
        _connection.Open();

        const string sql = """
                           SELECT id, 
                                  first_name, last_name, patronymic 
                           FROM table_persons
                           WHERE id = @id
                           """;
        using var command = new NpgsqlCommand(sql, _connection);
        command.Parameters.AddWithValue("@id", id);
        /*var parameter = new NpgsqlParameter("@id", id);
        command.Parameters.Add(parameter);*/
        
        var reader = command.ExecuteReader();
        
        if (!reader.HasRows) return null;
        
        reader.Read();
        
        var person = new Person()
        {
            Id = reader.GetInt32("id"),
            LastName = reader.GetString("last_name"),
            FirstName = reader.GetString("first_name"),
            Patronymic = reader.GetString("patronymic")
        };
        
        _connection.Close();
        return person;
    }
}