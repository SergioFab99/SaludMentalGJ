using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SistemaMensajes : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject celularPeque�o;               // El icono del celular (puede tener Image)
    public RectTransform fondoCelularPeque�o;       // Imagen negra que vibra
    public GameObject alertaNotificacion;           // Icono o imagen de alerta
    public Image imagenCelular;                      // Componente Image del celular para cambiar sprite

    [Header("Configuraci�n")]
    public float intensidadVibracion = 5f;

    private bool notificacionActiva = false;

    // M�todo para simular llegada de mensaje, solo cambia el sprite del celular
    public void LlegoMensaje(Sprite nuevoSprite)
    {
        if (imagenCelular != null)
        {
            imagenCelular.sprite = nuevoSprite;
        }

        // Vibrar el celular para avisar
        StartCoroutine(VibrarCelular());

        // Activar la notificaci�n visual
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
