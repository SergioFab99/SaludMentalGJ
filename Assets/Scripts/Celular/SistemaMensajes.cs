using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SistemaMensajes : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject panelOpciones;
    public GameObject celular;
    public GameObject canvasTransicion; // Canvas negro para parpadeo
    public GameObject personaje;
    public GameObject textoDialogo; // Texto que se desactiva en la transici�n

    public Transform[] puntosFlashback; // Puntos donde el personaje aparecer�
    public float duracionParpadeo = 5f;
    public float intervaloParpadeo = 0.2f;

    private bool accionEjecutada = false;
    private Quaternion rotacionCamaraOriginal;

    public void OpcionSeleccionada()
    {
        if (accionEjecutada) return;
        accionEjecutada = true;

        panelOpciones.SetActive(false);
        celular.SetActive(false);

        StartCoroutine(FlashbackConParpadeo());
    }

    IEnumerator FlashbackConParpadeo()
    {
        float tiempoTranscurrido = 0f;
        int indicePosicion = 0;

        // Guardar rotaci�n original de la c�mara
        Camera camara = Camera.main;
        if (camara != null)
            rotacionCamaraOriginal = camara.transform.rotation;

        // Desactivar texto si est� activo
        if (textoDialogo != null)
            textoDialogo.SetActive(false);

        // Asegurar que el canvas est� activo para iniciar
        canvasTransicion.SetActive(true);

        while (tiempoTranscurrido < duracionParpadeo)
        {
            // Fijar rotaci�n de la c�mara hacia adelante
            if (camara != null)
                camara.transform.rotation = rotacionCamaraOriginal;

            // Alternar visibilidad para parpadeo
            canvasTransicion.SetActive(!canvasTransicion.activeSelf);

            // Mover al personaje si el canvas est� activo
            if (canvasTransicion.activeSelf && puntosFlashback.Length > 0)
            {
                personaje.transform.position = puntosFlashback[indicePosicion].position;
                indicePosicion = (indicePosicion + 1) % puntosFlashback.Length;
            }

            yield return new WaitForSeconds(intervaloParpadeo);
            tiempoTranscurrido += intervaloParpadeo;
        }

        // Al final, asegurar que el canvas quede activo
        canvasTransicion.SetActive(true);

        // Si quieres volver a activar el texto o continuar historia, hazlo aqu�
        // textoDialogo.SetActive(true); (si lo deseas m�s adelante)

        accionEjecutada = false;
    }
}