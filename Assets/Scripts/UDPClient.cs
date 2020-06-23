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
    [SerializeField] private Text InputText;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Text ObjectIDText;
    [SerializeField] private Text JumpInputText;

    public Int32 udpPort;

    public LobbyUDPClient lobbyUDP;

    public PlayerMovement[] charachters = new PlayerMovement[2];

    private NetworkManager networkManager;
    private InputManager inputManager;

    PlayerMovement newPlayer3;
    PlayerMovement newPlayer4;
   

    public struct UdpState
    {
        public UdpClient u;
        public IPEndPoint e;
    }

    // Start is called before the first frame update
    void Start()
    {
        lobbyUDP = GetComponent<LobbyUDPClient>();

        ObjectIDText =  mainCanvas.GetComponentInChildren<Text>();
        JumpInputText =  mainCanvas.GetComponentInChildren<Text>();

        SendCounter = 0f;

        SpawnPlayers();

        inputMsg = new InputStruct();

        networkManager = FindObjectOfType<NetworkManager>();
        inputManager = FindObjectOfType<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Send information every 3rd frame 
        SendCounter += Time.deltaTime;

        //inputs.Add(inputMsg);

        if(SendCounter >= 0.048)
        {

            SendInput();
            SendCounter = 0;
                      
        }

        Recieve();
    }



    void SendInput()
    {
        //if (Input.GetAxis("Jump") > 0)
        //{
        //inputMsg.Jump = 1;
        inputMsg.ObjectID = lobbyUDP.teamID;

        inputMsg.Jump = networkManager.input.Jump;
        inputMsg.Move = networkManager.input.Move;
        inputMsg.Attack = networkManager.input.Attack;

        Byte[] sendBytes = new Byte[Marshal.SizeOf(inputMsg)];
        SerializeStruct<InputStruct>(inputMsg, ref sendBytes, 0);

        //sendBytes[0] = 0;

        SendAll(sendBytes);

        //}

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

            //InputStruct[] msgArray = ParseBytes(b);

            InputStruct msg;
            msg =  DeserializeStruct<InputStruct>(b);

            InputText.text = "Recieved Input: " + msg.Jump;

            //RecievedObjectID(msg);

            RecievedInput(msg);
        }
    }

    void SpawnPlayers()
    {
        //Initialize Positions for client in X axis
        Client_positionX.Add(-9.11f);
        Client_positionX.Add(9.22f);

        //Initialize Position for client in Y axis
        Client_positionY = 23.3f;

        if(lobbyUDP.teamID == 1 )
        {
            PlayerMovement newPlayer =  Instantiate<PlayerMovement> (charachters[0], new Vector2(Client_positionX[0], Client_positionY), Quaternion.identity);
            newPlayer.IsRefMe = true;

            Instantiate(charachters[1], new Vector2(Client_positionX[1], Client_positionY), Quaternion.identity);

        }


        if (lobbyUDP.teamID == 2)
        {
            PlayerMovement newPlayer2 = Instantiate<PlayerMovement> (charachters[1], new Vector2(Client_positionX[1], Client_positionY), Quaternion.identity);
            newPlayer2.IsRefMe = true;

           Instantiate(charachters[0], new Vector2(Client_positionX[0], Client_positionY), Quaternion.identity);
        }

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
            inputManager.IsJumpPressed = true;

        }

        else
        {
            inputManager.IsJumpPressed = false;

        }

        if (msg.Move == 1)
        {
            inputManager.IsForwardPressed = true;

        }

        else
        {
            inputManager.IsForwardPressed = false;

        }

        if (msg.Move == -1)
        {
            inputManager.IsBackPressed = true;

        }

        else
        {
            inputManager.IsBackPressed = false;

        }

        if (msg.Attack == 1)
        {
            inputManager.IsAttackPressed = true;

        }

        else
        {
            inputManager.IsAttackPressed = false;

        }


    }

    void RecievedObjectID(InputStruct msg)
    {
        if(msg.ObjectID == 1)
        {
            newPlayer4.IsRefMe = true;
        }

        if (msg.ObjectID == 2)
        {
            newPlayer3.IsRefMe = true;
        }
    }
}
