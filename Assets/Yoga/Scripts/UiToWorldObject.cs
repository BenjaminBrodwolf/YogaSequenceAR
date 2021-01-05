using UnityEngine;
using UnityEngine.UI;

public class UiToWorldObject : MonoBehaviour
{

    public Text uiText;
    void Update()
    {
       // Vector3 worldPos = Camera.main.ScreenToWorldPoint(uiText.transform.position);
       // transform.position = worldPos;
       Vector3 worldPos = Camera.main.WorldToScreenPoint(this.transform.position);
       uiText.transform.position = worldPos;
    }
}
