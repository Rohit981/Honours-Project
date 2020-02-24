using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class NetworkManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
