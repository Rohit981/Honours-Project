using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

public class Lobby : NetworkManager
{
    TcpListener server = null;
    String data = null;

    // Buffer for reading data
    Byte[] bytes;

    public List <TcpClient> ClientsConnected = new List<TcpClient>();

    //public List<TCPClient> clients = new List<TCPClient>();

    public List<Int32> ports = new List<Int32>();
    public List<IPAddress> IpAdresses = new List<IPAddress>();

    string ClientPort = "Hello";

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
            
    }

    // Update is called once per frame
    void Update()
    {
       
        //SendData();
    }

    void Sent()
    {
        //for(int i = 0; i < 4; i++)
        //{

        //    if (tcpClients.Available > 0)
        //    {
        //        print("Connected!");
        //        ports[i] = ((IPEndPoint)tcpClients.Client.RemoteEndPoint).Port;

        //    }

        //}

        //ClientPort = IpAdresses[0].ToString();
        //print(ClientPort);

        //// Get a stream object for reading and writing
        for (int i = 0; i < 4; i++)
        {
            NetworkStream stream = ClientsConnected[i].GetStream();
            data = null;


            byte[] msg = System.Text.Encoding.ASCII.GetBytes(ClientPort);

            // Send back a response.
            //stream.BeginWrite(msg, 0, msg.Length, SendCallBack, null);
            stream.Write(msg, 0, msg.Length);
            print("sent");


        }

        // Shutdown and end connection
        //client.Close();

    }

    void OnServerConnect(IAsyncResult ar)
    {

        TcpClient tcpClients = server.EndAcceptTcpClient(ar);


        ClientsConnected.Add(tcpClients);
        ports.Add(((IPEndPoint)tcpClients.Client.RemoteEndPoint).Port);
        IpAdresses.Add(((IPEndPoint)tcpClients.Client.RemoteEndPoint).Address);


        if (ports.Count == 4)
        {
            
            print("All clients Connected");
             Sent();
            server.EndAcceptTcpClient(ar);

        }
        else
        {
            server.BeginAcceptTcpClient(OnServerConnect, null);

        }

    }

    void SendCallBack(IAsyncResult ar)
    {
        for(int i= 0; i< 4; i++)
        {
            ClientsConnected[i] = (TcpClient)ar.AsyncState;  

        }
    }
}
