using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Binary : MonoBehaviour
{
    [SerializeField] float minInterval;
    [SerializeField] float maxInterval;
    [SerializeField] Sprite[] sprites;

    bool _one = false;
    SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();

        _one = Random.Range(1, 999999) % 2 == 1;

        SetSprite();

        StartCoroutine(ChangeNum());
    }

    private IEnumerator ChangeNum()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            _one = !_one;

            SetSprite();
        }
    }

    private void SetSprite()
    {
        _renderer.sprite = sprites[_one ? 1 : 0];
    }
}
