using RESTful;
using Npgsql;
using System.Data;

namespace RESTful;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddCommandLine(args);

        var config = configurationBuilder.Build();
        string connectionString = config["ConnectionString"];

        await using var dataSource = NpgsqlDataSource.Create(connectionString);

        builder.Services.AddSingleton<KeyValueRepository>(_ => new KeyValueRepository(dataSource));
        builder.Services.AddControllers();

        var app = builder.Build();

        app.MapControllers();

        app.Run();
    }
}
