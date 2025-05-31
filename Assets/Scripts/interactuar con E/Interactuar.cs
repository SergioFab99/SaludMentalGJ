using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class InteractableNote : MonoBehaviour
{
    [TextArea(2, 4)]
    public string[] pensamientosPosibles;
    public PensamientosTristes pensamientos;
    public GameObject indicadorE;

    private bool jugadorCerca = false;

    private void Update()
    {
        // Si no hay jugador cerca o si el celular está activo o si ya hay pensamiento en pantalla, oculta el indicador y no permita usar E
        if (!jugadorCerca || CelularUI.celularEstaActivo || pensamientos.PensamientoMostrandose)
        {
            if (indicadorE.activeSelf) indicadorE.SetActive(false);
            return;
        }
        else
        {
            if (!indicadorE.activeSelf) indicadorE.SetActive(true);
        }

        // Detecta la tecla E y solo si no hay pensamiento en pantalla
        if (Input.GetKeyDown(KeyCode.E) && !pensamientos.PensamientoMostrandose)
        {
            if (pensamientosPosibles.Length > 0)
            {
                string pensamientoRandom = pensamientosPosibles[Random.Range(0, pensamientosPosibles.Length)];
                pensamientos.MostrarPensamiento(pensamientoRandom);
                indicadorE.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
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