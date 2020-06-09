﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.SceneManagement;
public class NetworkManager : MonoBehaviour
{

   public struct InputStruct
   {
       public byte Jump;
   }

   

    public struct Player
    {
        public float PosX;
        public float PosY;
        public int ScaleX;
        public int Health;

    }

    public struct ClientConnection
    {
        public IPAddress ClientIPAdress;
        public Int32 port;
       
    }

     public struct PortNumber
    {
        //public List<int> ClientUDPports ;

        //UDP port Variables
        public Int32 Client1_UDP_port;
        public Int32 Client2_UDP_port;
        public Int32 Client3_UDP_port;
        public Int32 Client4_UDP_port;

        //Client Team ID
        public int Team1ID;
        public int Team2ID;
        public int Team3ID;
        public int Team4ID;

    }

    
    public List<float> Client_positionX = new List<float>();
    public float Client_positionY;
    public List<int> Client_scaleX = new List<int>();
    public ClientConnection[] players = new ClientConnection[4];

    public List<IPAddress> IpAdresses = new List<IPAddress>();
    


    public void Update()
    {
        
    }

    public void ChangeScene()
    {

        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }



    public void InitializeClientVariables()
    {
        print("Network Manager started");

       

        players[0].port = 5556;
        players[1].port = 5557;
        players[2].port = 5558;
        players[3].port = 5559;

      

    }

    //Generic function to serialize a struct into a byte array
    public static byte[] SerializeStruct<T>(T str)
    {
        int size = Marshal.SizeOf(str);                 //Get the size of the struct in bytes
        byte[] arr = new byte[size];                    //Create an empty byte array to store the data

        System.IntPtr ptr = Marshal.AllocHGlobal(size); //Allocate a block of memory on the heap
        Marshal.StructureToPtr(str, ptr, true);         //Move the data from the struct to the heap memory
        Marshal.Copy(ptr, arr, 0, size);                //Copy the data from the heap to our byte array

        Marshal.FreeHGlobal(ptr);                       //Free the allocated memory

        return arr;
    }

    //Generic function to serialize a struct into a byte array
    public static void SerializeStruct<T>(T str, ref byte[] arr, int startIndex)
    {
        int size = Marshal.SizeOf(str);                 //Get the size of the struct in bytes
                                                        // byte[] arr = new byte[size];                    //Create an empty byte array to store the data

        System.IntPtr ptr = Marshal.AllocHGlobal(size); //Allocate a block of memory on the heap
        Marshal.StructureToPtr(str, ptr, true);         //Move the data from the struct to the heap memory
        Marshal.Copy(ptr, arr, startIndex, size);                //Copy the data from the heap to our byte array

        Marshal.FreeHGlobal(ptr);                      //Free the allocated memory
    }

    //Generic function to serialize a struct into a byte array
    public static T DeserializeStruct<T>(byte[] arr, int startIndex = 0) where T : new()
    {
        T str = new T();                                //Create an empty struct to store our data
        int size = Marshal.SizeOf(str);                 //Get the size of the struct in bytes

        System.IntPtr ptr = Marshal.AllocHGlobal(size); //Allocate a block of memory on the heap

        Marshal.Copy(arr, startIndex, ptr, size);       //Copy the data from the byte array to allocated memory, starting at startIndex

        str = (T)Marshal.PtrToStructure(ptr, typeof(T));//Move the data from the heap memory to the struct

        Marshal.FreeHGlobal(ptr);                       //Free the allocated memory

        return str;
    }
}
