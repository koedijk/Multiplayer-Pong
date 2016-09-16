using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    private GameStats _Stats;
    [SerializeField]
    private GameObject _BallPrefab;
    private float Vertical;
    private Vector2 TouchPos;
    private float Speed = 5f;
    private float beginPosx;
    private Vector2 _Pos;
    private int playerCount;
    private Transform StartPos;
    private GameObject[] _PlayerPos;

    void Awake()
    {
        Speed = Speed * Time.deltaTime;        
    }

    void Start()
    {
        _PlayerPos = GameObject.FindGameObjectsWithTag("StartPosition");
        if (isServer)
        {
            transform.position = _PlayerPos[0].transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y,0);
        }
        else if (!isServer)
        {
            transform.position = _PlayerPos[1].transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        beginPosx = transform.position.x;
        TouchPos = new Vector2(beginPosx, 0);
        _Pos = new Vector2(beginPosx, 0);
       
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Application.isMobilePlatform)
        {
            if (Input.GetMouseButton(0))
            {
                _Pos = new Vector2(beginPosx, transform.position.y);
                TouchPos = new Vector2(beginPosx, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                transform.position = Vector3.MoveTowards(_Pos, TouchPos, Speed);
            }
        }

        Vertical = Input.GetAxis("Vertical") * Speed;
        transform.Translate(0, Vertical, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdSpawnBall(); 
        }



    }
    [Command]
    void CmdSpawnBall()
    {
        GameObject _Ball = (GameObject)Instantiate(_BallPrefab, _BallPrefab.transform.position, _BallPrefab.transform.rotation);
        _Ball.GetComponent<Rigidbody2D>().velocity = new Vector2(5, 5);
        NetworkServer.Spawn(_Ball);
    }
    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}