using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Script: "What is my purpose?"
/// Me: "You change one text box"
/// Script: "Oh GOD ; A; "
/// </summary>
public class ShopButtons : MonoBehaviour
{
    public TextMeshProUGUI currCoins;
    public DataHolder data;

    public void UIUpdate()
    {
        currCoins.text = data.getCoins().ToString();
    }
}
