using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointerSystemController : MonoBehaviour
{
    [SerializeField] float _speed = 2f;
    [SerializeField] GameObject _pointingArrow; // настраивается только местоположение (на середине шашки игрока) и поворот (изначально смотрит вперёд)
    [SerializeField] GameObject _tensionForce; // настраивается только длина (Z координата) (изначально на максимальной или почти максимальной длине)
    [SerializeField] LayerMask _layerMask;

    public GameObject _playerChecker;

    Camera _camera;
    Vector3 _pointerPos;
    //KeyValuePair<Vector3, Vector3> _launchVector;



    void DrawArrow(Vector3 launch_vector)
    {
        if (Input.GetMouseButton(0))
        {
            _pointingArrow.SetActive(true);
            _pointingArrow.transform.position = _playerChecker.transform.position;
            _pointingArrow.transform.rotation = Quaternion.LookRotation(launch_vector);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Rigidbody rigidbody = _playerChecker.GetComponentInChildren<Rigidbody>();
            rigidbody.velocity = launch_vector * 4;

            _pointingArrow.SetActive(false);
        }
    }

    void RotateCamera()
    {
        if (Input.GetMouseButton(1))
        {
            gameObject.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * _speed);
        }
    }



    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _layerMask))
        {
            if (_playerChecker.activeInHierarchy) // && 
            {
                Vector3 launch_vector = new Vector3(_playerChecker.transform.position.x, 0, _playerChecker.transform.position.z);
                launch_vector += new Vector3(-hit.point.x, 0, -hit.point.z);

                DrawArrow(launch_vector);
            }
        }
        else
        {
            _pointingArrow.SetActive(false);
        }

        RotateCamera();
    }
}
