using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    ////////////////////  VARIABLES PUBLICAS  //////////////////////////
    public Transform sun;                  
    public Transform[] otherBodies;        // Array de otros cuerpos (planetas)
    public float massSun = 1f;             // Masa del Sol (normalizada, usualmente 1 para el Sol)
    public float massPlanet;              
    public float[] massesOtherBodies;      // Masas correspondientes a los otros cuerpos en otherBodies (planetas)

    public float initialPos;               
    public float initialVelocity;          
    public float gravity = 39.478f;
    public Vector3 acceleration;

    private Vector3 previosPos;
    

    ////////////////////  METODO START  //////////////////////////
    // Inicializa la posicion y velocidad del planeta
    void Start()
    {
        // Posicionamos el planeta a la distancia inicial del Sol en el eje X
        transform.position = new Vector3(initialPos, 0, 0);

        // Calculamos velocidad orbital circular inicial: v = raiz(GM/r)
        // G  = constante gravitacional
        // M = masa del Sol
        // r = distancia inicial
        initialVelocity = Mathf.Sqrt(gravity * massSun / initialPos);

        Vector3 tangentDirection = new Vector3(0, 0, 1);

        // Preparamos la posicion previa para Verlet:
        previosPos = transform.position - tangentDirection * initialVelocity * Time.fixedDeltaTime;
    }

    ////////////////////  METODO FIXEDUPDATE  //////////////////////////
    // Calcula física en intervalos fijos para precision
    void FixedUpdate()
    {
        // 1. CALCULAR ACELERACION GRAVITACIONAL DEL SOL
        Vector3 directionToSun = sun.position - transform.position;
        float distanceToSun = directionToSun.magnitude;

        // Ley de Gravitacion Universal: F = GMm/r²
        // Como a = F/m, la aceleracion es GM/r² en direccion al Sol
        acceleration = directionToSun.normalized * (gravity * massSun) / (distanceToSun * distanceToSun);

        // 2. SUMAR ACELERACIONES DE OTROS CUERPOS CELESTES
        for (int i = 0; i < otherBodies.Length; i++)
        {
            if (otherBodies[i] != null && otherBodies[i] != this.transform)
            {
                Vector3 directionToBody = otherBodies[i].position - transform.position;
                float distanceToBody = directionToBody.magnitude;

                // Calculamos la aceleracion debido a este cuerpo
                float bodyAccelerationMagnitude = (gravity * massesOtherBodies[i]) / (distanceToBody * distanceToBody);

                // Sumamos el vector de aceleracion (direccion normalizada * magnitud)
                acceleration += directionToBody.normalized * bodyAccelerationMagnitude;
            }
        }

        // 3. INTEGRACION CON ALGORITMO DE VERLET
        // Formula de Verlet: x(t+dt) = 2x(t) - x(t-dt) + a(t)^dt²
        Vector3 newPos = 2 * transform.position - previosPos + acceleration * Mathf.Pow(Time.fixedDeltaTime, 2);

        // Actualizamos posicion
        previosPos = transform.position;
        transform.position = newPos;
    }
}