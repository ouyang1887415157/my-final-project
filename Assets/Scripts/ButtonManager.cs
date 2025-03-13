using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSoundManager : MonoBehaviour
{
    private MusicManager musicManager;

    void Start()
    {
        musicManager = FindFirstObjectByType<MusicManager>();

        // Find all Button components in the scene
        Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Button button in buttons)
        {
            AddEventTriggers(button);
        }
    }

    void AddEventTriggers(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        // Click Sound
        EventTrigger.Entry clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener((eventData) => { OnButtonClick(); });

        // Hover Sound
        EventTrigger.Entry hoverEntry = new EventTrigger.Entry();
        hoverEntry.eventID = EventTriggerType.PointerEnter;
        hoverEntry.callback.AddListener((eventData) => { OnButtonHover(); });

        trigger.triggers.Add(clickEntry);
        trigger.triggers.Add(hoverEntry);
    }

    private void OnButtonClick()
    {
        musicManager?.PlayClickSound();
    }

    private void OnButtonHover()
    {
        musicManager?.PlayHoverSound();
    }
}
