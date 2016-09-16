using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManagerCustom : NetworkManager
{
    [SerializeField]
    private GameStats _GameStats;
    [SerializeField]
    private GameObject _BallPrefab;
    private int _NetworkPort = 25001;
    [SerializeField]
    private string _IPAdress = "";
    private Transform[] StartPos;

    public void Start()
    {
        
    }
    public void StartupHost()
    {
        Setport();
        _IPAdress = Network.player.ipAddress;
        NetworkManager.singleton.StartHost();
        transform.position = new Vector2(0, 30);
    }

    public void JoinGame()
    {
        if (Application.isMobilePlatform)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;            
        }
        SetIPAdress();
        Setport();
        NetworkManager.singleton.StartClient();
        
    }

    void SetIPAdress()
    {
        _IPAdress = GameObject.Find("InputFieldAdress").transform.FindChild("Text").GetComponent<Text>().text;
        if (_IPAdress == "")
        {
            _IPAdress = "Localhost";
        }
        NetworkManager.singleton.networkAddress = _IPAdress;
    }

    void Setport()
    {
        NetworkManager.singleton.networkPort = _NetworkPort;               
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            StartCoroutine(SetupMenuSceneButtons());
        }
        else
        {
            SetupOtherSceneButtons();
        }

    }

    IEnumerator SetupMenuSceneButtons()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.AddListener(JoinGame);
    }

    void SetupOtherSceneButtons()
    {
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
    }
    
}
