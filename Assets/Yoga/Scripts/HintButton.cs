using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintButton : MonoBehaviour
{
    public Sprite iconActive;
    public Sprite iconInactive;
    public Button button;

    private bool isActive;
    
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnClick);
        isActive = false;
    }

    void OnClick(){
        if (isActive)
        {
            isActive = false;
            button.image.sprite = iconInactive;
        }
        else
        {
            button.image.sprite = iconActive;
            isActive = true;
        }
    }
}
