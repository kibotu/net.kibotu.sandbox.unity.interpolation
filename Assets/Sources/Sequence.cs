using UnityEngine;

namespace Assets.Sources
{
    public class Sequence : MonoBehaviour
    {
        public MoveAlongPath[] Path;

        void Start () {
            foreach (var path in Path)
            {
                path.OnComplete += alongPath => Debug.Log(alongPath.gameObject.name + " complete.");
            }
        }
	
        void Update () {
	
        }
    }
}
