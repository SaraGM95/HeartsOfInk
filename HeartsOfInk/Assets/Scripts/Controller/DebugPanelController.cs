using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanelController : MonoBehaviour
{
    [SerializeField]
    private GlobalLogicController globalLogic;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private Text txtIsMultiplayer;
    [SerializeField]
    private Text txtCameraPosition;
    [SerializeField]
    private Text textMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        txtIsMultiplayer.text = "Is multiplayer host?: " + globalLogic.IsMultiplayerHost;
        txtCameraPosition.text = "Cam position: " + cameraController.transform.position;
        textMousePosition.text = "Mouse position: " + cameraController.ScreenToWorldPoint();
    }
}
