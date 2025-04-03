using UnityEngine;

public class MoonMovement_RK4_Simple : MonoBehaviour
{
    public Transform earth;
    public float earthMass = 3e-6f; // Masa de la Tierra en masas solares
    public float initialDistance = 0.0027f; // UA (384,400 km = 0.0027 UA)
    public float gravityConstant = 39.478f; // Constante gravitacional en UA³/(año²·masa solar)
    public float velocityScaleFactor = 1.0f; // Ajusta si necesitas órbitas más rápidas

    private Vector3 relativePosition;
    private Vector3 relativeVelocity;
    private Vector3 previousPosition;

    void Start()
    {
        // Posición inicial relativa a la Tierra (eje X)
        relativePosition = new Vector3(initialDistance, 0, 0);
        transform.position = earth.position + relativePosition;

        // Velocidad orbital inicial (eje Z para plano X-Z)
        float orbitalSpeed = Mathf.Sqrt(gravityConstant * earthMass / initialDistance) * velocityScaleFactor;
        relativeVelocity = new Vector3(0, 0, orbitalSpeed);
        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime / 31557600f; // dt en años

        // 1. Integrar movimiento relativo con RK4
        RungeKuttaStep(dt);

        // 2. Actualizar posición global (siguiendo a la Tierra)
        transform.position = earth.position + relativePosition;

        Debug.DrawLine(previousPosition, transform.position, Color.cyan, 5f);
        previousPosition = transform.position;
        Debug.DrawLine(earth.position, transform.position, Color.yellow, 0.1f);
    }

    private void RungeKuttaStep(float dt)
    {
        Vector3 pos = relativePosition;
        Vector3 vel = relativeVelocity;

        Vector3 k1v = CalculateEarthGravity(pos);
        Vector3 k1x = vel;

        Vector3 k2v = CalculateEarthGravity(pos + 0.5f * dt * k1x);
        Vector3 k2x = vel + 0.5f * dt * k1v;

        Vector3 k3v = CalculateEarthGravity(pos + 0.5f * dt * k2x);
        Vector3 k3x = vel + 0.5f * dt * k2v;

        Vector3 k4v = CalculateEarthGravity(pos + dt * k3x);
        Vector3 k4x = vel + dt * k3v;

        relativePosition += dt * (k1x + 2f * k2x + 2f * k3x + k4x) / 6f;
        relativeVelocity += dt * (k1v + 2f * k2v + 2f * k3v + k4v) / 6f;
    }

    private Vector3 CalculateEarthGravity(Vector3 relativePos)
    {
        // Solo gravedad de la Tierra (simplificado)
        return (-relativePos).normalized * (gravityConstant * earthMass) / relativePos.sqrMagnitude;
    }
}