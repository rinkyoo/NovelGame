using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EmissiveController : MonoBehaviour
{
    private Material mat;
    private float fact = 0;

    private float defIntensity = 0;

    const float variation = 0.02f;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }
    void Update()
    {
        if (fact < 1000f)
        {
            fact = Mathf.Pow(2, defIntensity += variation);
            mat.SetColor("_EmissionColor", new Color(mat.color.r * fact, mat.color.g * fact, mat.color.b * fact));
        }
        else
        {
            Destroy(this.gameObject, 0.5f);
        }
    }

    void OnDestroy()
    {
        GameObject.Find("GameController").GetComponent<GameController>().skipProgress = false; ;
    }
}