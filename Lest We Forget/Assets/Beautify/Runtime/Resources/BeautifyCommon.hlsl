#ifndef __BEAUTIFY_COMMON_INCLUDE
#define __BEAUTIFY_COMMON_INCLUDE

// Set to 1 to support orthographic mode
#define BEAUTIFY_ORTHO 0


SAMPLER(sampler_PointClamp);
SAMPLER(sampler_PointRepeat);
SAMPLER(sampler_LinearClamp);
SAMPLER(sampler_LinearRepeat);


#if BEAUTIFY_ORTHO
    #if UNITY_REVERSED_Z
        #define BEAUTIFY_GET_SCENE_DEPTH_01(x) (1.0 - SampleCameraDepth(x))
        #define BEAUTIFY_GET_SCENE_DEPTH_EYE(x) ((1.0 - SampleCameraDepth(x)) * _ProjectionParams.z)
    #else
        #define BEAUTIFY_GET_SCENE_DEPTH_01(x) SampleCameraDepth(x)
        #define BEAUTIFY_GET_SCENE_DEPTH_EYE(x) (SampleCameraDepth(x) * _ProjectionParams.z)
    #endif
#else
    #define BEAUTIFY_GET_SCENE_DEPTH_01(x) Linear01Depth(SampleCameraDepth(x), _ZBufferParams)
    #define BEAUTIFY_GET_SCENE_DEPTH_EYE(x) LinearEyeDepth(SampleCameraDepth(x), _ZBufferParams)
#endif


// Base for COC - required for Android
#define COC_BASE 128.0


// Optimization for SSPR
#define uvN uv1
#define uvE uv2
#define uvW uv3
#define uvS uv4


#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED) || defined(SINGLE_PASS_STEREO)
    #define BEAUTIFY_VERTEX_CROSS_UV_DATA
    #define BEAUTIFY_VERTEX_OUTPUT_CROSS_UV(o)
    #define BEAUTIFY_VERTEX_OUTPUT_GAUSSIAN_UV(o)

    #define BEAUTIFY_FRAG_SETUP_CROSS_UV(i) float3 uvInc = float3(_InputTex_TexelSize.x, _InputTex_TexelSize.y, 0); float2 uvN = i.uv + uvInc.zy; float2 uvE = i.uv + uvInc.xz; float2 uvW = i.uv - uvInc.xz; float2 uvS = i.uv - uvInc.zy;

    #if defined(BEAUTIFY_BLUR_HORIZ)
        #define BEAUTIFY_FRAG_SETUP_GAUSSIAN_UV(i) float2 inc = float2(_InputTex_TexelSize.x * 1.3846153846 * _BlurScale, 0); float2 uv1 = i.uv - inc; float2 uv2 = i.uv + inc; float2 inc2 = float2(_InputTex_TexelSize.x * 3.2307692308 * _BlurScale, 0); float2 uv3 = i.uv - inc2; float2 uv4 = i.uv + inc2;
    #else
        #define BEAUTIFY_FRAG_SETUP_GAUSSIAN_UV(i) float2 inc = float2(0, _InputTex_TexelSize.y * 1.3846153846 * _BlurScale); float2 uv1 = i.uv - inc; float2 uv2 = i.uv + inc; float2 inc2 = float2(0, _InputTex_TexelSize.y * 3.2307692308 * _BlurScale); float2 uv3 = i.uv - inc2; float2 uv4 = i.uv + inc2;
    #endif

#else
    #define BEAUTIFY_VERTEX_CROSS_UV_DATA float2 uvN : uv1; float2 uvW: uv2; float2 uvE: uv3; float2 uvS: uv4;

    #define BEAUTIFY_VERTEX_OUTPUT_CROSS_UV(o) float3 uvInc = float3(_InputTex_TexelSize.x, _InputTex_TexelSize.y, 0); o.uvN = o.uv + uvInc.zy; o.uvE = o.uv + uvInc.xz; o.uvW = o.uv - uvInc.xz; o.uvS = o.uv - uvInc.zy;
    #define BEAUTIFY_FRAG_SETUP_CROSS_UV(i) float2 uv1 = i.uv1; float2 uv2 = i.uv2; float2 uv3 = i.uv3; float2 uv4 = i.uv4;

    #if defined(BEAUTIFY_BLUR_HORIZ)
        #define BEAUTIFY_VERTEX_OUTPUT_GAUSSIAN_UV(o) float2 inc = float2(_InputTex_TexelSize.x * 1.3846153846 * _BlurScale, 0); o.uv1 = o.uv - inc; o.uv2 = o.uv + inc; float2 inc2 = float2(_InputTex_TexelSize.x * 3.2307692308 * _BlurScale, 0); o.uv3 = o.uv - inc2; o.uv4 = o.uv + inc2;
    #else
        #define BEAUTIFY_VERTEX_OUTPUT_GAUSSIAN_UV(o) float2 inc = float2(0, _InputTex_TexelSize.y * 1.3846153846 * _BlurScale); o.uv1 = o.uv - inc; o.uv2 = o.uv + inc; float2 inc2 = float2(0, _InputTex_TexelSize.y * 3.2307692308 * _BlurScale); o.uv3 = o.uv - inc2; o.uv4 = o.uv + inc2;
    #endif
    #define BEAUTIFY_FRAG_SETUP_GAUSSIAN_UV(i) float2 uv1 = i.uv1; float2 uv2 = i.uv2; float2 uv3 = i.uv3; float2 uv4 = i.uv4;

#endif


// Common functions


    TEXTURE2D_X(_SourceTex);
    float4 _SourceTex_TexelSize;

    TEXTURE2D_X(_InputTex);
    float4 _InputTex_TexelSize;
    float2 _InputScale;

    float4    _BokehData;
    float4    _BokehData2;
    float3    _BokehData3;
    float     _BlurScale;

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float2 uv   : uv0;
        UNITY_VERTEX_OUTPUT_STEREO
    };


    Varyings Vert(Attributes input)
    {
        Varyings output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.uv = GetFullScreenTriangleTexCoord(input.vertexID);
        return output;
    }

    float4 FragCopy(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        return SAMPLE_TEXTURE2D_X(_InputTex, sampler_PointClamp, input.uv);
    }


    inline float getLuma(float3 rgb) { 
	    const float3 lum = float3(0.299, 0.587, 0.114);
	    return dot(rgb, lum);
    }


    float3 getNormal(float depth, float depth1, float depth2, float2 offset1, float2 offset2) {
        float3 p1 = float3(offset1, depth1 - depth);
        float3 p2 = float3(offset2, depth2 - depth);
        float3 normal = cross(p1, p2);
        return normalize(normal);
    }
    
    
    float getCoc(float2 uv) {
        float depth  = BEAUTIFY_GET_SCENE_DEPTH_EYE(uv);
        if (depth>_BokehData3.y) return 0;
        float xd     = abs(depth - _BokehData.x) - _BokehData2.x * (depth < _BokehData.x);
        return 0.5 * _BokehData.y * xd/depth;    // radius of CoC
    }


#endif // __BEAUTIFY_COMMON_INCLUDE