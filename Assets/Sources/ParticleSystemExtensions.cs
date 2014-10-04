using System.Linq;
using UnityEngine;

namespace Assets.Sources
{
    internal static class ParticleSystemExtensions
    {
        /// <summary>
        /// Method that takes a Particle argument
        /// </summary>
        public delegate ParticleSystem.Particle UpdateParticleDelegate(ParticleSystem.Particle particle);

        /// <summary>
        /// Get particles Array
        /// </summary>
        public static ParticleSystem.Particle[] GetParticles(this ParticleSystem particleSystem)
        {
            var particles = new ParticleSystem.Particle[particleSystem.particleCount];
            particleSystem.GetParticles(particles);
            return particles;
        }

        /// <summary>
        /// Set particles Array
        /// </summary>
        public static void SetParticles(this ParticleSystem particleSystem, ParticleSystem.Particle[] particles)
        {
            particleSystem.SetParticles(particles, particles.Count());
        }

        /// <summary>
        /// Applies the given function to each particle in a ParticleSystem
        /// </summary>
        /// <param name="func">Function to perform on each particle</param>
        public static void UpdateParticles(this ParticleSystem particleSystem, UpdateParticleDelegate func)
        {
            if (particleSystem == null || func == null) return;
            var particles = new ParticleSystem.Particle[particleSystem.particleCount];
            var particleCount = particleSystem.GetParticles(particles);
            int i = 0;
            while (i < particleCount)
            {
                particles[i] = func.Invoke(particles[i]);
                i++;
            }
            particleSystem.SetParticles(particles, particleCount);
        }
    }
}