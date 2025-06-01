using UnityEngine;

public class TestingCursor : MonoBehaviour
{
    // Arrastra aquí el Canvas que quieres que controle el cursor desde el Inspector
    public Canvas uiCanvas;

    void Start()
    {
        // Asegúrate de que el Canvas esté asignado.
        if (uiCanvas == null)
        {
            Debug.LogWarning("Canvas no asignado en el TestingCursor. Por favor, asigna un Canvas en el Inspector.");
            return;
        }

        // Desactiva el GameObject que contiene el Canvas al inicio del juego.
        // Esto significa que la UI no será visible y el cursor estará bloqueado/invisible por defecto.
        uiCanvas.gameObject.SetActive(false);
        // Aseguramos el estado inicial del cursor si el Canvas está inactivo.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Si el Canvas no está asignado, muestra una advertencia y sale.
        if (uiCanvas == null)
        {
            // La advertencia ya se mostró en Start(), pero es bueno tenerla aquí también por si acaso.
            return;
        }

        // Comprueba si se presiona la tecla "E".
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Alterna el estado activo del GameObject del Canvas.
            // Si estaba inactivo, se activa. Si estaba activo, se desactiva.
            uiCanvas.gameObject.SetActive(!uiCanvas.gameObject.activeInHierarchy);
        }

        // Esta parte del código se encarga de controlar el estado del cursor
        // basándose en si el GameObject del Canvas está activo o inactivo.
        if (uiCanvas.gameObject.activeInHierarchy)
        {
            // Si el Canvas está activo/visible: desbloquea el cursor y hazlo visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Si el Canvas no está activo/visible: bloquea el cursor y hazlo invisible
            // Esto es típico para los controles de cámara en primera/tercera persona
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
