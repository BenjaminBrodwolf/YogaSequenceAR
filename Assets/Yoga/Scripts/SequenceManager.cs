using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceManager : MonoBehaviour
{
    private Animator _anim;
    private GameObject _hintButtonUI;
    private GameObject _placeOkButtonUI;
    private GameObject _placingButtonUI;
    private GameObject _nextButtonUI;
    private GameObject _backButtonUI;
    private GameObject _poseNameUI;

    private GameObject _kopf;
    private GameObject _handRecht;
    private GameObject _handLinks;
    private GameObject _schulterRechts;

    private bool isHintActive;
    private GameObject _hintPanel;
    private GameObject _hintLine;
    private Text _uiObjectForBodyPart;
    private GameObject currentBodyHintFocus;
    
    private bool wasForward;
    public string poseName;
    private int countSequence;

    void Start()
    {
        poseName = "Stehen";
        wasForward = true;
        countSequence = 0;

        _anim = GetComponent<Animator>();
        
        _poseNameUI = GameObject.FindGameObjectsWithTag("PoseText")[0];
        Debug.Log(_poseNameUI);
        _placingButtonUI = GameObject.FindGameObjectsWithTag("PlacingButton")[0];
        
        
        _placeOkButtonUI = FindInActiveObjectByTag("PlaceOkButton"); 
        _placeOkButtonUI = FindInActiveObjectByTag("PlaceOkButton");
        _placeOkButtonUI.SetActive(true);
        _placeOkButtonUI.GetComponent<Button>().onClick.AddListener(PlatzierungOk);

        _nextButtonUI = FindInActiveObjectByTag("NextButton");
        _nextButtonUI.GetComponent<Button>().onClick.AddListener(NextYogaSequence);

        _backButtonUI = FindInActiveObjectByTag("BackButton");
        _backButtonUI.GetComponent<Button>().onClick.AddListener(BackYogaSequence);
        
        
        // hint 
        isHintActive = false;
        _hintButtonUI = FindInActiveObjectByTag("HintButton");
        _hintPanel = FindInActiveObjectByTag("TextHint");
        _hintLine = FindInActiveObjectByTag("HintLine");
        _uiObjectForBodyPart = GameObject.FindGameObjectsWithTag("UiObjectForBodyPart")[0].GetComponent<Text>();
        
        // body parts
        _kopf = GameObject.FindGameObjectsWithTag("Kopf")[0];
        _handLinks = GameObject.FindGameObjectsWithTag("HandLinks")[0];
        currentBodyHintFocus = _kopf; // default for debug

    }

    void Update()
    {
        // Debug.Log(GetCurrentClipName());
        // Debug.Log(AnimatorIsPlaying());
        
        string currentClipName = GetCurrentClipName();
        string[] poses = currentClipName.Split('-');
        if (AnimatorIsPlaying())
        {
            if (poses.Length > 1)
            {
                poseName = wasForward ? poses[0] + " -> " + poses[1] : poses[1] + " -> " + poses[0] ;
            }
            else
            {
                poseName = poses[0];
            }
        }
        else
        {
            if (poses.Length > 1)
            {
                poseName = wasForward ? poses[1] : poses[0];
            }
            else
            {
                poseName = poses[0];
            }
        }
        _poseNameUI.GetComponent<Text>().text = poseName;

        if (isHintActive)
        {
            setHintPos();
        }
    }

    public void PlatzierungOk()
    {
        // Display the Next Button
        _nextButtonUI.SetActive(true);
        _backButtonUI.SetActive(true);
        
        // Display the Hint Button
        _hintButtonUI.SetActive(true);
        _hintButtonUI.GetComponent<Button>().onClick.AddListener(HintButton);
        
        //Disable Scale and Rotate Scripts
        gameObject.GetComponent<rotateController>().enabled = false;
        gameObject.GetComponent<onClickForScaling>().enabled = false;

        //Disable another PlacingButton
        _placeOkButtonUI.SetActive(false);
        _placingButtonUI.SetActive(false);
        
        _hintButtonUI.SetActive(true);
    }


    public void NextYogaSequence()
    {
        Debug.Log("Next Sequence Button pressed");
        _anim.SetTrigger("nextYogaSequenceTrigger");
        wasForward = true;
        countSequence++;
    }

    public void BackYogaSequence()
    {
        Debug.Log("Back Sequence Button pressed");
        _anim.SetTrigger("backYogaSequenceTrigger");
        wasForward = false;
        countSequence--;
    }

    private bool AnimatorIsPlaying()
    {
        return _anim.GetCurrentAnimatorStateInfo(0).length >
               _anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    
    public void HintButton()
    {
        Debug.Log("Show Hint");
        isHintActive = !_hintPanel.gameObject.active;

        if (isHintActive)
        {     
            _hintPanel.SetActive(true);
            _hintLine.SetActive(true);
        }
        else
        {
            _hintPanel.SetActive(false);
            _hintLine.SetActive(false);
        }
    }

    private void setHintPos()
    {
        Vector3 worldPos = Camera.main.WorldToScreenPoint(currentBodyHintFocus.transform.position);
        _uiObjectForBodyPart.transform.position = worldPos;
        _uiObjectForBodyPart.text = _kopf.name;
    }
    

    public string GetCurrentClipName()
    {
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