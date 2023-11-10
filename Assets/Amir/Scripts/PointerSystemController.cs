using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PointerSystemController : MonoBehaviour
{
    [SerializeField] GameObject _playerChecker;
    [SerializeField] GameObject _pointingArrow; // настраивается только местоположение (на середине шашки игрока) и поворот (изначально смотрит вперёд)
    [SerializeField] GameObject _tensionForce; // настраивается только длина (Z координата) (изначально на максимальной или почти максимальной длине)
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _launchStrength;

    Camera _camera;
    Vector3 _playerPos;
    /// <summary>
    /// соотношение 5 к 1, если расстояние больше заданного, то оно равно ему, если меньше, то 0
    /// </summary>
    float _max_radius = 5.00f, // max_size = 1.00f
          _min_radius = 1.25f; // min_size = 0.25f
    float _current_radius;
    float _speed = 2f;
    //KeyValuePair<Vector3, Vector3> _launchVector;



    void DrawArrow(Vector3 hit_position)
    {
        _playerPos = new Vector3(_playerChecker.transform.position.x, 0.1f, _playerChecker.transform.position.z);
        
        Vector3 direction_move = GetDirectionVector(hit_position);

        if (Input.GetMouseButton(0))
        {
            GetCurrentRadius(hit_position);

            if (_current_radius != 0)
                TransformPointingArrow(direction_move);
            else
                _pointingArrow.SetActive(false);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (_current_radius != 0)
            {
                _playerChecker.GetComponent<CheckerState>()._isStopped = false;
                _playerChecker.transform.GetChild(2).gameObject.SetActive(true);

                Rigidbody rigidbody = _playerChecker.GetComponent<Rigidbody>();
                rigidbody.velocity = direction_move * (_current_radius * _launchStrength);
            }

            _pointingArrow.SetActive(false);
        }
    }

    Vector3 GetDirectionVector(Vector3 hit_position)
    {
        hit_position.y = _playerPos.y;

        Vector3 direction_move = _playerPos - hit_position;
        direction_move.Normalize();

        return direction_move;
    }

    void GetCurrentRadius(Vector3 hit_position)
    {
        _current_radius = Vector3.Distance(hit_position, _playerPos);

        if (_current_radius > _max_radius)
        {
            _current_radius = _max_radius;
            _playerChecker.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (_current_radius < _min_radius)
        {
            _current_radius = 0;
            _playerChecker.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
            _playerChecker.transform.GetChild(1).gameObject.SetActive(false);
    }

    void TransformPointingArrow(Vector3 direction_move)
    {
        _pointingArrow.SetActive(true);
        _pointingArrow.transform.position = _playerPos;
        _pointingArrow.transform.rotation = Quaternion.LookRotation(direction_move);

        _playerChecker.transform.rotation = Quaternion.LookRotation(direction_move);

        _tensionForce.transform.localScale = new Vector3(1, 1, _current_radius / 5);
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
                DrawArrow(hit.point);
            }
        }
        else
        {
            _pointingArrow.SetActive(false);
        }

        RotateCamera();
    }
}
