using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PageSelector : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pageNumText;
    [SerializeField] TMP_InputField codeBox;
    [SerializeField] RectTransform scrollRect;

    List<PageData> pages = new List<PageData>();
    int currentPage = 0;

    ScrollRectHack scrollRectHack;
    private void Start()
    {
        scrollRectHack = FindObjectOfType<ScrollRectHack>();
        pages.Add(new PageData());
    }

    void Save()
    {
        pages[currentPage].text = codeBox.text;
        pages[currentPage].codeBoxPosition = scrollRect.anchoredPosition;
        pages[currentPage].caretPosition = codeBox.caretPosition;
    }

    IEnumerator Load()
    {
        codeBox.text = pages[currentPage].text;
        codeBox.Select();
        codeBox.caretPosition = pages[currentPage].caretPosition;

        pageNumText.text = (currentPage + 1).ToString();

        int i = 0;

        yield return new WaitUntil(() => { return i++ > 2; });

        scrollRectHack.ignoreHack = true;
        scrollRect.anchoredPosition = pages[currentPage].codeBoxPosition;
    }

    public void Next()
    {
        Save();

        currentPage++;

        if (currentPage == pages.Count)
            pages.Add(new PageData());

        StartCoroutine(Load());
    }

    public void Previous()
    {
        Save();

        currentPage--;

        if (currentPage < 0)
            currentPage = 0;

        StartCoroutine(Load());
    }

    public string Script
    {
        get
        {
            Save();

            string script = "";

            for (int i = 0; i < pages.Count; i++)
                script += pages[i].text;

            return script;
        }
    }
}

public class PageData
{
    public string text;
    public Vector2 codeBoxPosition;
    public int caretPosition;
}