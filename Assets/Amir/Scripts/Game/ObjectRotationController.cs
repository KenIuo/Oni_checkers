using UnityEngine;

public class ObjectRotationController : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 2f;
    [SerializeField] bool _autoRotate = true;



    void Update()
    {
        if (_autoRotate)
            transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * _rotationSpeed);
        else if (Input.GetMouseButton(1))
            transform.RotateAround(transform.position, Vector3.up, Input.GetAxis(AxisTags.MOUSE_X) * _rotationSpeed);
    }
}
