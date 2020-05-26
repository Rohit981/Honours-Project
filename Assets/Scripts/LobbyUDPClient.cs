using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

public class LobbyUDPClient : MonoBehaviour
{

    private UdpClient udpClient;

    public Int32 udpPort;


    // Start is called before the first frame update
    void Start()
    {
        udpClient = new UdpClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendUDPPort(String serverIP, Int32 Port)
    {
        udpClient.Connect(serverIP, Port);
        udpPort = ((IPEndPoint)udpClient.Client.LocalEndPoint).Port;
    }
}
