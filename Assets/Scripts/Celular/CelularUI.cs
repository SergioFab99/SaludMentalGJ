using UnityEngine;

public class CelularUI : MonoBehaviour
{
    public GameObject celularPequeño;       // Icono abajo a la derecha
    public GameObject celularGrande;        // Panel completo del celular
    public GameObject[] pestañas;           // Paneles de pestañas

    public GameObject canvasMarco;          // Marco del celular (siempre al frente)
    public GameObject[] otrosCanvas;        // Otros canvas que se deben ocultar al abrir el celular
    public GameObject imagenFondoOscuro;    // Imagen negra detrás del celular pequeño

    public GameObject player;               // Objeto del jugador
    public MonoBehaviour movimientoJugador; // Script de movimiento (FPS)
    public MonoBehaviour controlCamara;     // Script de cámara (FPS)

    private bool celularActivo = false;
    public static bool celularEstaActivo = false;

    void Start()
    {
        celularGrande.SetActive(false);
        celularPequeño.SetActive(true);
        if (canvasMarco != null) canvasMarco.SetActive(false);
        if (imagenFondoOscuro != null) imagenFondoOscuro.SetActive(true); // Fondo solo detrás del celular chico

        foreach (GameObject pestaña in pestañas)
        {
            pestaña.SetActive(false);
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
        celularPequeño.SetActive(!celularActivo);

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

            ActivarPestaña(0); // Abrir la pestaña principal
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (movimientoJugador != null) movimientoJugador.enabled = true;
            if (controlCamara != null) controlCamara.enabled = true;

            foreach (GameObject pestaña in pestañas)
            {
                pestaña.SetActive(false);
            }
        }
    }

    public void ActivarPestaña(int index)
    {
        for (int i = 0; i < pestañas.Length; i++)
        {
            pestañas[i].SetActive(i == index);
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
        ActivarPestaña(0); // Vuelve a la pestaña 0
    }

    public void CerrarAplicacionActual()
    {
        // Cierra solo la "app" actual dentro del celular y vuelve a la pestaña de inicio
        ActivarPestaña(0);
    }
}