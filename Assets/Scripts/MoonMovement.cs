using UnityEngine;

public class MoonMovement_RK4 : MonoBehaviour
{
    [Header("Referencias")]
    public Transform earth; // Asignar desde el Editor
    public Transform sun;   // Asignar desde el Editor

    [Header("Parámetros Tierra-Luna")]
    public float earthMass = 3e-6f; // Masa de la Tierra (en masas solares)
    public float initialDistance = 0.0027f; // UA (distancia Tierra-Luna)
    public float gravityConstant = 39.478f; // UA³/(año²·masa solar)

    [Header("Variables de Estado")]
    private Vector3 relativePosition; // Posición relativa a la Tierra
    private Vector3 relativeVelocity; // Velocidad relativa a la Tierra
    private Vector3 previousPosition;

    void Start()
    {
        // Posición inicial relativa a la Tierra (eje X)
        relativePosition = new Vector3(initialDistance, 0, 0);
        transform.position = earth.position + relativePosition;

        // Velocidad orbital inicial (dirección Z para plano X-Z)
        float orbitalSpeed = Mathf.Sqrt(gravityConstant * earthMass / initialDistance);
        relativeVelocity = new Vector3(0, 0, orbitalSpeed);

        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        // 1. Capturar posición actual de la Tierra al inicio del paso
        Vector3 earthCurrentPos = earth.position;

        // 2. Integrar movimiento relativo usando RK4 (dt en años)
        float dt = Time.fixedDeltaTime / 31557600f;
        RungeKuttaStep(dt, earthCurrentPos);

        // 3. Actualizar posición global de la Luna
        transform.position = earthCurrentPos + relativePosition;

        // 4. Dibujar trayectoria
        Debug.DrawLine(previousPosition, transform.position, Color.cyan, 5f);
        previousPosition = transform.position;
        Debug.DrawLine(earthCurrentPos, transform.position, Color.yellow, 0.1f);
    }

    private void RungeKuttaStep(float dt, Vector3 earthPos)
    {
        // Posición y velocidad actuales (relativas)
        Vector3 pos = relativePosition;
        Vector3 vel = relativeVelocity;

        // Calcular coeficientes RK4
        Vector3 k1v = CalculateAcceleration(pos, earthPos);
        Vector3 k1x = vel;

        Vector3 k2v = CalculateAcceleration(pos + 0.5f * dt * k1x, earthPos);
        Vector3 k2x = vel + 0.5f * dt * k1v;

        Vector3 k3v = CalculateAcceleration(pos + 0.5f * dt * k2x, earthPos);
        Vector3 k3x = vel + 0.5f * dt * k2v;

        Vector3 k4v = CalculateAcceleration(pos + dt * k3x, earthPos);
        Vector3 k4x = vel + dt * k3v;

        // Actualizar posición y velocidad relativas
        relativePosition += dt * (k1x + 2 * k2x + 2 * k3x + k4x) / 6f;
        relativeVelocity += dt * (k1v + 2 * k2v + 2 * k3v + k4v) / 6f;
    }

    private Vector3 CalculateAcceleration(Vector3 relativePos, Vector3 earthPos)
    {
        // Aceleración debida a la Tierra (dirección relativa)
        Vector3 earthAcceleration = (-relativePos).normalized * (gravityConstant * earthMass) / relativePos.sqrMagnitude;

        // Aceleración debida al Sol (dirección global)
        Vector3 moonGlobalPos = earthPos + relativePos;
        Vector3 sunDirection = sun.position - moonGlobalPos;
        Vector3 sunAcceleration = sunDirection.normalized * (gravityConstant * sun.GetComponent<PlanetMovement>().massSun) / sunDirection.sqrMagnitude;

        return earthAcceleration + sunAcceleration;
    }
}