using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _rotationSpeed;

    void Update()
    {
        if (Input.GetMouseButton(1))
            gameObject.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * _rotationSpeed);
    }
}
