using Amazon;
using Amazon.EC2;
using Amazon.Runtime;
using EC2OnDemandControl;
using EC2OnDemandGameHostingAPI.ConfigStuff;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers;

[ApiController]
[Route("/serverState")]
public class ServerStateController : ControllerBase
{
    // GET 
    [HttpGet(Name = "GetServerState")]
    public string Get(int gameCode)
    {
        switch (gameCode)
        {
            case 1:
                return EC2OnDemandControll.GetInstanceState( new AmazonEC2Client( new BasicAWSCredentials(Settings.Instance.acessKey,Settings.Instance.secretKey ),RegionEndpoint.EUCentral1),Settings.Instance.instanceID).ToString();
            break;
        }
        //ERROR: 104 | WRONG GAME CODE
        return "ERROR: 104 | WRONG GAME CODE";
    }
}
//GAMECODES:

//1 - Minecraft Java Edition
//… add more !