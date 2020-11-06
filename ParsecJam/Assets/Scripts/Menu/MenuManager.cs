﻿using Rewired.Utils.Platforms.Windows;
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
    [SerializeField] private Canvas _canvas;

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
    private GameObject currentSelectedGameObject;

    [Header("Eye Settings")]
    [SerializeField] private Vector3[] _eyePossiblePositions;
    [SerializeField] private RectTransform _eyeTransform;
    [SerializeField] private RectTransform _eyeParent;
    [SerializeField] private float _eyeSpeed = 2;
    private bool _hasToChangeEyePosition = true;
    private Vector3 _previousPosition;
    private Vector3 _newPosition;
    private float _eyePositionStep = 0;

    private Vector3 _baseEyePosition;
    private Vector3 _lastMousePosition;

    private bool _mouseIsMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        _baseEyePosition = _eyeTransform.position;
        _previousPosition = _eyePossiblePositions[0];
        _lastMousePosition = Input.mousePosition;
        _newPosition = _eyePossiblePositions[0];
        GoToBasePanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (_eventSystem.currentSelectedGameObject != null)
        {
            currentSelectedGameObject = _eventSystem.currentSelectedGameObject;
        }
        if (currentSelectedGameObject != _lastSelectedGameObject)
        {
            if (_lastSelectedGameObject != null)
            {
                AudioManager.instance.Play("UINavigation");
            }
            if (currentSelectedGameObject != null && _lastSelectedGameObject != currentSelectedGameObject)
            {
                _lastSelectedGameObject = currentSelectedGameObject;
            }
        }
        if(_eventSystem.currentSelectedGameObject == null)
        {
            currentSelectedGameObject.GetComponent<Button>().Select();
        }

        //if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        //{
        //    _mouseIsMoving = true;
        //    _hasToChangeEyePosition = false;
        //    _lastMousePosition = Input.mousePosition;
        //    Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20));
        //    newPos = new Vector3(Mathf.Clamp(newPos.x - 15, -35, 35), Mathf.Clamp(newPos.y + 1, -35, 35), newPos.z);
        //    _eyeTransform.localPosition = newPos;
        //}
        //else
        //{
        //    _mouseIsMoving = false;
        //    if (_eyeTransform.localPosition != _newPosition){
        //        _hasToChangeEyePosition = true;
        //    }
        //    if (_hasToChangeEyePosition)
        //    {
        //        _eyePositionStep += Time.deltaTime * _eyeSpeed;
        //        _eyeTransform.localPosition = Vector3.Lerp(_previousPosition, _newPosition, _eyePositionStep);
        //        if (_eyeTransform.localPosition == _newPosition)
        //        {
        //            _hasToChangeEyePosition = false;
        //            _eyePositionStep = 0;
        //        }
        //    }
        //}
        if (_hasToChangeEyePosition)
        {
            _eyePositionStep += Time.deltaTime * _eyeSpeed;
            _eyeTransform.localPosition = Vector3.Lerp(_previousPosition, _newPosition, _eyePositionStep);
            if (_eyeTransform.localPosition == _newPosition)
            {
                _hasToChangeEyePosition = false;
                _eyePositionStep = 0;
            }
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

    public void ChangeEyePosition(int posIndex)
    {
        _previousPosition = _eyeTransform.localPosition;
        _newPosition = _eyePossiblePositions[posIndex];
        _eyePositionStep = 0;
        _hasToChangeEyePosition = true;
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
