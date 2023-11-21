using UnityEngine;

public class PointerSystemController : MonoBehaviour
{
    [SerializeField] GameObject _pointingArrow; // ������������� ������ �������������� (�� �������� ����� ������) � ������� (���������� ������� �����)
    [SerializeField] GameObject _tensionForce; // ������������� ������ ����� (Z ����������) (���������� �� ������������ ��� ����� ������������ �����)
    [SerializeField] Camera _camera;
    [SerializeField] LayerMask _layerMask;

    CheckerController _playerCheckerController;
    Vector3 _playerPos;
    float _currentRadius;



    void DrawArrow(Vector3 hit_position)
    {
        _playerPos = new Vector3(gameObject.transform.position.x,
                                 0.1f,
                                 gameObject.transform.position.z);

        Vector3 direction_move = GetDirectionVector(hit_position);

        if (Input.GetMouseButton(0))
        {
            _currentRadius = _playerCheckerController.GetCurrentRadius(hit_position);

            if (_currentRadius != 0)
            {
                gameObject.transform.rotation = Quaternion.LookRotation(direction_move);

                TransformPointingArrow(direction_move);
            }
            else
                _pointingArrow.SetActive(false);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_currentRadius != 0)
                _playerCheckerController.LaunchChecker(_currentRadius, direction_move);

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

    void TransformPointingArrow(Vector3 direction_move)
    {
        _pointingArrow.SetActive(true);
        _pointingArrow.transform.position = _playerPos;
        _pointingArrow.transform.rotation = Quaternion.LookRotation(direction_move);

        _tensionForce.transform.localScale = new Vector3(1, 1, _currentRadius / 5);
    }



    void Awake()
    {
        _playerCheckerController = gameObject.GetComponent<CheckerController>();
    }

    void Update()
    {
        if (_playerCheckerController._state.Equals(CheckerState.Turning))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (TurnSystem.Instance._playerID == TurnSystem.Instance._currentPlayer
            &&  Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _layerMask))
            {
                if (gameObject.activeInHierarchy)
                    DrawArrow(hit.point);
            }
            else
                _pointingArrow.SetActive(false);
        }
    }
}
