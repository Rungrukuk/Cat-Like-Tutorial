using UnityEngine;


namespace _Script
{
    public class GPUGraph : MonoBehaviour
    {

        [SerializeField,Range(10,200)] private int resolution = 10;
        [SerializeField] private FunctionLibrary.FunctionName function;
        [SerializeField] private TransitionMode transitionMode;
        [SerializeField, Min(0f)] private float funcDuration = 1f,transitionDuration = 1f;
        [SerializeField] private ComputeShader computeShader;
        private enum TransitionMode { Cycle,Random }

        [SerializeField] private Material material;

        [SerializeField] private Mesh mesh;
        
        private float duration;
        
        private bool transitioning;
        
        private FunctionLibrary.FunctionName transitionFunction;


        private static readonly int
            PositionsId = Shader.PropertyToID("_Positions"),
            ResolutionId = Shader.PropertyToID("_Resolution"),
            StepId = Shader.PropertyToID("_Step"),
            TimeId = Shader.PropertyToID("_Time");
        
        private ComputeBuffer positionBuffer;
        private void OnEnable()
        {
            positionBuffer = new ComputeBuffer(resolution * resolution,3*4);
        }

        private void OnDisable()
        {
            positionBuffer.Release();
            positionBuffer.Dispose();
            positionBuffer = null;
        }

        private void Update()
        {
            duration += Time.deltaTime;
            if (transitioning)
            {
                if (duration>=transitionDuration)
                {
                    duration -= transitionDuration;
                    transitioning = false;
                }
            }
            else if (duration>=funcDuration)
            {
                duration -= funcDuration;
                //function = FunctionLibrary.GetNextFunctionName(function);
                transitioning = true;
                transitionFunction = function;
                PickNextFunction();
            }
            UpdateFunctionOnGpu();
        }

        void UpdateFunctionOnGpu()
        {
            float step = 2f / resolution;
            computeShader.SetInt(ResolutionId, resolution);
            computeShader.SetFloat(StepId,step);
            computeShader.SetFloat(TimeId,Time.time);
            computeShader.SetBuffer(0,PositionsId,positionBuffer);
            int groups = Mathf.CeilToInt(resolution / 8f);
            computeShader.Dispatch(0,groups,groups,1);
            material.SetBuffer(PositionsId, positionBuffer);
            material.SetFloat(StepId, step);
            var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution)) ; 
            Graphics.DrawMeshInstancedProcedural(mesh,0,material,bounds,positionBuffer.count);
        }
        
        private void PickNextFunction()
        {
            function = transitionMode == TransitionMode.Cycle
                ? FunctionLibrary.GetNextFunctionName(function)
                : FunctionLibrary.GetRandomFunctionName(function);
        }
    }
}
