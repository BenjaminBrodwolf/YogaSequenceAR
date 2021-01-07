using UnityEngine;

[System.Serializable]
public class YogaAdjustment
{
    public string YogaPose;
    public GameObject BodyHintFocus1;
    public GameObject BodyHintFocus2;
    public AudioClip YogaPoseSound; 
    [TextArea]
    public string AdjustmentText;
}