#ifndef __BEAUTIFY_CORE
#define __BEAUTIFY_CORE

    #include "BeautifyCommon.hlsl"
    #include "BeautifyACESFitted.hlsl"

    float4 _Sharpen;   // x = intensity, y = depth threshold, z = clamp, w = luma relaxation
	float4 _ColorBoost; // x = Brightness, y = Contrast, z = Saturate, w = Daltonize;
    float4 _DepthParams; // x = min/max threshold
	float3 _HardLight;
    float  _Exposure;
    float4 _Vignetting;
    float3 _VignettingData;
    float3 _VignettingData2;
    #define VIGNETTE_ASPECT_RATIO _VignettingData.z
    #define VIGNETTE_CENTER _VignettingData.xy
    #define VIGNETTE_FADE _VignettingData2.x
    #define VIGNETTE_INNER_RING _VignettingData2.y
    #define VIGNETTE_OUTER_RING _VignettingData2.z

    TEXTURE2D(_VignettingMask);
    TEXTURE2D(_LUTTex);
    float4 _LUTTex_TexelSize;
    #define LUT_INTENSITY _HardLight.z

	TEXTURE2D(_FrostTex);
	TEXTURE2D(_FrostNormals);
	float3 _FrostIntensity;
	float4 _FrostTintColor;
    TEXTURE2D_X(_DoFTex);
    float4 _DoFTex_TexelSize;

	float3 ApplyHardLight(float3 rgbM, float lumaM) {
		rgbM = saturate(rgbM);
		float3 hc = rgbM * _HardLight.y;
		if (lumaM < 0.5f) {
			hc *= 2.0 * rgbM;
		} else {
			hc = 1.0 - 2.0 * (1.0 - rgbM) * (1.0 - hc);
		}
		rgbM = lerp(rgbM, hc, _HardLight.x);
		return rgbM;
    }


    float4 ApplyBeautify(float2 uv) : SV_Target
    {
        float3 inc        = float3(_SourceTex_TexelSize.xy, 0);
        float2 scaledUV   = uv * _InputScale;
        float2 uvS        = scaledUV - inc.zy;
        float2 uvN        = scaledUV + inc.zy;
        float2 uvW        = scaledUV - inc.xz;
        float2 uvE        = scaledUV + inc.xz;
		float  depthS     = BEAUTIFY_GET_SCENE_DEPTH_01(uvS);
		float  depthW     = BEAUTIFY_GET_SCENE_DEPTH_01(uvW);
		float  depthE     = BEAUTIFY_GET_SCENE_DEPTH_01(uvE);		
		float  depthN     = BEAUTIFY_GET_SCENE_DEPTH_01(uvN);
		float  maxDepth   = max(depthN, depthS);
		       maxDepth   = max(maxDepth, depthW);
		       maxDepth   = max(maxDepth, depthE);
		float  minDepth   = min(depthN, depthS);
		       minDepth   = min(minDepth, depthW);
		       minDepth   = min(minDepth, depthE);
		
        float  dDepth     = maxDepth - minDepth + 0.00001;
		float  lumaDepth  = saturate(_Sharpen.y / dDepth);

        float3 rgbM       = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, scaledUV).rgb;
               rgbM       = max(0.0.xxx, rgbM);

		float3 rgbS       = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uvS).rgb;
	    float3 rgbW       = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uvW).rgb;
	    float3 rgbE       = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uvE).rgb;
        float3 rgbN       = SAMPLE_TEXTURE2D_X(_SourceTex, sampler_LinearClamp, uvN).rgb;
        float  lumaM      = getLuma(rgbM);

		float3 rgb0       = 1.0.xxx - saturate(rgbM);
		       rgbM.r    *= 1.0 + rgbM.r * rgb0.g * rgb0.b * _ColorBoost.w;
			   rgbM.g    *= 1.0 + rgbM.g * rgb0.r * rgb0.b * _ColorBoost.w;
			   rgbM.b    *= 1.0 + rgbM.b * rgb0.r * rgb0.g * _ColorBoost.w;	
			   rgbM      *= lumaM / (getLuma(rgbM) + 0.0001);

    	float  lumaN      = getLuma(rgbN);
    	float  lumaE      = getLuma(rgbE);
    	float  lumaW      = getLuma(rgbW);
    	float  lumaS      = getLuma(rgbS);

    	float  maxLuma    = max(lumaN,lumaS);
    	       maxLuma    = max(maxLuma, lumaW);
    	       maxLuma    = max(maxLuma, lumaE);
	    float  minLuma    = min(lumaN,lumaS);
	           minLuma    = min(minLuma, lumaW);
	           minLuma    = min(minLuma, lumaE) - 0.000001;
	    float  lumaPower  = 2.0 * lumaM - minLuma - maxLuma;
		float  lumaAtten  = saturate(_Sharpen.w / (maxLuma - minLuma));

        float  depthDist  = _DepthParams.z / abs(depthW - _DepthParams.y);
		float  depthClamp = depthDist > 1.0;
			   depthClamp = max(depthClamp, saturate(_DepthParams.x * depthDist));

		       rgbM      *= 1.0 + clamp(lumaPower * lumaAtten * lumaDepth * _Sharpen.x, -_Sharpen.z, _Sharpen.z) * depthClamp;

               lumaM      = getLuma(rgbM);

        // DOF
        #if BEAUTIFY_DEPTH_OF_FIELD
            float4 dofPix  = SAMPLE_TEXTURE2D_X(_DoFTex, sampler_LinearClamp, uv);
            if (_DoFTex_TexelSize.z < _SourceTex_TexelSize.z) {
                float  CoC = getCoc(uv) / COC_BASE;
                dofPix.a   = lerp(CoC, dofPix.a, _DoFTex_TexelSize.z / _SourceTex_TexelSize.z);
            }
            rgbM = lerp(rgbM, dofPix.rgb, saturate(dofPix.a * COC_BASE));
            lumaM = getLuma(rgbM);
        #endif

        #if BEAUTIFY_TONEMAP_ACES
            rgbM   *= _Exposure;
            rgbM    = ACESFitted(rgbM);
            lumaM   = getLuma(rgbM);
        #endif

        // LUT
        #if BEAUTIFY_LUT
            #if !UNITY_COLORSPACE_GAMMA
                rgbM          = LinearToSRGB(rgbM);
            #endif
    	    float3 lutST  = float3(_LUTTex_TexelSize.x, _LUTTex_TexelSize.y, _LUTTex_TexelSize.w - 1);
            float3 lookUp = saturate(rgbM) * lutST.zzz;
            lookUp.xy     = lutST.xy * (lookUp.xy + 0.5);
            float slice   = floor(lookUp.z);
            lookUp.x     += slice * lutST.y;
            float2 lookUpNextSlice = float2(lookUp.x + lutST.y, lookUp.y);
            float3 lut    = lerp(SAMPLE_TEXTURE2D(_LUTTex, sampler_LinearClamp, lookUp.xy).rgb, SAMPLE_TEXTURE2D(_LUTTex, sampler_LinearClamp, lookUpNextSlice).rgb, lookUp.z - slice);
            rgbM          = lerp(rgbM, lut, LUT_INTENSITY);
            #if !UNITY_COLORSPACE_GAMMA
                rgbM          = SRGBToLinear(rgbM);
            #endif
            lumaM         = getLuma(rgbM);
        #endif

        // color tweaks
		float maxComponent = max(rgbM.r, max(rgbM.g, rgbM.b));
		float minComponent = min(rgbM.r, min(rgbM.g, rgbM.b));
		float sat          = saturate(maxComponent - minComponent);
		      rgbM        *= 1.0 + _ColorBoost.z * (1.0 - sat) * (rgbM - lumaM);
		      rgbM         = (rgbM - 0.5.xxx) * _ColorBoost.y + 0.5.xxx;
              rgbM         = ApplyHardLight(rgbM, lumaM);
	          rgbM        *= _ColorBoost.x;

        // dither
        const float3 magic = float3(0.06711056, 0.00583715, 52.9829189);
        float dither       = frac(magic.z * frac(dot(uv * _SourceTex_TexelSize.zw, magic.xy))) - 0.5;
               rgbM       += dither * _DepthParams.w;

               rgbM        = max(0.0.xxx, rgbM);

        // vignetting
        #if BEAUTIFY_VIGNETTING_MASK
            float2 vd = float2(uv.x - VIGNETTE_CENTER.x, (uv.y - VIGNETTE_CENTER.y) * VIGNETTE_ASPECT_RATIO);
            float  vmask = SAMPLE_TEXTURE2D(_VignettingMask, sampler_LinearClamp, uv).a;
            float  vt = _Vignetting.a * saturate(VIGNETTE_INNER_RING + vmask * dot(vd, vd));
            rgbM = lerp(rgbM, _Vignetting.rgb * lumaM, vt);
        #else
            float2 vd = float2(uv.x - VIGNETTE_CENTER.x, (uv.y - VIGNETTE_CENTER.y) * VIGNETTE_ASPECT_RATIO);
            rgbM = lerp(_Vignetting.rgb, rgbM, saturate( (1.0 - _Vignetting.a) + saturate((dot(vd, vd) - VIGNETTE_OUTER_RING) / (VIGNETTE_INNER_RING - VIGNETTE_OUTER_RING) ) - VIGNETTE_FADE)) ;
        #endif

        return float4(rgbM, 1);
    }


    float4 FragBeautify(Varyings input) : SV_Target {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        float2 uv = input.uv;

        #if BEAUTIFY_FROZEN
            float2 da = uv - 0.5.xx;
		    float dd = dot(da, da) * 2.0;
		    dd = saturate(pow(dd, _FrostIntensity.y)) * _FrostIntensity.x;
		    float4 frost = SAMPLE_TEXTURE2D(_FrostTex, sampler_LinearRepeat, uv);
			float4 norm = SAMPLE_TEXTURE2D(_FrostNormals, sampler_LinearRepeat,uv);
			norm.rgb = UnpackNormal(norm);
			float2 disp = norm.xy * _FrostIntensity.z * dd;
			uv -= normalize(da) * disp;
		#endif

        float4 color = ApplyBeautify(uv);

        #if BEAUTIFY_FROZEN
            frost.rgb *= dd;
            color.rgb = frost.rgb * _FrostTintColor.rgb + color.rgb * (1.0 - frost.g);
        #endif

        return color;
    }

#endif // __BEAUTIFY_CORE