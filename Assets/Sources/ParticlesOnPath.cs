using System.Security.AccessControl;
using Assets.Plugins.radical.System;
using UnityEngine;

namespace Assets.Sources
{
    public class ParticlesOnPath : MonoBehaviour {

        public Transform[] ControlPoints;
        private ParticleSystem ps;
        public bool Gizmos = false;

        public void Awake()
        {
            ps = GetComponent<ParticleSystem>();
        }

        public void OnDrawGizmos()
        {
            if(Gizmos)
                ps.UpdateParticles(particle =>
                {
                    var percent = 1 - particle.lifetime / particle.startLifetime;
                    Spline.GizmoDraw(ControlPoints, percent);
                    return particle;
                }); 
        }

        public void LateUpdate()
        {
            ps.UpdateParticles(particle =>
            {
                var percent = 1- particle.lifetime/particle.startLifetime;
                particle.position = Spline.InterpConstantSpeed(ControlPoints, percent);
                
                return particle;
            }); 
        }
    }
}
