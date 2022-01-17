using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace BeautifyHDRP {
    [VolumeComponentEditor(typeof(Beautify))]
    sealed class BeautifyEditor : VolumeComponentEditor {

        SerializedDataParameter intensity;
        SerializedDataParameter sharpen, sharpenMinDepth, sharpenMaxDepth, sharpenMinMaxDepthFallOff, sharpenDepthThreshold, sharpenRelaxation, sharpenClamp, sharpenMotionSensibility;
        SerializedDataParameter ditherStrength;
        SerializedDataParameter tonemap, tonemapExposure;
        SerializedDataParameter saturate, brightness, contrast, hardlightIntensity, hardlightBlend, daltonize, lut, lutIntensity;
        SerializedDataParameter vignettingOuterRing, vignettingInnerRing, vignettingFade, vignettingCircularShape, vignettingAspectRatio, vignettingBlink, vignettingBlinkStyle, vignettingCenter, vignettingColor, vignettingMask;
        SerializedDataParameter pixelateSize;
        SerializedDataParameter frostIntensity, frostSpread, frostDistortion, frostTintColor, frostTexture, frostDistortTexture;
        SerializedDataParameter depthOfField, depthOfFieldFocusMode, depthOfFieldAutofocusMinDistance, depthOfFieldAutofocusMaxDistance, depthOfFieldAutofocusViewportPoint, depthOfFieldAutofocusDistanceShift, depthOfFieldAutofocusLayerMask, depthOfFieldDistance;
        SerializedDataParameter depthOfFieldCameraSettings, depthOfFieldFocalLength, depthOfFieldAperture, depthOfFieldFocalLengthReal, depthOfFieldFStop, depthOfFieldImageSensorHeight, depthOfFieldFocusSpeed;
        SerializedDataParameter depthOfFieldForegroundBlur, depthOfFieldForegroundBlurHQ, depthOfFieldForegroundBlurHQSpread, depthOfFieldForegroundDistance, depthOfFieldBokeh, depthOfFieldBokehComposition, depthOfFieldBokehThreshold, depthOfFieldBokehIntensity, depthOfFieldDownsampling, depthOfFieldMaxSamples, depthOfFieldMaxBrightness, depthOfFieldMaxDistance;
        Texture2D headerTex;
        GUIStyle blackBack;

        public override void OnEnable() {
            base.OnEnable();

            headerTex = Resources.Load<Texture2D>("beautifyHeader");
            blackBack = new GUIStyle();
            blackBack.normal.background = MakeTex(4, 4, Color.black);
            blackBack.alignment = TextAnchor.MiddleCenter;

            var o = new PropertyFetcher<Beautify>(serializedObject);

            intensity = Unpack(o.Find(x => x.intensity));

            sharpen = Unpack(o.Find(x => x.sharpen));
            sharpenMinDepth = Unpack(o.Find(x => x.sharpenMinDepth));
            sharpenMaxDepth = Unpack(o.Find(x => x.sharpenMaxDepth));
            sharpenMinMaxDepthFallOff = Unpack(o.Find(x => x.sharpenMinMaxDepthFallOff));
            sharpenDepthThreshold = Unpack(o.Find(x => x.sharpenDepthThreshold));
            sharpenRelaxation = Unpack(o.Find(x => x.sharpenRelaxation));
            sharpenClamp = Unpack(o.Find(x => x.sharpenClamp));
            sharpenMotionSensibility = Unpack(o.Find(x => x.sharpenMotionSensibility));

            ditherStrength = Unpack(o.Find(x => x.ditherStrength));

            tonemap = Unpack(o.Find(x => x.tonemap));
            tonemapExposure = Unpack(o.Find(x => x.tonemapExposure));

            saturate = Unpack(o.Find(x => x.saturate));
            brightness = Unpack(o.Find(x => x.brightness));
            contrast = Unpack(o.Find(x => x.contrast));
            hardlightIntensity = Unpack(o.Find(x => x.hardlightIntensity));
            hardlightBlend = Unpack(o.Find(x => x.hardlightBlend));
            daltonize = Unpack(o.Find(x => x.daltonize));
            lut = Unpack(o.Find(x => x.lut));
            lutIntensity = Unpack(o.Find(x => x.lutIntensity));

            depthOfField = Unpack(o.Find(x => x.depthOfField));
            depthOfFieldFocusMode = Unpack(o.Find(x => x.depthOfFieldFocusMode));
            depthOfFieldAutofocusMinDistance = Unpack(o.Find(x => x.depthOfFieldAutofocusMinDistance));
            depthOfFieldAutofocusMaxDistance = Unpack(o.Find(x => x.depthOfFieldAutofocusMaxDistance));
            depthOfFieldAutofocusViewportPoint = Unpack(o.Find(x => x.depthOfFieldAutofocusViewportPoint));
            depthOfFieldAutofocusDistanceShift = Unpack(o.Find(x => x.depthOfFieldAutofocusDistanceShift));
            depthOfFieldAutofocusLayerMask = Unpack(o.Find(x => x.depthOfFieldAutofocusLayerMask));
            depthOfFieldDistance = Unpack(o.Find(x => x.depthOfFieldDistance));
            depthOfFieldCameraSettings = Unpack(o.Find(x => x.depthOfFieldCameraSettings));
            depthOfFieldFocalLengthReal = Unpack(o.Find(x => x.depthOfFieldFocalLengthReal));
            depthOfFieldFStop = Unpack(o.Find(x => x.depthOfFieldFStop));
            depthOfFieldImageSensorHeight = Unpack(o.Find(x => x.depthOfFieldImageSensorHeight));
            depthOfFieldFocalLength = Unpack(o.Find(x => x.depthOfFieldFocalLength));
            depthOfFieldAperture = Unpack(o.Find(x => x.depthOfFieldAperture));
            depthOfFieldFocusSpeed = Unpack(o.Find(x => x.depthOfFieldFocusSpeed));

            depthOfFieldForegroundBlur = Unpack(o.Find(x => x.depthOfFieldForegroundBlur));
            depthOfFieldForegroundBlurHQ = Unpack(o.Find(x => x.depthOfFieldForegroundBlurHQ));
            depthOfFieldForegroundBlurHQSpread = Unpack(o.Find(x => x.depthOfFieldForegroundBlurHQSpread));
            depthOfFieldForegroundDistance = Unpack(o.Find(x => x.depthOfFieldForegroundDistance));
            depthOfFieldBokeh = Unpack(o.Find(x => x.depthOfFieldBokeh));
            depthOfFieldBokehComposition = Unpack(o.Find(x => x.depthOfFieldBokehComposition));
            depthOfFieldBokehThreshold = Unpack(o.Find(x => x.depthOfFieldBokehThreshold));
            depthOfFieldBokehIntensity = Unpack(o.Find(x => x.depthOfFieldBokehIntensity));
            depthOfFieldDownsampling = Unpack(o.Find(x => x.depthOfFieldDownsampling));
            depthOfFieldMaxSamples = Unpack(o.Find(x => x.depthOfFieldMaxSamples));
            depthOfFieldMaxBrightness = Unpack(o.Find(x => x.depthOfFieldMaxBrightness));
            depthOfFieldMaxDistance = Unpack(o.Find(x => x.depthOfFieldMaxDistance));

            vignettingOuterRing = Unpack(o.Find(x => x.vignettingOuterRing));
            vignettingInnerRing = Unpack(o.Find(x => x.vignettingInnerRing));
            vignettingFade = Unpack(o.Find(x => x.vignettingFade));
            vignettingCircularShape = Unpack(o.Find(x => x.vignettingCircularShape));
            vignettingAspectRatio = Unpack(o.Find(x => x.vignettingAspectRatio));
            vignettingBlink = Unpack(o.Find(x => x.vignettingBlink));
            vignettingBlinkStyle = Unpack(o.Find(x => x.vignettingBlinkStyle));
            vignettingCenter = Unpack(o.Find(x => x.vignettingCenter));
            vignettingColor = Unpack(o.Find(x => x.vignettingColor));
            vignettingMask = Unpack(o.Find(x => x.vignettingMask));

            pixelateSize = Unpack(o.Find(x => x.pixelateSize));

            frostIntensity = Unpack(o.Find(x => x.frostIntensity));
            frostSpread = Unpack(o.Find(x => x.frostSpread));
            frostDistortion = Unpack(o.Find(x => x.frostDistortion));
            frostTintColor = Unpack(o.Find(x => x.frostTintColor));
            frostTexture = Unpack(o.Find(x => x.frostTexture));
            frostDistortTexture = Unpack(o.Find(x => x.frostDistortTexture));
        }

        public override void OnInspectorGUI() {

            GUILayout.BeginHorizontal(blackBack);
            GUILayout.Label(headerTex, blackBack, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();

            PropertyField(intensity);

            PropertyField(sharpen, new GUIContent("Sharpen"));
            if (sharpen.value.floatValue > 0) {
                PropertyField(sharpenMinDepth, new GUIContent("Min Depth", "Applies sharpen beyond certain depth in the scene"));
                PropertyField(sharpenMaxDepth, new GUIContent("Max Depth", "Applies sharpen until certain depth in the scene"));
                PropertyField(sharpenMinMaxDepthFallOff, new GUIContent("Min/Max FallOff", "Sharpen falloff around min/max limits"));
                PropertyField(sharpenDepthThreshold, new GUIContent("Depth Threshold", "Limits sharpen depending on depth difference around pixels"));
                PropertyField(sharpenRelaxation, new GUIContent("Relaxation", "Limits sharpen on bright areas"));
                PropertyField(sharpenClamp, new GUIContent("Sharpen Clamp", "Limits (clamps) final sharpen modifier so small sharpen occurs but high sharpen will be limited to this intensity."));
                PropertyField(sharpenMotionSensibility, new GUIContent("Motion Sensibility", "Reduces sharpen while camera moves/rotates to reduce flickering and contribute to motion blur effect"));
            }

            PropertyField(ditherStrength, new GUIContent("Dither Strength", "Fast dithering that reduces banding artifacts in scene"));

            PropertyField(tonemap, new GUIContent("Tonemap", "Apply linear or custom ACES tonemap operator"));
            if (tonemap.value.boolValue) {
                EditorGUI.indentLevel++;
                PropertyField(tonemapExposure, new GUIContent("Exposure", "Exposure applied before tonemapping"));
                EditorGUI.indentLevel--;
            }
            PropertyField(saturate, new GUIContent("Saturate", "Positive value increases color saturation making the scene more vivid. Negative value desaturares the colors towards grayscale"));
            PropertyField(brightness);
            PropertyField(contrast);
            PropertyField(hardlightIntensity, new GUIContent("Hard Light Intensity", "Intensity of the hard light effect."));
            PropertyField(hardlightBlend, new GUIContent("Hard Light Blending", "Blend amount of the hard light effect."));
            PropertyField(daltonize, new GUIContent("Daltonize", "Boosts primary colors to compensate daltonism"));

            EditorGUILayout.BeginHorizontal();
            PropertyField(lut, new GUIContent("LUT", "Lookup texture"));
            if (lut.overrideState.boolValue) {
                if (GUILayout.Button("Help", GUILayout.Width(50))) {
                    EditorUtility.DisplayDialog("LUT Requirements", "Sample LUT textureS can be found in Beautify/Demo/DemoSources/Textures folder.\n\nEnsure the following import settings are set in your LUT textures:\n- Uncheck sRGB Texture (no gamma conversion)\n- No compression\n- Disable mip mapping\n- Aniso set to 0\n- Filtering set to Bilinear\n- Wrapping set to Clamp", "Ok");
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel++;
                CheckLUTSettings((Texture2D)lut.value.objectReferenceValue);
                PropertyField(lutIntensity, new GUIContent("Intensity", "LUT blending intensity"));
                EditorGUI.indentLevel--;
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("", GUILayout.Width(EditorGUIUtility.labelWidth + 16));
                if (GUILayout.Button("Open LUT Browser")) {
                    LUTBrowser.ShowBrowser();
                }
                EditorGUILayout.EndHorizontal();
            } else {
                EditorGUILayout.EndHorizontal();
            }

            PropertyField(depthOfField, new GUIContent("Depth of Field", "Enabled depth of field effect"));
            if (depthOfField.value.boolValue) {
                PropertyField(depthOfFieldFocusMode, new GUIContent("Focus Mode"));
                EditorGUI.indentLevel++;
                switch ((Beautify.DoFFocusMode)depthOfFieldFocusMode.value.intValue) {
                    case Beautify.DoFFocusMode.AutoFocus:
                        PropertyField(depthOfFieldAutofocusMinDistance, new GUIContent("Min Distance"));
                        PropertyField(depthOfFieldAutofocusMaxDistance, new GUIContent("Max Distance"));
                        PropertyField(depthOfFieldAutofocusDistanceShift, new GUIContent("Distance Shift"));
                        PropertyField(depthOfFieldAutofocusViewportPoint, new GUIContent("Viewport Center"));
                        PropertyField(depthOfFieldAutofocusLayerMask, new GUIContent("Layer Mask"));
                        break;
                    case Beautify.DoFFocusMode.FixedDistance:
                        PropertyField(depthOfFieldDistance, new GUIContent("Distance"));
                        break;
                    case Beautify.DoFFocusMode.FollowTarget:
                        if (BeautifySettings.instance.depthOfFieldTarget == null) {
                            EditorGUILayout.HelpBox("Assign target in the Beautify Settings component.", MessageType.Info);
                        }
                        break;
                }
                EditorGUI.indentLevel--;
                PropertyField(depthOfFieldCameraSettings, new GUIContent("Camera Lens Settings"));
                if (depthOfFieldCameraSettings.value.intValue == (int)Beautify.DoFCameraSettings.Classic) {
                    PropertyField(depthOfFieldFocalLength, new GUIContent("Focal Length"));
                    PropertyField(depthOfFieldAperture, new GUIContent("Aperture"));
                } else {
                    PropertyField(depthOfFieldFocalLengthReal, new GUIContent("Focal Length"));
                    PropertyField(depthOfFieldFStop, new GUIContent("F-Stop"));
                    PropertyField(depthOfFieldImageSensorHeight, new GUIContent("Image Sensor Height"));
                }
                PropertyField(depthOfFieldFocusSpeed, new GUIContent("Focus Speed"));

                PropertyField(depthOfFieldForegroundBlur, new GUIContent("Foreground Blur"));
                if (depthOfFieldForegroundBlur.value.boolValue) {
                    EditorGUI.indentLevel++;
                    PropertyField(depthOfFieldForegroundBlurHQ, new GUIContent("HQ Blur"));
                    if (depthOfFieldForegroundBlurHQ.value.boolValue) {
                        PropertyField(depthOfFieldForegroundBlurHQSpread, new GUIContent("Blur Spread"));
                    }
                    PropertyField(depthOfFieldForegroundDistance, new GUIContent("Foreground Distance"));
                    EditorGUI.indentLevel--;
                }
                PropertyField(depthOfFieldBokeh, new GUIContent("Bokeh"));
                if (depthOfFieldBokeh.value.boolValue) {
                    EditorGUI.indentLevel++;
                    PropertyField(depthOfFieldBokehComposition, new GUIContent("Composition", "Integrated means the bokeh is computed in the same pass than the DoF blur (faster)"));
                    PropertyField(depthOfFieldBokehThreshold, new GUIContent("Threshold"));
                    PropertyField(depthOfFieldBokehIntensity, new GUIContent("Intensity"));
                    EditorGUI.indentLevel--;
                }
                PropertyField(depthOfFieldDownsampling, new GUIContent("Downsampling"));
                PropertyField(depthOfFieldMaxSamples, new GUIContent("Max Samples"));
                PropertyField(depthOfFieldMaxBrightness, new GUIContent("Max Brightness"));
                PropertyField(depthOfFieldMaxDistance, new GUIContent("Max Distance"));
            }

            PropertyField(vignettingOuterRing, new GUIContent("Outer Ring"));
            PropertyField(vignettingInnerRing, new GUIContent("Inner Ring"));
            PropertyField(vignettingFade, new GUIContent("Fade"));
            PropertyField(vignettingCircularShape, new GUIContent("Circular Shape"));
            if (!vignettingCircularShape.value.boolValue) {
                PropertyField(vignettingAspectRatio, new GUIContent("Aspect Ratio"));
            }
            PropertyField(vignettingBlink, new GUIContent("Blink"));
            PropertyField(vignettingBlinkStyle, new GUIContent("Blink Style"));
            PropertyField(vignettingCenter, new GUIContent("Center"));
            PropertyField(vignettingColor, new GUIContent("Color"));
            PropertyField(vignettingMask, new GUIContent("Mask", "Optional texture mask for the vignetting effect"));

            PropertyField(pixelateSize, new GUIContent("Size"));

            PropertyField(frostIntensity, new GUIContent("Intensity"));
            PropertyField(frostSpread, new GUIContent("Spread"));
            PropertyField(frostDistortion, new GUIContent("Distortion"));
            PropertyField(frostDistortTexture, new GUIContent("Distortion Map"));
            PropertyField(frostTintColor, new GUIContent("Tint Color"));
            PropertyField(frostTexture, new GUIContent("Frost Texture"));
        }

        public static void CheckLUTSettings(Texture2D tex) {
            if (Application.isPlaying || tex == null)
                return;
            string path = AssetDatabase.GetAssetPath(tex);
            if (string.IsNullOrEmpty(path))
                return;
            TextureImporter imp = (TextureImporter)AssetImporter.GetAtPath(path);
            if (imp == null)
                return;
            if (imp.textureType != TextureImporterType.Default || imp.sRGBTexture || imp.mipmapEnabled || imp.textureCompression != TextureImporterCompression.Uncompressed || imp.wrapMode != TextureWrapMode.Clamp || imp.filterMode != FilterMode.Bilinear) {
                EditorGUILayout.HelpBox("Texture has invalid import settings.", MessageType.Warning);
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Fix Texture Import Settings", GUILayout.Width(200))) {
                    imp.textureType = TextureImporterType.Default;
                    imp.sRGBTexture = false;
                    imp.mipmapEnabled = false;
                    imp.textureCompression = TextureImporterCompression.Uncompressed;
                    imp.wrapMode = TextureWrapMode.Clamp;
                    imp.filterMode = FilterMode.Bilinear;
                    imp.anisoLevel = 0;
                    imp.SaveAndReimport();
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }

        Texture2D MakeTex(int width, int height, Color col) {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            TextureFormat tf = SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat) ? TextureFormat.RGBAFloat : TextureFormat.RGBA32;
            Texture2D result = new Texture2D(width, height, tf, false);
            result.hideFlags = HideFlags.DontSave;
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

    }
}