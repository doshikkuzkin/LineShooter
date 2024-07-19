using UnityEngine;

public class RestartGameUIView : StartGameUIView
{
    [SerializeField]
    private GameObject _winText;

    [SerializeField]
    private GameObject _loseText;

    public void SetupText(bool isWin)
    {
        _winText.SetActive(isWin);
        _loseText.SetActive(!isWin);
    }
}
