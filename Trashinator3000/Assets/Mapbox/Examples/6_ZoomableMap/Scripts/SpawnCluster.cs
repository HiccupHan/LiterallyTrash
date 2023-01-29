namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine.Networking;
	using System.Linq;
	using Mapbox.Unity.Location;
	using SimpleJSON;
	using UnityEngine.UI;
	using TMPro;

	public class SpawnCluster : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		// [SerializeField]
		// [Geocode]

		[SerializeField]
		float _spawnScale = 20f;

		[SerializeField]
		List<GameObject> _markerPrefabs;

		List<GameObject> _spawnedObjects;
		List<string> _locationStrings;
		Vector2d[] _locations;
		void Start()
		{
			_spawnedObjects = new List<GameObject>();
			_locationStrings = new List<string>();
			
			GetClusterLocation();

		}

		private void Update()
		{
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
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
            _locationStrings = ParseLocation(json);
			_locations = new Vector2d[_locationStrings.Count];
			for (int i = 0; i < _locationStrings.Count; i++)
			{
				//Debug.Log(_locationStrings[i]);
				var locationString = _locationStrings[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				//Debug.Log(_locations[i]);
				// Randomly select one of the prefabs
				var randomIndex = Random.Range(0, _markerPrefabs.Count-1); 
				var prefab = _markerPrefabs[randomIndex];
				var instance = Instantiate(prefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				_spawnedObjects.Add(instance);
			}
        }
    }
	private List<string> ParseLocation(string json) {
    	List<string> results = new List<string>();
        int currentIndex = 0;
        while (currentIndex < json.Length - 1)
        {
            int startIndex = json.IndexOf("\"", currentIndex) + 1;
            int endIndex = json.IndexOf("\"", startIndex);
            results.Add(json.Substring(startIndex, endIndex - startIndex));
            currentIndex = endIndex + 1;
        }
        return results;
}

    private void GetClusterLocation()
    {
        string url = "https://us-central1-trashinator3000.cloudfunctions.net/getHotspotLocations";
        UnityWebRequest request = UnityWebRequest.Get(url);
        StartCoroutine(OnResponse(request));
    }
	}
}