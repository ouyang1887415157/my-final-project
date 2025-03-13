using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;          // Main AudioSource

    public AudioSource audioSource2;         // Second AudioSource

    public AudioClip backgroundMusic1; // Background music

    public AudioClip backgroundMusic2;

    public AudioClip clickSound;  // Battle music

    public AudioClip hoverSound; // The sound of hover

    public AudioClip buyingSound;

    public AudioClip hurtSound;

    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayBackgroundMusic1(); // Start playing the background music

        BigBoardProperties gameManager = FindFirstObjectByType<BigBoardProperties>();
        
        if(gameManager != null)
        {
            Debug.Log("Find GameManager");

            gameManager.GameHasStarted += OnGameStarted;
        }

    }
    // Event handler for when the game starts
    private void OnGameStarted()
    {
        PlayBackgroundMusic2(); // Play the second background music
    }


    // Play the background music and set it to loop
    public void PlayBackgroundMusic1()
    {
        if (audioSource.clip != backgroundMusic1)
        {
            audioSource.clip = backgroundMusic1; // Set to background music
            audioSource.loop = true;              // Set to loop
            audioSource.Play();                    // Play the background music
        }
    }

    // Play the background music and set it to loop
    public void PlayBackgroundMusic2()
    {
        if (audioSource.clip != backgroundMusic2)
        {
            audioSource.clip = backgroundMusic2; // Set to background music
            audioSource.loop = true;              // Set to loop
            audioSource.Play();                    // Play the background music
        }
    }

    // Play Click Sound
    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound,5.0f);
        }
    }

    // Play Hover Sound
    public void PlayHoverSound()
    {
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound,5.0f);
        }
    }

    // Play Buying Sound
    public void PlayBuyingSound()
    {
        if (buyingSound != null)
        {
            audioSource.PlayOneShot(buyingSound, 3.0f);
        }
    }

    // Play Buying Sound
    public void PlayHurtSound()
    {
        if (hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound, 3.0f);
        }
    }

}
