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

    private Button button;

    NetworkStream stream;
    Byte[] data;
    Byte[] Teamdata;

    [SerializeField] private string sceneName;
   

    bool IsReceiveing = false;

    bool IsChangingScene = false;

    public LobbyUDPClient udpRef;

    public GameObject udpConnections;
    public GameObject chatCanvas;

   
    public Int32 udpPort;

    public List<Int32> Recived_UDP_Port = new List<Int32>();

    [SerializeField] private Text RecieveText;



    // Start is called before the first frame update
    void Start()
    {
        button = GetComponentInChildren<Button>();
        data = new Byte[256];
        Teamdata = new Byte[256];

    }

    // Update is called once per frame
    void Update()
    {
        if(IsReceiveing == true)
        {
            RecieveTeamID();
            RecieveText.text = "Team ID:" + udpRef.teamID;
            Recieve();
        }

        if(IsChangingScene == true)
        {
            udpRef.IsSceneChanged = true;
            client.Close();
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
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

        udpRef.SendUDPPort();

        Send();

        IsReceiveing = true;
        //Recieve();

        button.enabled = false;
    }

    void Send()
    {
       Byte[] msg = System.Text.Encoding.ASCII.GetBytes(udpRef.udpPort.ToString());

        stream = client.GetStream();

        stream.Write(msg, 0, msg.Length);

    }

    void Recieve()
    {
        
        stream = client.GetStream();
        // Buffer to store the response bytes.

        stream.BeginRead(data, 0, data.Length, OnRead, null);

    }

    void RecieveTeamID()
    {
        stream = client.GetStream();
        // Buffer to store the response bytes.

        stream.BeginRead(Teamdata, 0, Teamdata.Length, ReadTeamID, client);
    }

    void ReadTeamID(IAsyncResult ar)
    {
        int msgLength = stream.EndRead(ar);

        String responseData = String.Empty;

        responseData = System.Text.Encoding.ASCII.GetString(Teamdata, 0, msgLength);

        udpRef.teamID = int.Parse(responseData);

        stream.BeginRead(Teamdata, 0, Teamdata.Length, ReadTeamID, client);
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

        udpRef.playersTeamID[0] = portmsg.Team1ID;
        udpRef.playersTeamID[1] = portmsg.Team2ID;
        udpRef.playersTeamID[2] = portmsg.Team3ID;
        udpRef.playersTeamID[3] = portmsg.Team4ID;

        if(udpRef.playersPort.Length == 4)
        {
           IsChangingScene = true;

        }

        stream.BeginRead(data, 0, data.Length, OnRead, null);

    }

  
}
