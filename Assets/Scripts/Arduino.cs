using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;
using System.IO.Ports;
using System.Runtime.Remoting.Messaging;

public class Arduino : MonoBehaviour
{
    [SerializeField] string portName;
    [SerializeField] int baudRate = 115200; // Higer baud rate due to the Gyro
    SerialPort serialPort;
    Thread serialThread;
    bool isRunning = false;

    // Arduino Inputs 
    string[] inputValues;
    [Header("Arduino Inputs Values")]
    [SerializeField] int potentValue;
    [SerializeField] Vector3 gValues;
    [SerializeField] Vector3 joySValues;
    

    [field: SerializeField]

    public string InputText { get; private set; }




    void Start()
    {
        OpenSerial();
        StartThread();
    }

    private void Update()
    {
        //GetInputs();
        //test
        //inputValues = InputText.Split(',');
        //Debug.Log("item 1: " + inputValues[0] + " 2: " + inputValues[1] + " 3: " + inputValues[2]);
        //Debug.Log("uhh: " + inputValues.Length);
        /*gValues.x = float.Parse(inputValues[0]);
        gValues.y = float.Parse(inputValues[1]);
        gValues.z = float.Parse(inputValues[2]);*/
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

    // Can either be called through code or through the event system
    public void SendData(string data)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Write(data);
            print(data);
        }
    }

    // Needs to be called in update 
    void GetInputs() 
    {
        // Splits the CSV from arudino into list elements
        inputValues = InputText.Split(',');

        // Takes values and applyes them to individual values
        potentValue = int.Parse(inputValues[0]);
        
        gValues.x = float.Parse(inputValues[1]);
        gValues.y = float.Parse(inputValues[2]);
        gValues.z = float.Parse(inputValues[3]);

        // These are the joysticks x and y values 
        joySValues.x = int.Parse(inputValues[4]);
        joySValues.y = int.Parse(inputValues[5]);
        // This is the joystick button value
        joySValues.z = int.Parse(inputValues[6]); 
    }

    private void OnApplicationQuit()
    {
        isRunning = false;
        if (serialPort != null && serialThread.IsAlive)
        {
            serialThread.Join();
        }
        if (serialPort == null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }


    //To send vals to other scripts
    public int GetPotVal()
    {
        return potentValue;
    }

    public Vector3 GetGyroVal()
    {
        return gValues;
    }

    public Vector3 GetJoyVal()
    {
        //TEST CHANGE THE INDEX NUMBERS
        inputValues = InputText.Split(',');
        Vector3 sendVals = new Vector3(float.Parse(inputValues[0]), float.Parse(inputValues[1]), float.Parse(inputValues[2]));
        return sendVals;
    }

    //------------------------------
}
