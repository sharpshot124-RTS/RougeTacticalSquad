using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BitFSM
{
    [CreateAssetMenu(menuName = "BitFSM/Settings")]
    public class BitFSMSettings : SingletonScriptableObject<BitFSMSettings>
    {
        public BitFSM currentAI = null;

        //NODE STYLES
        public Color styleEntryColor = new Color(0.6f, 0.7f, 0.3f, 1f);
        public Color styleNormalColor = new Color(0.5f, 0.5f, 0.5f, 1);
        public Color styleSelectedColor = new Color(0.43f, 0.72f, 0.72f, 1);
        public Color styleActiveColor = new Color(1, 1, 1, 1);
        public Color styleConnectorColor = new Color(0.65f, 0.65f, 0.65f, 0.75f);
        public Color styleConnectorSuccessColor = new Color(0.6f, 0.7f, 0.1f, 0.75f);
        public Color styleConnectorFailureColor = new Color(0.7f, 0.3f, 0.3f, 0.75f);
        public Color styleEditorNormalColor = new Color(0.6f, 0.6f, 0.6f, 1);
        public Color styleLineColor = new Color(0.4f, 0.4f, 0.4f, 1f);

        [NonSerialized]
        public GUIStyle nodeStyleEntry = null;
        [NonSerialized]
        public GUIStyle nodeStyleNormal = null;
        [NonSerialized]
        public GUIStyle nodeStyleSelected = null;
        [NonSerialized]
        public GUIStyle nodeStyleActive = null;
        [NonSerialized]
        public GUIStyle nodeStyleConnector = null;
        [NonSerialized]
        public GUIStyle nodeStyleConnectorSuccess = null;
        [NonSerialized]
        public GUIStyle nodeStyleConnectorFailure = null;
        [NonSerialized]
        public GUIStyle editorStyleNormal = null;
        //[NonSerialized]
        //public GUIStyle centeredLabelStyle = null;
        [NonSerialized]
        public GUIStyle nodeStyleTitle = null;

        //TEXTURES
        [HideInInspector] public Texture moveDownIcon = null;
        [HideInInspector] public Texture moveUpIcon = null;
        [HideInInspector] public Texture deleteIcon = null;

#if UNITY_EDITOR
        public static List<string> actions;
        public static List<string> decisions;
#endif

        public void LoadTextures()
        {
            if (moveDownIcon == null || moveUpIcon == null || deleteIcon == null)
            {
                moveDownIcon = Resources.Load<Texture>("Icons/MoveDownIcon");
                moveUpIcon = Resources.Load<Texture>("Icons/MoveUpIcon");
                deleteIcon = Resources.Load<Texture>("Icons/DeleteIcon");
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }

        public void SetupNodeStyles()
        {
            LoadTextures();

            nodeStyleEntry = new GUIStyle(GUI.skin.box);
            nodeStyleNormal = new GUIStyle(GUI.skin.box);
            nodeStyleSelected = new GUIStyle(GUI.skin.box);
            nodeStyleActive = new GUIStyle(GUI.skin.box);
            nodeStyleConnector = new GUIStyle(GUI.skin.box);
            nodeStyleConnectorSuccess = new GUIStyle(GUI.skin.box);
            nodeStyleConnectorFailure = new GUIStyle(GUI.skin.box);
            editorStyleNormal = new GUIStyle(GUI.skin.box);
            nodeStyleTitle = new GUIStyle(GUI.skin.box);

            nodeStyleEntry.normal.background = MakeTex(2, 2, styleEntryColor);
            nodeStyleEntry.alignment = TextAnchor.MiddleCenter;
            nodeStyleNormal.normal.background = MakeTex(2, 2, styleNormalColor);
            nodeStyleSelected.normal.background = MakeTex(2, 2, styleSelectedColor);
            nodeStyleSelected.alignment = TextAnchor.MiddleCenter;
            nodeStyleActive.normal.background = MakeTex(2, 2, styleActiveColor);
            nodeStyleConnector.normal.background = MakeTex(2, 2, styleConnectorColor);
            nodeStyleConnectorSuccess.normal.background = MakeTex(2, 2, styleConnectorSuccessColor);
            nodeStyleConnectorFailure.normal.background = MakeTex(2, 2, styleConnectorFailureColor);
            editorStyleNormal.normal.background = MakeTex(2, 2, styleEditorNormalColor);

            Color titleColor = styleNormalColor * 0.85f;
            titleColor.a = 1f;
            nodeStyleTitle.normal.background = MakeTex(2, 2, titleColor);
            nodeStyleTitle.alignment = TextAnchor.MiddleCenter;

            //centeredLabelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            //centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
        }

        #region UTILITY
        public static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        public static Texture2D rotateTexture(Texture2D originalTexture, bool clockwise)
        {
            Color32[] original = originalTexture.GetPixels32();
            Color32[] rotated = new Color32[original.Length];
            int w = originalTexture.width;
            int h = originalTexture.height;

            int iRotated, iOriginal;

            for (int j = 0; j < h; ++j)
            {
                for (int i = 0; i < w; ++i)
                {
                    iRotated = (i + 1) * h - j - 1;
                    iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                    rotated[iRotated] = original[iOriginal];
                }
            }

            Texture2D rotatedTexture = new Texture2D(h, w);
            rotatedTexture.SetPixels32(rotated);
            rotatedTexture.Apply();
            return rotatedTexture;
        }

#if UNITY_EDITOR
        public static void DrawHorizontalLine(int lineHeight, Color color)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, lineHeight);
            rect.height = lineHeight;
            EditorGUI.DrawRect(rect, color);
        }
#endif

#endregion
    }
}
