using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameStats : NetworkBehaviour {

    private Text PlayerLeftText;
    private Text PlayerRightText;
    private GameObject PlayerHost;
    [SerializeField]
    private GameObject _BallPrefab;
    private int ScoreLeftPlayer;
    private int ScoreRightPlayer;

    // Use this for initialization
    void Awake()
    {
        PlayerLeftText = GameObject.Find("PlayerLeftText").GetComponent<Text>();
        PlayerRightText = GameObject.Find("PlayerRightText").GetComponent<Text>();
    }

    void Start()
    {
        Debug.Log("Hi");
        StartCoroutine(Wait());
    }
        

    
    [ClientRpc]
    public void RpcGameStart()
    {
        GameObject _Ball = (GameObject)Instantiate(_BallPrefab, _BallPrefab.transform.position, _BallPrefab.transform.rotation);
        _Ball.GetComponent<Rigidbody2D>().velocity = new Vector2(5, 5);
        NetworkServer.Spawn(_Ball);
    }

    [ClientRpc]
    public void RpcSetScore(bool Left)
    {
        if (Left)
        {
            ScoreLeftPlayer++;
            PlayerLeftText.text = ScoreLeftPlayer.ToString();
        }
        else if (!Left)
        {
            ScoreRightPlayer++;
            PlayerRightText.text = ScoreRightPlayer.ToString();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        RpcGameStart();
    }


}
