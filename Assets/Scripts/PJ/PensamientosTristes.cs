using UnityEngine;
using TMPro;
using System.Collections;

public class PensamientosTristes : MonoBehaviour
{
    public GameObject panelDePensamientos;         // Panel que contiene el texto
    public TextMeshProUGUI textoPensamiento;       // Texto que muestra el mensaje
    public float tiempoVisible = 4f;                // Tiempo que el texto se mantiene visible después de escribirlo
    public float velocidadEscritura = 0.05f;       // Velocidad de la animación de escritura

    private Coroutine pensamientoActual;
    private bool pensamientoMostrandose = false;
    public bool PensamientoMostrandose => pensamientoMostrandose;

    public void MostrarPensamiento(string mensaje)
    {
        if (pensamientoActual != null)
            StopCoroutine(pensamientoActual);

        pensamientoActual = StartCoroutine(MostrarConEscritura(mensaje));
    }

    private IEnumerator MostrarConEscritura(string mensaje)
    {
        pensamientoMostrandose = true;

        panelDePensamientos.SetActive(true);
        textoPensamiento.text = "";

        foreach (char letra in mensaje)
        {
            textoPensamiento.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        yield return new WaitForSeconds(tiempoVisible);

        panelDePensamientos.SetActive(false);
        pensamientoMostrandose = false;
    }
}