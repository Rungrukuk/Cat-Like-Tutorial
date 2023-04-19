using UnityEngine;


namespace _Script
{
    public class Graph : MonoBehaviour
    {
        [SerializeField] private Transform pointPrefab;
        [SerializeField,Range(10,100)] private int resolution = 10;
        [SerializeField] private FunctionLibrary.FunctionName function;
        private Transform[] points;

        private void Awake()
        {
            float step = resolution / 2;
            Vector3 scale = Vector3.one / step;
            points = new Transform[resolution * resolution];

            
            for (int i = 0; i < points.Length; i++)
            {
                Transform point = points[i] = Instantiate(pointPrefab,transform,false);
                // position.x = ((x + 0.5f)/step) - 1f;
                // position.z = ((z + 0.5f)/step) - 1f;
                // position.y = position.x * position.x * position.x;
                // point.localPosition = position;
                point.localScale = scale;

            }
        }

        private void Update()
        {
            float time = Time.time;
            float step = 2f / resolution;
            float v = 0.5f * step - 1f;
            FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
            for (int i = 0,x = 0, z = 0; i < points.Length; i++,x++)
            {
                if (x==resolution)
                {
                    x = 0;
                    z += 1;
                    v = (z + 0.5f) * step - 1f;
                }
                float u = (x + 0.5f) * step - 1f;
                points[i].localPosition = f(u, v, time);
            }
        }
    }
}
