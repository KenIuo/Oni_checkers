using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class CheckerAppearance : MonoBehaviour
{
    [SerializeField] MeshRenderer _bodyMesh;
    [SerializeField] Material _bodyMaterial;
    [SerializeField] VisualEffect _chargeVFX;
    [SerializeField] Image _markerImage;
    [SerializeField] Color _playerColor;



    void Awake()
    {
        _bodyMaterial.SetVector(NamesTags.SHADER_COLOR, _playerColor);
        _bodyMesh.material = _bodyMaterial;

        _markerImage.color = _playerColor;
        _chargeVFX.SetVector4(NamesTags.VFX_COLOR, _playerColor);

        //collision.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
    }
}
