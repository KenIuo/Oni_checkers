using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveControl : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect VFXDeath;
    public VisualEffect VFXSpawn;

    float dissolveRate = 0.0125f;
    float refreshRate = 0.025f;
    float currentDissolve;
    Material[] skinnedMaterials;

    void Start()
    {
        if (skinnedMesh != null)
        {
            skinnedMaterials = skinnedMesh.materials;
            currentDissolve = skinnedMaterials[0].GetFloat("_DissolveAmount");

            StartCoroutine(SpawnCoroutine());  
        }
    }

    public void SetDissolve(float dissolve_amount)
    {
        currentDissolve = dissolve_amount;

        for (int i = 0; i < skinnedMaterials.Length; i++)
            skinnedMaterials[i].SetFloat("_DissolveAmount", currentDissolve);
    }

    public IEnumerator DeathCoroutine ()
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
                    skinnedMaterials[i].SetFloat("_DissolveAmount", currentDissolve);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }

    public IEnumerator SpawnCoroutine()
    {
        if (VFXSpawn != null)
            VFXSpawn.Play();

        if (skinnedMaterials.Length > 0)
        {
            while (currentDissolve > 0)
            {
                currentDissolve -= dissolveRate;

                foreach (Material mat in skinnedMaterials)
                {
                    mat.SetFloat("_DissolveAmount", currentDissolve);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
