using UnityEngine;

public class WallConstructor : MonoBehaviour
{
    [SerializeField] GameObject _asset;
	[Space]
	[SerializeField] float _radius = 1f;
    [Min(2)]
    [SerializeField] sbyte _count = 2;
    [SerializeField] bool _rotateAroundCenter = true;



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        float anglePerSphere = 360f / _count;
        /*
            float rotation = transform.localRotation.eulerAngles.y;

            float cos = Mathf.Cos((angle - anglePerSphere + rotation) * Mathf.Deg2Rad) * _radius;
            float sin = Mathf.Sin((angle - anglePerSphere + rotation) * Mathf.Deg2Rad) * _radius;
        */

        for (float angle = 0f; angle < 360f; angle += anglePerSphere)
        {
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad) * _radius;
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad) * _radius;

            Gizmos.DrawSphere(new Vector3(transform.position.x + sin,
                                          transform.position.y,
                                          transform.position.z + cos), 1f);
        }
    }



    public void BuildScene()
    {
        ClearScene();

        float anglePerSphere = 360f / _count;
        int repeats = 1;

        for (float angle = 0f; angle < 360f; angle += anglePerSphere, repeats++)
        {
            if (_rotateAroundCenter)
            {
                GameObject box = Instantiate(_asset);

                box.name = $"{_asset.name}{repeats}";
                box.layer = transform.gameObject.layer;
                box.transform.parent = transform;
                box.transform.rotation = transform.rotation * Quaternion.AngleAxis(angle, Vector3.up);

                float cos = Mathf.Cos(angle * Mathf.Deg2Rad) * _radius;
                float sin = Mathf.Sin(angle * Mathf.Deg2Rad) * _radius;

                box.transform.position = new Vector3(transform.position.x + sin,
                                                     transform.position.y,
                                                     transform.position.z + cos);
            }
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
