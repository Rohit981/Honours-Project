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

    internal bool IsSceneChanged = false;

    private UDPClient udp;

    // Start is called before the first frame update
    void Start()
    {
        udpClient = new UdpClient();
        udp = GetComponent<UDPClient>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsSceneChanged == true)
        {
            udp.enabled = true;
            this.enabled = false;
        }
    }

    public void SendUDPPort(String serverIP, Int32 Port)
    {
        udpClient.Connect(serverIP, Port);
        udpPort = ((IPEndPoint)udpClient.Client.LocalEndPoint).Port;
    }
}
