using System;
using UnityEngine;
using UnityEngine.UI;

public class StartGameUIView : MonoBehaviour
{
    [SerializeField]
    private Button _startButton;

    public event Action StartButtonClicked;

    private void Awake()
    {
        _startButton.onClick.AddListener(() => StartButtonClicked?.Invoke());
    }
}
