using UnityEngine;
using UnityEngine.UI;


public class WorldToScreenUI : MonoBehaviour
{
    public Text uiText;
    void Update()
    {
        // Vector3 worldPos = Camera.main.ScreenToWorldPoint(uiText.transform.position);
        // transform.position = worldPos;
        Vector3 worldPos = Camera.main.WorldToScreenPoint(transform.position);
        uiText.transform.position = worldPos;
    }
}