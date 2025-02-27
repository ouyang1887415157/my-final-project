using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarChart : MonoBehaviour
{
    public List<GameObject> players; // List to hold player properties
    public Image[] bars; // UI Images for the bars
    public TextMeshProUGUI[] barTexts; // UI Texts for displaying coins

    public TextMeshProUGUI totalAssetsText;

    private const float maxCoins = 20000f; // Max coins for scaling

    // Update UI Bars based on current player coins
    public void UpdateBars()
    {
        totalAssetsText.text = "";// initilize total assets text

        for (int i = 0; i < players.Count; i++)
        {
            // Calculate the width of the bar based on the player's coins

            float normalizedCoins = players[i].GetComponent<PlayerProperties>().getCoinsNumber() / maxCoins; // Normalized coin value (0 to 1)
            bars[i].rectTransform.localScale = new Vector3(normalizedCoins, 1, 1);

            int playerNumber = i+1;
            barTexts[i].text = $"P{playerNumber} coins:"; 
            barTexts[i].text += players[i].GetComponent<PlayerProperties>().getCoinsNumber().ToString(); // The text to show the coins of the player

            string assetsNumber = players[i].GetComponent<PlayerProperties>().GetTotalAssets().ToString(); // The text to show the assets of the player
            totalAssetsText.text += $"P{playerNumber} total assets: {assetsNumber}";

            if (i != players.Count -1)
            {
                totalAssetsText.text += $"\n"; // Change the line
            }
        }
    }

}
