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
    private GameObject _sayAgainButtonUI;
    private bool isAllowedPlaySoundPoseName;

    private GameObject _kopf;
    private GameObject _handRecht;
    private GameObject _handLinks;
    private GameObject _schulterRechts;

    private bool isHintActive;
    private GameObject _hintPanel;
    private GameObject _hintLine;
    private Text _uiObjectForBodyPart;
    private GameObject _currentBodyHintFocus;

    private string lastAnimationEventName;

    private bool wasForward;
    private string poseName;
    private int countSequence;

    private AudioSource _audioSource;
    private AudioClip _currenYogaPoseSound;
    private string _currenAdjustText;
    public YogaAdjustment[] yogaAdjustments;

    private Hashtable poseDictionary = new Hashtable()
    {
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
        _audioSource = GetComponent<AudioSource>();
        _currenYogaPoseSound = _audioSource.clip;

        _poseNameUI = GameObject.FindGameObjectsWithTag("PoseText")[0];
        _placingButtonUI = GameObject.FindGameObjectsWithTag("PlacingButton")[0];


        _placeOkButtonUI = FindInActiveObjectByTag("PlaceOkButton");
        _placeOkButtonUI = FindInActiveObjectByTag("PlaceOkButton");
        _placeOkButtonUI.SetActive(true);
        _placeOkButtonUI.GetComponent<Button>().onClick.AddListener(PlatzierungOkBtn);

        _nextButtonUI = FindInActiveObjectByTag("NextButton");
        _nextButtonUI.GetComponent<Button>().onClick.AddListener(NextYogaSequenceBtn);

        _backButtonUI = FindInActiveObjectByTag("BackButton");
        _backButtonUI.GetComponent<Button>().onClick.AddListener(BackYogaSequenceBtn);

        _sayAgainButtonUI = FindInActiveObjectByTag("SayAgainButton");
        _sayAgainButtonUI.GetComponent<Button>().onClick.AddListener(PlayAgainSoundBtn);
        isAllowedPlaySoundPoseName = false;


        // hint 
        isHintActive = false;
        _hintButtonUI = FindInActiveObjectByTag("HintButton");
        _hintPanel = FindInActiveObjectByTag("TextHint");
        _hintLine = FindInActiveObjectByTag("HintLine");
        _uiObjectForBodyPart = GameObject.FindGameObjectsWithTag("UiObjectForBodyPart")[0].GetComponent<Text>();

        // body parts
        _kopf = GameObject.FindGameObjectsWithTag("Kopf")[0];
        _currentBodyHintFocus = _kopf; // default for debug
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
                poseName = wasForward
                    ? poseDictionary[poses[0]] + " -> " + poseDictionary[poses[1]]
                    : poseDictionary[poses[1]] + " -> " + poseDictionary[poses[0]];
            }
            else
            {
                poseName = poseDictionary[poses[0]].ToString();
            }

            isAllowedPlaySoundPoseName = true;
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

            SetCurrentYogaSequenceData();        }

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

    public void PlatzierungOkBtn()
    {
        // Display the Buttons
        _nextButtonUI.SetActive(true);
        _backButtonUI.SetActive(true);
        _hintButtonUI.SetActive(true);
        _sayAgainButtonUI.SetActive(true);

        _hintButtonUI.SetActive(true);
        _hintButtonUI.GetComponent<Button>().onClick.AddListener(HintButton);

        //Disable Scale and Rotate Scripts
        gameObject.GetComponent<rotateController>().enabled = false;
        gameObject.GetComponent<onClickForScaling>().enabled = false;

        //Disable PlacingButtons
        _placeOkButtonUI.SetActive(false);
        _placingButtonUI.SetActive(false);
    }


    public void NextYogaSequenceBtn()
    {
        Debug.Log("Next Sequence Button pressed");
        _anim.SetTrigger("nextYogaSequenceTrigger");
        wasForward = true;
        countSequence++;
    }

    public void BackYogaSequenceBtn()
    {
        Debug.Log("Back Sequence Button pressed");
        _anim.SetTrigger("backYogaSequenceTrigger");
        wasForward = false;
        countSequence--;
    }

    public void PlayAgainSoundBtn()
    {
        isAllowedPlaySoundPoseName = true;
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
            // SetCurrentYogaSequenceData();
            Text textElement = _hintPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            textElement.text = _currenAdjustText;
        }
    }

    private void PlaySoundPoseName()
    {
        if (isAllowedPlaySoundPoseName && !_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(_currenYogaPoseSound);
            isAllowedPlaySoundPoseName = false;
        }
    }
    public void SetCurrentYogaSequenceData()
    {
        //currentClip = _anim.GetCurrentAnimatorClipInfo(0)[0].clip;
        var poseNameExist = yogaAdjustments.Any(e => e.YogaPose.Equals(lastAnimationEventName));
        Debug.Log("Pose vorhanden " + poseNameExist);
        if (poseNameExist)
        {
            var y = yogaAdjustments.First(e => e.YogaPose.Equals(lastAnimationEventName));
            _currenAdjustText = y.AdjustmentText;
            _currentBodyHintFocus = y.BodyHintFocus;
            _currenYogaPoseSound = y.YogaPoseSound;
            // AudioSource.PlayClipAtPoint(y.YogaPoseSound, Camera.main.transform.position);
            // _audioSource.Play();
            PlaySoundPoseName();
        }
        else
        {
            Debug.LogError("Pose nicht vorhanden");
            _currenAdjustText = "Keine Infos vorhanden";
            _currentBodyHintFocus = _kopf;
        }
    }
    private bool AnimatorIsPlaying()
    {
        return _anim.GetCurrentAnimatorStateInfo(0).length >
               _anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    void OnGUI()
    {
        //Output the current Animation name and length to the screen
        GUI.Label(new Rect(0, 0, 200, 20), "Event PoseName : " + lastAnimationEventName);
        GUI.Label(new Rect(0, 30, 200, 20), "Sequence Number : " + countSequence);
        GUI.Label(new Rect(0, 15, 200, 20),  "Animation Time : " + _anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    private void setHintPos()
    {
        Vector3 worldPos = Camera.main.WorldToScreenPoint(_currentBodyHintFocus.transform.position);
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