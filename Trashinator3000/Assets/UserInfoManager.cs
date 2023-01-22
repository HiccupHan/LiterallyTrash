using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Linq;
using Mapbox.Unity.Location;
using SimpleJSON;

public class UserInfoManager : MonoBehaviour
{
    private readonly string FUNCTION_URL = "https://us-central1-trashinator3000.cloudfunctions.net/addTrashLocation?location=";
    public string xp = "0";
    public TMP_Text username;
    public GameObject input;
    public TMP_Text xpDisplay;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void changeUsername() {
        username.text = input.GetComponent<Text>().text;
        GetUserXP(username.text);
    }

IEnumerator OnResponse(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            xp = ParseXP(json);
            xpDisplay.text = "XP: " + xp;
        }
    }
private string ParseXP(string json) {
    int startIndex = json.IndexOf("XP") + 4;
    int endIndex = json.IndexOf("}");
    string XPstring = json.Substring(startIndex, endIndex - startIndex);
    return XPstring;
}

    private void GetUserXP(string username)
    {
        string url = "https://us-central1-trashinator3000.cloudfunctions.net/getOrCreateUserXP?username=" + username;
        UnityWebRequest request = UnityWebRequest.Get(url);
        StartCoroutine(OnResponse(request));
    }
}
