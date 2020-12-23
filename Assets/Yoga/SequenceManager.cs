using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceManager : MonoBehaviour
{
    private Animator anim;
    public Button startButton;

    void Start()
    {
        anim = GetComponent<Animator>();
        Button btn = startButton.GetComponent<Button>();
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
