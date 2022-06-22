using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    
    [SerializeField]private GameObject _ReplayLabel;
    [SerializeField]private Text _ScoreLabel;

    [SerializeField]private float _JumpForce;
    [SerializeField]private float _MoveForce;

    [SerializeField]private Vector3 _GameOverScale;
    [SerializeField]private float _GameOverScaleSpeed;
    [SerializeField]private Collider2D _Collider;


    private Rigidbody2D _Rigidbody;
    public Rigidbody2D Rigidbody
    {
        get
        {
            if (_Rigidbody == null)
            {
                _Rigidbody = GetComponent<Rigidbody2D>();
            }
            return _Rigidbody;
        }
    }

    private int _Score = 0;
    private bool _GameOver;


    void Start()
    {
        UpdateScoreLabel();
    }

    void Update()
    {
        if (_GameOver)
        {
            _ReplayLabel.SetActive(Time.time % 0.5f < 0.4f);
            transform.localScale = Vector3.Lerp(transform.localScale, _GameOverScale, Time.deltaTime * _GameOverScaleSpeed);
        } else
        {
            
            float torque = 0.0f;

            var jumpDir = Vector2.down;

            if (Input.GetKey((KeyCode.D)))
            {
                torque = -_MoveForce;
                jumpDir += Vector2.right;
            }
            if (Input.GetKey((KeyCode.A)))
            {
                torque = _MoveForce;
                jumpDir += Vector2.left;
            }

            Rigidbody.AddTorque(torque);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Rigidbody.AddForce(jumpDir * _JumpForce, ForceMode2D.Impulse);
            }

            Rigidbody.angularVelocity = Mathf.Clamp(Rigidbody.angularVelocity, -5.0f, 5.0f);

            if (transform.position.y < -10.0f)
            {
                GameOver();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    public void AddScore()
    {
        _Score++;
        UpdateScoreLabel();
    }

    public void UpdateScoreLabel()
    {
        _ScoreLabel.text = _Score.ToString();
    }

    public void GameOver()
    {
        _GameOver = true;
        _Collider.enabled = false;
        Rigidbody.gravityScale = 0.2f;
        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0.0f);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Score":
                AddScore();
                Destroy(collision.gameObject);
                break;
                
            case "IronNail":
                GameOver();
                break;
               
            case "Ground":
                break;
                
        }

    }
}
