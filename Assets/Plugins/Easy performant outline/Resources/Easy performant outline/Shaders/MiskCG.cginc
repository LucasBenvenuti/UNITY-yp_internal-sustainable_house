#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
#define FetchTexel(uv) UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex,uv)
#define FetchTexelAt(uv) FetchTexelAtFrom(_MainTex,uv,_MainTex_ST)
#define FetchTexelAtWithShift(uv,shift) FetchTexelAtFrom(_MainTex,(uv)+(shift),_MainTex_ST)
#define FetchTexelAtFrom(tex,uv,texST) UNITY_SAMPLE_SCREENSPACE_TEXTURE(tex,uv)
#else
#define FetchTexel(uv) tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(uv,_MainTex_ST))
#define FetchTexelAt(uv) FetchTexelAtFrom(_MainTex,uv,_MainTex_ST)
#define FetchTexelAtWithShift(uv,shift) tex2D(_MainTex,UnityStereoScreenSpaceUVAdjust((uv),(_MainTex_ST))+(shift))
#define FetchTexelAtFrom(tex,uv,texST) tex2D(tex,UnityStereoScreenSpaceUVAdjust((uv),(texST)))
#endif

#define ANY 0
#define DILATE 1
#define BLUR 2
#define NOT_DILATE -1
#define NOT_BLUR -2

#define DefineEdgeDilateParameters float3 normal : TEXCOORD6;

#define ComputeScreenShift float2 clipNormal = (mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, v.normal))).xy; o.vertex.xy += (clipNormal / abs(clipNormal)) * _MainTex_TexelSize.xy * 2.0f * (_EffectSize + v.additionalScale.x) * o.vertex.w;

#define ComputeSmoothScreenShift float2 clipNormal = (mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, v.normal))).xy; o.vertex.xy += (normalize(clipNormal) / _ScreenParams.xy) * 2.0f * _DilateShift * o.vertex.w;

#define DefineTransform float4 first : TEXCOORD0; float4 second : TEXCOORD1; float4 third : TEXCOORD2; float4 fourth : TEXCOORD3; float3 center : TEXCOORD4; float3 size : TEXCOORD5; float3 stageInfo : TEXCOORD6; float2 additionalScale : TEXCOORD7;

#define GetStageModifier(index) (index >= 0 ? v.stageInfo[abs(index)] > 0.9f : v.stageInfo[abs(index)] < 0.9f)

#define TransformVertex(stage) v.vertex =  mul(GetStageModifier(stage) * v.vertex * float4(v.size.xyz, 1) + float4(v.center, 0), float4x4(v.first, v.second, v.third, v.fourth));

#define TransformNormal(stage) v.normal = GetStageModifier(stage) * mul(v.normal, float4x4(v.first, v.second, v.third, float4(0, 0, 0, 0)));

#if UNITY_UV_STARTS_AT_TOP
#define CheckY o.vertex.y *= -_ProjectionParams.x;
#else
#define CheckY;
#endif

#if defined(UNITY_REVERSED_Z) 
#define FixDepth//o.vertex.z += 0.01f; o.vertex.w += 0.01f;
#else
#define FixDepth// o.vertex.z -= 0.01f; o.vertex.w -= 0.01f;
#endif

#define ModifyUV //o.uv.y = 1.0f - o.uv.y;