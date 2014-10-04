using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources
{
    public class Timer
    {
        public float Interval;
        public bool Debug = false;
        public event Action OnComplete;

        public IEnumerable<IEnumerator> Start()
        {
            yield return WaitUntilInterval();
        }

        public IEnumerator WaitUntilInterval() {
            yield return new WaitForSeconds(Interval);
            OnComplete();
        }

        public static IEnumerator WaitUntilInterval(float duration, Action onComplete)
        {
            yield return new WaitForSeconds(duration);
            onComplete();
        }
    }
}
