using System.IO;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

/// <summary>
/// Unity 6: strip MessagingUnityPlayerActivity.java so Gradle compiles
/// (we launch UnityPlayerActivity from AndroidManifest).
/// </summary>
public class StripFirebaseMessagingActivity : IPostGenerateGradleAndroidProject
{
    public int callbackOrder => 1000;

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        string java = Path.Combine(path, "src/main/java/com/google/firebase/MessagingUnityPlayerActivity.java");
        if (File.Exists(java))
        {
            File.Delete(java);
            Debug.Log("StripFirebaseMessagingActivity: removed " + java);
        }
    }
}
