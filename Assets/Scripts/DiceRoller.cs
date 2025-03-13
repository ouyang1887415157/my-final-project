using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
    [SerializeField] Image diceImage; // Show the Image of the dice

    public float rollDuration = 2f; // The total time to play the animation

    public float frameDuration = 0.1f; // The time every frame last

    public Button NextRoundButton;
    public void RollDice(int finalFace)
    {
        StartCoroutine(RollDiceCoroutine(finalFace));
    }

    private IEnumerator RollDiceCoroutine(int finalFace)
    {
        if (NextRoundButton.gameObject.activeSelf)
        {
            NextRoundButton.gameObject.SetActive(false); // Set the nextButton to false
        }


        float elapsed = 0f;

        // Perform the animation to roll the dice 
        while (elapsed < rollDuration)
        {
            // Randomly choose a face of the dice
            int randomFace = Random.Range(1, 7);

            SetDiceImage(randomFace);

            // Wait for frame Duration Seconds
            yield return new WaitForSeconds(frameDuration);
            elapsed += frameDuration;
        }

        SetDiceImage(finalFace); // Show the final result

        if (!NextRoundButton.gameObject.activeSelf)
        {
            NextRoundButton.gameObject.SetActive(true); // Set the nextButton to false
        }
    }

    private void SetImageBackground(string imagePath) // Set the big board of image when the game start
    {

        // To load Texture2D
        Texture2D texture = Resources.Load<Texture2D>(imagePath);

        if (texture != null)
        {
            Debug.Log("Texture loaded successfully!");

            // Load the sprite of the image to use it into the sprite of the Image on board
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            diceImage.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to load texture from Resources!");
        }
    }

    private void SetDiceImage(int randomFace)
    {
        if (randomFace == 1)
        {
            SetImageBackground("Images/Dice1");
        }
        else if (randomFace == 2)
        {
            SetImageBackground("Images/Dice2");
        }
        else if (randomFace == 3)
        {
            SetImageBackground("Images/Dice3");
        }
        else if (randomFace == 4)
        {
            SetImageBackground("Images/Dice4");
        }
        else if (randomFace == 5)
        {
            SetImageBackground("Images/Dice5");
        }
        else if (randomFace == 6)
        {
            SetImageBackground("Images/Dice6");
        }

    }
}

