using UnityEngine;
using TMPro;
using System.Collections.Generic; // Ensure you import the correct namespace
using System.Collections;
using Unity.VisualScripting;
using static TileProperties;
using UnityEngine.UI;
using System;

public class BigBoardProperties : MonoBehaviour
{

    private TextMeshPro textMeshPro; // Reference to TextMeshPro component on the big board

    [SerializeField] Image ImageOnBoard;

    [SerializeField] Image ImageOnQuestion;

    // GameManager properties
    public List<GameObject> players; // The list of the players in the game
    private int currentPlayerIndex = 0; // The index of the current player
    public int currentRound = 0; // Current Round
    public int maxRounds = 48; // Max Round of the entire game

    // DiceRoller properties
    private int rollResult; // The result of the rolling dice
    private GameObject pendingTile; // The tile that are being handled (in the buying process)

    public delegate void AfterInteraction(); // Handle the logic after the interaction between player and the tile has been done
    public static event AfterInteraction OnAfterInteraction;


    public Button buyButton; // Button to buy
    public Button cancelButton; // Button to cancel

    public Button NextRoundButton;

    private bool isPurchasing = false; // To indicate if the player is purchasing the land

    private bool hasStartedGame = false;

    [SerializeField] GameObject DiceController1; // The empty gameobject to control dice1

    [SerializeField] GameObject DiceController2; // The empty gameobject to control dice2

    [SerializeField] GameObject BarchartController; // The empty gameobject to control the bar chart

    [SerializeField] GameObject ProfileController; // The empty game object to control the profile

    public event Action GameHasStarted; // New event, when the game starts, this will trigger

    public MusicManager musicManager; // The reference to the music manager

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideButtons();
        // Find TextMeshPro child component
        textMeshPro = GetComponentInChildren<TextMeshPro>();

        questionCanvas.SetActive(false); // Hide the question canvas first

        SetImageOnBoard("Images/Tang_Dynasty"); // Set the big board of image when the game start

        SetImageOnQuestion("Images/Examination"); // Set the image of question in examination when the game start

