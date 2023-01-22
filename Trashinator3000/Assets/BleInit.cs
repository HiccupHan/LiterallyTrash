using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BleInit : MonoBehaviour
{
    public string deviceName;
    public string dataToSend;
    private bool IsConnected;
    public static string dataRecived = "";

    [SerializeField] Text displayReceived;
    // Start is called before the first frame update
    void Start()
    {
        // displayReceived.text = "CONNECTED";
        IsConnected = false;
        BluetoothService.CreateBluetoothObject();
        deviceName = "Trashinator3000";
        StartButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsConnected) {
            
            try
            {
               string datain =  BluetoothService.ReadFromBluetooth();
                if (datain.Length > 1)
                {
                    dataRecived = datain;
                    // print(dataRecived);
                    displayReceived.text = dataRecived;
                }

            }
            catch (Exception e)
            {

            }
        }
        else
        {
            StartButton();
        }
        
    }

    public void StartButton()
    {
        if (!IsConnected)
        {
            // print(deviceName);
            IsConnected =  BluetoothService.StartBluetoothConnection(deviceName);
            if(IsConnected)
            {
                // displayReceived.text = "CONNECTED";
            }
            else
            {
                // displayReceived.text = "NOTCONN";
            }
        }
    }

    public void Send(string text)
    {
        if (IsConnected && (text != "" || text != null))
        {
            BluetoothService.WritetoBluetooth(text);
        }
    }


    public void StopButton()
    {
        if (IsConnected)
        {
            BluetoothService.StopBluetoothConnection();
        }
        Application.Quit();
    }
}
