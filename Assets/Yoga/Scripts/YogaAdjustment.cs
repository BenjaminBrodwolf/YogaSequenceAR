using UnityEngine;

[System.Serializable]
public class YogaAdjustment
{
    public string YogaPose;
    public GameObject BodyHintFocus;
    public AudioClip YogaPoseSound; 
    [TextArea]
    public string AdjustmentText;
}