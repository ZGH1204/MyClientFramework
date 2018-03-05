using UnityEngine;
using System.Collections;

namespace Moba
{
    public class TextureAnimation : MonoBehaviour
    {
        // Scroll main texture based on time
        public Vector2 scrollSpeed = new Vector2(0, 0.1f);

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector2 offset = Time.time * scrollSpeed;
            GetComponent<Renderer>().material.SetTextureOffset("_EffectTex", offset);
        }
    }
}