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

    public List<Int32> ports = new List<Int32>();

    TcpClient tcpClients;

    private PortNumber ClientPortNumber;

    private float changeSceneTimer = 0;

    private bool startTimer = false;

    public List<String> IpAdresses = new List<String>();


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

    private void Update()
    {
        if(startTimer == true)
        {
            changeSceneTimer += Time.deltaTime;
        }

        if(changeSceneTimer >= 1f)
        {
            Sent();
            print("All clients Connected");
            server.Stop();

        }
    }

    void Sent()
    {

        //// Get a stream object for reading and writing
        for (int i = 0; i < ClientsConnected.Count; i++)
        {
            NetworkStream stream = ClientsConnected[i].GetStream();
            data = null;

            Byte[] msg = new Byte[Marshal.SizeOf(ClientPortNumber)];
            SerializeStruct<PortNumber>(ClientPortNumber, ref msg, 0);

            // Send back a response.            
            stream.Write(msg, 0, msg.Length);
            print("sent :" + msg.ToString());

        }

    }

    void Recieve()
    {
        NetworkStream stream = tcpClients.GetStream();

        stream.Read(bytes, 0, bytes.Length);

        //LobbyUDP udpMsg;

        String data = System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);

        //udpMsg = DeserializeStruct<LobbyUDP>(bytes);

        //print("Data Recived: " + data);

        if (ClientsConnected.Count == 1)
        {
           ClientPortNumber.Client1_UDP_port = Int32.Parse(data);
           //ClientPortNumber.Client1_UDP_IP = IpAdresses[0];

            //print("IP Recived: " + ClientPortNumber.Client1_UDP_IP);
            print("Port Recived: " + ClientPortNumber.Client1_UDP_port);

        }

        else if(ClientsConnected.Count == 2)
        {
            ClientPortNumber.Client2_UDP_port = Int32.Parse(data);
            //ClientPortNumber.Client2_UDP_IP = IpAdresses[1];


        }

        else if (ClientsConnected.Count == 3)
        {
            ClientPortNumber.Client3_UDP_port = Int32.Parse(data);
            //ClientPortNumber.Client3_UDP_IP = IpAdresses[2];


        }

        else if (ClientsConnected.Count == 4)
        {
            ClientPortNumber.Client4_UDP_port = Int32.Parse(data);
            //ClientPortNumber.Client4_UDP_IP = IpAdresses[3];


        }


    }

    void SendTeamID()
    {
        NetworkStream stream = tcpClients.GetStream();


        if (ClientsConnected.Count == 1)
        {
            Byte[] msg = System.Text.Encoding.ASCII.GetBytes(1.ToString());

            stream.Write(msg, 0, msg.Length);

        }

        else if(ClientsConnected.Count == 2)
        {
            Byte[] msg2 = System.Text.Encoding.ASCII.GetBytes(2.ToString());

            stream.Write(msg2, 0, msg2.Length);
        }

        else if (ClientsConnected.Count == 3)
        {
            Byte[] msg3 = System.Text.Encoding.ASCII.GetBytes(3.ToString());

            stream.Write(msg3, 0, msg3.Length);
        }

        else if (ClientsConnected.Count == 4)
        {
            Byte[] msg4 = System.Text.Encoding.ASCII.GetBytes(4.ToString());

            stream.Write(msg4, 0, msg4.Length);
        }
    }

    void OnServerConnect(IAsyncResult ar)
    {

        tcpClients = server.EndAcceptTcpClient(ar);


        ClientsConnected.Add(tcpClients);
        IpAdresses.Add(((IPEndPoint)tcpClients.Client.RemoteEndPoint).Address.ToString());

        Recieve();

        SendTeamID();

        if (ClientsConnected.Count == 2)
        {

            startTimer = true;

        }
        else
        {
            server.BeginAcceptTcpClient(OnServerConnect, null);

        }

    }


}
