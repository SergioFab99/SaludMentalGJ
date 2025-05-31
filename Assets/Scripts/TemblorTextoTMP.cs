using UnityEngine;

public class TemblorTextoTMP : MonoBehaviour
{
    public float intensidad = 5f;   // Cuánto se mueve
    public float velocidad = 50f;   // Qué tan rápido tiembla

    private RectTransform rectTransform;
    private Vector2 posicionInicial;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        posicionInicial = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float offsetX = Mathf.PerlinNoise(Time.time * velocidad, 0f) * 2 - 1;
        float offsetY = Mathf.PerlinNoise(0f, Time.time * velocidad) * 2 - 1;

        rectTransform.anchoredPosition = posicionInicial + new Vector2(offsetX, offsetY) * intensidad;
    }

    void OnDestroy()
    {
        // Restaurar por si acaso
        if (rectTransform != null)
            rectTransform.anchoredPosition = posicionInicial;
    }
}