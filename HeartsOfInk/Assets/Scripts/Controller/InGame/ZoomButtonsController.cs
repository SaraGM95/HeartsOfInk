using UnityEngine;
using UnityEngine.UI;

public class ZoomButtonsController : MonoBehaviour
{
    [SerializeField]
    private int AndroidXPosition;
    [SerializeField]
    private int AndroidYPosition;
    [SerializeField]
    private RectTransform ZoomInButton;
    [SerializeField]
    private RectTransform ZoomOutButton;
    [SerializeField]
    private int AndroidZoomButtonSizes;
    [SerializeField]
    private int AndroidInterButtonSpace;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            this.transform.position = new Vector3(AndroidXPosition, AndroidYPosition);
            ZoomInButton.sizeDelta = new Vector2(AndroidZoomButtonSizes, AndroidZoomButtonSizes);
            ZoomInButton.anchoredPosition = new Vector2(ZoomInButton.anchoredPosition.x, AndroidInterButtonSpace);
            ZoomOutButton.sizeDelta = new Vector2(AndroidZoomButtonSizes, AndroidZoomButtonSizes);
        }
    }
}
