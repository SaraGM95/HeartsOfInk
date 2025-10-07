using UnityEngine;

public class DebugButtonController : MonoBehaviour
{
    public GameObject debugButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        debugButton.SetActive(Debug.isDebugBuild);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
