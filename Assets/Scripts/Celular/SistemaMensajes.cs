using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SistemaMensajes : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Referencias")]
    public GameObject celular;
    public GameObject imagenConBotones;
    public GameObject canvasTransicion;
    public GameObject personaje;
    public GameObject textoDialogo;
    public Transform[] puntosFlashback;
    
    [Header("Referencias de Arrastre")]
    public GameObject miniCelular; // El objeto minicelular que se puede arrastrar
    public GameObject sombraCelular; // La sombra del minicelular

    [Header("Configuración de Escena")]
    public string nombreSiguienteEscena = "SiguienteEscena"; // Nombre de la escena a cargar
    public int indiceSiguienteEscena = -1; // Índice de la escena (usar -1 para usar nombre)

    public float duracionParpadeo = 5f;
    public float intervaloParpadeo = 0.2f;

    private bool yaEntroADiscord = false;
    private bool accionEjecutada = false;
    private Quaternion rotacionCamaraOriginal;

    public float duracionVibracion = 1f;
    public float intensidadVibracion = 10f;
    public float frecuenciaVibracion = 0.05f;

    // Variables para el arrastre
    private Vector3 posicionInicialMiniCelular;
    private Vector3 posicionInicialSombra;
    private bool estaArrastrando = false;
    private Camera camaraUI;

    void Start()
    {
        // Guardar posiciones iniciales
        if (miniCelular != null)
            posicionInicialMiniCelular = miniCelular.transform.position;
        if (sombraCelular != null)
            posicionInicialSombra = sombraCelular.transform.position;

        // Obtener referencia de la cámara (UI o Main)
        camaraUI = Camera.main;
        if (camaraUI == null)
            camaraUI = FindObjectOfType<Camera>();
    }

    public void OnDiscordClick()
    {
        if (yaEntroADiscord) return;

        yaEntroADiscord = true;
        StartCoroutine(VibrarCelular(() =>
        {
            StartCoroutine(EsperarYActivarOpciones(20f));
        }));
    }

    IEnumerator VibrarCelular(System.Action onComplete)
    {
        if (celular == null)
        {
            onComplete?.Invoke();
            yield break;
        }

        Vector3 posicionOriginal = celular.transform.localPosition;
        float tiempo = 0f;

        // También vibrar el minicelular si existe
        Vector3 posicionOriginalMini = miniCelular != null ? miniCelular.transform.position : Vector3.zero;
        Vector3 posicionOriginalSombra = sombraCelular != null ? sombraCelular.transform.position : Vector3.zero;

        while (tiempo < duracionVibracion)
        {
            float offsetX = Random.Range(-1f, 1f) * intensidadVibracion;
            float offsetY = Random.Range(-1f, 1f) * intensidadVibracion;
            
            celular.transform.localPosition = posicionOriginal + new Vector3(offsetX, offsetY, 0f);

            // Vibrar también el minicelular y su sombra
            if (miniCelular != null && !estaArrastrando)
            {
                miniCelular.transform.position = posicionOriginalMini + new Vector3(offsetX * 0.1f, offsetY * 0.1f, 0f);
            }
            if (sombraCelular != null && !estaArrastrando)
            {
                sombraCelular.transform.position = posicionOriginalSombra + new Vector3(offsetX * 0.1f, offsetY * 0.1f, 0f);
            }

            yield return new WaitForSeconds(frecuenciaVibracion);
            tiempo += frecuenciaVibracion;
        }

        celular.transform.localPosition = posicionOriginal;
        
        // Restaurar posiciones del minicelular y sombra si no se están arrastrando
        if (miniCelular != null && !estaArrastrando)
            miniCelular.transform.position = posicionOriginalMini;
        if (sombraCelular != null && !estaArrastrando)
            sombraCelular.transform.position = posicionOriginalSombra;

        onComplete?.Invoke();
    }

    IEnumerator EsperarYActivarOpciones(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        if (imagenConBotones != null)
            imagenConBotones.SetActive(true);
    }

    public void OpcionSeleccionada()
    {
        if (accionEjecutada) return;

        accionEjecutada = true;

        if (celular != null) celular.SetActive(false);
        if (imagenConBotones != null) imagenConBotones.SetActive(false);
        if (textoDialogo != null) textoDialogo.SetActive(false);

        StartCoroutine(FlashbackConParpadeo());
    }

    IEnumerator FlashbackConParpadeo()
    {
        float tiempoTranscurrido = 0f;
        int indicePosicion = 0;

        Camera camara = Camera.main;
        if (camara != null)
            rotacionCamaraOriginal = camara.transform.rotation;

        if (canvasTransicion != null)
            canvasTransicion.SetActive(true);

        while (tiempoTranscurrido < duracionParpadeo)
        {
            if (camara != null)
                camara.transform.rotation = rotacionCamaraOriginal;

            if (canvasTransicion != null)
                canvasTransicion.SetActive(!canvasTransicion.activeSelf);

            if (canvasTransicion.activeSelf && puntosFlashback.Length > 0)
            {
                personaje.transform.position = puntosFlashback[indicePosicion].position;
                indicePosicion = (indicePosicion + 1) % puntosFlashback.Length;
            }

            yield return new WaitForSeconds(intervaloParpadeo);
            tiempoTranscurrido += intervaloParpadeo;
        }

        if (canvasTransicion != null)
            canvasTransicion.SetActive(true);

        // Cambiar a la siguiente escena
        CambiarEscena();
    }

    private void CambiarEscena()
    {
        if (indiceSiguienteEscena >= 0)
        {
            // Usar índice de escena
            SceneManager.LoadScene("Nivel2");
        }
        else if (!string.IsNullOrEmpty("Nivel3"))
        {
            // Usar nombre de escena
            SceneManager.LoadScene("Nivel4");
        }
        else
        {
            Debug.LogWarning("No se ha especificado una escena válida para cargar.");
        }
    }

    // Implementación del arrastre
    public void OnBeginDrag(PointerEventData eventData)
    {
        estaArrastrando = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (miniCelular != null && camaraUI != null)
        {
            Vector3 posicionMundo = camaraUI.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, camaraUI.nearClipPlane + 1f));
            miniCelular.transform.position = posicionMundo;

            // Mover la sombra con un pequeño offset
            if (sombraCelular != null)
            {
                Vector3 offsetSombra = new Vector3(5f, -5f, 0f); // Ajusta este offset según necesites
                sombraCelular.transform.position = posicionMundo + offsetSombra;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        estaArrastrando = false;
        
        // Opcional: volver a la posición original después de soltar
        // StartCoroutine(RegresarAPosicionOriginal());
    }

    // Método opcional para hacer que el minicelular regrese a su posición original
    IEnumerator RegresarAPosicionOriginal()
    {
        float duracion = 0.5f;
        float tiempo = 0f;
        
        Vector3 posicionActualMini = miniCelular.transform.position;
        Vector3 posicionActualSombra = sombraCelular != null ? sombraCelular.transform.position : Vector3.zero;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            if (miniCelular != null)
                miniCelular.transform.position = Vector3.Lerp(posicionActualMini, posicionInicialMiniCelular, t);
            
            if (sombraCelular != null)
                sombraCelular.transform.position = Vector3.Lerp(posicionActualSombra, posicionInicialSombra, t);

            yield return null;
        }

        if (miniCelular != null)
            miniCelular.transform.position = posicionInicialMiniCelular;
        if (sombraCelular != null)
            sombraCelular.transform.position = posicionInicialSombra;
    }
}