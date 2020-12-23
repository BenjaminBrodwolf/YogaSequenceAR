using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceManager : MonoBehaviour
{

    private Animator anim;
    private Button startButton;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        Debug.Log(anim);
        GameObject[] gmButton = GameObject.FindGameObjectsWithTag("StartButton");  //startButton.GetComponent<Button>();
        Button btn = gmButton[0].GetComponent<Button>();
        btn.onClick.AddListener(StartButton);

    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void StartButton()
    {
        Debug.Log("Start Button pressed");
        anim.SetBool("isStartYogaSequence", true);
    }
}
