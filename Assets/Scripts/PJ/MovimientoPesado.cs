using UnityEngine;

public class MovimientoPesado : MonoBehaviour
{
    public float velocidad = 5f;
    public float sensibilidadMouse = 2f;
    public Transform camaraJugador;
    public PensamientosTristes pensamientosTristes;
    public AudioSource audioPasos;

    private CharacterController controlador;
    private float rotacionX = 0f;
    private float velocidadVertical = 0f;
    public float gravedad = -9.81f;
    public static bool cinemáticaActiva = false;

    void Start()
    {
        controlador = GetComponent<CharacterController>();
        if (cinemáticaActiva) return;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (audioPasos != null)
        {
            audioPasos.loop = true;
        }
    }

    void Update()
    {
        if (cinemáticaActiva)
        {
            float mouseX = Input.GetAxis("Mouse X") * (sensibilidadMouse * 0.1f);
            float mouseY = Input.GetAxis("Mouse Y") * (sensibilidadMouse * 0.1f);

            rotacionX -= mouseY;
            rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

            camaraJugador.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);

            if (audioPasos != null && audioPasos.isPlaying)
                audioPasos.Stop();
            return;
        }

        // Camera rotation
        float mouseXNormal = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseYNormal = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        rotacionX -= mouseYNormal;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        camaraJugador.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseXNormal);

        // Movement
        float movX = Input.GetAxis("Horizontal");
        float movZ = Input.GetAxis("Vertical");

        Vector3 direccion = transform.right * movX + transform.forward * movZ;
        direccion *= velocidad;

        // Gravity
        if (controlador.isGrounded)
        {
            if (velocidadVertical < 0)
                velocidadVertical = -2f;
        }
        else
        {
            velocidadVertical += gravedad * Time.deltaTime;
        }

        direccion.y = velocidadVertical;

        // Check if player is interacting or canvas is shown
        bool isInteracting = Input.GetKey(KeyCode.E) || (pensamientosTristes != null && pensamientosTristes.PensamientoMostrandose);

        if (movX != 0f || movZ != 0f)
        {
            if (!isInteracting)
            {
                controlador.Move(direccion * Time.deltaTime);

                // Play footsteps only if moving and not interacting
                if (audioPasos != null && !audioPasos.isPlaying)
                {
                    audioPasos.Play();
                }
            }
            else
            {
                // Stop footsteps during interaction or canvas display
                if (audioPasos != null && audioPasos.isPlaying)
                {
                    audioPasos.Stop();
                }
            }
        }
        else
        {
            // Stop footsteps when not moving
            if (audioPasos != null && audioPasos.isPlaying)
            {
                audioPasos.Stop();
            }
        }
    }
}