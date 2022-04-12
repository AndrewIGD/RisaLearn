using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCanvas : MonoBehaviour
{
    public void Win()
    {
        GetComponent<Animator>().Play("win");

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
    }

    public IEnumerator LoadScene(string scene)
    {
        GameObject.Find("FadeCanvas").transform.GetChild(1).GetComponent<Animator>().Play("in");

        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/click"), Camera.main.transform.position);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(scene);
    }
}
