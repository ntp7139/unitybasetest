using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour, ICanListenEvent
{
    EventSubscription _onDeathEvent;
    public Button resetButton;
    public GameObject imageBackground;

    void OnEnable()
    {
        _onDeathEvent = this.Listen<OnDeathEvent>(OnPlayerDie);
    }

    void OnDisable()
    {
        _onDeathEvent?.Dispose();
    }

    void Start()
    {
        HideButton();
    }

    public void OnPlayerDie(OnDeathEvent evt)
    {
        ShowButton();
    }

    void HideButton()
    {
        if (imageBackground != null)
        {
            imageBackground.SetActive(false);
        }
        if (resetButton != null)
        {
            resetButton.enabled = false;
            resetButton.gameObject.SetActive(false);
        }
    }

    void ShowButton()
    {
        if (imageBackground != null)
        {
            imageBackground.SetActive(true);
        }
        if (resetButton != null)
        {
            resetButton.gameObject.SetActive(true);
            resetButton.enabled = true;
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
