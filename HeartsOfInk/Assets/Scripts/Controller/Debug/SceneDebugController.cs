using Assets.Scripts.Controller.Debug;
using Assets.Scripts.Data.Constants;
using Assets.Scripts.DataAccess;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneDebugController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI lobbyUrl;
    [SerializeField]
    private TextMeshProUGUI ingameUrl;
    [SerializeField]
    private TextMeshProUGUI logsUrl;
    [SerializeField]
    private TextMeshProUGUI mapsPath;
    [SerializeField]
    private TextMeshProUGUI mapsCount;
    [SerializeField]
    private TextMeshProUGUI firstBug;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lobbyUrl.text = ApiConfig.LobbyHOIServerUrl;
        ingameUrl.text = ApiConfig.IngameServerUrl;
        logsUrl.text = ApiConfig.LoggingServerUrl;
        mapsPath.text = MapSpriteDAC.GetMapSpritePath();
        firstBug.text = DebugStaticHolder.FirstBugMessage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
