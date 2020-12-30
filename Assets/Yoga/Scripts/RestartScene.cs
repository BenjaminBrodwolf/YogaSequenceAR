using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartScene : MonoBehaviour
{
    // private void Start()
    // {
    //     gameObject.GetComponent<Button>().onClick.AddListener(RestartSceneFunction);
    // }

    public void RestartSceneFunction()
    {
        Debug.Log("Restarted");
        SceneManager.LoadScene("YogaSequenceARScene");
    }
}