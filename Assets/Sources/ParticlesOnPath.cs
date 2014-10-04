using System.Collections;
using System.Collections.Generic;
using Assets.Plugins.radical.System;
using UnityEngine;

namespace Assets.Sources
{
    public class ParticlesOnPath : MonoBehaviour {

        public Transform[] ControlPoints;
        private ParticleSystem ps;

        public void Awake()
        {
            ps = GetComponent<ParticleSystem>();
        }

        public void OnDrawGizmos()
        {
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
