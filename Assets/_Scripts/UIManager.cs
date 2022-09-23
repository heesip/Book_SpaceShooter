using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    UnityAction action;

    private void Start()
    {
        action = () => OnStartClick();
        startButton.onClick.AddListener(action);

        optionButton.onClick.AddListener(delegate
                                        { OnButtonClick(optionButton.name); });

        shopButton.onClick.AddListener(() => OnButtonClick(shopButton.name));
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene(AllString.Level_01);
        SceneManager.LoadScene(AllString.Play, LoadSceneMode.Additive);
    }

    public void OnButtonClick(string msg)
    {
        Debug.Log($"Click Button : {msg}");
    }
}
