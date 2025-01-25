float2x2  Rot(float a) {
    float s = sin(a);
    float c = cos(a);
    return float2x2(c,-s,s,c);
}

float smoothmin(float a, float b, float k) {
    float h = saturate(0.5+0.5*((b-a)/k));
    return lerp(b,a,h) - k*h*(1.0-h);
}

float SDCapsule (float3 p, float3 a, float3 b, float r) {
    float3 ab = b-a;
    float3 ap = p-a;

    float t = dot(ab, ap) /  dot (ab, ab);
    t = saturate(t);
    float3 c = a + t*ab;
    return length(p-c) - r;
}

float SDSphere (float3 p, float3 a, float r) {
    return length(a-p) - r;
}

float SDSphere (float3 p, float4 sphere) {
    return length(sphere.xyz - p) - sphere.w;
}

float SDTorus (float3 p, float3 a, float2 r) {
    float o = p-a;
    float x = length(o.xz) - r.x;
    return length(vec2(x,o.y)) - r.y;
}

float DBox (float3 p, float3 a, float3 s) {
    return length(max(abs(a-p)-s, 0));
}

float SDCylinder (float3 p, float3 a, float3 b, float r) {
    float3 ab = b-a;
    float3 ap = p-a;

    float t = dot(ab, ap) /  dot (ab, ab);
    t = saturate(t);
    float3 c = a + t*ab;
    float x = length(p-c) - r;
    float y = abs(t-0.5)-.5 * length(ab);
    e = length(max(float2(x,y), 0));
    float i = min(max(x,y),0);
    return e + i;
}