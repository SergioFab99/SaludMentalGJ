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

        // Selección del texto según el número de interacciones
        string textoAMostrar = "Nada que decir...";
        if (contadorInteracciones < textosInteractuar.Length)
        {
            textoAMostrar = textosInteractuar[contadorInteracciones];
        }
        else
        {
            textoAMostrar = textosInteractuar[textosInteractuar.Length - 1]; // Último texto
        }

        textoUI.text = textoAMostrar;
        panelTexto.SetActive(true);

        yield return new WaitForSeconds(tiempoMostrarTexto);

        panelTexto.SetActive(false);
        textoMostrandose = false;
        puedeInteractuar = true;
        contadorInteracciones++;

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

            if (puedeInteractuar && !textoMostrandose)
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