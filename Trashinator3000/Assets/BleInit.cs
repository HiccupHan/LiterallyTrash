using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mapbox.Examples;

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using Mapbox.Unity.Location;


public class BleInit : MonoBehaviour
{
		[SerializeField]
		TMP_Text _username;

        [SerializeField]
		TMP_Text _xptext;

        [SerializeField] DeviceLocationProvider blp;


    public string deviceName;
    public string dataToSend;
    private bool IsConnected;
    public static string dataRecived = "";

    [SerializeField] SpawnOnMap som;

    [SerializeField] Text displayReceived;
    // Start is called before the first frame update
    void Start()
    {
        displayReceived.text = "NOT CONNECTED";
        IsConnected = false;
        BluetoothService.CreateBluetoothObject();
        deviceName = "ESP32";
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
                    som.addFlower(blp._publicLocation[0] + ", " + blp._publicLocation[1]);
                    AddXP();
                    AddFlowerLoc();


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
                displayReceived.text = "CONNECTED";
            }
            else
            {
                displayReceived.text = "NOT CONNECTED";
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


    private readonly string FUNCTION_URL = "https://us-central1-trashinator3000.cloudfunctions.net/addFlowerLocation?location=";

    public void AddFlowerLoc(){
        string location = blp._publicLocation[0] + ", " + blp._publicLocation[1];
        Debug.Log(location);
        StartCoroutine(AddFlowerLocation(location));
    }
    private IEnumerator AddFlowerLocation(string location) {

        

        UnityWebRequest request = UnityWebRequest.Get(FUNCTION_URL + location);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError) {
            Debug.Log(request.error);
        } else {
            Debug.Log("Trash location added: " + request.downloadHandler.text);
        }
    }



    public void AddXP(){

        StartCoroutine(AddUserXPAsync());
    }
    private IEnumerator AddUserXPAsync() {

        string requesturl = "https://us-central1-trashinator3000.cloudfunctions.net/incrementXP?username=" + _username.text + "&amount=10";

        UnityWebRequest request = UnityWebRequest.Get(requesturl);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError) {
            Debug.Log(request.error);
        } else {
            Debug.Log("xp added");
            _xptext.text = "XP: " + (int.Parse(_xptext.text.Substring(4)) + 10).ToString();
        }
    }


}
