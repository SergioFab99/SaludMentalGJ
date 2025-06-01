using UnityEngine;

public class MovimientoPesado : MonoBehaviour
{
    public float velocidad = 5f;
    public float sensibilidadMouse = 2f;
    public Transform camaraJugador;

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

        controlador.Move(direccion * Time.deltaTime);
    }
}