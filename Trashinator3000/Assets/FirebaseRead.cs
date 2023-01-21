using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;


public class FirebaseRead : MonoBehaviour
{
    [SerializeField] FirebaseInit firebaseInit;
    bool SetupDefaultFirebase;
    FirebaseFirestore db;
    bool added = false;
    
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
            db = FirebaseFirestore.DefaultInstance;
            SetupDefaultFirebase = true;
        }
        if(SetupDefaultFirebase && added == false)
        {

            DocumentReference docRef = db.Collection("users").Document("37wrA8Es5jjVUtuW75BI");
            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                Debug.Log("task");
                Debug.Log(task);
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Debug.Log(string.Format("Document data for {0} document:", snapshot.Id));
                    Dictionary<string, object> city = snapshot.ToDictionary();
                    foreach (KeyValuePair<string, object> pair in city)
                    {
                        Debug.Log(string.Format("{0}: {1}", pair.Key, pair.Value));
                    }
                }
                else
                {
                    //Debug.Log(string.Format("Document {0} does not exist!", snapshot.Id));
                    Debug.Log("DNE!");
                }
            });

            //            DocumentReference docRef = db.Collection("users").Document("alovelace");
            //            Dictionary<string, object> user = new Dictionary<string, object>
            //{
            //        { "First", "Ada" },
            //        { "Last", "Lovelace" },
            //        { "Born", 1815 },
            //};
            //            docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            //                Debug.Log("Added data to the alovelace document in the users collection.");
            //            });

            added = true;

        }
    }

}
