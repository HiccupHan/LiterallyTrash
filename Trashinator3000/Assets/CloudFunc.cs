using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using Mapbox.Unity.Location;

public class CloudFunc : MonoBehaviour {
    private readonly string FUNCTION_URL = "https://us-central1-trashinator3000.cloudfunctions.net/addTrashLocation?location=";

    [SerializeField] DeviceLocationProvider blp;
    private void Start() {
        
    }

    public void AddTrashLoc(){
        string location = blp._publicLocation[0] + ", " + blp._publicLocation[1];
        Debug.Log(location);
        StartCoroutine(AddTrashLocation(location));
    }
    private IEnumerator AddTrashLocation(string location) {

        

        UnityWebRequest request = UnityWebRequest.Get(FUNCTION_URL + location);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError) {
            Debug.Log(request.error);
        } else {
            Debug.Log("Trash location added: " + request.downloadHandler.text);
        }
    }
}