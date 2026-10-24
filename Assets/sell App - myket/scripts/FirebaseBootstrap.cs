using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using UnityEngine;

/// <summary>
/// Single gate for Firebase init. SDK 13.16.0+ required on Android 14+.
/// </summary>
public static class FirebaseBootstrap
{
    public const bool Enabled = true;

    static Task<DependencyStatus> _depsTask;
    static readonly object _lock = new object();
    public static bool IsReady { get; private set; }

    public static Task<DependencyStatus> EnsureReadyAsync()
    {
        if (!Enabled)
            return Task.FromResult(DependencyStatus.UnavailableOther);

        lock (_lock)
        {
            if (_depsTask == null)
                _depsTask = FirebaseApp.CheckAndFixDependenciesAsync();
            return _depsTask;
        }
    }

    public static void WhenReady(System.Action onReady)
    {
        if (!Enabled)
        {
            Debug.LogWarning("Firebase is disabled. Skipping Firebase init.");
            return;
        }

        EnsureReadyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Firebase dependency check failed: " + task.Exception);
                return;
            }

            if (task.Result != DependencyStatus.Available)
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
                return;
            }

            IsReady = true;
            Debug.Log("Firebase ready (SDK via UPM 13.16.0).");
            onReady?.Invoke();
        });
    }

    public static void SafeLogEvent(string name, string parameterName, long value)
    {
        if (!Enabled || !IsReady)
            return;
        try
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameterName, value);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Firebase LogEvent skipped: " + e.Message);
        }
    }
}
