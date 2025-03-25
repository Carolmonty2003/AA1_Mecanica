using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;

    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;
        float moveZ = 0f;


        // todo la logica de los inputs de la persona
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

        // logica para cuando te muevas por el mundo con wasd siga a la direccion del raton
        Vector3 moveDirection = transform.right * moveX + transform.up * moveY + transform.forward * moveZ;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        
        // input de cuando se mantiene el click izquierdo
        if (Input.GetMouseButton(0)) 
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed;

            //hacer la rotacion donde este apuntando la camara
            transform.Rotate(Vector3.up, mouseX, Space.World);
            transform.Rotate(Vector3.right, mouseY, Space.Self);
        }
    }
}
