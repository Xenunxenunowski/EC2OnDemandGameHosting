using System;
using Amazon;
using Amazon.EC2;
using Amazon.Runtime;
using EC2OnDemandControl;
using Microsoft.AspNetCore.Mvc;

namespace EC2OnDemandGameHostingAPI.Controllers;
using ConfigurationManager = System.Configuration.ConfigurationManager;

[ApiController]
[Route("/serverState")]
public class ServerStateController : ControllerBase
{
    // GET 
    [HttpGet(Name = "GetServerState")]
    public string Get(int gameCode)
    {
       
                Console.WriteLine("WHOT"+ConfigurationManager.AppSettings["secretKey"]);
                return EC2OnDemandControll.GetInstanceState( new AmazonEC2Client(
                    new BasicAWSCredentials( ConfigurationManager.AppSettings["accessKey"],
                    ConfigurationManager.AppSettings["secretKey"]),RegionEndpoint.EUCentral1),ConfigurationManager.AppSettings["instanceID"]).ToString();

                //ERROR: 104 | WRONG GAME CODE
        return "ERROR: 104 | WRONG GAME CODE";
    }
}
//GAMECODES:

//1 - Minecraft Java Edition
//… add more !