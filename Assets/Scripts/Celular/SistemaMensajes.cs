using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SistemaMensajes : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject celularPequeño;               // El icono del celular (puede tener Image)
    public RectTransform fondoCelularPequeño;       // Imagen negra que vibra
    public GameObject alertaNotificacion;           // Icono o imagen de alerta
    public Image imagenCelular;                      // Componente Image del celular para cambiar sprite

    [Header("Configuración")]
    public float intensidadVibracion = 5f;

    private bool notificacionActiva = false;

    // Método para simular llegada de mensaje, solo cambia el sprite del celular
    public void LlegoMensaje(Sprite nuevoSprite)
    {
        if (imagenCelular != null)
        {
            imagenCelular.sprite = nuevoSprite;
        }

        // Vibrar el celular para avisar
        StartCoroutine(VibrarCelular());

        // Activar la notificación visual
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
