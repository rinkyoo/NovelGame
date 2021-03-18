using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    //上から花びらの様に降ってくるparticle
    public ParticleSystem fallParticle;
    private Material tempMaterial;
    
    
    public void SetFallParticle(string material)
    {
        fallParticle.gameObject.SetActive(true);
        fallParticle.GetComponent<Renderer>().material = (Material)Resources.Load("Material/Particle/"+material,typeof(Material));
        fallParticle.Play();
    }
    public void StopFallParticle()
    {
        fallParticle.Stop(true);
        fallParticle.gameObject.SetActive(false);
    }
}
