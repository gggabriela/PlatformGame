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
    [SerializeField]private SpriteRenderer _Renderer;

    [Header("Effects")]
    [SerializeField]private GameObject _ScoreParticles;

    [Header("Sounds")]
    [SerializeField]private AudioSource _GameOverSound;
    [SerializeField]private AudioSource _JumpSound;
    [SerializeField]private AudioSource _TouchGroundSound;
    [SerializeField]private AudioSource _ScoreSound;

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

    private bool _InputLeft;
    private bool _InputRight;
    private Vector2 _JumpDir;

    void Start()
    {
        UpdateScoreLabel();
    }

    private void Update()
    {
        if (!_GameOver)
        {
            _InputLeft = Input.GetKey(KeyCode.LeftArrow);
            _InputRight = Input.GetKey(KeyCode.RightArrow);
        
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Rigidbody.AddForce(_JumpDir * _JumpForce, ForceMode2D.Impulse);
                _JumpSound.Play();
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

    private void FixedUpdate()
    {
        
        if (_GameOver)
        {
            _ReplayLabel.SetActive(Time.time % 0.5f < 0.4f);
            transform.localScale = Vector3.Lerp(transform.localScale, _GameOverScale, Time.deltaTime * _GameOverScaleSpeed);
        } else
        {
            
            float torque = 0.0f;

            _JumpDir = Vector2.down;
            if (_InputRight)
            {
                torque = -_MoveForce;
                _JumpDir += Vector2.right;
            }
            if (_InputLeft)
            {
                torque = _MoveForce;
                _JumpDir += Vector2.left;
            }

            Rigidbody.AddTorque(torque);

            Rigidbody.angularVelocity = Mathf.Clamp(Rigidbody.angularVelocity, -25.0f, 25.0f);

            if (transform.position.y < -10.0f)
            {
                GameOver();
            }
        }
    }


    public void AddScore()
    {
        _Score++;
        _ScoreSound.Play();
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
        _Renderer.sortingOrder = 3;
        _GameOverSound.Play();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Score":
                AddScore();
                var particles = Instantiate(_ScoreParticles);
                particles.transform.position = collision.gameObject.transform.position;
                Destroy(collision.gameObject);
                break;
                
            case "IronNail":
                GameOver();
                break;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            _TouchGroundSound.volume = Rigidbody.velocity.sqrMagnitude * 0.03f;
            _TouchGroundSound.Play();
        }
    }
}
