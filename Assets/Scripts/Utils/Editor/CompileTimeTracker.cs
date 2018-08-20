using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace G4AW2.Utils.Editor {

    public class CompileTimeTracker : EditorWindow, IPreprocessBuildWithReport, IPostprocessBuildWithReport {

        [MenuItem("Test/Compile Window")]
        static void Init() {
            //GetWindow<CompileTimeTracker>();
            //AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            //AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
            EditorWindow window = GetWindowWithRect(typeof(CompileTimeTracker), new Rect(0, 0, 200, 200));
            window.Show();
        }

        public DateTime LastCompileTime = DateTime.MinValue;
        public bool Compiling = false;

        void OnGUI() {

            if (Compiling && !EditorApplication.isCompiling) {
                // Done Compiling
                LastCompileTime = DateTime.Now;
                Compiling = false;
            } else if (!Compiling && EditorApplication.isCompiling) {
                Compiling = true;
            }

            EditorGUILayout.LabelField("Compiling: ", Compiling ? "Yes" : "No");
            EditorGUILayout.LabelField("Last Compile Time: ", LastCompileTime.ToString("T"));
            Repaint();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded() {
            //Debug.Log("Scripts reloaded!");
        }

        #region Assembly Reload
        private static void OnAfterAssemblyReload() {
            Debug.LogWarning("Assembly reload done!");
        }

        private static void OnBeforeAssemblyReload() {
            Debug.LogWarning("Assembly reload start!");
        }
        #endregion

        #region Build
        public int callbackOrder { get { return 0; } }
        public void OnPostprocessBuild( BuildReport report ) {
            Debug.LogWarning(string.Format("Build {0}! Time: {1}, Errors: {2}, Warnings: {3}",
                report.summary.totalErrors > 0 ? "Failed" : "Succeeded",
                report.summary.totalTime.Seconds,
                report.summary.totalErrors,
                report.summary.totalWarnings));
        }

        public void OnPreprocessBuild( BuildReport report ) {
            Debug.LogWarning("Building! Please wait...");
        }
        #endregion

    }
}

