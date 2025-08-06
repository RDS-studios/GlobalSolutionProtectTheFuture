using UnityEngine;
using UnityEngine.UI;

public class SecretsScruot : MonoBehaviour
{
    public Image displayImage;              // UI image to display secrets
    public Sprite[] secretSprites;          // All secret images
    public Sprite lockedSprite;             // Image to show when a secret is locked

    private int currentIndex = 0;
    private bool[] unlockedSecrets;

    void Start()
    {
        displayImage.preserveAspect = true;
        unlockedSecrets = new bool[secretSprites.Length];

        // Check each individual secret key
         

        for (int i = 0; i < secretSprites.Length; i++)
        {
            unlockedSecrets[i] = PlayerPrefs.GetInt($"segredo{i}", 0) == 1;
        }

        UpdateImage();

        UpdateImage();
    }

    public void ShowNext()
    {
        currentIndex = (currentIndex + 1) % secretSprites.Length;
        UpdateImage();
    }

    public void ShowPrevious()
    {
        currentIndex = (currentIndex - 1 + secretSprites.Length) % secretSprites.Length;
        UpdateImage();
    }

    private void UpdateImage()
    {
        if (currentIndex < secretSprites.Length)
        {
            if (unlockedSecrets[currentIndex])
            {
                displayImage.sprite = secretSprites[currentIndex];
            }
            else
            {
                displayImage.sprite = lockedSprite;
            }

            displayImage.enabled = true;
        }
        else
        {
            displayImage.enabled = false;
        }
    }
}
