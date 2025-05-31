using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioSource audioSource;
    public Button button;

    void Start()
    {
        if (button != null && audioSource != null)
        {
            button.onClick.AddListener(PlaySound);
        }
        else
        {
            Debug.LogWarning("Asigna el botón y el AudioSource en el inspector.");
        }
    }

    void PlaySound()
    {
        audioSource.Play();
    }
}
