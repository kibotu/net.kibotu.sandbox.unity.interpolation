using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources
{
    public class Sequence : MonoBehaviour
    {
        public MoveAlongPath[] Path;
        public Interpolate.EaseType EaseType = Interpolate.EaseType.EaseInOutSine;
        public Transform [] ControlPoints;
        public float Duration;
        public int Slices;
        private IEnumerable<Vector3> _sequence;
        public enum Type
        {
            Line, Bezier, CatmullRom, ParticleCatMullRom
        }

        private bool _isRunning;
        public ParticleSystem Particles;

        public void StartCube(int type)
        {
            if (_isRunning) 
                return;

            _isRunning = true;
            switch (type)
            {
                case (int)Type.Line:
                    Path[0].LoopMoving().OnFulfilled += result => Path[1].LoopMoving().OnFulfilled += result2 => Path[2].LoopMoving().OnFulfilled += result3 => Path[3].LoopMoving().OnFulfilled += vector3 => _isRunning = false;
                    break;
                case (int)Type.Bezier:
                    StartCoroutine(MoveBezier());
                    break;
                case (int)Type.CatmullRom:
                    StartCoroutine(MoveCatmullRom());
                    break;
                case (int)Type.ParticleCatMullRom:
                    Particles.Play();
                    break;
            }
        }

        private IEnumerator MoveBezier()
        {
            yield return new WaitForSeconds(1);
            IEnumerable<Vector3> sequence = Interpolate.NewBezier(Interpolate.Ease(EaseType), ControlPoints, Duration);
            foreach (Vector3 newPoint in sequence) {
                transform.position = newPoint;
                yield return new WaitForEndOfFrame();
            }
            _isRunning = false;
        }

        private IEnumerator MoveCatmullRom()
        {
            yield return new WaitForSeconds(1);
            IEnumerable<Vector3> sequence = Interpolate.NewCatmullRom(ControlPoints, Slices, false);
            foreach (Vector3 newPoint in sequence) {
                transform.position = newPoint;
                yield return new WaitForEndOfFrame();
            }
            _isRunning = false;
        }
    }
}
