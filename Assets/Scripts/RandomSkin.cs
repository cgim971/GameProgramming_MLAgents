using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkin : MonoBehaviour {

     private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Sprite> _spriteList;

    private void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _spriteList[Random.Range(0, _spriteList.Count - 1)];
    }
}
