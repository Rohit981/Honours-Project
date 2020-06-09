using System.Collections;
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

    string ClientPortMessage = "Hello";

    Ping ping;

    TcpClient tcpClients;

    public Int32 udpPort;
    public Int32 udpPort2;

   

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

       //ClientPortNumber.ClientUDPports = new List<int>();
            
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

            //int[] msgArray = ClientPortNumber.ClientUDPports.ToArray();

            //byte[] msg = new byte[msgArray.Length * 4];
            //Buffer.BlockCopy(msgArray, 0, msg, 0, msg.Length);

            //byte[] msg = ObjectToByteArray(ClientPortNumber.ClientUDPports);

            // Send back a response.            
            stream.Write(msg, 0, msg.Length);
            print("sent :" + msg.ToString());

        }

    }

    void Recieve()
    {
        NetworkStream stream = tcpClients.GetStream();

        stream.Read(bytes, 0, bytes.Length);

        String data = System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);

        print("Data Recived: " + data);

       //The code needs to be changed for recieving the clients port

        if (ClientsConnected.Count == 1)
        {
           ClientPortNumber.Client1_UDP_port = Int32.Parse(data);
            ClientPortNumber.Team1ID = 1;
        }

        else if(ClientsConnected.Count == 2)
        {
            ClientPortNumber.Client2_UDP_port = Int32.Parse(data);
            ClientPortNumber.Team2ID = 2;

        }

        else if (ClientsConnected.Count == 3)
        {
            ClientPortNumber.Client3_UDP_port = Int32.Parse(data);
            ClientPortNumber.Team3ID = 3;


        }

        else if (ClientsConnected.Count == 4)
        {
            ClientPortNumber.Client4_UDP_port = Int32.Parse(data);
            ClientPortNumber.Team4ID = 4;


        }


    }

    void OnServerConnect(IAsyncResult ar)
    {

        tcpClients = server.EndAcceptTcpClient(ar);


        ClientsConnected.Add(tcpClients);
        ports.Add(((IPEndPoint)tcpClients.Client.RemoteEndPoint).Port);
        IpAdresses.Add(((IPEndPoint)tcpClients.Client.RemoteEndPoint).Address);

        Recieve();

        //ClientPortNumber.ClientUDPports.Add(udpPort);

        if (ports.Count == 4)
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
