using UnityEngine;
using Color = System.Drawing.Color;

namespace _Script
{
    public class Graph : MonoBehaviour
    {
        [SerializeField] private Transform pointPrefab;
        [SerializeField,Range(10,100)] private int resolution = 10;

        private void Awake()
        {
            float step = resolution / 2;
            Vector3 scale = Vector3.one / step;
            Vector3 position = Vector3.zero;
            
            for (int i = 0; i < resolution; i++)
            {
                Transform point = Instantiate(pointPrefab,transform,false);
                position.x = ((i + 0.5f)/step) - 1f;
                position.y = position.x * position.x;
                // point.GetComponent<MeshRenderer>().material;
                point.localPosition = position;
                point.localScale = scale;

            }
        }
    }
}
