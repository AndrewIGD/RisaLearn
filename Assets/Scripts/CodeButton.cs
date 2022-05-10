using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeButton : MonoBehaviour
{
    [SerializeField] float timeToComplete;
    [SerializeField] Tank tankToActivate;

    public void CloseCanvas()
    {
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/slide"), Camera.main.transform.position);

        canvas.SetBool("active", false);

        Invoke("DeactivateCanvas", 1);
    }

    void DeactivateCanvas()
    {
        canvas.gameObject.SetActive(false);

        if (Timer.instance != null)
            Timer.Start(timeToComplete);

        if (tankToActivate != null)
            tankToActivate.enabled = true;

        //Time.timeScale = 1;
    }

    private void OnMouseDown()
    {
        spriteRenderer.color = pressColor;

        canvas.gameObject.SetActive(true);
        canvas.SetBool("active", true);

        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/slide"), Camera.main.transform.position);

        //Time.timeScale = 0.0000001f;

        CancelInvoke("DeactivateCanvas");
    }

    private void OnMouseUp()
    {
        spriteRenderer.color = highlightColor;
    }

    private void OnMouseEnter()
    {
        spriteRenderer.color = highlightColor;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = defaultColor;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    private void FixedUpdate()
    {
        transform.eulerAngles = Vector3.zero;
    }

    private void Update()
    {
            //if (Input.GetKeyDown(KeyCode.Escape))
            //    CloseCanvas();
            //if (Input.GetKeyDown(KeyCode.E))
            //    OnMouseDown();
    }

    Color defaultColor;
    SpriteRenderer spriteRenderer;
    [SerializeField] Animator canvas;
    [SerializeField] Color highlightColor;
    [SerializeField] Color pressColor;
}
