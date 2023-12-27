using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WallConstructor : MonoBehaviour
{
    [SerializeField] GameObject _asset;
    [SerializeField] Mesh _cubeMeshExample;
	[Space]
	[SerializeField] float _radius = 1f;
    [Min(2)]
    [SerializeField] sbyte _count = 2;
    [SerializeField] bool _rotateAroundCenter = true;



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        float anglePerSphere = 360f / _count;

        for (float angle = 0f; angle < 360f; angle += anglePerSphere)
        {
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad) * _radius;
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad) * _radius;

            if (_cubeMeshExample != null)
            {
                Quaternion rotation = transform.rotation;

                if (_rotateAroundCenter)
                    rotation *= Quaternion.AngleAxis(angle, Vector3.up);

                Gizmos.DrawMesh(_cubeMeshExample,
                                new Vector3(transform.position.x + sin,
                                            transform.position.y,
                                            transform.position.z + cos),
                                rotation);
            }
        }
    }



    public void BuildScene()
    {
        ClearScene();

        float anglePerSphere = 360f / _count;
        sbyte repeats = 1;

        for (float angle = 0f; angle < 360f; angle += anglePerSphere)
        {
            GameObject box = PrefabUtility.InstantiatePrefab(_asset, transform) as GameObject;

            box.name = $"{_asset.name} {repeats++}";
            box.layer = transform.gameObject.layer;
            box.transform.parent = transform;
                
            if (_rotateAroundCenter)
                box.transform.rotation *= Quaternion.AngleAxis(angle, Vector3.up);

            float cos = Mathf.Cos(angle * Mathf.Deg2Rad) * _radius;
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad) * _radius;

            box.transform.position = new Vector3(transform.position.x + sin,
                                                 transform.position.y,
                                                 transform.position.z + cos);
        }
    }

	public void ClearScene()
    {
        if (transform.childCount > 0)
        {
            while (transform.childCount != 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                }
            }
        }
    }
}
