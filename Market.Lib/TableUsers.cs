using System.Data;
using Market.Lib.Models;
using Npgsql;

namespace Market.Lib;

public class TableUsers
{
    private const string ConnectionString = "Server=127.0.0.1;Port=5432;Database=market_db;User Id=postgres;Password=1234;SearchPath=test;";
    
    private readonly NpgsqlConnection _connection = new(ConnectionString);

    public IEnumerable<User>? GetAllUsers()
    {
        var result = new List<User>();
        _connection.Open();

        const string sql = "SELECT id, user_name FROM table_users";
        using var command = new NpgsqlCommand(sql, _connection);
        var reader = command.ExecuteReader();
        
        if (!reader.HasRows) return null;
        
        while (reader.Read())
        {
            result.Add(new User
            {
                Id = reader.GetInt32("id"),
                UserName = reader.GetString("user_name")
            });
        }
        
        _connection.Close();
        return result;
    }

    public User? GetUserById(int id)
    {
        _connection.Open();

        const string sql = "SELECT id, user_name FROM table_users WHERE id = @id";
        using var command = new NpgsqlCommand(sql, _connection);
        command.Parameters.AddWithValue("@id", id);
        /*var parameter = new NpgsqlParameter("@id", id);
        command.Parameters.Add(parameter);*/
        
        var reader = command.ExecuteReader();
        
        if (!reader.HasRows) return null;
        
        reader.Read();
        
        var user = new User
        {
            Id = reader.GetInt32("id"),
            UserName = reader.GetString("user_name")
        };
        
        _connection.Close();
        return user;
    }
}