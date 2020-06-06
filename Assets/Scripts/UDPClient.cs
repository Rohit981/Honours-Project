﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

public class UDPClient : NetworkManager
{
    //Client and port variables   
    private Int32 UDP_port;
    private float SendCounter;
    List<InputStruct> inputs;
    InputStruct inputMsg;
    [SerializeField] private Text InputText;
    [SerializeField] private Text RecievedText;
    [SerializeField] private Int32 Port;
    [SerializeField] private Int32 ConnectionPort;

    public Int32 udpPort;

    public LobbyUDPClient lobbyUDP;


    public struct UdpState
    {
        public UdpClient u;
        public IPEndPoint e;
    }

    // Start is called before the first frame update
    void Start()
    {
        lobbyUDP = GetComponent<LobbyUDPClient>();

        SendCounter = 0f;

        //UDP_port = 5557;
        foreach (Int32 c in lobbyUDP.playersPort)
        {
            //Initializing port value and client instance
            //udpClient = new UdpClient(c);

            //Connecting client to the port
            //lobbyUDP.udpClient.Connect("127.0.0.1", lobbyUDP.playersPort[0]);
            //lobbyUDP.udpClient.Connect("127.0.0.1", lobbyUDP.playersPort[1]);
            //lobbyUDP.udpClient.Connect("127.0.0.1", lobbyUDP.playersPort[2]);
            //lobbyUDP.udpClient.Connect("127.0.0.1", lobbyUDP.playersPort[3]);

            byte[] sendBytes = Encoding.ASCII.GetBytes(c.ToString());


            lobbyUDP.udpClient.BeginSend( sendBytes, sendBytes.Length,SendCallback , lobbyUDP.udpClient);

            print("Connected Port" + c.ToString());

            Port = c;


        }


        inputMsg = new InputStruct();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Send information every 3rd frame 
        SendCounter += Time.deltaTime;

        //inputs.Add(inputMsg);

        if(SendCounter >= 0.048)
        {
            //print("Entered Send state");
          
            JumpingSendInput();
            SendCounter = 0;
                      
        }

        Recieve();
    }

   

    
    

    void JumpingSendInput()
    {

        if (Input.GetAxis("Jump1") > 0)
        {

             inputMsg.Jump = 1;

            Byte[] sendBytes = new Byte[1 + Marshal.SizeOf(inputMsg)];
            SerializeStruct<InputStruct>(inputMsg, ref sendBytes, 0);

            sendBytes[0] = 0;

            //InputText.text = "Pressed Input";

            //udpClient.BeginSend(sendBytes, sendBytes.Length, "127.0.0.1", 5557 , SendCallback, null);
            lobbyUDP.udpClient.Send(sendBytes, sendBytes.Length);

        }
    }

    void Recieve()
    {
        IPEndPoint e = new IPEndPoint(IPAddress.Any, 0);



        UdpState s = new UdpState();
        s.e = e;
        s.u = lobbyUDP.udpClient;


        while (lobbyUDP.udpClient.Available > 0)
        {
            byte[] b = lobbyUDP.udpClient.Receive(ref e);

            InputStruct[] msgArray = ParseBytes(b);

            InputText.text = "Recieved Input";
            
            


        }
    }

    static InputStruct[] ParseBytes(byte[] receiveBytes)
    {
        int messageID = receiveBytes[0]; //The first byte is the typoe of message 


        InputStruct[] msgArray = new InputStruct[receiveBytes[1]];   //Second byte is how many messages (for message type 0)

        //ServerTime = DeserializeStruct<int>(receiveBytes, 2);    //3-7 bytes are the timein milliseconds


        
        switch (messageID)
        {
            case 0:
                ////For each message we've been told to receive, unpack it.
                //for (int i = 0; i < msgArray.Length; i++)
                //{
                //    //msgArray[i] = DeserializeStruct<InputStruct>(receiveBytes, 2 + (i * Marshal.SizeOf(typeof(InputStruct))));
                //}
                
                print("Jump Button Recieved");
                break;
            case 1:
                //print("Shooting");

                break;
            case 2:
                break;
            case 3:
                int winnerID;
                int score;
                int time;
                break;
        }
        return msgArray;
    }

    public static void SendCallback(IAsyncResult ar)
    {
        print("DONE");
        UdpClient u = (UdpClient)ar.AsyncState;

    }

    public static void ReceiveCallback(IAsyncResult ar)
    {
        UdpClient u = ((UdpState)(ar.AsyncState)).u;
        IPEndPoint e = ((UdpState)(ar.AsyncState)).e;

        byte[] receiveBytes = u.EndReceive(ar, ref e);
        string receiveString = Encoding.ASCII.GetString(receiveBytes);

       

    }
}
