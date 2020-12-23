using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    private PlacementIndicator _placementIndicator;
    void Start()
    {
        _placementIndicator = FindObjectOfType<PlacementIndicator>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            GameObject obj = Instantiate(objectToSpawn, 
                _placementIndicator.transform.position,
                _placementIndicator.transform.rotation);
            
            
        }
    }
}
