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

    public LobbyUDPClient udp;

    

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
            //Recieve();

        }

        if(IsChangingScene == true)
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }

    public void Connect()
    {
        string serverIP = "127.0.0.1";
        Int32 Port = 5599;

        client = new TcpClient(serverIP, Port);

        print("TCP client Connected");

        udp.SendUDPPort(serverIP, Port);

        IsReceiveing = true;

        Send();

        Recieve();

        button.enabled = false;
    }

    void Send()
    {
       Byte[] msg = System.Text.Encoding.ASCII.GetBytes(udp.udpPort.ToString());

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
       int msgLength = stream.EndRead(ar);

        //ClientPortNumber = DeserializeStruct<PortNumber>(data);

        //print("Response: " + ClientPortNumber.ToString());

        String responseData = String.Empty;

        responseData = System.Text.Encoding.ASCII.GetString(data, 0, msgLength);

        //int responseData = ByteArrayToObject(data);

        print("Response: " +  responseData.ToString());

        Recived_UDP_Port.Add(Int32.Parse (responseData));

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
