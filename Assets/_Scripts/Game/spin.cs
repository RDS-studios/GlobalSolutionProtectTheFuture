using UnityEngine;

public class spin : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] Vector3 rotationAxis = Vector3.up; // Default: Y-axis
    [SerializeField] float rotationSpeed = 90f; // Degrees per second

    void Update()
    {
        transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime);
    }
}
