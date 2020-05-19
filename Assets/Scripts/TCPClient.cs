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



public class TCPClient : NetworkManager
{
    public TcpClient client;
    private string message = "Message Sent";

    private Button button;

    NetworkStream stream;
    Byte[] data;


    bool IsReceiveing = false;

    bool IsChangingScene = false;

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
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }

    public void Connect()
    {
        string serverIP = "127.0.0.1";
        Int32 Port = 5599;

        client = new TcpClient(serverIP, Port);

        print("TCP client Connected");

        IsReceiveing = true;

        //Send();

        button.enabled = false;
    }

    void Send()
    {
       

        //// Send the message to the connected TcpServer.
        //stream.Write(data, 0, data.Length);

        //print( message);
    }

    void Recieve()
    {
        //// Translate the passed message into ASCII and store it as a Byte array.
        //data = System.Text.Encoding.ASCII.GetBytes(message);

        stream = client.GetStream();
        // Buffer to store the response bytes.

        stream.BeginRead(data, 0, data.Length, OnRead,null);

        //// Read the first batch of the TcpServer response bytes.
        //Int32 bytes = stream.Read(data, 0, data.Length);
        
        //Console.WriteLine("Received: {0}", responseData);
    }

    void OnRead(IAsyncResult ar)
    {
        int length = stream.EndRead(ar);

        //// String to store the response ASCII representation.
        String responseData = String.Empty;

        responseData = System.Text.Encoding.ASCII.GetString(data, 0, length);

        print("Response: " +  responseData);

        if(responseData == "Hello")
        {
            IsChangingScene = true;
        }

        //stream.BeginRead(data, 0, data.Length, OnRead, null);
    }
}
