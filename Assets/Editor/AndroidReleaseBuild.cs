using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class AndroidReleaseBuild
{
    [MenuItem("Build/Android Release APK")]
    public static void BuildReleaseApk()
    {
        AndroidUpgradeBootstrap.ApplyAndroidSettings();

        string keystorePass = Environment.GetEnvironmentVariable("PAPITA_KEYSTORE_PASS");
        string keyaliasPass = Environment.GetEnvironmentVariable("PAPITA_KEYALIAS_PASS");
        string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string localPassFile = Path.Combine(projectRoot, "keystore.local");
        if (File.Exists(localPassFile))
        {
            string localPass = File.ReadAllText(localPassFile).Trim();
            if (string.IsNullOrEmpty(keystorePass)) keystorePass = localPass;
            if (string.IsNullOrEmpty(keyaliasPass)) keyaliasPass = localPass;
        }
        if (string.IsNullOrEmpty(keystorePass))
            keystorePass = keyaliasPass;
        if (string.IsNullOrEmpty(keyaliasPass))
            keyaliasPass = keystorePass;
        if (string.IsNullOrEmpty(keystorePass))
            throw new Exception("Set PAPITA_KEYSTORE_PASS or create keystore.local before building.");

        string outputDir = Path.Combine(projectRoot, "Builds", "Android");
        Directory.CreateDirectory(outputDir);

        if (PlayerSettings.Android.bundleVersionCode < 34)
            PlayerSettings.Android.bundleVersionCode = 34;
        if (string.IsNullOrEmpty(PlayerSettings.bundleVersion) || PlayerSettings.bundleVersion == "3.2")
            PlayerSettings.bundleVersion = "3.3";

        string versionName = PlayerSettings.bundleVersion;
        int versionCode = PlayerSettings.Android.bundleVersionCode;
        string apkPath = Path.Combine(outputDir, $"Papita-fruits-bazar-v{versionName}-{versionCode}-release.apk");

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        EditorUserBuildSettings.buildAppBundle = false;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = Path.Combine(projectRoot, "fruits.keystore");
        PlayerSettings.Android.keyaliasName = "papita fruits";
        PlayerSettings.Android.keystorePass = keystorePass;
        PlayerSettings.Android.keyaliasPass = keyaliasPass;

        string[] scenes = GetEnabledScenes();
        if (scenes.Length == 0)
            throw new Exception("No enabled scenes in Build Settings.");

        Debug.Log($"Building release APK -> {apkPath}");
        BuildReport report = BuildPipeline.BuildPlayer(scenes, apkPath, BuildTarget.Android, BuildOptions.None);
        BuildSummary summary = report.summary;

        if (summary.result != BuildResult.Succeeded)
            throw new Exception($"Android release build failed: {summary.result}");

        Debug.Log($"Android release build succeeded: {apkPath} ({summary.totalSize} bytes)");
    }

    static string[] GetEnabledScenes()
    {
        var list = new System.Collections.Generic.List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled && !string.IsNullOrEmpty(scene.path))
                list.Add(scene.path);
        }
        return list.ToArray();
    }
}
