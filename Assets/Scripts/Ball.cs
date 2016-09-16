using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class Ball : NetworkBehaviour
{
    private GameStats _GameStats;

    private float _Speed = 7f;

    [SerializeField]
    private GameObject _Player;


    private Rigidbody2D _Rigid;

    private string _BorderLeft;
    private string _BorderRight;

    void Start()
    {
        _BorderLeft = "BorderLeft";
        _BorderRight = "BorderRight";
        _Rigid = GetComponent<Rigidbody2D>();
        _GameStats = GameObject.Find("GameStats").GetComponent<GameStats>();
        _Player = GameObject.Find("Player_1(Clone)");

        _Rigid = GetComponent<Rigidbody2D>();
        _Speed = 7f;
    }

    float hitFactor(Vector2 ballPos, Vector2 racketPos,
                float racketHeight)
    {
        // ascii art:
        // ||  1 <- at the top of the racket
        // ||
        // ||  0 <- at the middle of the racket
        // ||
        // || -1 <- at the bottom of the racket
        return (ballPos.y - racketPos.y) / racketHeight;
    }

    void GetPlayerHit(Collision2D col, float x)
    {
        if (x > 0.1f)
        {
            float y = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
            Vector2 dir = new Vector2(-1, y).normalized;
            _Rigid.velocity = dir * _Speed;
        }
        else if (x < -0.1f)
        {
            float y = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
            Vector2 dir = new Vector2(1, y).normalized;
            _Rigid.velocity = dir * _Speed;
        }

    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!isServer)
            return;
        if (coll.gameObject.tag == _Player.tag)
            {
                float x = coll.gameObject.transform.position.x;
                GetPlayerHit(coll, x);
                _Speed = _Speed + 0.2f;
                if (Application.isMobilePlatform)
                {
                    //Handheld.Vibrate();
                }
            }
        if (coll.gameObject.tag == _BorderLeft)
        {
            _GameStats.RpcSetScore(true);
        }
        if (coll.gameObject.tag == _BorderRight)
        {
            _GameStats.RpcSetScore(false);
        }

    }
}
        
    

