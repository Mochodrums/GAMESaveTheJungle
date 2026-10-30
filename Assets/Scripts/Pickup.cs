using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pickup : MonoBehaviour
{ 
      [Header("Pickup Audio")]
      [SerializeField] private AudioClip _extrabombSound;
      [SerializeField] private AudioClip _blastSound;
      [SerializeField] private float _minPitch = 0.9f;
      [SerializeField] private float _maxPitch = 1.1f;
      [SerializeField] private float _minVolume = 0.7f;
      [SerializeField] private float _maxVolume = 1f;
      
      private void PlayPickupSound(AudioSource audioSource)
      {
            AudioClip sound = _pickupType == ItemType.ExtraBomb
                  ? _extrabombSound
                  : _blastSound;

            float randomPitch = Random.Range(_minPitch, _maxPitch);
            float randomVolume = Random.Range(_minVolume, _maxVolume);

            audioSource.pitch = randomPitch;
            audioSource.PlayOneShot(sound, randomVolume);
      }
      
      public enum ItemType
      {
            ExtraBomb,
            BlastRadius,
      }
      
      [SerializeField] private ItemType _pickupType;

      private void OnItemPickup(GameObject _player)
      {
            switch (_pickupType)
            {
                  case ItemType.BlastRadius:
                        _player.GetComponent<BombBehaviour>()._explosionRadius++;
                        break;
                  case ItemType.ExtraBomb:
                        _player.GetComponent<BombBehaviour>().AddBomb();
                        break;
            }
      }
      private void OnTriggerEnter2D(Collider2D _other)
      {
            if (_other.CompareTag("Player"))
            {
                  OnItemPickup(_other.gameObject);

                  PlayerBehaviour player = _other.GetComponent<PlayerBehaviour>();
                  AudioSource playerAudio = _other.GetComponent<AudioSource>();

                  if (playerAudio != null)
                  {
                        PlayPickupSound(playerAudio);
                  }

                  GameManager.Instance.AddScore(player.PlayerID, 20);

                  Destroy(gameObject);
            }
      }
}
