using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioClip _Music;

    private void Start()
    {
        _musicSource.clip = _Music;
        _musicSource.loop = true;
        _musicSource.Play();
    }
}
