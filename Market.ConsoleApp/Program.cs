using Market.Lib.Tables;

var tableUsers = new TableUsers();

foreach (var user in tableUsers.GetAll())
{
    Console.WriteLine(user);
}