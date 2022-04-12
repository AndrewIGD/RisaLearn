using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] Animator mainMenu, tutorial, challenges, scenarios, settings;
    [SerializeField] GameObject fade;

    private void PlaySwipe()
    {
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/swipe"), Camera.main.transform.position);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Tutorial()
    {
        SlideMainMenuOut();
        tutorial.Play("slideIn");

        PlaySwipe();
    }

    public void MainMenu()
    {
        if (tutorial.GetCurrentAnimatorStateInfo(0).IsName("slideIn"))
            tutorial.Play("slideOut");
        if (challenges.GetCurrentAnimatorStateInfo(0).IsName("slideIn"))
            challenges.Play("slideOut");
        if (scenarios.GetCurrentAnimatorStateInfo(0).IsName("slideIn"))
            scenarios.Play("slideOut");
        if (settings.GetCurrentAnimatorStateInfo(0).IsName("slideIn"))
            settings.Play("slideOut");

        SlideMainMenuIn();

        PlaySwipe();
    }

    public void Challenges()
    {
        SlideMainMenuOut();
        challenges.Play("slideIn");

        PlaySwipe();
    }

    public void Scenarios()
    {
        SlideMainMenuOut();
        scenarios.Play("slideIn");

        PlaySwipe();
    }

    public void Settings()
    {
        SlideMainMenuOut();
        settings.Play("slideIn");

        PlaySwipe();
    }

    private void SlideMainMenuOut()
    {
        mainMenu.Play("slideOut");
    }

    private void SlideMainMenuIn()
    {
        mainMenu.Play("slideIn");
    }

    private void Start()
    {
        tutorial.Play("slideOut", 0, 1f);
        scenarios.Play("slideOut", 0, 1f);
        challenges.Play("slideOut", 0, 1f);
        settings.Play("slideOut", 0, 1f);

        mainMenu.Play("slideIn", 0, 1f);
    }

    private void Fade()
    {
        fade.SetActive(true);
    }

    public IEnumerator LoadScene(string scene)
    {
        fade.SetActive(true);

        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/click"), Camera.main.transform.position);

        try
        {
            GameObject.Find("Song").SetActive(false);
        }
        catch
        {

        }

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(scene);
    }
}
