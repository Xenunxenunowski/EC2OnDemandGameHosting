namespace EC2OnDemandGameHostingAPI.ConfigStuff;

public sealed class Settings
{
    private static readonly Settings instance = new Settings();

    public string secretKey,acessKey,instanceID;
    
    static Settings(){}
    private Settings(){}

    public static Settings Instance
    {
        get
        {
            return instance;
        }
    }
}