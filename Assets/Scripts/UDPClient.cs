using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;


public class UDPClient : NetworkManager
{
    //Client and port variables
    UdpClient udpClient;
    Int32 UDP_port;

    // Start is called before the first frame update
    void Start()
    {
        //Initializing port value and client instance
        UDP_port = 5557;
        udpClient = new UdpClient();

        //Connecting client to the port
        udpClient.Connect("127.0.0.1", UDP_port);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Send()
    {
        Byte[] sendBytes = new Byte[1 + Marshal.SizeOf(msg)];
    }

    void Recieve()
    {

    }
}
