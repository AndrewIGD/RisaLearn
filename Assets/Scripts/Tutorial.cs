using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject[] tutorialObjects;

    Animator _animator;
    int _index;
    private void Start()
    {
        _animator = GetComponent<Animator>();

        StartCoroutine(TutorialCoroutine());
    }

    public void StartTutorial()
    {
        StartCoroutine(TutorialCoroutine());
    }

    public IEnumerator TutorialCoroutine()
    {
        _animator.Play("in");

        yield return new WaitForSecondsRealtime(1);

        _index = 0;
        tutorialObjects[0].SetActive(true);
    }

    public void Advance()
    {
        tutorialObjects[_index++].SetActive(false);

        if (_index == tutorialObjects.Length)
            _animator.Play("out");
        else tutorialObjects[_index].SetActive(true);
    }
}
