using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Scripts.Components
{
    public struct NoiseSettings : IComponentData
    {
        public int test;

        public NoiseSettings(int test)
        {
            this.test = test;
        }
        public float Evaluate(float2 position)
        {
            return noise.snoise(position);
        }
    }
}