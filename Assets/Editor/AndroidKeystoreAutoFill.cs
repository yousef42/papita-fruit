using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

[InitializeOnLoad]
public static class AndroidKeystoreAutoFill
{
    const string LocalPassFile = "keystore.local";
    const string KeystoreFile = "fruits.keystore";
    const string KeyAlias = "papita fruits";

    static AndroidKeystoreAutoFill()
    {
        EditorApplication.delayCall += Apply;
    }

    [MenuItem("Build/Apply Android Keystore Passwords")]
    public static void Apply()
    {
        string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string passPath = Path.Combine(projectRoot, LocalPassFile);
        if (!File.Exists(passPath))
        {
            Debug.LogWarning(
                $"Create '{LocalPassFile}' in project root with keystore password (one line) so Android Build And Run can sign.");
            return;
        }

        string pass = File.ReadAllText(passPath).Trim();
        if (string.IsNullOrEmpty(pass))
            return;

        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = Path.Combine(projectRoot, KeystoreFile);
        PlayerSettings.Android.keyaliasName = KeyAlias;
        PlayerSettings.Android.keystorePass = pass;
        PlayerSettings.Android.keyaliasPass = pass;
        Debug.Log("Android keystore passwords applied from keystore.local");
    }

    class Preprocess : IPreprocessBuildWithReport
    {
        public int callbackOrder => -1000;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.Android)
                return;
            Apply();
        }
    }
}
