using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

[InitializeOnLoad]
public static class AndroidUpgradeBootstrap
{
    static AndroidUpgradeBootstrap()
    {
        EditorApplication.delayCall += ApplyAndroidSettings;
    }

    [MenuItem("Build/Apply Android Upgrade Settings")]
    public static void ApplyAndroidSettings()
    {
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel25;
        PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)36;
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        PlayerSettings.insecureHttpOption = InsecureHttpOption.AlwaysAllowed;
        PlayerSettings.Android.useCustomKeystore = true;

        string root = System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, ".."));
        PlayerSettings.Android.keystoreName = System.IO.Path.Combine(root, "fruits.keystore");
        PlayerSettings.Android.keyaliasName = "papita fruits";

        Debug.Log("AndroidUpgradeBootstrap: minSdk=25 targetSdk=36 IL2CPP ARM64+ARMv7");
    }
}
