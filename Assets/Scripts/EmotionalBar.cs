using UnityEngine;
using UnityEngine.UI;
public class EmotionalBar : MonoBehaviour
{
    public float emotionValue = 50f;

    public Slider emotionSlider;    // Slider con handle que mueve el círculo
    public Image fillImage;         // Imagen fill para cambiar color
    public Gradient emotionColor;   // Gradiente para el color según emoción

    [Header("Iconos de emoción")]
    public Image iconImage;         // Aquí va el handle Image del slider
    public Sprite verySadIcon;
    public Sprite sadIcon;
    public Sprite neutralIcon;
    public Sprite happyIcon;
    public Sprite veryHappyIcon;


    void Update()
    {
        UpdateUI();
    }
    void Start()
    {
        emotionColor = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[3];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

        colorKeys[0].color = Color.red;    // Triste
        colorKeys[0].time = 0f;
        colorKeys[1].color = Color.yellow; // Intermedio
        colorKeys[1].time = 0.5f;
        colorKeys[2].color = new Color(0f, 0.6f, 0.2f);
        colorKeys[2].time = 1f;

        alphaKeys[0].alpha = 1f;
        alphaKeys[0].time = 0f;
        alphaKeys[1].alpha = 1f;
        alphaKeys[1].time = 1f;

        emotionColor.SetKeys(colorKeys, alphaKeys);
    }
    void UpdateUI()
    {
        // Actualiza el valor del slider (mueve el handle automáticamente)
        emotionSlider.value = emotionValue;

        // Cambia el color del fill
        fillImage.color = emotionColor.Evaluate(emotionValue / 100f);

        // Cambia el sprite del handle (iconImage)
        UpdateIconSprite();
    }

    void UpdateIconSprite()
    {
        if (emotionValue <= 20)
            iconImage.sprite = verySadIcon;
        else if (emotionValue <= 40)
            iconImage.sprite = sadIcon;
        else if (emotionValue <= 60)
            iconImage.sprite = neutralIcon;
        else if (emotionValue <= 80)
            iconImage.sprite = happyIcon;
        else
            iconImage.sprite = veryHappyIcon;
    }

    public void ModifyEmotion(float amount)
    {
        emotionValue = Mathf.Clamp(emotionValue + amount, 0, 100);
    }
}