#include "ClassicNoise3D.hlsl"

#define MAX_DIST 300
#define SAMPLE_DIST 0.01

float GetDist(float3 p) {
    float dist = 0;
    //raymarch mask shape here
    float4 s = float4(0,0,0,1);
    dist = length(p-s.xyz)-s.w;

    return dist;
}


float cloudDensity(float3 lookupPoint, float time) {
    float density = 0;
    density += saturate(1+cnoise(lookupPoint + float3(-time*0.03,-time*0.002,time*0.002)).x);
    density += saturate(1+cnoise(lookupPoint +  float3(-time*0.10,-time*0.002,time*0.001) * 2.52154).x * 0.25);
    density += saturate(1+cnoise(lookupPoint + float3(time*0.01,0,time*0.001) * 6.125211).x * 0.2);


    return saturate(density);
}

void CloudRaymarch_float(
    in float3 rayOrigin,
    in float3 rayDirection,
    in int numSteps,
    in float stepSize,
    in float densityScale,
    in float4 sphere,
    in float time,
    in int lightNumSteps,
    in float lightStepSize,
    in float3 lightDirection,
    in float lightDensityScale,
    out float density,
    out float lightDensity)
{
    float transmission = 0;
    density = 0;
    lightDensity = 0;
    float transmittance = 1;
    float3 lightEnergy = 0;
    float dist = 0;
    float3 rayPos;
    
    rayDirection = normalize(rayDirection);

for (int i=0; i < numSteps; i++) {
        
        float samplePoint = rayOrigin + rayDirection * dist;
        float distSample = GetDist(samplePoint);
        dist += max(distSample, stepSize);
        if (dist > MAX_DIST) break;
        if (distSample > 0) continue;
        float3 samplingPos = samplePoint;

        //Lookup texture

        //density = 1;
        density += cloudDensity(samplingPos, time) * densityScale;

        //LIGHTING
    }
    
    lightDensity = saturate(lightDensity);

}


