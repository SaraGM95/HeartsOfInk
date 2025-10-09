using UnityEngine;
using UnityEngine.UI;

public class ButtonPlatformOptionsController : MonoBehaviour
{
    private Button button;
    [SerializeField]
    private bool isActiveAndroid;
    [SerializeField]
    private bool isInteractableAndroid;
    [SerializeField]
    private bool isActivePC;
    [SerializeField]
    private bool isInteractablePC;

    void Awake()
    {
        button = GetComponent<Button>();

        if (Application.platform == RuntimePlatform.Android)
        {
            button.gameObject.SetActive(isActiveAndroid);
            button.interactable = isInteractableAndroid;
        }
        else
        {
            button.gameObject.SetActive(isActivePC);
            button.interactable = isInteractablePC;
        }
    }

    void Update()
    {
        
    }
}
