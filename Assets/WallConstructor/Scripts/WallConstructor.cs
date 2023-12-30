using UnityEditor;
using UnityEngine;

public class WallConstructor : MonoBehaviour
{
    [SerializeField] GameObject _asset;
	[Space]
	[SerializeField] float _radius = 1f;
    [Min(2)]
    [SerializeField] sbyte _count = 2;
    [SerializeField] bool _lookAtTheCenter = true;
    [SerializeField] RotateEnum _rotateAround = RotateEnum.Self;

    Mesh _cubeMesh,
         _prismMesh,
         _currentMesh;
    float _anglePerSphere;

    enum RotateEnum { Self, Parent }



    void OnDrawGizmosSelected()
    {
        ChooseMesh();

        _anglePerSphere = 360f / _count;
        Gizmos.color = Color.white;

        for (float angle = 0f; angle < 360f; angle += _anglePerSphere)
        {
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad) * _radius;
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad) * _radius;

            Quaternion rotation = transform.rotation;

            if (_lookAtTheCenter)
                rotation *= Quaternion.AngleAxis(angle, Vector3.up);

            Gizmos.DrawMesh(_currentMesh,
                            new Vector3(transform.position.x + sin,
                                        transform.position.y,
                                        transform.position.z + cos),
                            rotation);
        }
    }



    public void BuildScene()
    {
        ClearScene();

        _anglePerSphere = 360f / _count;
        sbyte repeats = 1;

        for (float angle = 0f; angle < 360f; angle += _anglePerSphere)
        {
            GameObject go = CreateGameObject(repeats++);

            if (_lookAtTheCenter)
                go.transform.rotation *= Quaternion.AngleAxis(angle, Vector3.up);

            float cos = Mathf.Cos(angle * Mathf.Deg2Rad) * _radius;
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad) * _radius;

            go.transform.position = new Vector3(transform.position.x + sin,
                                                 transform.position.y,
                                                 transform.position.z + cos);
        }
    }

	public void ClearScene()
    {
        if (transform.childCount > 0)
            while (transform.childCount != 0)
                DestroyImmediate(transform.GetChild(0).gameObject);
    }



    void ChooseMesh()
    {
        if (_lookAtTheCenter)
        {
            if (_prismMesh == null)
                _prismMesh = Resources.Load<Mesh>("Meshes/prism");

            if (_currentMesh != _prismMesh)
                _currentMesh = _prismMesh;
        }
        else
        {
            if (_cubeMesh == null)
                _cubeMesh = Resources.Load<Mesh>("Meshes/cube");

            if (_currentMesh != _cubeMesh)
                _currentMesh = _cubeMesh;
        }
    }

    GameObject CreateGameObject(sbyte repeats)
    {
        GameObject go = PrefabUtility.InstantiatePrefab(_asset, transform) as GameObject;

        go.name = $"{_asset.name} {repeats}";
        go.layer = transform.gameObject.layer;
        go.transform.parent = transform;

        return go;
    }
}
