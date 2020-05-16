using System.Collections;
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
    private UdpClient udpClient;
    private Int32 UDP_port;
    private float SendCounter;
    List<InputStruct> inputs;
    InputStruct inputMsg;
    [SerializeField] private Text InputText;
    [SerializeField] private Text RecievedText;
    [SerializeField] private Int32 Port;
    [SerializeField] private Int32 ConnectionPort;
   

    public struct UdpState
    {
        public UdpClient u;
        public IPEndPoint e;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeClientVariables();
        SendCounter = 0f;

        //UDP_port = 5557;
     foreach(ClientConnection c in players)
     {
        //Initializing port value and client instance
        udpClient = new UdpClient(c.port);

        //Connecting client to the port
         udpClient.Connect("127.0.0.1", c.port);

        InputText.text = c.port.ToString();

        Port = c.port;


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
            udpClient.Send(sendBytes, sendBytes.Length);

        }
    }

    void Recieve()
    {
        IPEndPoint e = new IPEndPoint(IPAddress.Any, 0);



        UdpState s = new UdpState();
        s.e = e;
        s.u = udpClient;


        while (udpClient.Available > 0)
        {
            byte[] b = udpClient.Receive(ref e);

            InputStruct[] msgArray = ParseBytes(b);
        
            //RecievedText.text = "Recieved Input";

           
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
        Console.WriteLine("DONE");
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
