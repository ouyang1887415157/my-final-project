using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileImage : MonoBehaviour
{
    [SerializeField] Image profileImage;

    [SerializeField] TextMeshProUGUI profileText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetProfile(int playerNumber)
    {
        if(playerNumber == 0)
        {
            SetImageOnProfile("Images/Libai");
            profileText.text = "P1:";
        } 
        else if (playerNumber == 1)
        {
            SetImageOnProfile("Images/WangWei");
            profileText.text = "P2:";
        }
        else if (playerNumber == 2)
        {
            SetImageOnProfile("Images/WuZeTian");
            profileText.text = "P3:";
        }
        else if (playerNumber == 3)
        {
            SetImageOnProfile("Images/LiShiMing");
            profileText.text = "P4:";
        }
    }

    private void SetImageOnProfile(string imagePath) // Set the big board of image when the game start
    {

        // To load Texture2D
        Texture2D texture = Resources.Load<Texture2D>(imagePath);

        if (texture != null)
        {
            Debug.Log("Texture loaded successfully!");

            // Load the sprite of the image to use it into the sprite of the Image on board
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            profileImage.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to load texture from Resources!");
        }
    }
}
