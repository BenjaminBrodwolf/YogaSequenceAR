using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceManager : MonoBehaviour
{

    private Animator _anim;
    private GameObject _startButtonUI;
    private GameObject _hintButtonUI;
    private GameObject _placeOkButtonUI;
    private GameObject _placingButtonUI;
    
    void Start()
    {
        _anim = GetComponent<Animator>();
        
        _placingButtonUI = GameObject.FindGameObjectsWithTag("PlacingButton")[0];

        _startButtonUI = FindInActiveObjectByTag("StartButton"); 
        
        _hintButtonUI = FindInActiveObjectByTag("HintButton"); 
        
        _placeOkButtonUI = FindInActiveObjectByTag("PlaceOkButton"); 
        _placeOkButtonUI.SetActive(true);
        _placeOkButtonUI.GetComponent<Button>().onClick.AddListener(PlatzierungOk);
    }

    void Update()
    {
 
    }

    public void PlatzierungOk()
    {
        // Display the Start Button
        _startButtonUI.SetActive(true);
        _startButtonUI.GetComponent<Button>().onClick.AddListener(StartButton);
        
        // Display the Hint Button
        _hintButtonUI.SetActive(true);
        _hintButtonUI.GetComponent<Button>().onClick.AddListener(HintButton);
        
        //Disable Scale and Rotate Scripts
        gameObject.GetComponent<rotateController>().enabled = false;
        gameObject.GetComponent<onClickForScaling>().enabled = false;
        
        //Disable another PlacingButton
        _placeOkButtonUI.SetActive(false);
        _placingButtonUI.SetActive(false);
    }
    public void StartButton()
    {
        Debug.Log("Start Button pressed");
        _anim.SetBool("isStartYogaSequence", true);
        _hintButtonUI.SetActive(true);
    }
    
    public void HintButton()
    {
        Debug.Log("Show Hint");
    }
    
    GameObject FindInActiveObjectByTag(string tag)
    {

        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].CompareTag(tag))
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
