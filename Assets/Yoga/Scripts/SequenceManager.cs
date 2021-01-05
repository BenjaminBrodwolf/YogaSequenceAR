using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private string lastAnimationEventName;
    
    private bool wasForward;
    private string poseName;
    private int countSequence;

    public YogaAdjustment[] yogaAdjustments;

    private Hashtable poseDictionary = new Hashtable(){
        {"Berg", "Bergposition (Tadasana)"},
        {"KopfKnie", "Kopf Knie Position (Uttanasana)"},
        {"Vorbeuge", "Halbe Vorbeuge (Ardha Uttanasana)"},
        {"Brett", "Tiefes Brett (Chaturanga)"},
        {"Liegend", "Liegend"},
        {"Kobra", "Kobra (Bhujangasana)"}
    };
    
    void Start()
    {
        poseName = "Stehen";
        wasForward = true;
        countSequence = 0;

        _anim = GetComponent<Animator>();
        
        _poseNameUI = GameObject.FindGameObjectsWithTag("PoseText")[0];
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
                poseName = wasForward ? poseDictionary[poses[0]] + " -> " + poseDictionary[poses[1]] : poseDictionary[poses[1]] + " -> " + poseDictionary[poses[0]] ;
            }
            else
            {
                poseName = poseDictionary[poses[0]].ToString();
            }
        }
        else
        {
            if (poses.Length > 1)
            {
                poseName = wasForward ? poseDictionary[poses[1]].ToString() : poseDictionary[poses[0]].ToString();
            }
            else
            {
                poseName = poseDictionary[poses[0]].ToString();
            }
        }
        SetPoseName(poseName);

        if (isHintActive)
        {
            setHintPos();
        }
    }

    private void SetPoseName(string pose)
    {
        _poseNameUI.GetComponent<Text>().text = pose;
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
            setHintText();
        }
        else
        {
            _hintPanel.SetActive(false);
            _hintLine.SetActive(false);
        }
    }
    public void YogaPoseEvent(AnimationEvent poseEvent)
    {
        Debug.Log("AnimationEvent neue Pose: " + poseEvent.stringParameter);
        lastAnimationEventName = poseEvent.stringParameter;
        setHintText();
    }

    private void setHintText()
    {
        if (_hintPanel.active)
        {
            Text textElement = _hintPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            textElement.text = getAdjustmentText();
        }
    }
    
    public string getAdjustmentText()
    {
        string adjustText = "";
        
        //currentClip = _anim.GetCurrentAnimatorClipInfo(0)[0].clip;
        var poseNameExist = yogaAdjustments.Any(e => e.YogaPose.Equals(lastAnimationEventName));
        Debug.Log("Pose vorhanden " + poseNameExist);
        if (poseNameExist)
        {
            var y = yogaAdjustments.First(e => e.YogaPose.Equals(lastAnimationEventName));
            adjustText = y.AdjustmentText;
            currentBodyHintFocus = y.BodyHintFocus;
        }
        else
        {
            Debug.LogError("Pose nicht vorhanden");
            adjustText = "Keine Infos vorhanden";
            currentBodyHintFocus = _kopf;
        }
       
        return adjustText;
    }
    
    void OnGUI()
    {
        //Output the current Animation name and length to the screen
        GUI.Label(new Rect(0, 0, 200, 20),  "Event PoseName : " + lastAnimationEventName);
        GUI.Label(new Rect(0, 20, 200, 20),  "Sequence Number : " + countSequence);
        // GUI.Label(new Rect(0, 40, 200, 20),  "Clip Length : " + currentClip.length);
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