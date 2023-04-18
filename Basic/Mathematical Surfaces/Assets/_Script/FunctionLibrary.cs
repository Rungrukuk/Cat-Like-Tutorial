using static UnityEngine.Mathf;

namespace _Script
{
    public static class FunctionLibrary
    {
        public delegate float Function(float x,float z, float t);
        
        public enum FunctionName { Wave,MultiWave,Ripple }

        private static readonly Function[] Functions = {Wave,MultiWave,Ripple};

        public static Function GetFunction(FunctionName name)
        {
            return Functions[(int)name];
        }
        public static float Wave(float x, float z,float time)
        {
            
            return Sin(PI * (x  + z + time));
        }

        public static float MultiWave(float x,float z,float time)
        {
            float y = Sin(PI * (x + 0.5f*time));
            y += 0.5f * Sin(2f * PI * (z + time));
            y += Sin(PI * (x + z + 0.25f * time));
            return y*(1f/2.5f);
        }

        public static float Ripple(float x,float z, float time)
        {
            float d = Sqrt(x*x + z*z);
            float y = Sin(PI * (4f*d-time));
            return y/(1f+10f*d);
        }
        
    }
}
