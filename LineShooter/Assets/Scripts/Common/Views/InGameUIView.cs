using TMPro;
using UnityEngine;

public class InGameUIView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _hpText;

    public void SetHpText(int hp)
    {
        _hpText.SetText(hp.ToString());
    }
}
