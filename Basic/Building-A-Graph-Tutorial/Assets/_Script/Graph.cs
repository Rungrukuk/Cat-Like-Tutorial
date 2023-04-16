using UnityEngine;


namespace _Script
{
    public class Graph : MonoBehaviour
    {
        [SerializeField] private Transform pointPrefab;
        [SerializeField,Range(10,100)] private int resolution = 10;
        private Transform[] points;

        private void Awake()
        {
            float step = resolution / 2;
            Vector3 scale = Vector3.one / step;
            Vector3 position = Vector3.zero;
            points = new Transform[resolution];

            
            for (int i = 0; i < points.Length; i++)
            {
                Transform point = points[i] = Instantiate(pointPrefab,transform,false);
                position.x = ((i + 0.5f)/step) - 1f;
                // position.y = position.x * position.x * position.x;
                point.localPosition = position;
                point.localScale = scale;

            }
        }

        private void Update()
        {
            float time = Time.time;
            for (int i = 0; i < points.Length; i++)
            {
                Transform point = points[i];
                Vector3 position = point.localPosition;
                position.y = Mathf.Sin((position.x + time)* Mathf.PI);
                point.localPosition = position;
            }
        }
    }
}
