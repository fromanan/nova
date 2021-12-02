using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NovaEditor.Extensions
{
    public static class EditorExtensions
    {
        private static readonly Assembly UnityEditorAssembly = typeof(AudioImporter).Assembly;
        private static readonly Type AudioUtilClass = UnityEditorAssembly.GetType("UnityEditor.AudioUtil");
        private const BindingFlags Flags = BindingFlags.Static | BindingFlags.Public;

        private static readonly MethodInfo PlayClipMethodInfo = GetMethod("PlayClip", typeof(AudioClip));
        private static readonly MethodInfo StopAllClipsMethodInfo = GetMethod("StopAllClips");
        
        // https://answers.unity.com/questions/36388/how-to-play-audioclip-in-edit-mode.html
        public static void PlayClip(AudioClip clip) => Invoke(PlayClipMethodInfo, clip);
        
        public static void StopAllClips() => Invoke(StopAllClipsMethodInfo);

        private static MethodInfo GetMethod(string name, params Type[] types)
        {
            return AudioUtilClass.GetMethod(name, Flags, null, types, null);
        }

        private static void Invoke(MethodInfo method, params object[] parameters) => method?.Invoke(null, parameters);
    }
}