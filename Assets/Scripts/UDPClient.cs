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
    private Int32 UDP_port;   
    private float SendCounter;
    List<InputStruct> inputs;
    InputStruct inputMsg;
    [SerializeField] private Text LocalTimeText;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Text InputRecievedTimeText;
    [SerializeField] private Text OtherRecievedTimeText;
    [SerializeField] private Text DifferenceTimeText;

    public Int32 udpPort;

    public LobbyUDPClient lobbyUDP;

    public PlayerMovement[] charachters = new PlayerMovement[4];

    private NetworkManager networkManager;
    
   

    private float Owntime;
    private float OtherInputRecievetime;
    private float LocalInputRecievetime;
    private int frameCounter;
    private float timeLastFrame;    

    PlayerMovement newPlayer1;
    PlayerMovement newPlayer2;
    PlayerMovement newPlayer3;
    PlayerMovement newPlayer4;

    private bool StartRewinding = false;


    public struct UdpState
    {
        public UdpClient u;
        public IPEndPoint e;
    }

    // Start is called before the first frame update
    void Start()
    {
        lobbyUDP = GetComponent<LobbyUDPClient>();

        //ObjectIDText =  mainCanvas.GetComponentInChildren<Text>();
        //JumpInputText =  mainCanvas.GetComponentInChildren<Text>();

        SendCounter = 0f;

        SpawnPlayers();

        inputMsg = new InputStruct();

        networkManager = FindObjectOfType<NetworkManager>();
        

        timeLastFrame = Time.realtimeSinceStartup;

        
    }

    // Update is called once per frame
    void Update()
    {
        
        //Send information every 3rd frame 
        SendCounter += Time.deltaTime;

        Owntime += Time.deltaTime;

        LocalTimeText.text = "Local Time:" + Owntime.ToString();

        if(LocalInputRecievetime > 0 && OtherInputRecievetime > 0)
        {
            float differenceInframeTime = Math.Abs(OtherInputRecievetime - LocalInputRecievetime);
            DifferenceTimeText.text = "Difference:" + differenceInframeTime.ToString();

            newPlayer1.IsRewinding = true;
            newPlayer2.IsRewinding = true;
        }

       

        //inputs.Add(inputMsg);

        if (SendCounter >= 0.048)
        {

            SendInput();
            SendCounter = 0;
                      
        }

        Recieve();
    }



    void SendInput()
    {
        inputMsg.ClientTime = Owntime;

        inputMsg.ObjectID = lobbyUDP.teamID;

        ///Inputs Byte
        inputMsg.Jump = networkManager.input.Jump;
        inputMsg.Move = networkManager.input.Move;
        inputMsg.MoveBackward = networkManager.input.MoveBackward;
        inputMsg.Attack = networkManager.input.Attack;

        Byte[] sendBytes = new Byte[Marshal.SizeOf(inputMsg)];
        SerializeStruct<InputStruct>(inputMsg, ref sendBytes, 0);

        
        SendAll(sendBytes);

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

            InputStruct msg;
            msg =  DeserializeStruct<InputStruct>(b);

            RecievedInput(msg);

        }
    }

    void SpawnPlayers()
    {
        //Initialize Positions for client in X axis
        Client_positionX.Add(-9.11f);
        Client_positionX.Add(9.22f);
        Client_positionX.Add(-34.8f);
        Client_positionX.Add(38.3f);

        //Initialize Position for client in Y axis
        Client_positionY.Add(2.72f);
        Client_positionY.Add(24.8f);

        if(lobbyUDP.teamID == 1 )
        {
            newPlayer1 =  Instantiate<PlayerMovement> (charachters[0], new Vector2(Client_positionX[0], Client_positionY[0]), Quaternion.identity);

            newPlayer2 = Instantiate(charachters[1], new Vector2(Client_positionX[1], Client_positionY[0]), Quaternion.identity);
            newPlayer3 = Instantiate(charachters[2], new Vector2(Client_positionX[2], Client_positionY[1]), Quaternion.identity);
            newPlayer4 = Instantiate(charachters[3], new Vector2(Client_positionX[3], Client_positionY[1]), Quaternion.identity);

        }


        if (lobbyUDP.teamID == 2)
        {
            newPlayer2 = Instantiate<PlayerMovement> (charachters[1], new Vector2(Client_positionX[1], Client_positionY[0]), Quaternion.identity);

            newPlayer1 = Instantiate(charachters[0], new Vector2(Client_positionX[0], Client_positionY[0]), Quaternion.identity);
            newPlayer3 = Instantiate(charachters[2], new Vector2(Client_positionX[2], Client_positionY[1]), Quaternion.identity);
            newPlayer4 = Instantiate(charachters[3], new Vector2(Client_positionX[3], Client_positionY[1]), Quaternion.identity);
        }

        //if(lobbyUDP.teamID == 3)
        //{
        //    newPlayer3 = Instantiate<PlayerMovement>(charachters[2], new Vector2(Client_positionX[2], Client_positionY[1]), Quaternion.identity);
        //    newPlayer3.IsRefMe = true;

        //    newPlayer1 = Instantiate(charachters[0], new Vector2(Client_positionX[0], Client_positionY[0]), Quaternion.identity);
        //    newPlayer2 = Instantiate(charachters[1], new Vector2(Client_positionX[1], Client_positionY[0]), Quaternion.identity);
        //    newPlayer4 = Instantiate(charachters[3], new Vector2(Client_positionX[3], Client_positionY[1]), Quaternion.identity);
        //}

        //if(lobbyUDP.teamID == 4)
        //{
        //    newPlayer4 = Instantiate<PlayerMovement>(charachters[3], new Vector2(Client_positionX[3], Client_positionY[1]), Quaternion.identity);
        //    newPlayer4.IsRefMe = true;

        //    newPlayer1 = Instantiate(charachters[0], new Vector2(Client_positionX[0], Client_positionY[0]), Quaternion.identity);
        //    newPlayer2 = Instantiate(charachters[1], new Vector2(Client_positionX[1], Client_positionY[0]), Quaternion.identity);
        //    newPlayer3 = Instantiate(charachters[2], new Vector2(Client_positionX[2], Client_positionY[1]), Quaternion.identity);
        //}

    }

    void SendAll(Byte[] sendBytes)
    {
        lobbyUDP.udpClient.Send(sendBytes, sendBytes.Length, "127.0.0.1", lobbyUDP.playersPort[0]);
        lobbyUDP.udpClient.Send(sendBytes, sendBytes.Length, "127.0.0.1", lobbyUDP.playersPort[1]);
        //lobbyUDP.udpClient.Send(sendBytes, sendBytes.Length, "127.0.0.1", lobbyUDP.playersPort[2]);
        //lobbyUDP.udpClient.Send(sendBytes, sendBytes.Length, "127.0.0.1", lobbyUDP.playersPort[3]);
    }

    static InputStruct[] ParseBytes(byte[] receiveBytes)
    {
        int messageID = receiveBytes[0]; //The first byte is the typoe of message 


        InputStruct[] msgArray = new InputStruct[receiveBytes[1]];   //Second byte is how many messages (for message type 0)

        //ServerTime = DeserializeStruct<int>(receiveBytes, 2);    //3-7 bytes are the timein milliseconds

        for (int i = 0; i < msgArray.Length; i++)
        {
            msgArray[i] = DeserializeStruct<InputStruct>(receiveBytes, i * Marshal.SizeOf(typeof(InputStruct)));
        }



        print("Jump Button Recieved");

        
        switch (messageID)
        {
            case 0:
                //For each message we've been told to receive, unpack it.
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

    void RecievedInput(InputStruct msg)
    {
        if (msg.Jump == 1)
        {
            if (msg.ObjectID == 1)
            {

                RecievedTime(msg);
                newPlayer1.inputStruct.Jump = 1;

            }
            else
            {
                newPlayer1.inputStruct.Jump = 0;

            }

            if (msg.ObjectID == 2)
            {

                RecievedTime(msg);

                newPlayer2.inputStruct.Jump = 1;

            }

            else
            {
                newPlayer2.inputStruct.Jump = 0;

            }

        }



        if (msg.Move == 1)
        {
            if(msg.ObjectID == 1)
            {
                RecievedTime(msg);

                newPlayer1.inputStruct.Move = 1;
               

            }

            else
            {
                newPlayer1.inputStruct.Move = 0;

            }

            if (msg.ObjectID == 2)
            {
                RecievedTime(msg);

                newPlayer2.inputStruct.Move = 1;
               

            }
            else
            {
                newPlayer2.inputStruct.Move = 0;

            }

        }



        if (msg.MoveBackward == 1)
        {
            if (msg.ObjectID == 1)
            {
                RecievedTime(msg);

                newPlayer1.inputStruct.MoveBackward = 1;

            }
            else
            {
                newPlayer1.inputStruct.MoveBackward = 0;

            }

            if (msg.ObjectID == 2)
            {
                RecievedTime(msg);

                newPlayer2.inputStruct.MoveBackward = 1;

            }
            else
            {
                newPlayer2.inputStruct.MoveBackward = 0;

            }

        }



        if (msg.Attack == 1)
        {
            if (msg.ObjectID == 1)
            {
                RecievedTime(msg);

                newPlayer1.inputStruct.Attack = 1;

            }
            else
            {
                newPlayer1.inputStruct.Attack = 0;

            }

            if (msg.ObjectID == 2)
            {
                RecievedTime(msg);

                newPlayer2.inputStruct.Attack = 1;
            }
            else
            {
                newPlayer2.inputStruct.Attack = 0;

            }

        }

        


    }

    void RecievedTime(InputStruct msg)
    {
        if(msg.ObjectID == lobbyUDP.teamID)
        {
            LocalInputRecievetime = msg.ClientTime;
            InputRecievedTimeText.text = "Recieved Time:" + LocalInputRecievetime.ToString();

        }
        else
        {
            OtherInputRecievetime = msg.ClientTime;
            OtherRecievedTimeText.text = "OtherRecievedTime:" + OtherInputRecievetime.ToString();

            StartRewinding = true;
           

        }
    }

    
}
