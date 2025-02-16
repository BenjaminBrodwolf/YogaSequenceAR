using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineBetweenObjects : MonoBehaviour
{
    public GameObject one;
    public GameObject two;
    
    private RectTransform object1;
    private RectTransform object2;
    private Image image;
    private RectTransform rectTransform;
    void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        SetObjects();
    }
 
    public void SetObjects()
    {
        object1 = one.GetComponent<RectTransform>();
        object2 = two.GetComponent<RectTransform>();
 
        RectTransform aux;
        if (object1.localPosition.x > object2.localPosition.x)
        {
            aux = object1;
            object1 = object2;
            object2 = aux;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (object1.gameObject.activeSelf && object2.gameObject.activeSelf)
        {
            rectTransform.localPosition = (object1.localPosition + object2.localPosition) / 2;
            Vector3 dif = object2.localPosition - object1.localPosition;
            rectTransform.sizeDelta = new Vector3(dif.magnitude, 10);
            rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
        }
    }
}
