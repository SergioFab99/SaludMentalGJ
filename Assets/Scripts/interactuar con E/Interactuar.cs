using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class InteractableNote : MonoBehaviour
{
    public GameObject indicadorE;          // UI que dice "Presiona E"
    public GameObject panelTexto;          // UI del texto
    public TextMeshProUGUI textoUI;        // TextMeshPro donde se muestra el mensaje
    public float tiempoMostrarTexto = 2f;

    private bool jugadorCerca = false;
    private bool puedeInteractuar = true;
    private bool textoMostrandose = false;
    private int contadorInteracciones = 0;

    public string[] textosInteractuar;     // Lista de textos para cada interacción

    void Start()
    {
        indicadorE.SetActive(false);
        panelTexto.SetActive(false);
    }

    void Update()
    {
        // NO permitir interacción si el celular está activo
        if (CelularUI.celularEstaActivo) return;

        if (jugadorCerca && puedeInteractuar && !textoMostrandose && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(MostrarTexto());
        }
    }

    private System.Collections.IEnumerator MostrarTexto()
    {
        puedeInteractuar = false;
        textoMostrandose = true;
        indicadorE.SetActive(false);

        // Escoger el texto correcto
        string textoAMostrar = (contadorInteracciones < textosInteractuar.Length)
            ? textosInteractuar[contadorInteracciones]
            : textosInteractuar[textosInteractuar.Length - 1];

        textoUI.text = textoAMostrar;
        panelTexto.SetActive(true);

        yield return new WaitForSeconds(tiempoMostrarTexto);

        panelTexto.SetActive(false);
        textoMostrandose = false;
        puedeInteractuar = true;
        contadorInteracciones++;

        // Mostrar la E si el jugador sigue cerca
        if (jugadorCerca)
        {
            indicadorE.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;

            if (puedeInteractuar && !textoMostrandose && !CelularUI.celularEstaActivo)
            {
                indicadorE.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            indicadorE.SetActive(false);
        }
    }
}