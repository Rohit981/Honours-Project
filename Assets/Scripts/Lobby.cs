﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class Lobby : NetworkManager
{
    TcpListener server = null;
    String data = null;

    // Buffer for reading data
    Byte[] bytes;

    public List <TcpClient> ClientsConnected = new List<TcpClient>();

    //public List<TCPClient> clients = new List<TCPClient>();

    public List<Int32> ports = new List<Int32>();

    string ClientPort = "Hello";

    Ping ping;

    TcpClient tcpClients;

    public Int32 udpPort;

    public struct PortNumber
    {
        public List<int> ClientUDPports ;
    }

    private PortNumber ClientPortNumber;


    // Start is called before the first frame update
    void Start()
    {
        Int32 port = 5599;

        IPAddress localAdress = IPAddress.Parse("127.0.0.1");

        server = new TcpListener(localAdress, port);

        server.Start();

        print("TCP server Started");
        bytes = new Byte[256];

        server.BeginAcceptTcpClient(OnServerConnect, null);

       ClientPortNumber.ClientUDPports = new List<int>();
            
    }

    // Update is called once per frame
    void Update()
    {
       
        //SendData();
    }

    void Sent()
    {

        //// Get a stream object for reading and writing
        for (int i = 0; i < ClientsConnected.Count; i++)
        {
            NetworkStream stream = ClientsConnected[i].GetStream();
            data = null;

            //byte[] msg = System.Text.Encoding.ASCII.GetBytes(ClientPortNumber.ClientUDPports[i].ToString());

            Byte[] msg = new Byte[Marshal.SizeOf(ClientPortNumber)];
            SerializeStruct<PortNumber>(ClientPortNumber, ref msg, 0);

            //BinaryFormatter bf = new BinaryFormatter();
            // bf.Serialize(stream, ClientPortNumber.ClientUDPports);

            //byte[] msg = ObjectToByteArray(ClientPortNumber.ClientUDPports);

            // Send back a response.            
            stream.Write(msg, 0, msg.Length);
            print("sent :" + ClientPortNumber.ClientUDPports[i].ToString());

        }

    }

    void Recieve()
    {
        NetworkStream stream = tcpClients.GetStream();

        stream.Read(bytes, 0, bytes.Length);

        String data = System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);

        print("Data Recived: " + data);

        udpPort = Int32.Parse(data);

    }

    void OnServerConnect(IAsyncResult ar)
    {

        tcpClients = server.EndAcceptTcpClient(ar);


        ClientsConnected.Add(tcpClients);
        ports.Add(((IPEndPoint)tcpClients.Client.RemoteEndPoint).Port);
        IpAdresses.Add(((IPEndPoint)tcpClients.Client.RemoteEndPoint).Address);

        Recieve();

        ClientPortNumber.ClientUDPports.Add(udpPort);

        if (ports.Count == 1)
        {
            
            Sent();
            print("All clients Connected");
            server.EndAcceptTcpClient(ar);

        }
        else
        {
            server.BeginAcceptTcpClient(OnServerConnect, null);

        }

    }

   

    public static byte[] ObjectToByteArray(List<int> obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }


}
