using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace BeautifyHDRP {

    [Serializable, VolumeComponentMenu("Post-processing/Beautify")]
    public sealed class Beautify : CustomPostProcessVolumeComponent, IPostProcessComponent {

        #region Effect parameters
        public enum BlinkStyle {
            Cutscene,
            Human
        }

        public enum DoFFocusMode {
            FixedDistance,
            AutoFocus,
            FollowTarget
        }

        public enum DoFBokehComposition {
            Integrated,
            Separated
        }
        public enum DoFCameraSettings {
            Classic,
            Real
        }

        public enum TonemapOperator {
            Linear = 0,
            ACES = 1
        }


        [Serializable]
        public sealed class BeautifyBlinkStyleParameter : VolumeParameter<BlinkStyle> { }

        [Serializable]
        public sealed class BeautifyDoFFocusModeParameter : VolumeParameter<DoFFocusMode> { }

        [Serializable]
        public sealed class BeautifyDoFFilterModeParameter : VolumeParameter<FilterMode> { }

        [Serializable]
        public sealed class BeautifyDoFBokehCompositionParameter : VolumeParameter<DoFBokehComposition> { }

        [Serializable]
        public sealed class BeautifyDoFCameraSettingsParameter : VolumeParameter<DoFCameraSettings> { }

        [Serializable]
        public sealed class BeautifyLayerMaskParameter : VolumeParameter<LayerMask> { }

        [Serializable]
        public sealed class BeautifyTonemapOperatorParameter : VolumeParameter<TonemapOperator> { }



        [Tooltip("Global Beautify intensity")]
        [Header("Global Intensity")]
        public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

        [Header("Sharpen Options")]
        [Tooltip("Controls the intensity of the effect.")]
        public ClampedFloatParameter sharpen = new ClampedFloatParameter(2f, 0f, 25f);
        [Tooltip("Minimum depth to apply sharpen"),]
        public ClampedFloatParameter sharpenMinDepth = new ClampedFloatParameter(0f, 0f, 1f);
        [Tooltip("Maximum depth to apply sharpen")]
        public ClampedFloatParameter sharpenMaxDepth = new ClampedFloatParameter(0.999f, 0f, 1f);
        [Tooltip("Min/max depth range falloff.")]
        public ClampedFloatParameter sharpenMinMaxDepthFallOff = new ClampedFloatParameter(0f, 0f, 1f);
        [Tooltip("Depth difference sensibility. Avoids artifacts on geometry edges.")]
        public ClampedFloatParameter sharpenDepthThreshold = new ClampedFloatParameter(0.035f, 0f, 0.05f);
        [Tooltip("Attenuates sharpen effect on bright areas to reduce artifacts.")]
        public ClampedFloatParameter sharpenRelaxation = new ClampedFloatParameter(0.08f, 0f, 0.2f);
        [Tooltip("Limits final sharpen to be applied.")]
        public ClampedFloatParameter sharpenClamp = new ClampedFloatParameter(0.45f, 0f, 1f);
        [Tooltip("Reduces sharpen effect while camera is moving to reduce flickering.")]
        public ClampedFloatParameter sharpenMotionSensibility = new ClampedFloatParameter(0.5f, 0f, 1f);

        [Header("Dithering")]
        [Tooltip("Controls the intensity of the dithering to reduce banding.")]
        public ClampedFloatParameter ditherStrength = new ClampedFloatParameter(0.006f, 0f, 0.03f);

        [Header("Color Tweaks")]
        public BeautifyTonemapOperatorParameter tonemap = new BeautifyTonemapOperatorParameter { value = TonemapOperator.Linear };
        public FloatParameter tonemapExposure = new FloatParameter(1f);
        [Tooltip("Increase to make colors more vivid. Reduce to desaturate colors.")]
        public ClampedFloatParameter saturate = new ClampedFloatParameter(0.5f, -2f, 3f);
        public FloatParameter brightness = new FloatParameter(1.05f);
        public ClampedFloatParameter contrast = new ClampedFloatParameter(1.02f, 0.5f, 1.5f);
        public ClampedFloatParameter hardlightIntensity = new ClampedFloatParameter(0.5f, 0, 1f);
        public ClampedFloatParameter hardlightBlend = new ClampedFloatParameter(0f, 0, 1f);
        [Tooltip("Boosts primary colors to compensate daltonism.")]
        public ClampedFloatParameter daltonize = new ClampedFloatParameter(0f, 0f, 2f);
        public TextureParameter lut = new TextureParameter(null);
        public ClampedFloatParameter lutIntensity = new ClampedFloatParameter(0f, 0, 1f);

        [Header("Depth of Field")]
        public BoolParameter depthOfField = new BoolParameter(false);
        public BeautifyDoFFocusModeParameter depthOfFieldFocusMode = new BeautifyDoFFocusModeParameter { value = DoFFocusMode.FixedDistance };
        public FloatParameter depthOfFieldAutofocusMinDistance = new FloatParameter(0);
        public FloatParameter depthOfFieldAutofocusMaxDistance = new FloatParameter(10000);
        public Vector2Parameter depthOfFieldAutofocusViewportPoint = new Vector2Parameter(new Vector2(0.5f, 0.5f));
        public FloatParameter depthOfFieldAutofocusDistanceShift = new FloatParameter(0);
        public BeautifyLayerMaskParameter depthOfFieldAutofocusLayerMask = new BeautifyLayerMaskParameter { value = -1 };
        public FloatParameter depthOfFieldDistance = new FloatParameter(10f);
        public BeautifyDoFCameraSettingsParameter depthOfFieldCameraSettings = new BeautifyDoFCameraSettingsParameter { value = DoFCameraSettings.Classic };
        [Tooltip("The distance between the lens center and the camera's sensor in m.")]
        public ClampedFloatParameter depthOfFieldFocalLength = new ClampedFloatParameter(0.050f, 0.005f, 0.5f);
        public FloatParameter depthOfFieldAperture = new FloatParameter(2.8f);
        [Tooltip("The distance between the lens center and the camera's sensor in mm.")]
        public ClampedFloatParameter depthOfFieldFocalLengthReal = new ClampedFloatParameter(50, 1, 300);
        [Tooltip("The F-stop or F-number is the relation between the focal length and the diameter of the aperture")]
        public ClampedFloatParameter depthOfFieldFStop = new ClampedFloatParameter(2, 1, 32);
        [Tooltip("Represents the height of the virtual image sensor. By default, it uses a 24mm image sensor of a classic full-frame camera")]
        public ClampedFloatParameter depthOfFieldImageSensorHeight = new ClampedFloatParameter(24, 1, 48);
        public ClampedFloatParameter depthOfFieldFocusSpeed = new ClampedFloatParameter(1f, 0.001f, 5f);
        public BoolParameter depthOfFieldForegroundBlur = new BoolParameter(true);
        public BoolParameter depthOfFieldForegroundBlurHQ = new BoolParameter(false);
        public ClampedFloatParameter depthOfFieldForegroundBlurHQSpread = new ClampedFloatParameter(4, 0, 32);
        public FloatParameter depthOfFieldForegroundDistance = new FloatParameter(0.25f);
        public BoolParameter depthOfFieldBokeh = new BoolParameter(true);
        public ClampedFloatParameter depthOfFieldBokehThreshold = new ClampedFloatParameter(1f, 0.5f, 3f);
        public ClampedFloatParameter depthOfFieldBokehIntensity = new ClampedFloatParameter(2f, 0, 8f);
        public BeautifyDoFBokehCompositionParameter depthOfFieldBokehComposition = new BeautifyDoFBokehCompositionParameter { value = DoFBokehComposition.Integrated };
        public ClampedIntParameter depthOfFieldDownsampling = new ClampedIntParameter(2, 1, 5);
        public ClampedIntParameter depthOfFieldMaxSamples = new ClampedIntParameter(6, 2, 16);
        public FloatParameter depthOfFieldMaxBrightness = new FloatParameter(1000f);
        public ClampedFloatParameter depthOfFieldMaxDistance = new ClampedFloatParameter(1f, 0, 1f);


        [Header("Vignetting")]
        public ClampedFloatParameter vignettingOuterRing = new ClampedFloatParameter(0f, 0f, 1f);
        public ClampedFloatParameter vignettingInnerRing = new ClampedFloatParameter(0, 0, 1f);
        public ClampedFloatParameter vignettingFade = new ClampedFloatParameter(0, 0, 1f);
        public BoolParameter vignettingCircularShape = new BoolParameter(false);
        public ClampedFloatParameter vignettingAspectRatio = new ClampedFloatParameter(1f, 0, 1f);
        public ClampedFloatParameter vignettingBlink = new ClampedFloatParameter(0f, 0, 1);
        public BeautifyBlinkStyleParameter vignettingBlinkStyle = new BeautifyBlinkStyleParameter { value = BlinkStyle.Cutscene };
        public Vector2Parameter vignettingCenter = new Vector2Parameter(new Vector2(0.5f, 0.5f));
        public ColorParameter vignettingColor = new ColorParameter(new Color(0f, 0f, 0f, 1f));
        public TextureParameter vignettingMask = new TextureParameter(null);

        [Header("Pixelate")]
        public IntParameter pixelateSize = new IntParameter(1);

        [Header("Frost FX")]
        public ClampedFloatParameter frostIntensity = new ClampedFloatParameter(0f, 0f, 1.5f);
        public ClampedFloatParameter frostSpread = new ClampedFloatParameter(1.2f, 1f, 5f);
        public ClampedFloatParameter frostDistortion = new ClampedFloatParameter(0.25f, 0f, 1f);
        public TextureParameter frostTexture = new TextureParameter(null);
        public TextureParameter frostDistortTexture = new TextureParameter(null);
        public ColorParameter frostTintColor = new ColorParameter(Color.white);
        #endregion

        #region Private stuff
        Material m_Material;
        Vector3 camPrevForward, camPrevPos;
        float currSens;
        Vector4 dofLastBokehData;
        float dofPrevDistance, dofLastAutofocusDistance;

        // Scene dependant settings
        BeautifySettings sceneSettings;
        RTHandle rtSource;
        RTHandle rtDownscaled, rtDoF;
        RTHandle rtDoFTempBlur1, rtDoFTempBlur2;
        RTHandle rtDoFTempBlurAlpha;
        RTHandle rtBokeh;
        const UnityEngine.Experimental.Rendering.GraphicsFormat DOF_RT_GRAPHICS_FORMAT = UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SFloat;
        List<RTHandle> rtHandles;
        Texture2D defaultFrost, defaultFrostDisp;

#if UNITY_EDITOR
        public static CameraType captureCameraType = CameraType.SceneView;
        public static bool requestScreenCapture;
        RTHandle rtCapture;
#endif
        #endregion


        public bool IsActive() => m_Material != null && intensity.value > 0f;

        // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > HDRP Default Settings).
        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        const string kShaderName = "Hidden/Shader/Beautify";

        public static float blink;

        static class ShaderParams {
            public static int sharpen = Shader.PropertyToID("_Sharpen");
            public static int depthParams = Shader.PropertyToID("_DepthParams");
            public static int colorBoost = Shader.PropertyToID("_ColorBoost");
            public static int hardLight = Shader.PropertyToID("_HardLight");
            public static int sourceTex = Shader.PropertyToID("_SourceTex");
            public static int inputTex = Shader.PropertyToID("_InputTex");
            public static int inputScale = Shader.PropertyToID("_InputScale");
            public static int vignette = Shader.PropertyToID("_Vignetting");
            public static int vignetteData = Shader.PropertyToID("_VignettingData");
            public static int vignetteData2 = Shader.PropertyToID("_VignettingData2");
            public static int vignetteMask = Shader.PropertyToID("_VignettingMask");
            public static int lutTexture = Shader.PropertyToID("_LUTTex");
            public static int lutIntensity = Shader.PropertyToID("_LUTIntensity");
            public static int frostIntensity = Shader.PropertyToID("_FrostIntensity");
            public static int frostTintColor = Shader.PropertyToID("_FrostTintColor");
            public static int frostTexture = Shader.PropertyToID("_FrostTex");
            public static int frostDistortionTexture = Shader.PropertyToID("_FrostNormals");
            public static int dofBokehData = Shader.PropertyToID("_BokehData");
            public static int dofBokehData2 = Shader.PropertyToID("_BokehData2");
            public static int dofBokehData3 = Shader.PropertyToID("_BokehData3");
            public static int blurScale = Shader.PropertyToID("_BlurScale");
            public static int dofRT = Shader.PropertyToID("_DoFTex");
            public static int dofTempBlurDoFAlphaRT = Shader.PropertyToID("_BeautifyTempBlurAlphaDoF");
            public static int dofTempBlurDoFTemp1RT = Shader.PropertyToID("_BeautifyTempBlurPass1DoF");
            public static int dofTempBlurDoFTemp2RT = Shader.PropertyToID("_BeautifyTempBlurPass2DoF");
            public static int tonemapExposure = Shader.PropertyToID("_Exposure");
            public static int lutPreview = Shader.PropertyToID("_LUTPreview");

            public const string SKW_VIGNETTING_MASK = "BEAUTIFY_VIGNETTING_MASK";
            public const string SKW_LUT = "BEAUTIFY_LUT";
            public const string SKW_TONEMAP_ACES = "BEAUTIFY_TONEMAP_ACES";
            public const string SKW_FROST = "BEAUTIFY_FROZEN";
            public const string SKW_DEPTH_OF_FIELD = "BEAUTIFY_DEPTH_OF_FIELD";

            public enum Pass {
                Beautify = 0,
                Copy = 1,
                DoFCoC = 2,
                DoFBlurHorizontally = 3,
                DoFBlurVertically = 4,
                DoFBlur = 5,
                DoFBlurWithoutBokeh = 6,
                DoFBokeh = 7,
                Additive = 8,
                DoFBlurBokeh = 9
            }
        }

        void OnValidate() {
            pixelateSize.value = Mathf.Max(1, pixelateSize.value);
        }

        public override void Setup() {
            if (Shader.Find(kShaderName) != null) {
                m_Material = new Material(Shader.Find(kShaderName));
            } else {
                Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume Beautify is unable to load.");
            }

            if (defaultFrost == null) {
                defaultFrost = Resources.Load<Texture2D>("FrostFX/Frost");
                defaultFrostDisp = Resources.Load<Texture2D>("FrostFX/FrostNormals");
            }

            rtHandles = new List<RTHandle>();
            CheckSceneSettings();
            BeautifySettings.UnloadBeautify();
        }

        void PrepareRT(ref RTHandle rt, HDCamera camera, int downscale, UnityEngine.Experimental.Rendering.GraphicsFormat colorFormat, FilterMode filterMode = FilterMode.Bilinear) {
            int w = Mathf.Max(1, camera.actualWidth / downscale);
            int h = Mathf.Max(1, camera.actualHeight / downscale);

            if (rt != null) {
                if (rt.rt.width != w || rt.rt.height != h) {
                    rt.Release();
                } else {
                    return;
                }
            }

            rt = RTHandles.Alloc(w, h, colorFormat: colorFormat, filterMode: filterMode, depthBufferBits: 0, autoGenerateMips: false, msaaSamples: MSAASamples.None, dimension: rtSource.rt.dimension);
            rtHandles.Add(rt);
        }

        void CheckDownscaledRT(HDCamera camera) {
            PrepareRT(ref rtDownscaled, camera, pixelateSize.value, rtSource.rt.graphicsFormat);
        }

        void CheckDepthOfFieldRTs(HDCamera camera) {
            int downsampling = depthOfFieldDownsampling.value;
            PrepareRT(ref rtDoF, camera, downsampling, DOF_RT_GRAPHICS_FORMAT);
            PrepareRT(ref rtDoFTempBlur1, camera, downsampling, DOF_RT_GRAPHICS_FORMAT);
            PrepareRT(ref rtDoFTempBlur2, camera, downsampling, DOF_RT_GRAPHICS_FORMAT);
            PrepareRT(ref rtDoFTempBlurAlpha, camera, downsampling, DOF_RT_GRAPHICS_FORMAT);
            PrepareRT(ref rtBokeh, camera, downsampling, DOF_RT_GRAPHICS_FORMAT);
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination) {
            if (m_Material == null) return;

            rtSource = source;
            Camera cam = camera.camera;

#if UNITY_EDITOR
            if (requestScreenCapture && cam != null && cam.cameraType == captureCameraType) {
                requestScreenCapture = false;

                if (rtCapture != null) {
                    rtCapture.Release();
                }

                rtCapture = RTHandles.Alloc(Vector3.one);
                FullScreenBlit(cmd, source, rtCapture, (int)ShaderParams.Pass.Copy);
                cmd.SetGlobalTexture(ShaderParams.lutPreview, rtCapture);
            }
#endif

            float power = intensity.value;

            // Motion sensibility
            if (Application.isPlaying) {
                Transform camTransform = cam.transform;
                Vector3 camForward = camTransform.forward;
                Vector3 camPos = camTransform.position;
                float angleDiff = Vector3.Angle(camPrevForward, camForward) * sharpenMotionSensibility.value;
                float posDiff = (camPos - camPrevPos).sqrMagnitude * 10f * sharpenMotionSensibility.value;

                float diff = angleDiff + posDiff;

                if (diff > 0.1f) {
                    camPrevForward = camForward;
                    camPrevPos = camPos;

                    if (diff > sharpenMotionSensibility.value)
                        diff = sharpenMotionSensibility.value;

                    currSens += diff;
                    float min = sharpen.value * sharpenMotionSensibility.value * 0.75f;
                    float max = sharpen.value * (1f + sharpenMotionSensibility.value) * 0.5f;
                    currSens = Mathf.Clamp(currSens, min, max);
                } else {
                    currSens *= 0.75f;
                }
            }

            float tempSharpen = Mathf.Clamp(sharpen.value - currSens, 0, sharpen.value) * power;
            Vector4 sharpenData = new Vector4(tempSharpen, sharpenDepthThreshold.value, sharpenClamp.value, sharpenRelaxation.value);
            m_Material.SetVector(ShaderParams.sharpen, sharpenData);
            float dither = pixelateSize.value <= 1 ? ditherStrength.value * power : 0;
            m_Material.SetVector(ShaderParams.depthParams, new Vector4(sharpenMinMaxDepthFallOff.value, (sharpenMaxDepth.value + sharpenMinDepth.value) * 0.5f, Mathf.Abs(sharpenMaxDepth.value - sharpenMinDepth.value) * 0.5f, dither));
            m_Material.SetVector(ShaderParams.hardLight, new Vector4(hardlightBlend.value, hardlightIntensity.value, lutIntensity.value, 0));
            float cont = 1.0f + (contrast.value - 1.0f) / 2.2f;
            float tempContrast = (1f - power) + cont * power;
            float tempSat = (1f - power) + saturate.value * power;
            float tempBrightness = (1f - power) + brightness.value * power;
            m_Material.SetVector(ShaderParams.colorBoost, new Vector4(tempBrightness, tempContrast, tempSat, daltonize.value * 10f * power));

            DoDoF(camera, source, cmd);
            DoLUT();
            DoVignette(cam);
            DoFrost();

            if (tonemap.value == TonemapOperator.ACES) {
                m_Material.EnableKeyword(ShaderParams.SKW_TONEMAP_ACES);
                m_Material.SetFloat(ShaderParams.tonemapExposure, tonemapExposure.value);
            } else {
                m_Material.DisableKeyword(ShaderParams.SKW_TONEMAP_ACES);
            }

            cmd.SetGlobalTexture(ShaderParams.sourceTex, source);
            cmd.SetGlobalVector(ShaderParams.inputScale, source.rtHandleProperties.rtHandleScale);
            float pixelScale = 1f / pixelateSize.value;

            if (pixelScale != 1f) {
                CheckDownscaledRT(camera);
                HDUtils.DrawFullScreen(cmd, m_Material, rtDownscaled, shaderPassId: (int)ShaderParams.Pass.Beautify);
                FullScreenBlit(cmd, rtDownscaled, destination, (int)ShaderParams.Pass.Copy);
            } else {
                HDUtils.DrawFullScreen(cmd, m_Material, destination, shaderPassId: (int)ShaderParams.Pass.Beautify);
            }
        }

        void FullScreenBlit(CommandBuffer cmd, RTHandle source, RTHandle destination, int passIndex) {
            cmd.SetGlobalVector(ShaderParams.inputScale, source.rtHandleProperties.rtHandleScale);
            cmd.SetGlobalTexture(ShaderParams.inputTex, source);
            HDUtils.DrawFullScreen(cmd, m_Material, destination, shaderPassId: passIndex);
        }

        void DoDoF(HDCamera camera, RTHandle source, CommandBuffer cmd) {
            Camera cam = camera.camera;

            if (!depthOfField.value || cam.cameraType != CameraType.Game) {
                m_Material.DisableKeyword(ShaderParams.SKW_DEPTH_OF_FIELD);
                return;
            }

            m_Material.EnableKeyword(ShaderParams.SKW_DEPTH_OF_FIELD);
            CheckDepthOfFieldRTs(camera);
            UpdateDepthOfFieldData(cmd, cam);

            // compute coc
            FullScreenBlit(cmd, source, rtDoF, (int)ShaderParams.Pass.DoFCoC);

            if (depthOfFieldForegroundBlur.value && depthOfFieldForegroundBlurHQ.value) {
                BlurThisAlpha(cmd, rtDoF, depthOfFieldForegroundBlurHQSpread.value);
            }

            // hex blur
            if (depthOfFieldBokehComposition.value == DoFBokehComposition.Integrated || !depthOfFieldBokeh.value) {
                ShaderParams.Pass pass = depthOfFieldBokeh.value ? ShaderParams.Pass.DoFBlur : ShaderParams.Pass.DoFBlurWithoutBokeh;
                BlurThisDoF(cmd, rtDoF, (int)pass);
            } else {
                BlurThisDoF(cmd, rtDoF, (int)ShaderParams.Pass.DoFBlurWithoutBokeh);

                // separate & blend bokeh
                FullScreenBlit(cmd, source, rtBokeh, (int)ShaderParams.Pass.DoFBokeh);
                BlurThisDoF(cmd, rtBokeh, (int)ShaderParams.Pass.DoFBlurBokeh);
                FullScreenBlit(cmd, rtBokeh, rtDoF, (int)ShaderParams.Pass.Additive);
            }

            cmd.SetGlobalTexture(ShaderParams.dofRT, rtDoF);
        }

        void BlurThisDoF(CommandBuffer cmd, RTHandle rt, int renderPass) {
            UpdateDepthOfFieldBlurData(cmd, new Vector2(0.44721f, -0.89443f));
            FullScreenBlit(cmd, rt, rtDoFTempBlur1, renderPass);
            UpdateDepthOfFieldBlurData(cmd, new Vector2(-1f, 0f));
            FullScreenBlit(cmd, rtDoFTempBlur1, rtDoFTempBlur2, renderPass);
            UpdateDepthOfFieldBlurData(cmd, new Vector2(0.44721f, 0.89443f));
            FullScreenBlit(cmd, rtDoFTempBlur2, rt, renderPass);
        }


        void BlurThisAlpha(CommandBuffer cmd, RTHandle rt, float blurScale = 1f) {
            cmd.SetGlobalFloat(ShaderParams.blurScale, blurScale);
            FullScreenBlit(cmd, rt, rtDoFTempBlurAlpha, (int)ShaderParams.Pass.DoFBlurHorizontally);
            FullScreenBlit(cmd, rtDoFTempBlurAlpha, rt, (int)ShaderParams.Pass.DoFBlurVertically);
        }

        void UpdateDepthOfFieldData(CommandBuffer cmd, Camera cam) {
            // TODO: get focal length from camera FOV: FOV = 2 arctan (x/2f) x = diagonal of film (0.024mm)
            if (!CheckSceneSettings()) return;

            float d = depthOfFieldDistance.value;

            switch ((int)depthOfFieldFocusMode.value) {
                case (int)DoFFocusMode.AutoFocus:
                    UpdateDoFAutofocusDistance(cam);
                    d = dofLastAutofocusDistance > 0 ? dofLastAutofocusDistance : cam.farClipPlane;
                    BeautifySettings.depthOfFieldCurrentFocalPointDistance = dofLastAutofocusDistance;
                    break;
                case (int)DoFFocusMode.FollowTarget:
                    if (sceneSettings.depthOfFieldTarget != null) {
                        Vector3 spos = cam.WorldToScreenPoint(sceneSettings.depthOfFieldTarget.position);
                        if (spos.z < 0) {
                            d = cam.farClipPlane;
                        } else {
                            d = Vector3.Distance(cam.transform.position, sceneSettings.depthOfFieldTarget.position);
                        }
                    }
                    break;
            }

            if (sceneSettings.OnBeforeFocus != null) {
                d = sceneSettings.OnBeforeFocus(d);
            }

            dofPrevDistance = Mathf.Lerp(dofPrevDistance, d, Application.isPlaying ? depthOfFieldFocusSpeed.value * Time.unscaledDeltaTime * 30f : 1f);
            float dofCoc;

            if (depthOfFieldCameraSettings.value == DoFCameraSettings.Real) {
                float focalLength = depthOfFieldFocalLengthReal.value;
                float aperture = (focalLength / depthOfFieldFStop.value);
                dofCoc = aperture * (focalLength / Mathf.Max(dofPrevDistance * 1000f - focalLength, 0.001f)) * (1f / depthOfFieldImageSensorHeight.value) * cam.pixelHeight;
            } else {
                dofCoc = depthOfFieldAperture.value * (depthOfFieldFocalLength.value / Mathf.Max(dofPrevDistance - depthOfFieldFocalLength.value, 0.001f)) * (1f / 0.024f);
            }

            dofLastBokehData = new Vector4(dofPrevDistance, dofCoc, 0, 0);
            cmd.SetGlobalVector(ShaderParams.dofBokehData, dofLastBokehData);
            cmd.SetGlobalVector(ShaderParams.dofBokehData2, new Vector4(depthOfFieldForegroundBlur.value ? depthOfFieldForegroundDistance.value : cam.farClipPlane, depthOfFieldMaxSamples.value, depthOfFieldBokehThreshold.value, depthOfFieldBokehIntensity.value * depthOfFieldBokehIntensity.value));
            cmd.SetGlobalVector(ShaderParams.dofBokehData3, new Vector3(depthOfFieldMaxBrightness.value, depthOfFieldMaxDistance.value * (cam.farClipPlane + 1f), 0));
        }

        void UpdateDoFAutofocusDistance(Camera cam) {
            Vector3 p = depthOfFieldAutofocusViewportPoint.value;
            p.z = 10f;
            Ray r = cam.ViewportPointToRay(p);

            if (Physics.Raycast(r, out RaycastHit hit, cam.farClipPlane, depthOfFieldAutofocusLayerMask.value)) {
                // we don't use hit.distance as ray origin has a small shift from camera
                float distance = Vector3.Distance(cam.transform.position, hit.point);
                distance += depthOfFieldAutofocusDistanceShift.value;
                dofLastAutofocusDistance = Mathf.Clamp(distance, depthOfFieldAutofocusMinDistance.value, depthOfFieldAutofocusMaxDistance.value);
            } else {
                dofLastAutofocusDistance = cam.farClipPlane;
            }
        }

        void UpdateDepthOfFieldBlurData(CommandBuffer cmd, Vector2 blurDir) {
            float downsamplingRatio = 1f / depthOfFieldDownsampling.value;
            blurDir *= downsamplingRatio;
            dofLastBokehData.z = blurDir.x;
            dofLastBokehData.w = blurDir.y;
            cmd.SetGlobalVector(ShaderParams.dofBokehData, dofLastBokehData);
        }

        void DoLUT() {
            if (lutIntensity.value > 0 && lut.value != null) {
                m_Material.SetTexture(ShaderParams.lutTexture, lut.value);
                m_Material.EnableKeyword(ShaderParams.SKW_LUT);
            } else {
                m_Material.DisableKeyword(ShaderParams.SKW_LUT);
            }
        }

        void DoVignette(Camera cam) {
            float outerRing = 1f - vignettingOuterRing.value;
            float innerRing = 1f - vignettingInnerRing.value;
            float currentBlink = blink > 0 ? Mathf.Clamp01(blink) : vignettingBlink.value;
            bool vignettingEnabled = outerRing < 1 || innerRing < 1f || vignettingFade.value > 0 || currentBlink > 0;
            bool usesVignetteMask = false;

            if (vignettingEnabled) {
                if (innerRing >= outerRing) {
                    innerRing = outerRing - 0.0001f;
                }

                Color vignettingColorAdjusted = vignettingColor.value;
                float vb = 1f - currentBlink * 2f;
                if (vb < 0) vb = 0;
                vignettingColorAdjusted.r *= vb;
                vignettingColorAdjusted.g *= vb;
                vignettingColorAdjusted.b *= vb;
                m_Material.SetColor(ShaderParams.vignette, vignettingColorAdjusted);
                Vector4 vignetteData = new Vector4(vignettingCenter.value.x, vignettingCenter.value.y, (vignettingCircularShape.value && currentBlink <= 0) ? 1.0f / cam.aspect : vignettingAspectRatio.value + 1.001f / (1.001f - currentBlink) - 1f);

                if (vignettingBlinkStyle.value == BlinkStyle.Human) {
                    vignetteData.y -= currentBlink * 0.5f;
                }

                m_Material.SetVector(ShaderParams.vignetteData, vignetteData);
                float vd = vignettingFade.value + currentBlink * 0.5f;
                if (currentBlink > 0.99f) vd = 1f;
                Vector4 vignetteData2 = new Vector4(vd, innerRing, outerRing, 0);
                m_Material.SetVector(ShaderParams.vignetteData2, vignetteData2);

                if (vignettingMask.overrideState && vignettingMask.value != null) {
                    m_Material.SetTexture(ShaderParams.vignetteMask, vignettingMask.value);
                    usesVignetteMask = true;
                }
            }

            if (usesVignetteMask) {
                m_Material.EnableKeyword(ShaderParams.SKW_VIGNETTING_MASK);
            } else {
                m_Material.DisableKeyword(ShaderParams.SKW_VIGNETTING_MASK);
            }
        }

        void DoFrost() {
            if (frostIntensity.value <= 0) m_Material.DisableKeyword(ShaderParams.SKW_FROST);
            m_Material.EnableKeyword(ShaderParams.SKW_FROST);
            m_Material.SetVector(ShaderParams.frostIntensity, new Vector3(frostIntensity.value * 5f, 5.01f - frostSpread.value, frostDistortion.value * 0.01f));
            m_Material.SetColor(ShaderParams.frostTintColor, frostTintColor.value);
            m_Material.SetTexture(ShaderParams.frostTexture, frostTexture.value != null ? frostTexture.value : defaultFrost);
            m_Material.SetTexture(ShaderParams.frostDistortionTexture, frostDistortTexture.value != null ? frostDistortTexture.value : defaultFrostDisp);
        }

        bool CheckSceneSettings() {
            sceneSettings = BeautifySettings.instance;
            return sceneSettings != null;
        }

        public override void Cleanup() {
            CoreUtils.Destroy(m_Material);
            foreach (RTHandle rt in rtHandles) {
                rt.Release();
            }
#if UNITY_EDITOR
            if (rtCapture != null) {
                rtCapture.Release();
            }
#endif
        }

        #region API
        /// <summary>
        /// Animates blink parameter
        /// </summary>
        /// <returns>The blink.</returns>
        /// <param name="duration">Duration.</param>
        public static IEnumerator Blink(float duration, float maxValue = 1f) {
            float start = Time.time;
            WaitForEndOfFrame w = new WaitForEndOfFrame();
            float t;

            // Close
            do {
                t = (Time.time - start) / duration;
                if (t > 1f)
                    t = 1f;
                float easeOut = t * (2f - t);
                blink = easeOut * maxValue;
                yield return w;
            } while (t < 1f);

            // Open
            start = Time.time;
            do {
                t = (Time.time - start) / duration;
                if (t > 1f)
                    t = 1f;
                float easeIn = t * t;
                blink = (1f - easeIn) * maxValue;
                yield return w;
            } while (t < 1f);
        }
        #endregion
    }
}