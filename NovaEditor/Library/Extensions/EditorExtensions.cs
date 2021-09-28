using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NovaEditor.Library.Extensions
{
    public static class EditorExtensions
    {
        // https://answers.unity.com/questions/36388/how-to-play-audioclip-in-edit-mode.html
        public static void PlayClip(AudioClip clip) {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "PlayClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new [] { typeof(AudioClip) },
                null
            );
            method?.Invoke(
                null,
                new object[] { clip }
            );
        }
        
        public static void StopAllClips() {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "StopAllClips",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new Type[]{},
                null
            );
            method?.Invoke(
                null,
                new object[] {}
            );
        }
    }
}