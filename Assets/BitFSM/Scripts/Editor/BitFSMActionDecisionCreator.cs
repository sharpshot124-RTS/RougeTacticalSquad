using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BitFSM
{
    public class BitFSMActionDecisionCreator : EditorWindow
    {
        private Vector2 scrollPosition;
        private bool showDecisions = false;
        private bool showActions = false;
        private string path = "Assets/";
        private static MonoScript[] scripts;

        [MenuItem("BitFSM/Action Decision Creator")]
        static void Init()
        {
            BitFSMActionDecisionCreator window = (BitFSMActionDecisionCreator)EditorWindow.GetWindow(typeof(BitFSMActionDecisionCreator));
            window.minSize = new Vector2(200, 400);
            window.maxSize = new Vector2(200, 2000);
            window.titleContent = new GUIContent("BitFSM Action/Decision Creator");
            window.Show();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (!EditorApplication.isPlaying)
            {
                scripts = (MonoScript[])Object.FindObjectsOfTypeIncludingAssets(typeof(MonoScript));
            }
        }

        private void OnGUI()
        {
            if (scrollPosition == null)
            {
                scrollPosition = Vector2.zero;
            }

            EditorGUILayout.Space();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);          

            if (scripts == null)
            {
                if (EditorApplication.isPlaying)
                {
                    return;
                } else
                {
                    scripts = (MonoScript[])Object.FindObjectsOfTypeIncludingAssets(typeof(MonoScript));
                }
            }
            showActions = EditorGUILayout.Foldout(showActions, "ACTIONS");
            if (showActions)
            {
                foreach (MonoScript m in scripts)
                {
                    System.Type classDef = m.GetClass();
                    if (classDef != null && classDef.IsSubclassOf(typeof(Action)))
                    {
                        if (GUILayout.Button(classDef.Name))
                        {
                            path = EditorUtility.SaveFilePanel("Save Action", path, classDef.Name, "asset");
                            if (path.Substring(0, Application.dataPath.Length) == Application.dataPath)
                            {
                                path = FileUtil.GetProjectRelativePath(path);

                                if (string.IsNullOrEmpty(path)) return;

                                var asset = ScriptableObject.CreateInstance(classDef.Name);

                                AssetDatabase.CreateAsset(asset, path);
                                AssetDatabase.SaveAssets();
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("Error", "Could not create action asset at the selected path. Make sure the path is inside the Asset folder of the project.", "ok");
                            }
                        }
                    }
                }
            }

            showDecisions = EditorGUILayout.Foldout(showDecisions, "DECISIONS");
            if (showDecisions)
            {
                foreach (MonoScript m in scripts)
                {
                    System.Type classDef = m.GetClass();
                    if (classDef != null && classDef.IsSubclassOf(typeof(Decision)))
                    {
                        if (GUILayout.Button(classDef.Name))
                        {
                            path = EditorUtility.SaveFilePanel("Save Decision", path, classDef.Name, "asset");
                            if (path.Substring(0, Application.dataPath.Length) == Application.dataPath)
                            {
                                path = FileUtil.GetProjectRelativePath(path);

                                if (string.IsNullOrEmpty(path)) return;

                                var asset = ScriptableObject.CreateInstance(classDef.Name);

                                AssetDatabase.CreateAsset(asset, path);
                                AssetDatabase.SaveAssets();
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("Error", "Could not create decision asset at the selected path. Make sure the path is inside the Asset folder of the project.", "ok");
                            }
                        }
                    }
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.EndScrollView();
        }
    }
}
