using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset increaseFont;
    [SerializeField] private TMP_FontAsset decreaseFont;

    [SerializeField] private TextMeshPro floatingText;

    public void SetFloatingText(bool isIncrease, int value)
    {
        if (isIncrease)
            floatingText.font = increaseFont;
        else
            floatingText.font = decreaseFont;

        floatingText.text = value.ToString();
    }
}
