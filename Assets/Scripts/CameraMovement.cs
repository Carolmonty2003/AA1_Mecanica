using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    ///////////////// VARIABLES PUBLICAS ////////////////////////
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;

    ///////////////// METODO UPDATE ////////////////////////
    // Manejo de camara con inputs
    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;
        float moveZ = 0f;


        // Posibles inputs del teclado
        foreach (KeyCode key in new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space, KeyCode.LeftShift })
        {
            switch (key)
            {
                case KeyCode.W:
                    if (Input.GetKey(key)) moveZ = 1f;
                    break;
                case KeyCode.S:
                    if (Input.GetKey(key)) moveZ = -1f;
                    break;
                case KeyCode.A:
                    if (Input.GetKey(key)) moveX = -1f;
                    break;
                case KeyCode.D:
                    if (Input.GetKey(key)) moveX = 1f;
                    break;
                case KeyCode.Space:
                    if (Input.GetKey(key)) moveY = 1f;
                    break;
                case KeyCode.LeftShift:
                    if (Input.GetKey(key)) moveY = -1f;
                    break;
            }
        }

        // Logica de movimientoen la simulación con WASD o seguimiento de ratón
        Vector3 moveDirection = transform.right * moveX + transform.up * moveY + transform.forward * moveZ;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        
        // Input click izquierdo mateniendose presionado
        if (Input.GetMouseButton(0)) 
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;

            // Rotacion desde donde este apuntando la camara
            transform.Rotate(Vector3.up, mouseX, Space.World);
            transform.Rotate(Vector3.right, mouseY, Space.Self);
        }
    }
}
