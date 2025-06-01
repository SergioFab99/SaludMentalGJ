using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SistemaMensajes : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject celularPequeño;               // El icono del celular
    public RectTransform fondoCelularPequeño;       // Imagen negra que vibra
    public GameObject alertaNotificacion;           // Icono o imagen de alerta
    public Transform contenedorMensajes;            // Donde se colocan los mensajes
    public GameObject mensajePrefab;                // Prefab con TextMeshProUGUI

    [Header("Configuración")]
    public float intensidadVibracion = 5f;
    public int maxMensajes = 5;

    [Header("Mensajes Programados")]
    public List<MensajeProgramado> mensajesProgramados = new List<MensajeProgramado>();

    private bool notificacionActiva = false;

    [System.Serializable]
    public class MensajeProgramado
    {
        public string contenido;
        public float tiempoDeLlegada; // En segundos
    }

    void Start()
    {
        alertaNotificacion.SetActive(false);

        // Iniciar cada mensaje programado
        foreach (MensajeProgramado mensaje in mensajesProgramados)
        {
            StartCoroutine(EnviarMensajeEnTiempo(mensaje));
        }
    }

    IEnumerator EnviarMensajeEnTiempo(MensajeProgramado mensaje)
    {
        yield return new WaitForSeconds(mensaje.tiempoDeLlegada);
        LlegoMensaje(mensaje.contenido);
    }

    public void LlegoMensaje(string contenido)
    {
        // Vibración
        StartCoroutine(VibrarCelular());

        // Crear nuevo mensaje
        GameObject nuevo = Instantiate(mensajePrefab, contenedorMensajes);
        TextMeshProUGUI texto = nuevo.GetComponentInChildren<TextMeshProUGUI>();
        if (texto != null)
        {
            texto.text = contenido;
        }
        else
        {
            Debug.LogWarning("El prefab de mensaje no tiene un TextMeshProUGUI.");
        }

        // Eliminar el mensaje más antiguo si se excede el máximo
        if (contenedorMensajes.childCount > maxMensajes)
        {
            Destroy(contenedorMensajes.GetChild(0).gameObject);
        }

        // Activar notificación
        alertaNotificacion.SetActive(true);
        notificacionActiva = true;
    }

    IEnumerator VibrarCelular(float duracion = 0.3f)
    {
        Vector3 posOriginalFondo = fondoCelularPequeño.localPosition;
        Vector3 posOriginalCelular = celularPequeño.transform.localPosition;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            float x = Random.Range(-intensidadVibracion, intensidadVibracion);
            float y = Random.Range(-intensidadVibracion, intensidadVibracion);
            Vector3 offset = new Vector3(x, y, 0);

            fondoCelularPequeño.localPosition = posOriginalFondo + offset;
            celularPequeño.transform.localPosition = posOriginalCelular + offset;

            tiempo += Time.deltaTime;
            yield return null;
        }

        fondoCelularPequeño.localPosition = posOriginalFondo;
        celularPequeño.transform.localPosition = posOriginalCelular;
    }

    public void JugadorAbrioCelular()
    {
        if (notificacionActiva)
        {
            alertaNotificacion.SetActive(false);
            notificacionActiva = false;
        }
    }
}
