using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;


public class TCPClient : NetworkManager
{
    private TcpClient client;
    private string message = "Message Sent";

    private Button button;

    NetworkStream stream;
    Byte[] data;
   

    bool IsReceiveing = false;

    bool IsChangingScene = false;

    public LobbyUDPClient udpRef;

    public GameObject udpConnections;
    public GameObject chatCanvas;

    

    public Int32 udpPort;

    public List<Int32> Recived_UDP_Port = new List<Int32>(); 

    

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponentInChildren<Button>();
        data = new Byte[256];
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsReceiveing == true)
        {
            Recieve();

        }

        if(IsChangingScene == true)
        {
            udpRef.IsSceneChanged = true;
            SceneManager.LoadScene("NetworkingTestScene", LoadSceneMode.Single);
            DontDestroyOnLoad(udpConnections);
            DontDestroyOnLoad(chatCanvas);
        }
    }

    public void Connect()
    {
        string serverIP = "127.0.0.1";
        Int32 Port = 5599;

        client = new TcpClient(serverIP, Port);

        print("TCP client Connected");

        udpRef.SendUDPPort(serverIP, Port);

        IsReceiveing = true;

        Send();

        //Recieve();

        button.enabled = false;
    }

    void Send()
    {
       Byte[] msg = System.Text.Encoding.ASCII.GetBytes(udpRef.udpPort.ToString());

        stream = client.GetStream();

        stream.Write(msg, 0, msg.Length);

        //print("Sent: " + udpPort.ToString());
        //// Send the message to the connected TcpServer.
        //stream.Write(data, 0, data.Length);

        //print( message);
    }

    void Recieve()
    {
        
        stream = client.GetStream();
        // Buffer to store the response bytes.

        stream.BeginRead(data, 0, data.Length, OnRead, null);

        //Int32 bytes_value = stream.Read(data, 0, data.Length);

       

       

        //print("Response: " + msg);

    }

    void OnRead(IAsyncResult ar)
    {
        PortNumber portmsg;
        int msgLength = stream.EndRead(ar);

        portmsg = DeserializeStruct<PortNumber>(data);

        udpRef.playersPort[0] = portmsg.Client1_UDP_port;
        udpRef.playersPort[1] = portmsg.Client2_UDP_port;
        udpRef.playersPort[2] = portmsg.Client3_UDP_port;
        udpRef.playersPort[3] = portmsg.Client4_UDP_port;

        if(udpRef.playersPort.Length == 4)
        {
           IsChangingScene = true;

        }

        //print("Udp Port Recieved" + portmsg.Client1_UDP_port.ToString());


        //ClientPortNumber = DeserializeStruct<PortNumber>(data);

        //print("Response: " + ClientPortNumber.ToString());

        //String responseData = String.Empty;

        //responseData = System.Text.Encoding.ASCII.GetString(data, 0, msgLength);

        ////int responseData = ByteArrayToObject(data);

        //print("Response: " +  responseData.ToString());

        //Recived_UDP_Port.Add(Int32.Parse (responseData));

        stream.BeginRead(data, 0, data.Length, OnRead, null);


        //if(responseData == "Hello")
        //{
        //    IsChangingScene = true;
        //}

      

    }

    public static int ByteArrayToObject(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            int obj =  (int) binForm.Deserialize(memStream);
            return obj;
        }
    }
}
