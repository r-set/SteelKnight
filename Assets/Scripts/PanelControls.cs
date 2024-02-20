using UnityEngine;

public class PanelControl : MonoBehaviour
{
    public GameObject panelForTouchDevices;

    void Start()
    {
        if (Application.isMobilePlatform)
        {
            panelForTouchDevices.SetActive(true);
        }
        else
        {
            panelForTouchDevices.SetActive(false);
        }
    }
}