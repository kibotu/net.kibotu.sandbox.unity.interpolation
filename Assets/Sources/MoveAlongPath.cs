using System.Collections;
using System.Collections.Generic;
using Assets.Promises;
using UnityEngine;

namespace Assets.Sources
{
    public class MoveAlongPath : MonoBehaviour
    {
        public Transform Beginning;
        public Transform End;
        public float Duration;
        public Interpolate.EaseType EaseType = Interpolate.EaseType.EaseInOutSine;
        public bool Loop = false;
        public bool Reverse = false;
        public Transform Target;
        private Promise<Vector3> _promise;

        private IEnumerator<Vector3> GetSequence()
        {
            return (IEnumerator<Vector3>)Interpolate.NewEase(Interpolate.Ease(EaseType), Beginning.position, End.transform.position, Duration);
        }

        private IEnumerator<Vector3> GetReverseSequence()
        {
            return (IEnumerator<Vector3>)Interpolate.NewEase(Interpolate.Ease(EaseType), End.position, Beginning.transform.position, Duration);
        }

        private IEnumerator Move()
        {
            var sequence = GetSequence();
            while (sequence.MoveNext())
            {
                Target.position = sequence.Current;
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator MoveReversed()
        {
            var sequence = GetReverseSequence();
            while (sequence.MoveNext())
            {
                Target.position = sequence.Current;
                yield return new WaitForEndOfFrame();
            }
        }

        public Promise<Vector3> LoopMoving()
        {
            var promise = Promise.WithCoroutine<Vector3>(Move());
            if (Reverse)
                promise.OnFulfilled += result =>
                {
                    var promise2 = Promise.WithCoroutine<Vector3>(MoveReversed());
                    if (Loop) promise2.OnFulfilled += vector3 => LoopMoving();
                };
            else
            {
                if (Loop) promise.OnFulfilled += result => LoopMoving();
            }
            return promise;
        }
    }
}