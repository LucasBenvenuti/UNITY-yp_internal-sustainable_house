Shader "Mobile/Color Specular" {
Properties {
    _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
    _Color("Color", Color) = (1,1,1,1)
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 250
   
CGPROGRAM
#pragma surface surf MobileBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview interpolateview
 
inline fixed4 LightingMobileBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
    fixed diff = max (0, dot (s.Normal, lightDir));
    fixed nh = max (0, dot (s.Normal, halfDir));
   
    fixed4 c;
    c.rgb = (s.Albedo * _LightColor0.rgb * diff) * atten;
    UNITY_OPAQUE_ALPHA(c.a);
    return c;
}
 
sampler2D _MainTex;
// half _Shininess;
fixed4 _Color;
 
struct Input {
    float2 uv_MainTex;
};
 
void surf (Input IN, inout SurfaceOutput o) {
    fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
    o.Albedo = _Color.rgb*tex.rgb;
    o.Gloss = tex.a;
    o.Alpha = tex.a;
}
ENDCG
}
 
FallBack "Mobile/VertexLit"
}