using UnityEngine;

public class MovimientoPesado : MonoBehaviour
{
    public float velocidad = 5f;
    public float sensibilidadMouse = 2f;
    public Transform camaraJugador;
    public PensamientosTristes pensamientosTristes; // Asegúrate de arrastrar el componente en el Inspector

    public AudioSource audioPasos; // AudioSource para sonidos de pasos

    private CharacterController controlador;
    private float rotacionX = 0f;

    private bool primerIntento = true;

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
            audioPasos.loop = true; // Para que el sonido se repita mientras caminas
        }
    }

    void Update()
    {
        if (cinemáticaActiva)
        {
            // Sensibilidad mucho menor para cámara
            float mouseX = Input.GetAxis("Mouse X") * (sensibilidadMouse * 0.1f);
            float mouseY = Input.GetAxis("Mouse Y") * (sensibilidadMouse * 0.1f);

            rotacionX -= mouseY;
            rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

            camaraJugador.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);

            // Bloquear movimiento y detener sonidos pasos
            if (audioPasos != null && audioPasos.isPlaying)
                audioPasos.Stop();

            // Bloquear movimiento
            return;
        }

        // Código normal para mover y rotar con sensibilidad normal
        // Cámara con el mouse
        float mouseXNormal = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseYNormal = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        rotacionX -= mouseYNormal;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        camaraJugador.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseXNormal);

        // Movimiento con WASD
        float movX = Input.GetAxis("Horizontal");
        float movZ = Input.GetAxis("Vertical");

        Vector3 direccion = transform.right * movX + transform.forward * movZ;
        direccion *= velocidad;

        // Aplicar gravedad
        if (controlador.isGrounded)
        {
            if (velocidadVertical < 0)
                velocidadVertical = -2f; // Mantener al personaje pegado al suelo
        }
        else
        {
            velocidadVertical += gravedad * Time.deltaTime;
        }

        direccion.y = velocidadVertical;

        if (movX != 0f || movZ != 0f)
        {
            if (primerIntento)
            {
                primerIntento = false;
                if (pensamientosTristes != null)
                {
                    pensamientosTristes.MostrarPensamiento("¿Realmente tengo que moverme?");
                }
                return; // Bloquea movimiento la primera vez
            }

            controlador.Move(direccion * Time.deltaTime);

            // Reproducir sonido pasos si no está sonando
            if (audioPasos != null && !audioPasos.isPlaying)
            {
                audioPasos.Play();
            }
        }
        else
        {
            // Detener sonido pasos cuando no hay movimiento y también si se abre el panel de pensamientos
            if (pensamientosTristes != null && pensamientosTristes.PensamientoMostrandose)
            {
                return; // No detener sonido si se está mostrando un pensamiento
            }
            if (audioPasos != null && audioPasos.isPlaying)
            {
                audioPasos.Stop();
            }
        }
    }
}
    