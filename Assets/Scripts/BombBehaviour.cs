using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombBehaviour : MonoBehaviour
{
    [Header("Bomb")]
    [SerializeField] GameObject _bombprefab;
    [SerializeField] KeyCode _keybomb = KeyCode.Space;
    [SerializeField] float _bombtime = 3f;
    public int _bombsAmount = 1;
    private int _bombsRemaining;
    [SerializeField] private PlayerBehaviour _player;
    
    [Header("Explosion")]
    public Explosion _explosionPrefab;
    
    [Header("Bomb Audio")]
    [SerializeField] private AudioClip _bombSound;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private float _soundVolume = 1f;

    [SerializeField] private LayerMask _explosionLayerMask;
    [SerializeField] private float _explosionDuration = 1f;
    public int _explosionRadius = 1;

    [Header("Destructible")] 
    [SerializeField] private Tilemap _destructibleTiles;
    [SerializeField] private Destructible _destructiblePrefab;
    
    
    

    private void OnEnable()
    {
        _bombsRemaining = _bombsAmount;
    }

    private void Update()
    {
        if (_bombsRemaining > 0 && Input.GetKeyDown(_keybomb))
        {
            StartCoroutine(_placebomb());
        }
        
    }

    private IEnumerator _placebomb()
    {
        Vector2 _position = transform.position;
        _position.x = Mathf.Round(_position.x);
        _position.y = Mathf.Round(_position.y);

        GameObject bomb = Instantiate(_bombprefab, _position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(_bombSound, _position, _soundVolume);
        _bombsRemaining--;

        yield return new WaitForSeconds(_bombtime);
        
        _position = bomb.transform.position;
        _position.x = Mathf.Round(_position.x);
        _position.y = Mathf.Round(_position.y);
        
        AudioSource.PlayClipAtPoint(_explosionSound, _position, _soundVolume);
        Explosion explosion = Instantiate(_explosionPrefab, _position, Quaternion.identity);
        explosion.SetActiveRender(explosion._start);
        Destroy(explosion.gameObject, _explosionDuration);

        Explode(_position, Vector2.up, _explosionRadius);
        Explode(_position, Vector2.down, _explosionRadius);
        Explode(_position, Vector2.right, _explosionRadius);
        Explode(_position, Vector2.left, _explosionRadius);
        
        Destroy(bomb);
        _bombsRemaining++;
         
    }
    //Explode Sprite behaviour
    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0)
        {
            return;
        }
        
        position += direction.normalized;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, _explosionLayerMask))
        {
            ClearDestructibles(position);
            return;
        }
        
        Explosion explosion = Instantiate(_explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRender(length > 1 ? explosion._middle : explosion._end);
        explosion.SetDirection(direction);
        Destroy(explosion.gameObject, _explosionDuration);
        
        Explode(position, direction, length - 1);
    }

    private void ClearDestructibles(Vector2 position)
    {
        Vector3Int cell = _destructibleTiles.WorldToCell(position);
        TileBase tile = _destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(_destructiblePrefab, position, Quaternion.identity);
            _destructibleTiles.SetTile(cell, null);

            GameManager.Instance.AddScore(_player.PlayerID, 10);
        }
    }

    public void AddBomb()
    {
        _bombsAmount++;
        _bombsRemaining++;
    }
    
    // Exit Triger bomb    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            other.isTrigger = false;
        }
    }
}
