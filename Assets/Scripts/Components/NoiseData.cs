using Unity.Entities;
using Unity.Mathematics;

public struct NoiseData : IComponentData
{
    public int test;

    public NoiseData(int test)
    {
        this.test = test;
    }
    public float Evaluate(float2 position)
    {
        return noise.snoise(position);
    }
}
