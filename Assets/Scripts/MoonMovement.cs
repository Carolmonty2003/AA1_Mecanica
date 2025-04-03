using UnityEngine;

public class MoonMovement_RK4 : MonoBehaviour
{
    public Transform earth;
    public Transform sun; // Nuevo: Referencia al Sol
    public float earthMass = 3e-6f;
    public float sunMass = 1f; // Nuevo: Masa del Sol
    public float initialDistance = 0.0027f;
    public float gravityConstant = 39.478f;

    private Vector3 relativePosition;
    private Vector3 relativeVelocity;
    private Vector3 previousPosition;

    void Start()
    {
        relativePosition = new Vector3(initialDistance, 0, 0);
        transform.position = earth.position + relativePosition;

        float idealSpeed = Mathf.Sqrt(gravityConstant * earthMass / initialDistance);
        relativeVelocity = new Vector3(0, idealSpeed, 0);

        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime / 31557600f;
        RungeKuttaStep(dt);
        transform.position = earth.position + relativePosition;

        Debug.DrawLine(previousPosition, transform.position, Color.cyan, 5f);
        previousPosition = transform.position;
        Debug.DrawLine(earth.position, transform.position, Color.yellow, 0.1f);
    }

    private void RungeKuttaStep(float dt)
    {
        Vector3 pos = relativePosition;
        Vector3 vel = relativeVelocity;

        Vector3 k1v = CalculateAcceleration(pos);
        Vector3 k1x = vel;

        Vector3 k2v = CalculateAcceleration(pos + 0.5f * dt * k1x);
        Vector3 k2x = vel + 0.5f * dt * k1v;

        Vector3 k3v = CalculateAcceleration(pos + 0.5f * dt * k2x);
        Vector3 k3x = vel + 0.5f * dt * k2v;

        Vector3 k4v = CalculateAcceleration(pos + dt * k3x);
        Vector3 k4x = vel + dt * k3v;

        relativePosition += dt * (k1x + 2 * k2x + 2 * k3x + k4x) / 6f;
        relativeVelocity += dt * (k1v + 2 * k2v + 2 * k3v + k4v) / 6f;
    }

    private Vector3 CalculateAcceleration(Vector3 pos)
    {
        // Aceleración debido a la Tierra
        Vector3 earthAcceleration = (-pos).normalized * (gravityConstant * earthMass) / pos.sqrMagnitude;

        // Calcular posición absoluta de la Luna
        Vector3 moonAbsolutePos = earth.position + pos;

        // Dirección y distancia al Sol
        Vector3 directionToSun = sun.position - moonAbsolutePos;
        float distanceToSun = directionToSun.magnitude;

        // Aceleración debido al Sol
        Vector3 sunAcceleration = directionToSun.normalized * (gravityConstant * sunMass) / (distanceToSun * distanceToSun);

        // Sumar ambas aceleraciones
        return earthAcceleration + sunAcceleration;
    }

}