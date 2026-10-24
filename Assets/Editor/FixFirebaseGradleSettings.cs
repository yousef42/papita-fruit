using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Android;
using UnityEngine;

/// <summary>
/// EDM injects file:////.../GeneratedLocalRepo which breaks Gradle 9 / AGP 9
/// when the project path contains spaces. Keep only LocalMaven File().toURI().
/// </summary>
public class FixFirebaseGradleSettings : IPostGenerateGradleAndroidProject
{
    public int callbackOrder => 50;

    public void OnPostGenerateGradleAndroidProject(string unityLibraryPath)
    {
        string gradleRoot = Path.GetDirectoryName(unityLibraryPath);
        string settingsPath = Path.Combine(gradleRoot, "settings.gradle");
        if (File.Exists(settingsPath))
            PatchSettingsGradle(settingsPath);

        string cppAar = Path.Combine(unityLibraryPath, "libs", "firebase-messaging-cpp.aar");
        if (File.Exists(cppAar))
        {
            File.Delete(cppAar);
            Debug.Log("FixFirebaseGradleSettings: removed firebase-messaging-cpp.aar");
        }

        string buildGradle = Path.Combine(unityLibraryPath, "build.gradle");
        if (File.Exists(buildGradle))
        {
            string bg = File.ReadAllText(buildGradle);
            string bg2 = bg.Replace("    implementation(name: 'firebase-messaging-cpp', ext:'aar')\n", "");
            if (bg2 != bg)
            {
                File.WriteAllText(buildGradle, bg2);
                Debug.Log("FixFirebaseGradleSettings: removed firebase-messaging-cpp dependency");
            }
        }
    }

    static void PatchSettingsGradle(string settingsPath)
    {
        string text = File.ReadAllText(settingsPath);
        string original = text;

        text = Regex.Replace(
            text,
            @"// Android Resolver Repos Start[\s\S]*?// Android Resolver Repos End\r?\n?",
            string.Empty,
            RegexOptions.Multiline);

        if (text.Contains("GeneratedLocalRepo") || text.Contains("file:////"))
            text = StripBrokenRepos(text);

        string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string localMaven = Path.Combine(projectRoot, "LocalMaven", "Firebase", "m2repository")
            .Replace('\\', '/');

        if (!text.Contains("LocalMaven/Firebase/m2repository"))
        {
            const string marker = "dependencyResolutionManagement";
            int idx = text.IndexOf(marker);
            if (idx >= 0)
            {
                int insertAt = text.IndexOf("mavenCentral()", idx);
                if (insertAt >= 0)
                {
                    insertAt += "mavenCentral()".Length;
                    string inject =
                        "\n        maven {\n            url new File('" + localMaven + "').toURI()\n        }";
                    text = text.Insert(insertAt, inject);
                }
            }
        }
        else if (!text.Contains(".toURI()"))
        {
            text = Regex.Replace(
                text,
                @"url new File\('([^']*LocalMaven/Firebase/m2repository)'\)",
                "url new File('$1').toURI()");
        }

        if (text != original)
        {
            File.WriteAllText(settingsPath, text);
            Debug.Log("FixFirebaseGradleSettings: cleaned settings.gradle (removed GeneratedLocalRepo file:// URI)");
        }
        else if (text.Contains("GeneratedLocalRepo") || text.Contains("file:////"))
        {
            // Last resort: rewrite a known-good settings.gradle
            WriteCleanSettings(settingsPath, projectRoot, localMaven, text);
            Debug.Log("FixFirebaseGradleSettings: rewrote settings.gradle (fallback clean)");
        }
    }

    static string StripBrokenRepos(string text)
    {
        string[] lines = text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        var sb = new StringBuilder();
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string trimmed = line.Trim();

            if (trimmed.StartsWith("def unityProjectPath") ||
                trimmed.Contains("file:////") ||
                trimmed.Contains("GeneratedLocalRepo"))
                continue;

            // Drop "maven {" ... "}" block if any line inside references GeneratedLocalRepo / unityProjectPath
            if (trimmed == "maven {" || trimmed.StartsWith("maven {"))
            {
                int end = i;
                var block = new StringBuilder();
                int depth = 0;
                for (int j = i; j < lines.Length; j++)
                {
                    block.Append(lines[j]).Append('\n');
                    depth += CountChar(lines[j], '{') - CountChar(lines[j], '}');
                    end = j;
                    if (depth <= 0 && j > i)
                        break;
                }
                string blockText = block.ToString();
                if (blockText.Contains("GeneratedLocalRepo") ||
                    blockText.Contains("unityProjectPath") ||
                    blockText.Contains("file:////"))
                {
                    i = end;
                    continue;
                }
            }

            sb.Append(line).Append('\n');
        }
        return sb.ToString();
    }

    static void WriteCleanSettings(string settingsPath, string projectRoot, string localMaven, string previous)
    {
        // Preserve include lines from previous file.
        var includes = new StringBuilder();
        foreach (string line in previous.Split('\n'))
        {
            string t = line.Trim();
            if (t.StartsWith("include "))
                includes.AppendLine(t);
        }
        if (includes.Length == 0)
        {
            includes.AppendLine("include ':launcher'");
            includes.AppendLine("include ':unityLibrary'");
        }

        string content =
            "pluginManagement {\n" +
            "    repositories {\n" +
            "        maven { url \"https://maven.aliyun.com/repository/google\" }\n" +
            "        gradlePluginPortal()\n" +
            "        google()\n" +
            "        mavenCentral()\n" +
            "    }\n" +
            "}\n\n" +
            includes +
            "\n" +
            "dependencyResolutionManagement {\n" +
            "    repositoriesMode.set(RepositoriesMode.PREFER_SETTINGS)\n" +
            "    repositories {\n" +
            "        maven { url \"https://maven.aliyun.com/repository/google\" }\n" +
            "        google()\n" +
            "        mavenCentral()\n" +
            "        maven {\n" +
            "            url new File('" + localMaven + "').toURI()\n" +
            "        }\n" +
            "        maven {\n" +
            "            url \"https://maven.google.com\"\n" +
            "        }\n" +
            "        flatDir {\n" +
            "            dirs \"${project(':unityLibrary').projectDir}/libs\"\n" +
            "        }\n" +
            "    }\n" +
            "}\n";
        File.WriteAllText(settingsPath, content);
    }

    static int CountChar(string s, char c)
    {
        int n = 0;
        for (int i = 0; i < s.Length; i++)
            if (s[i] == c) n++;
        return n;
    }
}
