using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProperties : MonoBehaviour
{
    [SerializeField] int PlayerNumber; // The number of player

    [SerializeField] int PlayerCurrentAtPlace; // The place of the player

    [SerializeField] int PlayerCoins; // The coins of the player

    [SerializeField] List<GameObject> ownedLands; // The places player owns

    [SerializeField] bool isBankrupted;

    [SerializeField] bool isInjured=false;

    [SerializeField] Image ImageProfile; // The image of the profile

    [SerializeField] string ImagePath; // The Path of the input to the profile

    [SerializeField] string PlayerName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initially, the player is at place number 1
        PlayerCurrentAtPlace = 1;
        // Initially, the player has 20000 coins
        PlayerCoins = 20000;

        // Initilized ownedLands
        ownedLands = new List<GameObject>();
        isBankrupted = false;

        SetImageProfile(ImagePath);
    }

    private void SetImageProfile(string imagePath) // Set the profile of image when the game start
    {

        // To load Texture2D
        Texture2D texture = Resources.Load<Texture2D>(imagePath);

        if (texture != null)
        {
            Debug.Log("Texture loaded successfully!");

            // Load the sprite of the image to use it into the sprite of the Image on board
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            ImageProfile.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to load texture from Resources!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getPlayerName()// Get the name of the player
    {
        return PlayerName;
    }

    // Get the number of the player, player1, player2 etc
    public int getPlayerNumber()
    {
        return PlayerNumber;
    }

    // Getter method for PlayerCurrentAtIndex
    public int getPlayerCurrentAtPlace()
    {
        return PlayerCurrentAtPlace;
    }

    public void setPlayerCurrentAtPlace(int index)
    {
        PlayerCurrentAtPlace = index;
    } // Get the place of the player

    public int getCoinsNumber()
    {
        return PlayerCoins;
    } // Get the coins of the player

    public void setCoinsNumber(int coinNumber) // Set the coins of the player
    {
        PlayerCoins = coinNumber;
    }

    public void addCoinsNumber(int coinNumber)
    {
        PlayerCoins = PlayerCoins + coinNumber;
    }
    public void minusCoinsNumber(int coinNumber)
    {
        PlayerCoins = PlayerCoins - coinNumber;
    }

    public void buyOneLand(GameObject land)
    {
        if (land != null)
        {
            ownedLands.Add(land); // adds the land to the list of ownedLands
            Debug.Log($"Player {PlayerNumber} bought land: {land.name}"); // Recording the buying information
        }
        else
        {
            Debug.LogWarning("Attempted to buy a null land object.");
        }
    }

    public void setBankrupted()
    {
        isBankrupted = true;
    }

    public bool getBankruptedStatus()
    {
        return isBankrupted;
    }

    public int GetTotalAssets() // The total assets of a player
    {
        int totalValue = getCoinsNumber(); // The total coins of the player

        // Iterate through the player's owned land plots and accumulate the prices of the lands.
        foreach (GameObject land in ownedLands) // ownedLands is the list of the properties player has
        {
            TileProperties tileProperties = land.GetComponent<TileProperties>();
            if (tileProperties != null)
            {
                totalValue += tileProperties.GetPrice(); // Adds up the price of the lands
            }
        }

        return totalValue;
    }

    public void setInjured(bool injured) // Set the injured status
    {
        isInjured = injured;
    }

    public bool getInjured() //Get the injured status
    {
        return isInjured;
    }

    // Method to return the list of owned lands
    public List<GameObject> GetOwnedLands()
    {
        return ownedLands; // Return the list of owned lands
    }
}
