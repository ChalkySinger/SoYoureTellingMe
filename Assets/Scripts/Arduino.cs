using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;
using System.IO.Ports;

public class Arduino : MonoBehaviour
{
    [SerializeField] string portName;
    [SerializeField] int baudRate = 9600;
    SerialPort serialPort;
    Thread serialThread;
    bool isRunning = false;


    [field: SerializeField]

    public string InputText { get; private set; }




    void Start()
    {
        OpenSerial();
        StartThread();
    }



    void OpenSerial()
    {
        Debug.Log("open serial");
        serialPort = new SerialPort(portName,baudRate);
        serialPort.ReadTimeout = 1000;
        serialPort.Open();


    }

    void StartThread()
    {
        Debug.Log("open serial");
        isRunning = true;
        serialThread = new Thread(new ThreadStart(ReadData));
        serialThread.Start();   
    }


    void ReadData()
    {
        while (isRunning)
        {
            try
            {
                InputText = serialPort.ReadLine();
                Debug.Log(InputText);
            }
            catch(System.Exception e)
            {
                Debug.Log("serial port error: " + e);
            }
        }
    }

    public void SendData(string data)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Write(data);
            print(data);
        }
    }

    /*private void OnApplicationQuit()
    {
        isRunning = false;
        if(serialPort != null && serialThread.IsAlive)
        {
            serialThread.Join();
        }
        if(serialPort == null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }*/
}
