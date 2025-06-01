using UnityEngine;

public class CelularUI : MonoBehaviour
{
    public GameObject celularPeque�o;       // Icono abajo a la derecha
    public GameObject celularGrande;        // Panel completo del celular
    public GameObject[] pesta�as;           // Paneles de pesta�as

    public GameObject canvasMarco;          // Marco del celular (siempre al frente)
    public GameObject[] otrosCanvas;        // Otros canvas que se deben ocultar al abrir el celular
    public GameObject imagenFondoOscuro;    // Imagen negra detr�s del celular peque�o

    public GameObject player;               // Objeto del jugador
    public MonoBehaviour movimientoJugador; // Script de movimiento (FPS)
    public MonoBehaviour controlCamara;     // Script de c�mara (FPS)

    private bool celularActivo = false;
    public static bool celularEstaActivo = false;

    void Start()
    {
        celularGrande.SetActive(false);
        celularPeque�o.SetActive(true);
        if (canvasMarco != null) canvasMarco.SetActive(false);
        if (imagenFondoOscuro != null) imagenFondoOscuro.SetActive(true); // Fondo solo detr�s del celular chico

        foreach (GameObject pesta�a in pesta�as)
        {
            pesta�a.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            AlternarCelular();
        }
    }

    public void AlternarCelular()
    {
        celularActivo = !celularActivo;
        celularEstaActivo = celularActivo;

        celularGrande.SetActive(celularActivo);
        celularPeque�o.SetActive(!celularActivo);

        if (canvasMarco != null) canvasMarco.SetActive(celularActivo);
        if (imagenFondoOscuro != null) imagenFondoOscuro.SetActive(!celularActivo); // Solo aparece con el celular chico

        foreach (GameObject c in otrosCanvas)
        {
            if (c != null) c.SetActive(!celularActivo);
        }

        if (celularActivo)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (movimientoJugador != null) movimientoJugador.enabled = false;
            if (controlCamara != null) controlCamara.enabled = false;

            ActivarPesta�a(0); // Abrir la pesta�a principal
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (movimientoJugador != null) movimientoJugador.enabled = true;
            if (controlCamara != null) controlCamara.enabled = true;

            foreach (GameObject pesta�a in pesta�as)
            {
                pesta�a.SetActive(false);
            }
        }
    }

    public void ActivarPesta�a(int index)
    {
        for (int i = 0; i < pesta�as.Length; i++)
        {
            pesta�as[i].SetActive(i == index);
        }
    }

    public void BotonCerrarCelular()
    {
        if (celularActivo)
        {
            AlternarCelular();
        }
    }

    public void BotonInicio()
    {
        ActivarPesta�a(0); // Vuelve a la pesta�a 0
    }

    public void CerrarAplicacionActual()
    {
        // Cierra solo la "app" actual dentro del celular y vuelve a la pesta�a de inicio
        ActivarPesta�a(0);
    }
}