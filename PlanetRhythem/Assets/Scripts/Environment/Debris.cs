using UnityEngine;

namespace Rhythem.Objects
{
    public class Debris : MonoBehaviour
    {
        private Vector3 randomDirection;
        private Vector3 randomRotation;

        void Start()
        {
            var rX = Random.Range(-180f, 180f);
            var rY = Random.Range(-180f, 180f);
            var rZ = Random.Range(-180f, 180f);
            var tX = Random.Range(-1, 1f);
            var tY = Random.Range(-1, 1f);
            var tZ = Random.Range(0f, 3f);

            randomDirection = new Vector3 (tX, tY, tZ);
            randomRotation = new Vector3(tX, tY, tZ);

        }

        void Update()
        {
            transform.Rotate(randomRotation);
            transform.Translate(randomDirection);
        }
    }
}