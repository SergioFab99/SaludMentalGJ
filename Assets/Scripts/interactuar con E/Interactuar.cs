using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InteractableNote : MonoBehaviour
{
    public GameObject indicadorE;       // UI que dice "Presiona E"
    public GameObject panelTexto;       // UI del texto que se muestra al interactuar
    public float tiempoMostrarTexto = 2f;

    private bool jugadorCerca = false;
    private bool puedeInteractuar = true;
    private bool textoMostrandose = false;

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
        panelTexto.SetActive(true);

        yield return new WaitForSeconds(tiempoMostrarTexto);

        panelTexto.SetActive(false);
        textoMostrandose = false;
        puedeInteractuar = true;

        // Si el jugador aún está cerca, vuelve a aparecer la E
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

            // Solo mostrar la E si se puede interactuar y no hay texto mostrándose
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