using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintButton : MonoBehaviour
{
    public Sprite iconActive;
    public Sprite iconInactive;
    public Button button;
    public Button hintContainer;
    public RectTransform hintBackground;
    public Text hintText;
    

    private bool _isActive;
    private bool _isBig;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnClick);
        hintContainer.onClick.AddListener(Zoom);
        _isActive = false;
        _isBig = false;
    }

    void OnClick(){
        if (_isActive)
        {
            _isActive = false;
            button.image.sprite = iconInactive;
        }
        else
        {
            button.image.sprite = iconActive;
            _isActive = true;
        }
    }

    void Zoom()
    {
        Debug.Log("Hint Button Script");
        var layoutElement = hintBackground.GetComponent<LayoutElement>();
        var rectTransform = hintContainer.GetComponent<RectTransform>();
        if (_isBig)
        {
            _isBig = false;
            layoutElement.minWidth = 120;
            layoutElement.minHeight = 50;
            hintText.fontSize = 24;
            rectTransform.anchorMin = new Vector2(0, (float) 0.5);
            rectTransform.anchorMax = new Vector2(0, (float) 0.5);
            rectTransform.anchoredPosition = new Vector2(200, 50);
        }
        else
        {
            _isBig = true;
            layoutElement.minWidth = 900;
            layoutElement.minHeight = 500;
            hintText.fontSize = 90;
            rectTransform.anchorMin = new Vector2((float) 0.5, (float) 0.5);
            rectTransform.anchorMax = new Vector2((float) 0.5, (float) 0.5);
            rectTransform.anchoredPosition = new Vector2(0, 0);
        }
    }
}
