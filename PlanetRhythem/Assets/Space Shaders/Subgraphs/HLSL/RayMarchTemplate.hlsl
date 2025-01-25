#include "ClassicNoise3D.hlsl"

int MAX_STEPS = 800;
float MAX_DIST = 100;
float SURF_DIST = .01

// Return any SDF to change the shape of the cloud
float GetSDF(float3 atPoint) {
    float4 sphere = float4(0,0,0,1);

    float sphereDist = length(atPoint-sphere.xyz)-sphere.w;
    return sphereDist;
}
float RayMarch (float3 rayOrigin, float3 rayDirection, int numSteps, float stepSize) {
    float dist = 0;

    for (int i=0; i< min(MAX_STEPS, numSteps); i++) {
        float3 p = rayOrigin + rayDirection*dist;
        float distanceStep = GetSDF(p);
        dist += distanceStep;
        if (dist > MAX_DIST || dS < SURF_DIST) break;
    }

    return dist;
}

void RayMarch_float (
    in float3 rayOrigin,
    in float3 rayDirection,
    in int numSteps,
    in float stepSize,
    out float distance) {
    distance = RayMarch(rayOrigin, rayDirection, numSteps, stepSize);
}