using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private Button _playButton;

    [SerializeField] private int _sceneToLoad = 1;

    // Start is called before the first frame update
    void Start()
    {
        _playButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}
