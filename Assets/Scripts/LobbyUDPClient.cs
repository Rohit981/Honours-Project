﻿using System.Collections;
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

    internal UdpClient udpClient;
    public byte teamID = 0;

    internal bool IsSceneChanged = false;

    private UDPClient udp;

    public Int32[] playersPort = new Int32[4];
    public String[] playersIP = new String[4];

    public Int32 udpPort;
    // Start is called before the first frame update
    void Start()
    {
        udpClient = new UdpClient(0);
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

    public void SendUDPPort()
    {

        udpPort = ((IPEndPoint)udpClient.Client.LocalEndPoint).Port;

        //udpVariables.udpIP = ((IPEndPoint)udpClient.Client.LocalEndPoint).Address.ToString();

        
    }
}
