using Npgsql;
using System.Text.Json;

Console.OutputEncoding = System.Text.Encoding.Unicode;

var connectionString = "Host=localhost;Username=postgres;Password=1234;Database=securepass";
await using var dataSource = NpgsqlDataSource.Create(connectionString);
using var client = new HttpClient();

while (true)
{
    Console.WriteLine("Виберіть дію:" +
        "\n1. Вивести таблицю" +
        "\n2. Додати рандомні значення до таблиці" +
        "\n3. Вийти");
    int choice = Int32.Parse(Console.ReadLine());

    switch (choice)
    {
        case 1:
            while (true)
            {
                Console.WriteLine("Виберіть таблицю:" +
                    "\n1. Users" +
                    "\n2. Folders" +
                    "\n3. Passwords");
                int table = Int32.Parse(Console.ReadLine());
                switch (table)
                {
                    case 1:
                        await PrintTableAsync("users");
                        break;
                    case 2:
                        await PrintTableAsync("Folders");
                        break;
                    case 3:
                        await PrintTableAsync("Passwords");
                        break;
                }
                break;
            }
            break;     
        case 2:
            while (true)
            {
                Console.WriteLine("Виберіть таблицю:" +
                    "\n1. Users" +
                    "\n2. Folders" +
                    "\n3. Passwords");
                int table = Int32.Parse(Console.ReadLine());
                switch (table)
                {
                    case 1:
                        Console.WriteLine("Введіть кількість елементів: ");
                        int countUser = Int32.Parse(Console.ReadLine());
                        for (int i = 0; i < countUser; i++)
                        {
                            var responseUser = await client.GetAsync("https://my.api.mockaroo.com/users.json?key=c17bd660");
                            string userString = await responseUser.Content.ReadAsStringAsync();
                            var User = JsonSerializer.Deserialize<User>(userString);

                            await using (var cmd = dataSource.CreateCommand("INSERT INTO users (email, password) VALUES ($1, $2)"))
                            {
                                cmd.Parameters.AddWithValue(User.email);
                                cmd.Parameters.AddWithValue(User.password);
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                        break;
                    case 2:
                        Console.WriteLine("Введіть кількість елементів: ");
                        int countFolder = Int32.Parse(Console.ReadLine());
                        for (int i = 0; i < countFolder; i++)
                        {
                            var responseFolder = await client.GetAsync("https://my.api.mockaroo.com/folders.json?key=c17bd660");
                            string folderString = await responseFolder.Content.ReadAsStringAsync();
                            var folder = JsonSerializer.Deserialize<Folder>(folderString);

                            await using (var cmd = dataSource.CreateCommand("INSERT INTO folders (title, user_id) VALUES ($1, $2)"))
                            {
                                cmd.Parameters.AddWithValue(folder.title);
                                cmd.Parameters.AddWithValue(folder.user_id);
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                        break;
                    case 3:
                        Console.WriteLine("Введіть кількість елементів: ");
                        int countPassword = Int32.Parse(Console.ReadLine());
                        for (int i = 0; i < countPassword; i++)
                        {
                            var responsePasswords = await client.GetAsync("https://my.api.mockaroo.com/passwords.json?key=c17bd660");
                            string passwordString = await responsePasswords.Content.ReadAsStringAsync();
                            var password = JsonSerializer.Deserialize<Password>(passwordString);


                            await using (var cmd = dataSource.CreateCommand("INSERT INTO passwords (title, email_username, password, folder_id, user_id, last_updated, deleted) VALUES ($1, $2, $3, $4, $5, $6, $7)"))
                            {
                                cmd.Parameters.AddWithValue(password.title);
                                if (password.email_username != null)
                                {
                                    cmd.Parameters.AddWithValue(password.email_username);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue(DBNull.Value);
                                }
                                cmd.Parameters.AddWithValue(password.password);
                                if (password.folder_id != null)
                                {
                                    cmd.Parameters.AddWithValue(password.folder_id);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue(DBNull.Value);
                                }

                                cmd.Parameters.AddWithValue(password.user_id);
                                DateTime oDate = Convert.ToDateTime(password.last_updated);

                                cmd.Parameters.AddWithValue(oDate);
                                cmd.Parameters.AddWithValue(password.deleted);
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                        
                        break;
                }
                break;
            }
            break;
        case 3:
            return;
        default:
            break;
    }
}

async Task PrintTableAsync(string tableName)
{
    await using (var cmd = dataSource.CreateCommand("SELECT * FROM " + tableName))
    await using (var reader = await cmd.ExecuteReaderAsync())
    {  
        while (await reader.ReadAsync())
        {
            Console.WriteLine("-------------------------------------");
            for (int i = 0; i < reader.GetColumnSchema().Count; i++)
            {
                Console.WriteLine("{0}: {1} \t", reader.GetColumnSchema()[i].ColumnName, reader.GetValue(i) == DBNull.Value? "null" : reader.GetValue(i));
            }
            Console.WriteLine("-------------------------------------");
        }
    }
}

public class User
{
    public required string email { get; set; }
    public required string password { get; set; }
}

public class Password
{
    public required string title { get; set; }
    public string? email_username {  get; set; }
    public required string password { get; set; }
    public int? folder_id { get; set; }
    public int user_id { get; set; }
    public required string last_updated {  get; set; }
    public bool deleted {  get; set; }
}

public class Folder
{
    public required string title { get; set; }
    public required int user_id { get; set; }
}
