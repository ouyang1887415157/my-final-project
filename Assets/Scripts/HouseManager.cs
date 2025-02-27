using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public static HouseManager Instance; // Single instance of HouseManager

    [SerializeField] private GameObject housePrefab; // Reference to the House prefab

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject CreateHouse(Vector3 position) // Create a house on the tile
    {
        GameObject houseInstance = Instantiate(housePrefab, position, Quaternion.identity); // Instantiate house prefab
        return houseInstance; // Return the house instance
    }
}
