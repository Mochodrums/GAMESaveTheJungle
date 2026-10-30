using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum PlayerID
{
    Player1,
    Player2
}

public class PlayerBehaviour : MonoBehaviour
{

    public Rigidbody2D rb;
    private Vector2 _direction = Vector2.down;
    
    [Header("Movement")]
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private KeyCode _inputdown = KeyCode.S;
    [SerializeField] private KeyCode _inputup = KeyCode.W;
    [SerializeField] private KeyCode _inputright = KeyCode.D;
    [SerializeField] private KeyCode _inputleft = KeyCode.A;
    [SerializeField] private PlayerID _playerID;

    [Header("Sprites")]
    public AnimatedSpriteRender _SpriteRenderUp;
    public AnimatedSpriteRender _SpriteRenderDown;
    public AnimatedSpriteRender _SpriteRenderLeft;
    public AnimatedSpriteRender _SpriteRenderRight; 
    private AnimatedSpriteRender _activeSpriteRender;
    [SerializeField] private AnimatedSpriteRender _spriteRenderDeath;

    [Header("Death Audio")]
    [SerializeField] private AudioClip _player1DeathSound;
    [SerializeField] private AudioClip _player2DeathSound;
    [SerializeField] private float _minPitch = 0.9f;
    [SerializeField] private float _maxPitch = 1.1f;
    [SerializeField] private float _minVolume = 0.7f;
    [SerializeField] private float _maxVolume = 1f;
    
    public PlayerID PlayerID
    {
        get { return _playerID; }
    }
    
 
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _activeSpriteRender = _SpriteRenderDown;
    }

    private void Update()
    {
        if (Input.GetKey(_inputdown))
        {
            SetDirection(Vector2.down, _SpriteRenderDown);
        }
        else if (Input.GetKey(_inputup))
        {
            SetDirection(Vector2.up,  _SpriteRenderUp);
        }
        else if (Input.GetKey(_inputright))
        {
            SetDirection(Vector2.right,  _SpriteRenderRight);
        }
        else if (Input.GetKey(_inputleft))
        {
            SetDirection(Vector2.left,   _SpriteRenderLeft);
        }
        else
        {
            SetDirection(Vector2.zero, _activeSpriteRender);
        }
    }
    
    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        Vector2 translation = _direction * (_speed * Time.fixedDeltaTime);
        
        rb.MovePosition(position + translation);   
    }

    private void SetDirection(Vector2 _newDirection, AnimatedSpriteRender _spriteRender)
    {
        _direction = _newDirection;
        
        _SpriteRenderUp.enabled = _spriteRender == _SpriteRenderUp;
        _SpriteRenderDown.enabled = _spriteRender == _SpriteRenderDown;
        _SpriteRenderLeft.enabled = _spriteRender == _SpriteRenderLeft;
        _SpriteRenderRight.enabled = _spriteRender == _SpriteRenderRight;
        
        _activeSpriteRender = _spriteRender;
        _activeSpriteRender._idle = _direction == Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
        
    }

    private void DeathSequence()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        AudioClip deathSound = _playerID == PlayerID.Player1
            ? _player1DeathSound
            : _player2DeathSound;

        if (audioSource != null && deathSound != null)
        {
            float randomPitch = UnityEngine.Random.Range(_minPitch, _maxPitch);
            float randomVolume = UnityEngine.Random.Range(_minVolume, _maxVolume);

            audioSource.pitch = randomPitch;
            audioSource.PlayOneShot(deathSound, randomVolume);
        }
        
        enabled = false;
        GetComponent<BombBehaviour>().enabled = false;
        
        _SpriteRenderUp.enabled = false;
        _SpriteRenderDown.enabled = false;
        _SpriteRenderLeft.enabled = false;
        _SpriteRenderRight.enabled = false;
        _spriteRenderDeath.enabled = true;
        
        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
        
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        GameManager.Instance.CheckWin();
        
    }
}
