using System.Configuration;
using EC2OnDemandGameHostingAPI.ConfigStuff;
using ConfigurationManager = System.Configuration.ConfigurationManager;


public class Program
{
    static int Main(string[] args)
    {
        try
        {
            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings.Count == 0)  
            {  
                //CONFIG MISSING VALUES
                Console.WriteLine("Config file is empty");
                Console.WriteLine("ABORTING!");
                return 404;
            }  
            else
            {
                Settings.Instance.secretKey = appSettings.Get("secretKey");
                Settings.Instance.secretKey = appSettings.Get("accessKey");
                Settings.Instance.secretKey = appSettings.Get("instanceID");
            }  
        }  
        catch (ConfigurationErrorsException)  
        {  
            //CONFIG ERROR
            Console.WriteLine("Config file is empty and or corrupt/non existent");
            Console.WriteLine("ABORTING!");
            return 4040;
        }  
        var builder = WebApplication.CreateBuilder(args);
        

// Add services to the container.

        builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
        return 0;
    }
}
