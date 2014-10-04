using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources
{
    public class MoveAlongPath : MonoBehaviour
    {
        public Transform Beginning;
        public Transform End;
        private IEnumerator<Vector3> _sequence;
        private IEnumerator<Vector3> _reverseSequence;
        public float Duration;
        public Interpolate.EaseType EaseType = Interpolate.EaseType.Linear;
        public bool Loop = false;
        public bool Test = false;
        public event Action<MoveAlongPath> OnComplete;
        public Transform Target;

        private void Start()
        {
//            IEnumerator<Vector3> sequence = Interpolate.New[Ease | Bezier | CatmulRom](configuration).GetEnumerator();
            _sequence = GetSequence();
        }

        private IEnumerator<Vector3> GetSequence()
        {
            return (IEnumerator<Vector3>)Interpolate.NewEase(Interpolate.Ease(EaseType), Beginning.position, End.transform.position, Duration);
        }

        private IEnumerator<Vector3> GetReverseSequence()
        {
            return (IEnumerator<Vector3>)Interpolate.NewEase(Interpolate.Ease(EaseType), End.position, Beginning.transform.position, Duration);
        }

        private void Update()
        {
            if (Test)
            {
                Test = false;
                StopCoroutine("Move");
                StartCoroutine(Move());
            }
        }

        private IEnumerator Move()
        {
            while (_sequence.MoveNext())
            {
                Target.position = _sequence.Current;
            }
            else
            {
                if (OnComplete != null) OnComplete(this);
                if (!Loop) { yield break; }
                if (_reverseSequence == null) _reverseSequence = GetReverseSequence();
                if (_reverseSequence.MoveNext())
                    Target.position = _reverseSequence.Current;
                else
                {
                    if (OnComplete != null) OnComplete(this);
                    _sequence = GetSequence();
                    _reverseSequence = GetReverseSequence();
                }
            }
            yield return new WaitForEndOfFrame();
        }

        private void Complete()
        {
            OnComplete(this);
        }
    }
}