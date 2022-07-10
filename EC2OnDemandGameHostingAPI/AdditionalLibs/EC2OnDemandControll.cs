using System;
using System.Collections.Generic;
using System.Threading;
using Amazon;
using Amazon.Runtime;
using Amazon.EC2;
using Amazon.EC2.Model;

// To interact with Amazon S3.

namespace EC2OnDemandControl
{
  enum ServerState
  {
    Pending,
    Running,
    ShuttingDown,
    Terminated,
    Stopping,
    Stopped,
    Undefined
  }
  
  class EC2OnDemandControll
  {
    public static bool StartInstance()
    {
      
      ServerState serverStatus =
        GetInstanceState(
          new AmazonEC2Client(
            new BasicAWSCredentials("AKIAQ4RBTCNEYAUDRZOA", "ZQSYXuXYqx//ESFc3njeBMaN9stVSFhtIp9gan34"),
            RegionEndpoint.EUCentral1), "i-0ec31c9c7be036611");
      if (serverStatus != ServerState.Pending || serverStatus != ServerState.Running || serverStatus != ServerState.Terminated )
      {
        StartInstance(new AmazonEC2Client(new BasicAWSCredentials("AKIAQ4RBTCNEYAUDRZOA", "ZQSYXuXYqx//ESFc3njeBMaN9stVSFhtIp9gan34"), RegionEndpoint.EUCentral1), "i-0ec31c9c7be036611");
        ServerState serverState = ServerState.Undefined;
        while (serverState == ServerState.Pending || serverState == ServerState.Undefined || serverState == ServerState.Stopped)
        {
          Thread.Sleep(500);
          serverState = GetInstanceState(new AmazonEC2Client(
            new BasicAWSCredentials("AKIAQ4RBTCNEYAUDRZOA", "ZQSYXuXYqx//ESFc3njeBMaN9stVSFhtIp9gan34"),
            RegionEndpoint.EUCentral1), "i-0ec31c9c7be036611");
        }
        Console.WriteLine("server State changed to :" + serverState);
        return true;
      }
      else
      {
        Console.WriteLine("Impossible to start Instance, current state is: "+serverStatus);
        return false;
      }
    }
    
    // Main method
    public static void StartInstance(AmazonEC2Client amazonEc2Client, string instanceId)
    {
      Console.WriteLine("Sending server start signal …");
      List<String> instancesIds = new List<string>();
      instancesIds.Add(instanceId);
      StartInstancesRequest startInstancesRequest = new StartInstancesRequest();
      startInstancesRequest.InstanceIds = instancesIds;
      var startInstancesResponse =amazonEc2Client.StartInstancesAsync(startInstancesRequest);
      int responseTime = 0;
      while (startInstancesResponse.IsCompleted != true)
      {
        Thread.Sleep(1);
        responseTime += 1;
      }
      if (startInstancesResponse.IsCompletedSuccessfully)
      {
        Console.WriteLine("Successfully sent server start signal to "+ instanceId +" in " + responseTime + " ms || " + responseTime/1000f + " s");
      }
      if (startInstancesResponse.IsFaulted)
      {
        Console.Error.WriteLine(startInstancesResponse.Exception);
        
      }
    }
    
    public static void StopInstance(AmazonEC2Client amazonEc2Client, string instanceId)
    {
      Console.WriteLine("Sending server stop signal …");
      List<String> instancesIds = new List<string>();
      instancesIds.Add(instanceId);
      StopInstancesRequest startInstancesRequest = new StopInstancesRequest();
      startInstancesRequest.InstanceIds = instancesIds;
      var startInstancesResponse =amazonEc2Client.StopInstancesAsync(startInstancesRequest);
      int responseTime = 0;
      while (startInstancesResponse.IsCompleted != true)
      {
        Thread.Sleep(1);
        responseTime += 1;
      }
      if (startInstancesResponse.IsCompletedSuccessfully)
      {
        Console.WriteLine("Successfully sent a server stop signal to "+ instanceId +" in " + responseTime + " ms || " + responseTime/1000f + " s");
      }
      
      if (startInstancesResponse.IsFaulted)
      {
        Console.Error.WriteLine(startInstancesResponse.Exception);
        
      }
    }

    public static ServerState GetInstanceState(AmazonEC2Client amazonEc2Client, string instanceId)
    {
      var describeInstancesRequest = new DescribeInstanceStatusRequest
      {
        InstanceIds = { instanceId},
        IncludeAllInstances = true
      };
      describeInstancesRequest.InstanceIds.Add(instanceId);
      describeInstancesRequest.IncludeAllInstances = true;
      var response = amazonEc2Client.DescribeInstanceStatusAsync();
      int responseTime = 0;
      while (response.IsCompleted != true)
      {
        Thread.Sleep(1);
        responseTime += 1;
      }
      if (response.Result.InstanceStatuses.Count != 0)
      {
        Console.WriteLine("Successfully received instance state in "+ responseTime + " ms || " + responseTime/1000f + " s \n Response: "+response.Result.InstanceStatuses[0].InstanceState.Name);
        switch (response.Result.InstanceStatuses[0].InstanceState.Code)
        {
          case 0:
            return ServerState.Pending;
          case 16:
            return ServerState.Running;
          case 32:
            return ServerState.ShuttingDown;
          case 48:
            return ServerState.Terminated;
          case 64:
            return ServerState.Stopping;
          case 80:
            return ServerState.Stopped;
        }
      }
      Console.WriteLine("Successfully received instance state in "+ responseTime + " ms || " + responseTime/1000f + " s \n Response: Server is down /AWS fixit*");
      return ServerState.Stopped; 
    }
  }
}