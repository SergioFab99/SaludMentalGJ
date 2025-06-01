using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlSlider: MonoBehaviour
{
    public Slider slider; // Referencia al Slider
    public Button aceptarButton; // Referencia al botón Aceptar
    public Button rechazarButton; // Referencia al botón Rechazar

    // Incremento de la barra deslizante
    public float incremento = 0.1f; // La velocidad del incremento
    public float tiempoDeDesplazamiento = 1f; // Tiempo que tardará en llegar al valor final

    private bool aumentando = false; // Controla si se está incrementando el valor
    private bool disminuyendo = false; // Controla si se está disminuyendo el valor

    void Start()
    {
        // Asegurarse de que los botones estén asociados
        if (aceptarButton != null)
        {
            aceptarButton.onClick.AddListener(() => StartCoroutine(IncrementarGradual()));
        }

        if (rechazarButton != null)
        {
            rechazarButton.onClick.AddListener(() => StartCoroutine(DisminuirGradual()));
        }
    }

    // Coroutine para incrementar el valor de la barra de forma gradual
    IEnumerator IncrementarGradual()
    {
        // Si ya se está incrementando, no hacer nada
        if (aumentando || slider.value >= slider.maxValue)
            yield break;

        aumentando = true;

        float valorFinal = Mathf.Min(slider.value + incremento, slider.maxValue); // El valor final hacia el cual se moverá

        while (slider.value < valorFinal)
        {
            slider.value = Mathf.MoveTowards(slider.value, valorFinal, Time.deltaTime / tiempoDeDesplazamiento); // Mueve suavemente hacia el valor final
            yield return null;
        }

        aumentando = false;
    }

    // Coroutine para disminuir el valor de la barra de forma gradual
    IEnumerator DisminuirGradual()
    {
        // Si ya se está disminuyendo, no hacer nada
        if (disminuyendo || slider.value <= slider.minValue)
            yield break;

        disminuyendo = true;

        float valorFinal = Mathf.Max(slider.value - incremento, slider.minValue); // El valor final hacia el cual se moverá

        while (slider.value > valorFinal)
        {
            slider.value = Mathf.MoveTowards(slider.value, valorFinal, Time.deltaTime / tiempoDeDesplazamiento); // Mueve suavemente hacia el valor final
            yield return null;
        }

        disminuyendo = false;
    }
}
