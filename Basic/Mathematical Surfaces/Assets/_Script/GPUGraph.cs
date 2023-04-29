using System;
using UnityEngine;


namespace _Script
{
    public class GPUGraph : MonoBehaviour
    {

        [SerializeField,Range(10,200)] private int resolution = 10;
        [SerializeField] private FunctionLibrary.FunctionName function;
        [SerializeField] private TransitionMode transitionMode;
        [SerializeField, Min(0f)] private float funcDuration = 1f,transitionDuration = 1f;
        private enum TransitionMode { Cycle,Random }

        private float duration;
        
        private bool transitioning;
        
        private FunctionLibrary.FunctionName transitionFunction;

        private ComputeBuffer positionBuffer;
        private void OnEnable()
        {
            positionBuffer = new ComputeBuffer(resolution * resolution,3*4);
        }

        private void OnDisable()
        {
            positionBuffer.Release();
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
        }

        private void PickNextFunction()
        {
            function = transitionMode == TransitionMode.Cycle
                ? FunctionLibrary.GetNextFunctionName(function)
                : FunctionLibrary.GetRandomFunctionName(function);
        }
    }
}
