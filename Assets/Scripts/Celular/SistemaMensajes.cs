using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SistemaMensajes : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject celularPeque�o;               // El icono del celular
    public RectTransform fondoCelularPeque�o;       // Imagen negra que vibra
    public GameObject alertaNotificacion;           // Icono o imagen de alerta
    public Transform contenedorMensajes;            // Donde se colocan los mensajes
    public GameObject mensajePrefab;                // Prefab con TextMeshProUGUI

    [Header("Configuraci�n")]
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
        // Vibraci�n
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

        // Eliminar el mensaje m�s antiguo si se excede el m�ximo
        if (contenedorMensajes.childCount > maxMensajes)
        {
            Destroy(contenedorMensajes.GetChild(0).gameObject);
        }

        // Activar notificaci�n
        alertaNotificacion.SetActive(true);
        notificacionActiva = true;
    }

    IEnumerator VibrarCelular(float duracion = 0.3f)
    {
        Vector3 posOriginalFondo = fondoCelularPeque�o.localPosition;
        Vector3 posOriginalCelular = celularPeque�o.transform.localPosition;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            float x = Random.Range(-intensidadVibracion, intensidadVibracion);
            float y = Random.Range(-intensidadVibracion, intensidadVibracion);
            Vector3 offset = new Vector3(x, y, 0);

            fondoCelularPeque�o.localPosition = posOriginalFondo + offset;
            celularPeque�o.transform.localPosition = posOriginalCelular + offset;

            tiempo += Time.deltaTime;
            yield return null;
        }

        fondoCelularPeque�o.localPosition = posOriginalFondo;
        celularPeque�o.transform.localPosition = posOriginalCelular;
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
