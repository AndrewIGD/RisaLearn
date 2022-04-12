using UnityEngine;

public class Documentation : MonoBehaviour
{
    [SerializeField] Transform pageParent;

    private GameObject _lastOpenedPage;

    private void OnEnable()
    {
        LoadPage("Documentation");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void LoadPage(string name)
    {
        if (_lastOpenedPage != null)
            _lastOpenedPage.SetActive(false);

        foreach(Transform child in pageParent)
        {
            if(child.transform.name == name)
            {
                _lastOpenedPage = child.gameObject;
                break;
            }    
        }

        _lastOpenedPage.SetActive(true);
    }
}
