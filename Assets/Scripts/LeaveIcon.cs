using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveIcon : MonoBehaviour
{
    public void Exit()
    {
        StartCoroutine(ExitCoroutine());
    }

    private IEnumerator ExitCoroutine()
    {
        GameObject.Find("FadeCanvas").transform.GetChild(1).GetComponent<Animator>().Play("in");

        yield return new WaitForSecondsRealtime(1);

        SceneManager.LoadScene("Main Menu");
    }
}
