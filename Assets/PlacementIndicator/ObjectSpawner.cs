using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    private static GameObject instance;

    public GameObject objectToSpawn;
    private PlacementIndicator _placementIndicator;


    void Start()
    {
        _placementIndicator = FindObjectOfType<PlacementIndicator>();
    }

    // void Update()
    // {
    //     if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
    //     {
    //         if (instance == null)
    //         {
    //             instance = Instantiate(objectToSpawn,
    //                 _placementIndicator.transform.position,
    //                 _placementIndicator.transform.rotation);
    //         }
    //     }
    // }

    public void PlaceObject()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        
        instance = Instantiate(objectToSpawn,
            _placementIndicator.transform.position,
            _placementIndicator.transform.rotation);
    }
}