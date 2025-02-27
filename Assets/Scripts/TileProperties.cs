using UnityEngine;
using TMPro; // Import TextMeshPro namespace


public class TileProperties : MonoBehaviour
{
    [SerializeField] public enum TileType { Land, Parking }

    [SerializeField] TileType tileType; // The type of the tile

    [SerializeField] string tileName; // The name of the tile

    [SerializeField] int price; // The price of the tile

    [SerializeField] int tileNumber; // the Number of the tile

    public bool isOwned; // if the land has a owner

    public GameObject owner; // the owner of the land if the land has a owner

    private TextMeshPro textMeshPro; // Reference to TextMeshPro component

    private Renderer tileRenderer;    // Reference to the Renderer component

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find TextMeshPro child component
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        isOwned = false;

        tileRenderer = GetComponent<Renderer>(); // Get the Renderer component of the Tile


    }

    // OnValidate is called when the script is loaded or a value is changed in the Inspector


    // Update is called once per frame
    void Update()
    {

    }

    public int GetTileNumber() // Get the Tile number
    {
        return tileNumber;
    }

    public TileType GetTileType() // Get the tile type
    {
        return tileType;
    }

    public int GetPrice() // Get the tile price
    {
        return price;
    }

    public int GetRent() // The rent is half of the price of the land
    {
        return price / 2;
    }


    // Get the tile according to the tile number
    public static GameObject GetTileAtPosition(int position)
    {
        string tag = "Tile";
        // looking for the tile that has the specific tage
        GameObject[] tiles = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject tile in tiles)
        {
            TileProperties tileProperties = tile.GetComponent<TileProperties>();
            if (tileProperties != null && tileProperties.tileNumber == position)
            {
                return tile;
            }
        }

        // Not found, then warning
        Debug.LogWarning($"Tile with number {position} not found.");
        return null;
    }

    // OnValidate is called when the script is loaded or a value is changed in the Inspector
    private void OnValidate()
    {

        // Update text and material if the component exists
        if (textMeshPro != null)
        {
            UpdateText();
        }

        if (tileRenderer != null) // Check if tileRenderer is not null before updating the material
        {
            UpdateMaterial();
        }
    }

    //Method to update the text display
    private void UpdateText()
    {
        // If the tile is a parking lot, set the price to 0 and do not display it
        if (tileType == TileType.Parking)
        {
            price = 0;
            textMeshPro.text = $"{tileName}\nNumber: {tileNumber}"; // Only display the tile name
        }
        else
        {
            textMeshPro.text = $"{tileName}\nPrice: {price}\nNumber: {tileNumber}"; // Display tile name and price
        }
        Debug.Log("Updated TextMeshPro text: " + textMeshPro.text); // Log the updated text

    }

    // Method to update the material based on tile type
    public void UpdateMaterial()
    {
        if (tileRenderer != null)
        {
            // Check the tile type and set the material accordingly
            if (tileType == TileType.Parking && !isOwned)
            {
                tileRenderer.sharedMaterial.color = Color.yellow; // Change material color to yellow
            }
            else if (!isOwned) // If the land does not have an owner
            {
                tileRenderer.sharedMaterial.color = Color.white; // Reset to default color (or any other color)
            }
            else if (isOwned && owner != null) // If the land does have an owner
            {
                PlayerProperties playerProperties = owner.GetComponent<PlayerProperties>();
                if (playerProperties != null)
                {
                    int playerNumber = playerProperties.getPlayerNumber();

                    // Set different colors according to the PlayerNumber
                    switch (playerNumber)
                    {
                        case 1:
                            tileRenderer.sharedMaterial.color = Color.red; // Player 1 color: red
                            break;
                        case 2:
                            tileRenderer.sharedMaterial.color = Color.magenta; // Player 2 color: magenta
                            break;
                        case 3:
                            tileRenderer.sharedMaterial.color = Color.blue; // Player 3 color: blue
                            break;
                        case 4:
                            tileRenderer.sharedMaterial.color = Color.green; // Player 4 color: green
                            break;
                        default:
                            tileRenderer.sharedMaterial.color = Color.white; // Other cases: default white
                            break;
                    }
                }
            }

        }
    }

    [SerializeField] bool hasHouse = false; // Whether the tile has a house
    private GameObject houseInstance; // Reference to the instance of the house

    public bool HasHouseStatus() // Get the status of whether the tile has a house
    {
        return hasHouse;
    }

    public void buyHouse() // If the tile has a house on it, the price will be doubled
    {
        if (!hasHouse) // Ensure there is no house already
        {
            hasHouse = true;
            price *= 2; // Double the price when a house is built

            // Use transform.forward to get Tile forward direction£¬and use relative position to adjust the position
            Vector3 housePosition = transform.position + transform.forward * (3.5f) + new Vector3(0, 0.5f, 0); // Adjust Y for height

            // Use HouseManager to create a house at the calculated position
            houseInstance = HouseManager.Instance.CreateHouse(housePosition);
            houseInstance.transform.SetParent(transform); // Set the tile as parent of the house
        }
    }

    public void setPrice(int price)
    {
        this.price = price;
    }

    public string getTileName() 
    {
        return tileName;
    }



}
