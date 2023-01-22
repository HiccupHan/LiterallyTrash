using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.UI;
using Mapbox.Unity.Location;

public class FirebaseRead : MonoBehaviour
{
    [SerializeField] FirebaseInit firebaseInit;
    bool SetupDefaultFirebase;
    FirebaseFirestore db;
    bool added = false;
    string name = "gullu";

    [SerializeField] Text displayReceived;

    [SerializeField] DeviceLocationProvider dlp;
    
    // Start is called before the first frame update
    void Start()
    {
        //check if firebase is initialized
        
    }

    // Update is called once per frame
    void Update()
    {
        if(firebaseInit.FirebaseIsInitialized && !SetupDefaultFirebase)
        {
            displayReceived.text = "setup, not default yet";
            db = FirebaseFirestore.DefaultInstance;
            displayReceived.text = "setup, default";
            SetupDefaultFirebase = true;
        }
        // if(SetupDefaultFirebase && added == false)
        // {

            // DocumentReference docRef = db.Collection("users").Document("37wrA8Es5jjVUtuW75BI");
            // docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            // {
            //     Debug.Log("task");
            //     Debug.Log(task);
            //     DocumentSnapshot snapshot = task.Result;
            //     if (snapshot.Exists)
            //     {
            //         Debug.Log(string.Format("Document data for {0} document:", snapshot.Id));
            //         Dictionary<string, object> city = snapshot.ToDictionary();
            //         foreach (KeyValuePair<string, object> pair in city)
            //         {
            //             Debug.Log(string.Format("{0}: {1}", pair.Key, pair.Value));
            //         }
            //     }
            //     else
            //     {
            //         //Debug.Log(string.Format("Document {0} does not exist!", snapshot.Id));
            //         Debug.Log("DNE!");
            //     }
            // });

                      

        //     added = true;

        // }
    }

    public void AddTrashLoc()
    {
        if(firebaseInit.FirebaseIsInitialized)
        {
            //GeoPoint gp = new GeoPoint(dlp._publicLocation[0], dlp._publicLocation[1]);
            string location = "34.06269849777831, -118.44806257612713";
            DocumentReference docRef = db.Collection("trash_locs").Document();
                        Dictionary<string, string> user = new Dictionary<string, string>
                {
                    { "location", location },
                };

                //docRef.Id for getting the auto gen id

            docRef.SetAsync(user).ContinueWithOnMainThread(task => {
                Debug.Log("Added data to the document in the trash_loc collection.");
            });
        }
    }

}
