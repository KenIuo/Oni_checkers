using UnityEngine;

public class BackButtonScript : MonoBehaviour
{
    [SerializeField] GameObject _currentScreen;
    [SerializeField] GameObject _screenToOpen;

    public void OnBackClick()
    {
        _currentScreen.SetActive(false);

        GameObject.Find("Arena").SetActive(false);
        GameObject.Find("Checkers").SetActive(false);
        GameObject.Find("Environment").SetActive(false);

        _screenToOpen.SetActive(true);
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
