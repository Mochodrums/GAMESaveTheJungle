using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSpriteRender : MonoBehaviour
{
   private SpriteRenderer _sr;
   
   [SerializeField] private Sprite _idlesprite;
   [SerializeField] private Sprite[] _animationSprites;
   
   [SerializeField] private float _animationtime = 0.25f;
   private int _animationframe;

   [SerializeField] private bool _loop = true;
   public bool _idle = true;
      
   private void Awake()
   {
      _sr = GetComponent<SpriteRenderer>();
   }

   private void OnEnable()
   {
      _sr.enabled = true;
   }

   private void OnDisable()
   {
      _sr.enabled = false;
   }

   private void Start()
   {
      InvokeRepeating(nameof(NextFrame), _animationtime, _animationtime) ;
   }

   private void NextFrame()
   {
      _animationframe++;

      if (_loop && _animationframe >= _animationSprites.Length)
      {
         _animationframe = 0;
      }

      if (_idle)
      {
         _sr.sprite = _idlesprite;
      }
      else if (_animationframe >= 0 && _animationframe < _animationSprites.Length)
      {
         _sr.sprite = _animationSprites[_animationframe];
      }
   }
   
}
