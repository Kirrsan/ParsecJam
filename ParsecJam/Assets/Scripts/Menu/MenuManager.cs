using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private Button _playButton;
    [SerializeField] private int _sceneToLoad = 1;
    [SerializeField] private EventSystem _eventSystem;
    private GameObject _lastSelectedGameObject;

    // Start is called before the first frame update
    void Start()
    {
        _playButton.Select();
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

    public void Play()
    {
        AudioManager.instance.Play("UISelect");
        StartCoroutine(WaitAndPlay());
    }
    private IEnumerator WaitAndPlay()
    {
        yield return new WaitForSecondsRealtime(AudioManager.instance.GetClipLength("UISelect"));
        SceneManager.LoadScene(_sceneToLoad);
    }

    public void Quit()
    {
        AudioManager.instance.Play("UIBack");
        StartCoroutine(WaitAndQuit());
    }
    private IEnumerator WaitAndQuit()
    {
        yield return new WaitForSecondsRealtime(AudioManager.instance.GetClipLength("UIBack"));
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}
