using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveControl : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;

    [SerializeField] VisualEffect _spawnVFX;

    Material[] skinnedMaterials;
    float dissolveRate = 0.0125f;
    float refreshRate = 0.025f;
    float currentDissolve;

    const string _dissolveValue = "_DissolveAmount";

    void Start()
    {
        if (skinnedMesh != null)
        {
            skinnedMaterials = skinnedMesh.materials;
            currentDissolve = skinnedMaterials[0].GetFloat(_dissolveValue);

            StartCoroutine(SpawnCoroutine());  
        }
    }

    public void SetDissolve(float dissolve_amount)
    {
        currentDissolve = dissolve_amount;

        for (int i = 0; i < skinnedMaterials.Length; i++)
            skinnedMaterials[i].SetFloat(_dissolveValue, currentDissolve);
    }

    /*public IEnumerator DeathCoroutine ()
    {
        if(VFXDeath != null)
            VFXDeath.Play();

        if (skinnedMaterials.Length > 0)
        {
            while (currentDissolve < 1)
            {
                currentDissolve += dissolveRate;

                for (int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat(_dissolveValue, currentDissolve);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }*/

    public IEnumerator SpawnCoroutine()
    {
        if (_spawnVFX != null)
            _spawnVFX.Play();

        if (skinnedMaterials.Length > 0)
        {
            while (currentDissolve > 0)
            {
                currentDissolve -= dissolveRate;

                foreach (Material mat in skinnedMaterials)
                {
                    mat.SetFloat(_dissolveValue, currentDissolve);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
