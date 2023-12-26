using UnityEngine;
using UnityEngine.VFX;

public class CheckerAppearance : MonoBehaviour
{
    [SerializeField] Color _playerColor;
    //collision.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);



    public void SetBodyMaterialColor(Material body_material)
    {
        body_material.SetVector(NamesTags.SHADER_COLOR, _playerColor);
    }

    public void SetChargeVFXColor(VisualEffect charge_effect)
    {
        charge_effect.SetVector4(NamesTags.VFX_COLOR, _playerColor);
    }

    public void SetMarkerImageColor(UnityEngine.UI.Image marker_image)
    {
        marker_image.color = _playerColor;
    }
}
