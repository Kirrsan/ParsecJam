using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum FunctionToDo
{
    PLAY,
    BACK,
    CONTROL,
    CREDIT,
    QUIT
}

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _basePanel;
    [SerializeField] private GameObject _controlPanel;
    [SerializeField] private GameObject _creditPanel;

    [Header("Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _controlBackButton;
    [SerializeField] private Button _creditBackButton;

    [Header("Other Settings")]
    [SerializeField] private int _sceneToLoad = 1;
    [SerializeField] private EventSystem _eventSystem;
    private GameObject _lastSelectedGameObject;

    // Start is called before the first frame update
    void Start()
    {
        GoToBasePanel();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject currentSelectedGameObject = _eventSystem.currentSelectedGameObject;
        if (currentSelectedGameObject != _lastSelectedGameObject)
        {
            if (_lastSelectedGameObject != null)
            {
                AudioManager.instance.Play("UINavigation");
            }
            _lastSelectedGameObject = currentSelectedGameObject;
        }
    }

    public void DoFunction(int functionToDo)
    {
        StartCoroutine(WaitAndDoFunction((FunctionToDo)functionToDo));
    }

    private IEnumerator WaitAndDoFunction(FunctionToDo functionToDo)
    {
        AudioManager.instance.Play("UISelect");
        yield return new WaitForSeconds(AudioManager.instance.GetClipLength("UISelect"));
        switch (functionToDo)
        {
            case FunctionToDo.PLAY:
                Play();
                break;
            case FunctionToDo.BACK:
                GoToBasePanel();
                break;
            case FunctionToDo.CONTROL:
                GoToControl();
                break;
            case FunctionToDo.CREDIT:
                GoToCredit();
                break;
            case FunctionToDo.QUIT:
                Quit();
                break;
            default:
                print("There is no such functin fool");
                break;
        }
    }


    #region Buttons Funcions
    private void Play()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }

    private void GoToBasePanel()
    {
        _controlPanel.SetActive(false);
        _creditPanel.SetActive(false);
        _basePanel.SetActive(true);
        _playButton.Select();
    }

    private void GoToControl()
    {
        _basePanel.SetActive(false);
        _creditPanel.SetActive(false);
        _controlPanel.SetActive(true);
        _controlBackButton.Select();
    }
    private void GoToCredit()
    {
        _basePanel.SetActive(false);
        _controlPanel.SetActive(false);
        _creditPanel.SetActive(true);
        _creditBackButton.Select();
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    #endregion

}
