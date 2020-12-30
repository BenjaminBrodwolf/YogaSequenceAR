using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceManager : MonoBehaviour
{

    private Animator _anim;
    private GameObject _startButtonUI;
    private GameObject _placeOkButtonUI;
    private GameObject _placingButtonUI;
    private GameObject _nextButtonUI;
    private GameObject _backButtonUI;

    public bool isAnimationPlaying;
    private int countSequence;
    void Start()
    {
        isAnimationPlaying = false;
        countSequence = 0;
        
        _anim = GetComponent<Animator>();
        
        _placingButtonUI = GameObject.FindGameObjectsWithTag("PlacingButton")[0];
        
        _placeOkButtonUI = FindInActiveObjectByTag("PlaceOkButton"); 
        _placeOkButtonUI.SetActive(true);
        _placeOkButtonUI.GetComponent<Button>().onClick.AddListener(PlatzierungOk);
        
        _nextButtonUI = FindInActiveObjectByTag("NextButton"); 
        _nextButtonUI.GetComponent<Button>().onClick.AddListener(NextYogaSequence);

        _backButtonUI = FindInActiveObjectByTag("BackButton"); 
        _backButtonUI.GetComponent<Button>().onClick.AddListener(BackYogaSequence);
    }

    void Update()
    {
        Debug.Log(GetCurrentClipName());
        Debug.Log(AnimatorIsPlaying());
    }

    public void PlatzierungOk()
    {
        // Display the Next Button
        _nextButtonUI.SetActive(true);
        _backButtonUI.SetActive(true);

        //Disable Scale and Rotate Scripts
        gameObject.GetComponent<rotateController>().enabled = false;
        gameObject.GetComponent<onClickForScaling>().enabled = false;
        
        //Disable another PlacingButton
        _placeOkButtonUI.SetActive(false);
        _placingButtonUI.SetActive(false);
    }


    public void NextYogaSequence()
    {
        Debug.Log("Next Sequence Button pressed");
        _anim.SetTrigger("nextYogaSequenceTrigger");
        countSequence++;
    }
    
    public void BackYogaSequence()
    {
        Debug.Log("Back Sequence Button pressed");
        _anim.SetTrigger("backYogaSequenceTrigger");
        countSequence--;
    }
    
    private bool AnimatorIsPlaying(){
        return _anim.GetCurrentAnimatorStateInfo(0).length >
               _anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    
    
    
    public string GetCurrentClipName(){
        var clipInfo = _anim.GetCurrentAnimatorClipInfo(0);
        return clipInfo[0].clip.name;
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
