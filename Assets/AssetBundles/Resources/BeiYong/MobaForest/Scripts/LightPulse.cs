using UnityEngine;

namespace Moba
{
    [RequireComponent(typeof(Light))]
    public class LightPulse : MonoBehaviour
    {
        public float minIntensity = 0.5f;
        public float maxIntensity = 1f;
        public float speed = 1f;

        float random;

        void Start()
        {
            random = Random.Range(0.0f, 65535.0f);
        }

        void Update()
        {
            float noise = Mathf.PerlinNoise(random, Time.time * speed);
            GetComponent<Light>().intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        }

    }
}