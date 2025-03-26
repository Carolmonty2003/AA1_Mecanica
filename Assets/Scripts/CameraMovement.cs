using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    ///////////////// VARIABLES PUBLICAS ////////////////////////
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float timeScaleIncrement = 0.1f; 
    public float minTimeScale = 0.1f; 
    public float maxTimeScale = 3.0f;


    ///////////////// METODO UPDATE ////////////////////////
    
    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;
        float moveZ = 0f;


        // Inputs del teclado
        foreach (KeyCode key in new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space, KeyCode.LeftShift, KeyCode.P, KeyCode.M })
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
                case KeyCode.P:
                    if (Input.GetKey(key)) moveSpeed += 1f;
                    break;
                case KeyCode.M:
                    if (Input.GetKey(key)) moveSpeed -= 1f;
                    break;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale + timeScaleIncrement, minTimeScale, maxTimeScale);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale - timeScaleIncrement, minTimeScale, maxTimeScale);
        }


        // Logica de movimientoen la simulacion con WASD o seguimiento de raton
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