        musicManager = FindFirstObjectByType<MusicManager>(); // Get the music manager

    }

    // Update is called once per frame
    void Update()
    {
        BarchartController.GetComponent<BarChart>().UpdateBars();// Update the Bar chart when the coins number changed

    }

    private void SetImageOnBoard(string imagePath) // Set the big board of image when the game start
    {

        // To load Texture2D
        Texture2D texture = Resources.Load<Texture2D>(imagePath);

        if (texture != null)
        {
            Debug.Log("Texture loaded successfully!");

            // Load the sprite of the image to use it into the sprite of the Image on board
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            ImageOnBoard.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to load texture from Resources!");
        }
    }

    private void SetImageOnQuestion(string imagePath) // Set the big board of image when the game start
    {

        // To load Texture2D
        Texture2D texture = Resources.Load<Texture2D>(imagePath);

        if (texture != null)
        {
            Debug.Log("Texture loaded successfully!");

            // Load the sprite of the image to use it into the sprite of the Image on board
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            ImageOnQuestion.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to load texture from Resources!");
        }
    }

    public void NextRoundStarted() // Start the next round
    {
        if (hasStartedGame == false)
        {
            SetNextRoundButtonText("Next Round!"); // Set the button to be next round
            hasStartedGame = true;

            GameHasStarted?.Invoke();

        }
        else 
        {

        }

        RollAndMove();
    }

    // Method to change the button's text
    public void SetNextRoundButtonText(string newText)
    {
        // Find the Text component in the button's children
        TextMeshProUGUI buttonText = NextRoundButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = newText; // Set the new text
        }
        else
        {
            Debug.LogWarning("Text component not found in NextRoundButton.");
        }
    }

    // Simulate dice rolling and player movement
    public void RollAndMove()
    {

        // Hide the next round button
        //NextRoundButton.gameObject.SetActive(false);

        ProfileController.GetComponent<ProfileImage>().SetProfile(currentPlayerIndex); // Set the profile to be the current player index

        if (currentRound >= maxRounds)
        {
            EndGame();
            return;
        }

        //Get currentPlayer object
        PlayerProperties currentPlayer = players[currentPlayerIndex].GetComponent<PlayerProperties>();

        if(currentPlayer != null && currentPlayer.getBankruptedStatus() == true)// if the current player is bankrupted, he cannot move
        {
            SetImageOnBoard("Images/Bankrupt");


            textMeshPro.text = $"Round {currentRound}\nPlayer {currentPlayerIndex + 1} {currentPlayer.getPlayerName()} is bankrupted so he or she cannot move!"; // Notify players

            // Move to the next round after interaction
            OnAfterInteraction?.Invoke();

        } else if (currentPlayer.getInjured()) // if the player is injured, he cannot move in this round
        {
            SetImageOnBoard("Images/Injured");
            textMeshPro.text = $"Round {currentRound}\nPlayer {currentPlayerIndex + 1} {currentPlayer.getPlayerName()} is injured so he or she cannot move in this round! " +
                $"The player can move in next round!"; // Notify players

            musicManager.PlayHurtSound();

            currentPlayer.setInjured(false); // The injury is healed

            // Move to the next round after interaction
            OnAfterInteraction?.Invoke();
        }
        else
        {

            int roll1 = UnityEngine.Random.Range(1, 7);

            int roll2 = UnityEngine.Random.Range(1, 7);

            rollResult = roll1 + roll2;

            DiceController1.GetComponent<DiceRoller>().RollDice(roll1);// Roll the dice 1

            DiceController2.GetComponent<DiceRoller>().RollDice(roll2);// Roll the dice 2

            MovePlayer(rollResult); // Move the player

        }



    }

    // Move the player to new tile
    private void MovePlayer(int roll)
    {

        // Get currentPlayer PlayerProperties
        PlayerProperties currentPlayer = players[currentPlayerIndex].GetComponent<PlayerProperties>();

        // Logics for moving player will go here
        textMeshPro.text = $"Round {currentRound}\nPlayer {currentPlayerIndex + 1} {currentPlayer.getPlayerName()} moved forward {roll} spaces";

        int currentPosition = currentPlayer.getPlayerCurrentAtPlace(); // Get the currentPosition of the currentPlayer (which place the player currently at)

        int targetPosition = (currentPosition + roll) % 40; // Calculate the target position of the tile number

        // Get the world position of the target tile
        GameObject targetTile = TileProperties.GetTileAtPosition(targetPosition); //target tile

        string targetTileName = targetTile.GetComponent<TileProperties>().getTileName(); // The name of the target tile

        Vector3 basePosition = targetTile.transform.position;// The specific position of the target tile

        // Get the specific position according to the current player index
        Vector3 offset = Vector3.zero; // edge offset

        // Set every player's position
        switch (currentPlayerIndex)
        {
            case 0: // Player 1 - top-left corner
                offset = new Vector3(-2f, 0, 2f); // top-left corner
                break;
            case 1: // Player 2 - top-right corner
                offset = new Vector3(2f, 0, 2f); // top-right corner
                break;
            case 2: // Player 3 - bottom-left corner
                offset = new Vector3(-2f, 0, -2f); // bottom-left corner
                break;
            case 3: // Player 4 - botton-right corner
                offset = new Vector3(2f, 0, -2f); // botton-right corner
                break;
            default:
                Debug.LogWarning("Unsupported Player Index");
                break;
        }

        // Get the position of the target
        Vector3 targetWorldPosition = basePosition + offset;

        float moveTime = 1.0f * roll;

        // Start coroutine to perform SmoothMove
        StartCoroutine(SmoothMove(players[currentPlayerIndex], targetWorldPosition, moveTime)); // 1.0f is the moving time

        // Check if the player is passing Tile 11
        if (currentPosition < 11 && targetPosition >= 11)
        {
            // Player is passing Tile 11, starting to answer questions in Imperial Examination Hall
            StartCoroutine(AskQuizQuestions(players[currentPlayerIndex].GetComponent<PlayerProperties>(),moveTime));
        }

        //Check if the player is passing the starting point (TileNumber = 1)
        if (targetPosition >= 1 && (currentPosition + roll) / 40 > 0) // Check if it has at least finished one loop
        {
            currentPlayer.addCoinsNumber(2000); // Give salary of 2000 coins

            textMeshPro.text += $"\nPlayer {currentPlayerIndex + 1} passed the starting point (Sheng Wu Gate) and received 2000 coins!";
        }

        // Update the current position of the player (the tile number)
        currentPlayer.setPlayerCurrentAtPlace(targetPosition);

        // Display the information of movement
        textMeshPro.text += $"\nto place: {targetTileName} (tile number: {targetPosition})";

        Debug.Log($"Player {currentPlayerIndex + 1} moved to position {targetPosition}");

        // Trigger the interaction Between the player and the tile

        //InteractionBetween(currentPlayer,targetTile.GetComponent<TileProperties>());
        StartCoroutine(InteractionBetween(currentPlayer, targetTile.GetComponent<TileProperties>(),moveTime));

    }

    private void OnEnable() // Enable next round
    {
        OnAfterInteraction += nextRound;
    }
    private void Disable() // Enable next round
    {
        OnAfterInteraction -= nextRound;
    }

    public void nextRound()// The logic of Next round
    {
        currentRound++; // Next Round
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

        //NextRoundButton.gameObject.SetActive(true); // Set the next round button to be active

        Debug.Log($"Next Round {currentRound}"); //Debug Next Round

        Debug.Log($"Player {currentPlayerIndex + 1} will roll next"); //Debug Next Round
    }

    // In the EndGame function, the program will end the game and
    // find the player with the highest asset (including all the price of the lands) to be the winner.
    private void EndGame()
    {
        SetImageOnBoard("Images/Tang_Dynasty");

        Dictionary<int, int> playerAssets = new Dictionary<int, int>(); // Player index to total assets mapping

        textMeshPro.text = $"The Game ends at {currentRound}! Now Calculate the assets!";

        // Iterate through all players to calculate total assets
        for (int i = 0; i < players.Count; i++)
        {
            PlayerProperties playerProperties = players[i].GetComponent<PlayerProperties>();
            if (playerProperties != null)
            {
                int totalAssets = playerProperties.GetTotalAssets();
                playerAssets[i] = totalAssets; // Record the total assets of the player

                // Display the player's total assets
                textMeshPro.text += $"\nPlayer {i + 1} total assets: {totalAssets}";
            }
        }

        // Find the player with the highest total assets
        int winnerIndex = 0;
        int highestAssets = 0;

        foreach (var playerAsset in playerAssets)
        {
            if (playerAsset.Value > highestAssets) // Find the player with the highest total assets
            {
                highestAssets = playerAsset.Value;
                winnerIndex = playerAsset.Key;
            }
        }

        // Display the winning information
        textMeshPro.text += $"\nPlayer {winnerIndex + 1} wins with total assets of {highestAssets}!";
        Debug.Log($"Round {currentRound}\nGame Over");
    }


    // Coroutine for smooth movement
    private IEnumerator SmoothMove(GameObject player, Vector3 targetPosition, float duration)
    {
        // Get main camera
        Camera mainCamera = Camera.main;
        Vector3 initialCameraPosition = mainCamera.transform.position; // initial position
        Quaternion initialCameraRotation = mainCamera.transform.rotation; // initial rotation

        // Calculate the new camera position and angle
        Vector3 cameraOffset = new Vector3(0, 20, 0); // Offset upwards, you can adjust as needed
        Vector3 targetCameraPosition = player.transform.position + cameraOffset; // Centered on the player position
        Quaternion targetCameraRotation = Quaternion.Euler(90, 0, -180); // Vertical downward

        // Wait for 3.5 seconds to allow the dice animation to complete
        yield return new WaitForSeconds(3.5f);

        Vector3 startPosition = player.transform.position; // Initial position
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            // Gradually update player position
            player.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime; // Gradually increase with time
            yield return null; // Wait for the next frame

            targetCameraPosition = player.transform.position + cameraOffset; // Align with the continuously moving player's position

            // Set the camera position and angle
            mainCamera.transform.position = targetCameraPosition;
        }

        // Ensure the final position is accurate
        player.transform.position = targetPosition;

        // Restore the camera position and angle after moving
        mainCamera.transform.position = initialCameraPosition;
        mainCamera.transform.rotation = initialCameraRotation;

    }

    public IEnumerator InteractionBetween(PlayerProperties playerProperties, TileProperties tileProperties, float moveTime)
    {
        // Wait for several seconds to allow the moving player animation to complete
        yield return new WaitForSeconds(moveTime + 3.5f);

        if (playerProperties != null && tileProperties != null)
        {
            if (tileProperties.GetTileType() == TileType.Parking) // If the tile is a parking lot (Add more mechanics in the future)
            {
                // Gambling Mechanics here
                if(tileProperties.GetTileNumber() == 5 || tileProperties.GetTileNumber() == 14 || tileProperties.GetTileNumber() == 35)// If the player steps on tile number 5, gambling starts
                {
                    StartGambling(playerProperties);
                }

                // Black Hotel Mechanics here
                else if (tileProperties.GetTileNumber() == 18 || tileProperties.GetTileNumber() == 28)// If the player steps on tile number 18 or 28, black hotel starts
                {
                    BlackHotelMechanics(playerProperties, tileProperties);
                }

                // Notice Board Mechancis here
                else if (tileProperties.GetTileNumber() == 9 || tileProperties.GetTileNumber() == 20
                    || tileProperties.GetTileNumber() == 24  || tileProperties.GetTileNumber() == 40)
                {
                    TriggerNoticeBoard(playerProperties);

                }
            }
            else if (tileProperties.GetTileType() == TileType.Land) {

                if (!tileProperties.isOwned) // If the tile is a land and is not owned
                {
                    pendingTile = tileProperties.gameObject; // Save the pending tile

                    int ownedMoney = playerProperties.getCoinsNumber();
                    int tilePrice = tileProperties.GetPrice();

                    // Show the purchasing information
                    textMeshPro.text += $"\nThe land's price is {tilePrice}\nDo you want to buy the land?\nPress Y to buy or N to cancel.";

                    // Show the buttons to buy or not
                    StartPurchaseDialogue();


                }
                // if the land is already brought by player himself and did not have a house
                else if (tileProperties.isOwned && tileProperties.owner == playerProperties.gameObject && !tileProperties.HasHouseStatus())
                {

                    int ownedMoney = playerProperties.getCoinsNumber();
                    int tilePrice = tileProperties.GetPrice();


                    // Show the house purchasing information
                    textMeshPro.text += $"\nThis land is already owned by you.\nThe house's price is {tilePrice}\nWould you like to build a house on it?";

                    StartBuildHouseDialogue(tileProperties);

                }

                // if the land is already brought by player himself and did have a house
                else if (tileProperties.isOwned && tileProperties.owner == playerProperties.gameObject && tileProperties.HasHouseStatus())
                {
                    // Highlight that it is owned by the current player
                    textMeshPro.text += "\nThis land is already owned by you and you already have a house.";

                    SetImageOnBoard("Images/Buy_House");

                }

                else if (tileProperties.isOwned == true) // If the tile is a land and is owned by another player, then give the rent
                {
                    GameObject Renter = tileProperties.owner;// Owner of the land

                    PlayerProperties ownerProperties = Renter.GetComponent<PlayerProperties>(); // Properties of the owner of the land

                    PayRent(playerProperties, ownerProperties, tileProperties);

                    musicManager.PlayBuyingSound();
                }

            }

        } else
        {
            Debug.Log("There is an error here!");
        }

        // Move to the next round after interaction
        OnAfterInteraction?.Invoke();


    }

    private void PayRent(PlayerProperties playerProperties, PlayerProperties ownerProperties, TileProperties tileProperties)
    {
        

        // The number of the payer
        int playerNumber = playerProperties.getPlayerNumber();

        // The number of the owner
        int ownerNumber = ownerProperties.getPlayerNumber();

        // The original Money of the player
        int originalMoney = playerProperties.getCoinsNumber();

        int costRent = tileProperties.GetRent();

        if (ownerProperties.getBankruptedStatus() == true) // If the land owner is bankrupted, he cannot receive the rent!
        {
            SetImageOnBoard("Images/Bankrupt");

            textMeshPro.text += $"\nPlayer{ownerNumber} is bankrupted, so he cannot receive money!";
        }

        else if (ownerProperties != null && originalMoney < costRent) // If the player does not have the ability to pay the rent
        {
            SetImageOnBoard("Images/Bankrupt");

            playerProperties.setCoinsNumber(0);

            playerProperties.setBankrupted();

            ownerProperties.addCoinsNumber(originalMoney);

            textMeshPro.text += $"\nPlayer{playerNumber} gives {originalMoney} to Player{ownerNumber}!";
            textMeshPro.text += $"\nPlayer{playerNumber} bankrupted!";

        }
        else if (ownerProperties != null && originalMoney >= costRent) // If the player does have the ability to pay the rent
        {
            SetImageOnBoard("Images/Pay_Coins");

            playerProperties.minusCoinsNumber(costRent);

            ownerProperties.addCoinsNumber(costRent);

            textMeshPro.text += $"\nPlayer{playerNumber} gives {costRent} to Player{ownerNumber}!";
        }

    }

    public void StartPurchaseDialogue() // Start the dialogue to purchase
    {
        SetImageOnBoard("Images/Buy_Land"); // Set the big board of image to buy land

        buyButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
        isPurchasing = true; // Entering the purchasing state

        NextRoundButton.gameObject.SetActive(false);

        // Set the button clicking event
        buyButton.onClick.AddListener(ConfirmPurchase); // add the listener of ConfirmPurchase
        cancelButton.onClick.AddListener(CancelPurchase); // add the listener of CancelPurchase
    }

    public void ConfirmPurchase() // ConfirmPurchase for Yes Button
    {

        SetImageOnBoard("Images/Agreement"); // Set the big board of image to agree

        musicManager.PlayBuyingSound(); // Play the sound of buying

        ProcessPurchase(true);
        HideButtons();
        isPurchasing = false; // Exit the purchasing state
    }

    public void CancelPurchase() // CancelPurchase for No Button
    {
        SetImageOnBoard("Images/Refuse"); // Set the big board of image to refuse

        ProcessPurchase(false);
        HideButtons();
        isPurchasing = false; // Exit the purchasing state
    }

    private void HideButtons() // Hide the buttons for purchasing
    {
        
        buyButton.onClick.RemoveAllListeners(); // Remove all buyButton event listener
        cancelButton.onClick.RemoveAllListeners(); // Remove all cancelButton event listener

        buyButton.gameObject.SetActive(false);// Hide the buttons
        cancelButton.gameObject.SetActive(false);

        NextRoundButton.gameObject.SetActive(true);// Show the NextRoundButton
    }



    // Handle purchasing logic
    private void ProcessPurchase(bool purchase)
    {
        int buyPlayerIndex = 0; // Fix a bug that buys the land

        if (currentPlayerIndex == 0)
        {
            buyPlayerIndex = players.Count - 1;
        }
        else if (currentPlayerIndex >= 1)
        {
            buyPlayerIndex = currentPlayerIndex - 1;
        }

        if (purchase && pendingTile != null)
        {
            PlayerProperties currentPlayerProperties = players[buyPlayerIndex].GetComponent<PlayerProperties>();
            TileProperties tileProperties = pendingTile.GetComponent<TileProperties>();

            int originalMoney = currentPlayerProperties.getCoinsNumber();
            int costMoney = tileProperties.GetPrice();

            if (originalMoney >= costMoney)
            {
                currentPlayerProperties.minusCoinsNumber(costMoney); // The player cost the money
                currentPlayerProperties.buyOneLand(pendingTile); // The player buy the land on his list


                tileProperties.owner = players[buyPlayerIndex]; // Set the owner of the land to the player
                tileProperties.isOwned = true; // Set the land to be owned

                textMeshPro.text += $"Player {buyPlayerIndex + 1} bought the land!";

                tileProperties.UpdateMaterial();// Update the color of the material

                CheckBankrupted(currentPlayerProperties); // Check if the player bankrupted

            }
            else
            {
                textMeshPro.text += "Not enough coins to buy the land!";
                Debug.LogWarning("Player can't afford this land!");
            }
        }
        else if (!purchase)
        {
            textMeshPro.text += "Purchase canceled.";
        }

        //Reset the state

        pendingTile = null;
    }


    public TextMeshProUGUI questionText; // To display the question
    public Button[] optionButtons; // To display the option buttons

    public GameObject questionCanvas; // To reference to the Canvas GameObject that controls the questions

    private bool isAnsweringQuestion = false; // To indicate if the player is answering the question

    private IEnumerator AskQuizQuestions(PlayerProperties player, float movetime)
    {

        // Wait for several seconds to allow the moving player animation to complete
        yield return new WaitForSeconds(movetime+3.5f);

        questionCanvas.SetActive(true);
        isAnsweringQuestion = true;

        // Use Shuffle to randomize the question sequence
        List<QuizQuestion> selectedQuestions = QuizQuestion.GetRandomQuestions(3);

        int score = 0; // record the question correctly answered

        for (int i = 0; i < selectedQuestions.Count; i++)
        {
            ShowQuestionUI(selectedQuestions[i],i); // Display the questions

            // Wait for the user input
            yield return new WaitUntil(() => userHasSelected);

            // Check the user answer
            if (selectedOption == selectedQuestions[i].correctOptionIndex)
            {
                score++;

                // Display if the user selected the correct answer
                questionText.text = $"\nYou have answered this question correctly!";

                if(i != selectedQuestions.Count - 1)
                {
                    questionText.text += $"\nNext Question:";
                }
            }
            else
            {
                // Display the correct answer if the user selected the wrong one
                questionText.text = $"\nIncorrect! Correct answer: {selectedQuestions[i].options[selectedQuestions[i].correctOptionIndex]}";

                if (i != selectedQuestions.Count - 1)
                {
                    questionText.text += $"\nNext Question:";
                }
            }
        }

        int playerIndex = currentPlayerIndex == 0 ? 4 : currentPlayerIndex; // Fix the playerIndex bug

        // When the player finish all the questions
        if (score == 3)
        {
            // Reward the player when he answered all the questions correctly
            player.addCoinsNumber(4000);
            questionText.text += "\nYou answered all questions correctly! You received 4000 coins!";
            textMeshPro.text += $"\nPlayer {playerIndex} passed the Imperial Examination Hall and received 4000 coins!";
        }
        else
        { // Notify the user when he failed
            questionText.text += $"\nYou answered {score} out of 3 correctly.No reward to you if you get any question wrong.";
            textMeshPro.text += $"\nPlayer {playerIndex} did not pass the Imperial Examination and got no rewards!";
        }

        questionText.text += $"\nPress the exit button to exit.";

        // Remove the selected questions from the pool
        foreach (var question in selectedQuestions)
        {
            QuizQuestion.RemoveQuestion(question); // Ensure you have a method to remove the question from the pool
        }
    }

    // The functions to display the question menu
    private bool userHasSelected = false; // Record if the player has answered
    private int selectedOption = -1; // The reference to record the answer

    private void ShowQuestionUI(QuizQuestion question, int number)
    {
        // Show the questionText
        if(number == 0) //if it is the first question, add the player number hint and the question to the questionText
        {
            questionText.text = $"Round {currentRound}\nPlayer {currentPlayerIndex + 1} you should answer the following questions by passing the Imperial Exam Hall on tile 11:";
            questionText.text += question.question;
        }
        else // Just add the question to the questionText to connect previous question answer to the next question
        {
            questionText.text += question.question;
        }


        // Set every button's text and click event
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // Get the current index of the button
            optionButtons[i].GetComponentInChildren<Text>().text = question.options[i]; // Set the button's text

            // Add click event to the buttons
            optionButtons[i].onClick.RemoveAllListeners(); // Remove all current listeners
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index)); // Add new listener to select the option
            optionButtons[i].gameObject.SetActive(true); // Make sure the button is active
        }

        userHasSelected = false; // Reset the choosing state
    }

    // The function that operates when the option buttons are clicked
    public void OnOptionSelected(int optionIndex)
    {
        selectedOption = optionIndex; // Set the option the user selected
        userHasSelected = true; // Label that means the user has already selected
    }

    public void ExitQuestion()
    {
        HideQuestionUI(); // Hide the question UI
        isAnsweringQuestion = false; // Is not answering questions any more
        questionCanvas.SetActive(false);// Hide the question Canvas
    }
    private void HideQuestionUI()
    {
        questionText.text = ""; // Delete the question text
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(false); // Hide all buttons
        }
    }

    public void StartBuildHouseDialogue(TileProperties tileProperties) // Start the dialogue of whether building a house
    {
        SetImageOnBoard("Images/Buy_House"); // Set the image of considering buying the house

        isPurchasing = true; //is purchasing

        // Show Yes and No buttons for building a house
        buyButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);

        // Hide the Next Round Button
        NextRoundButton.gameObject.SetActive(false);

        buyButton.onClick.AddListener(() => ConfirmBuildHouse(tileProperties)); // Attach listener
        cancelButton.onClick.AddListener(() => CancelBuildHouse(tileProperties)); // Attach listener


    }

    private void ConfirmBuildHouse(TileProperties tileProperties) // If the player confirms to buy the house
    {
        SetImageOnBoard("Images/Agreement");// Set the image of buying the house

        musicManager.PlayBuyingSound();

        ProcessHousePurchase(true, tileProperties);
        isPurchasing = false;
        HideButtons(); // Hide buttons after the action
    }

    private void CancelBuildHouse(TileProperties tileProperties)// If the player cancels to buy the house
    {
        SetImageOnBoard("Images/Refuse");// Set the image of refuse to buying the house

        ProcessHousePurchase(false, tileProperties);
        isPurchasing = false;
        HideButtons(); // Hide buttons on cancel
    }

    // Handle purchasing logic
    private void ProcessHousePurchase(bool purchase, TileProperties tileProperties)
    {
        int buyPlayerIndex = 0; // Fix a bug that buys the land, buyPlayerIndex is the current play who buys the house

        if (currentPlayerIndex == 0)
        {
            buyPlayerIndex = players.Count - 1;
        }
        else if (currentPlayerIndex >= 1)
        {
            buyPlayerIndex = currentPlayerIndex - 1;
        }

        if (tileProperties != null && purchase) // If the player decides to buy the house
        {
            PlayerProperties currentPlayerProperties = players[buyPlayerIndex].GetComponent<PlayerProperties>();

            int originalMoney = currentPlayerProperties.getCoinsNumber();
            int costMoney = tileProperties.GetPrice();

            if (originalMoney >= costMoney) // If the player has enough money to buy the house
            {
                currentPlayerProperties.minusCoinsNumber(costMoney); // The player cost the money
                tileProperties.buyHouse(); // The tile has been brought

                textMeshPro.text += $"Player {buyPlayerIndex + 1} bought the house on his land!";

                CheckBankrupted(currentPlayerProperties); // Check if the player bankrupted

            }
            else // If the player does not have enough money to buy the house
            {
                textMeshPro.text += "Not enough coins to buy the house!";
            }

        } else
        {
            textMeshPro.text += "\nYou chose not to build a house.";
        }
    }

    private void StartGambling(PlayerProperties playerProperties)
    {
        SetImageOnBoard("Images/Gambling"); // Set the image to gambling

        isPurchasing = true; // Entering the gambling state 

        int gambleCost = 2000; // Cost to gamble

        // Check if the player has enough coins
        if (playerProperties.getCoinsNumber() < gambleCost)
        {
            textMeshPro.text += "\nYou don't have enough coins to gamble!";
            return;
        }

        // Deduct the gambling cost from the player
        playerProperties.minusCoinsNumber(gambleCost);
        textMeshPro.text += $"\n{playerProperties.name} wants to gamble in ChangLe Fang on tile 5 by paying {gambleCost} coins!";

        // Show a prompt for the user to choose
        textMeshPro.text += "\nDo you want to guess 'Large' (dice number 4-6)(Yes) or 'Small' (dice number 1-3)(No)?";

        // Show the gambling buttons
        buyButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);

        // Remove the next round button
        NextRoundButton.gameObject.SetActive(false);

        // Reset button listeners
        buyButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        // Bind the events to the buttons
        buyButton.onClick.AddListener(() => ConfirmGamble(playerProperties, true)); // Yes for large
        cancelButton.onClick.AddListener(() => ConfirmGamble(playerProperties, false)); // No for small
    }

    private void ConfirmGamble(PlayerProperties playerProperties, bool guessLarge)
    {
        // Simulate rolling the dice
        int diceRoll = UnityEngine.Random.Range(1, 7); // Roll the dice (1-6)
        bool isLarge = diceRoll >= 4; // Define large as 4-6

        // Determine if the player's guess is correct
        if (guessLarge == isLarge)
        {
            SetImageOnBoard("Images/Yeah"); // Set the image to gambling

            // Player guessed correctly
            playerProperties.addCoinsNumber(4000);
            textMeshPro.text += $"\nCongratulations! The dice showed {diceRoll}. You won 4000 coins!";
        }
        else
        {
            SetImageOnBoard("Images/Lose"); // Set the image to gambling

            // Player guessed incorrectly
            textMeshPro.text += $"\nSorry, you guessed wrong! The dice showed {diceRoll}. Better luck next time!";
        }

        isPurchasing = false; // Exiting the gambling state

        // Hide buttons after gambling
        HideButtons();
    }

    private void BlackHotelMechanics(PlayerProperties playerProperties, TileProperties tileProperties)
    {
        if (!tileProperties.isOwned)
        {  // if the player steps on the black hotel and the black hotel does not have an owner

            SetImageOnBoard("Images/Big_Hotel");

            StartBlackHotelPurchasing(playerProperties, tileProperties); // Call the black market method

            

        }
        else if (tileProperties.isOwned && tileProperties.owner == playerProperties.gameObject)
        {  // if the player steps on the black hotel and the player is the owner of the black hotel himself

            SetImageOnBoard("Images/Big_Hotel");

            // Show the black hotel information
            textMeshPro.text += $"\nThe big hotel is already owned by yourself.";

            if (!NextRoundButton.gameObject.activeSelf) //If the nextRoundButton is not active, set it to active
            {
                NextRoundButton.gameObject.SetActive(true);
            }

        }
        else if (tileProperties.isOwned && tileProperties.owner != playerProperties.gameObject)
        {
            musicManager.PlayBuyingSound();

            SetImageOnBoard("Images/Pay_Coins");

            int blackHotelRent = 4000;

            tileProperties.setPrice(blackHotelRent * 2); // The rent is half of the price

            // if the player steps on the black hotel and the owner of the black hotel is another person
            textMeshPro.text += $"\nThe big hotel is owned by {tileProperties.owner.name}. You need to pay {blackHotelRent} to him!";

            GameObject Renter = tileProperties.owner;// Owner of the land

            PlayerProperties ownerProperties = Renter.GetComponent<PlayerProperties>(); // Properties of the owner of the land

            PayRent(playerProperties, ownerProperties, tileProperties); // Pay 4000 coins

            tileProperties.isOwned = false; // The black hotel is no longer valid

            tileProperties.owner = null;

            tileProperties.UpdateMaterial();

            textMeshPro.text += $"\nNow you have paid 4000 coins.";

            if (!NextRoundButton.gameObject.activeSelf) //If the nextRoundButton is not active, set it to active
            {
                NextRoundButton.gameObject.SetActive(true);
            }


        }
    }

    private void StartBlackHotelPurchasing(PlayerProperties playerProperties, TileProperties tileProperties)
    {
        isPurchasing = true;

        int blackMarketCost = 2000; // Cost to interact with the black market

        // Check if the player has enough coins
        if (playerProperties.getCoinsNumber() < blackMarketCost)
        {
            textMeshPro.text += "\nYou don't have enough coins to use the big hotel!";
            return;
        }

        // Show a prompt to the player
        textMeshPro.text += $"\n{playerProperties.name}, do you want to pay {blackMarketCost} coins to set a rent of 4000 coins on this big hotel for the next player? (One time only)";

        // Show Yes/No buttons for decision
        buyButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);

        // Hide the NextRound Button
        NextRoundButton.gameObject.SetActive(false);

        buyButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        // Bind the events to the buttons
        buyButton.onClick.AddListener(() => ConfirmBlackHotel(playerProperties, tileProperties, true));
        cancelButton.onClick.AddListener(() => ConfirmBlackHotel(playerProperties, tileProperties, false));
    }

    private void ConfirmBlackHotel(PlayerProperties playerProperties, TileProperties tileProperties ,bool wantsToPay )
    {
        if (wantsToPay)
        {
            musicManager.PlayBuyingSound();

            // Deduct the black market cost from the player
            playerProperties.minusCoinsNumber(2000);
            textMeshPro.text += $"\n{playerProperties.name} has paid 2000 coins to the black market!";

            
            tileProperties.isOwned = true; // Reset ownership
            tileProperties.owner = playerProperties.gameObject; // Set the owner of the tile to be player

            tileProperties.UpdateMaterial();// Update the color of the material

            SetImageOnBoard("Images/Agreement");

            textMeshPro.text += "\nThe next player who lands on this tile will pay 4000 coins in rent!";
        }
        else
        {
            textMeshPro.text += $"{playerProperties.name} chose not to pay the black market.";

            SetImageOnBoard("Images/Refuse");
        }

        isPurchasing = false;
        // Hide buttons after decision
        HideButtons();
    }

    // Trigger the notice board effects
    private void TriggerNoticeBoard(PlayerProperties playerProperties)
    {
        // Create a random number generator
        int randomEffect = UnityEngine.Random.Range(1, 6); // Generate a random number between 1 and 5

        textMeshPro.text += $"\n{playerProperties.name} steps on the notice board!";

        switch (randomEffect)
        {
            case 1:
                // Go competition, player wins first place and receives 2000 coins
                playerProperties.addCoinsNumber(2000);
                SetImageOnBoard("Images/Go");

                textMeshPro.text += $"\n{playerProperties.name} participated in a go competition and won first place, earning 2000 coins!";
                break;

            case 2:
                // Player gains important enemy intelligence and receives 1000 coins as a reward
                playerProperties.addCoinsNumber(1000);
                SetImageOnBoard("Images/Intelligence");

                textMeshPro.text += $"\n{playerProperties.name} gained important enemy intelligence,so the emperor rewarded 1000 coins!";
                break;

            case 3:
                SetImageOnBoard("Images/Bandit");

                musicManager.PlayHurtSound();

                // Player is robbed by mountain bandits, loses 2000 coins, and gets injured, going to the hospital (Tile Number 21) for one turn
                playerProperties.minusCoinsNumber(2000);

                CheckBankrupted(playerProperties); // Check if the player is bankrupted

                playerProperties.setInjured(true); // Set the injured state

                textMeshPro.text += $"\n{playerProperties.name} was targeted by mountain bandits, losing 2000 coins, " +
                    $"and is injured, going to the hospital (TileNumber 21) to stay for 1 round!";

                MoveToHospital(playerProperties);
                

                break;

            case 4:
                SetImageOnBoard("Images/Drugged");

                musicManager.PlayHurtSound();

                // Player mistakenly enters a black market and is drugged, loses 1000 coins and stays in place for one turn
                playerProperties.minusCoinsNumber(1000);

                CheckBankrupted(playerProperties); // Check if the player is bankrupted

                textMeshPro.text += $"\n{playerProperties.name} mistakenly entered a black hotel, getting drugged, losing 1000 coins, " +
                    $"and staying in place for one turn!";

                playerProperties.setInjured(true); // Set the state to remain in place

                break;

            case 5:
                SetImageOnBoard("Images/Subsidy");

                // Government subsidy is issued, the player with the least land holdings receives 3000 coins
                PlayerProperties leastAssetsPlayer = GetPlayerWithLeastLands();

                leastAssetsPlayer.addCoinsNumber(3000);

                textMeshPro.text += $"\nThe Tang Dynasty government issued a subsidy, " +
                    $"and the player with the least land holdings, {leastAssetsPlayer.name}, received 3000 coins!";

                break;

            default:
                // This case should not occur
                Debug.LogWarning("Unexpected random effect selected.");
                break;
        }
    }

    // Method for handling player to move to the hospital
    private void MoveToHospital(PlayerProperties playerProperties)
    {
        int hospitalPosition = 21; // The position of hospital

        int roll = 0; // The move to the hospital

        int currentPosition = playerProperties.getPlayerCurrentAtPlace(); // Get the currentPosition of the currentPlayer (which place the player currently at)

        if(currentPosition <= hospitalPosition)
        {
            roll = hospitalPosition - currentPosition;
        } else
        {
            roll = currentPosition - hospitalPosition;
        }

        int targetPosition = hospitalPosition; // Calculate the target position of the tile number

        // Get the world position of the target tile
        GameObject targetTile = TileProperties.GetTileAtPosition(targetPosition); //target tile

        Vector3 basePosition = targetTile.transform.position;// The specific position of the target tile

        // Get the specific position according to the current player index
        Vector3 offset = Vector3.zero; // edge offset

        // Set every player's position
        switch (currentPlayerIndex)
        {
            case 0: // Player 1 - top-left corner
                offset = new Vector3(-2f, 0, 2f); // top-left corner
                break;
            case 1: // Player 2 - top-right corner
                offset = new Vector3(2f, 0, 2f); // top-right corner
                break;
            case 2: // Player 3 - bottom-left corner
                offset = new Vector3(-2f, 0, -2f); // bottom-left corner
                break;
            case 3: // Player 4 - botton-right corner
                offset = new Vector3(2f, 0, -2f); // botton-right corner
                break;
            default:
                Debug.LogWarning("Unsupported Player Index");
                break;
        }

        // Get the position of the target
        Vector3 targetWorldPosition = basePosition + offset;

        float moveTime = 1.0f * roll;

        // Start coroutine to perform SmoothMove
        StartCoroutine(SmoothMove(players[currentPlayerIndex], targetWorldPosition, moveTime)); // 1.0f is the moving time

        // Update the current position of the player (the tile number)
        playerProperties.setPlayerCurrentAtPlace(targetPosition);

    }

    // Method to find the player with the least amount of land holdings
    private PlayerProperties GetPlayerWithLeastLands()
    {
        PlayerProperties leastAssetsPlayer = null;
        int minLands = int.MaxValue;

        // Go through each player to find the one with the least amount of owned lands
        foreach (var playerObj in players)
        {
            PlayerProperties playerProps = playerObj.GetComponent<PlayerProperties>();
            // Update the least assets player if found
            if (playerProps != null && playerProps.GetOwnedLands().Count < minLands)
            {
                minLands = playerProps.GetOwnedLands().Count;
                leastAssetsPlayer = playerProps;
            }
        }

        return leastAssetsPlayer; // Return the player with the least land holdings
    }

    private void CheckBankrupted(PlayerProperties currentPlayerProperties) // Check if the player is bankrupted
    {
        if (currentPlayerProperties.getCoinsNumber() <= 0) //If the player has zero coins after buying the land
        {
            SetImageOnBoard("Images/Bankrupt");

            currentPlayerProperties.setBankrupted();
            textMeshPro.text += $"{currentPlayerProperties.name} is bankrupted!";
        }

    }
}
