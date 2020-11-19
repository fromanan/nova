#if false

/// DO NOT COMPILE

/*
 *           ~~ Screenshot Utility ~~ 
 *  Takes a screenshot of the game window with its
 *  current resolution. Should work on any platform
 *  or the editor.
 * 
 *  Notes:
 *    - Images are stored in a Screenshots folder within the Unity project directory.
 * 
 *    - Images will copied over if player prefs are reset!
 * 
 *    - If the resolution is 1024x768, and the scale factor
 *      is 2, the screenshot will be saved as 2048x1536.
 * 
 *    - The mouse is not captured in the screenshot.
 * 
 *  Michigan State University
 *  Games for Entertainment and Learning (GEL) Lab
 */

using System;
using UnityEngine;
using System.IO;
using System.Reflection;
using UnityEditor; // included for access to File IO such as Directory class

/// <summary>
/// Handles taking a screenshot of game window.
/// </summary>
[CustomEditor(typeof(ScreenshotUtility))]
public class ScreenshotUtility : Editor
{
    // static reference to ScreenshotUtility so can be called from other scripts directly (not just through gameobject component)
    public static ScreenshotUtility screenShotUtility;

    #region Public Variables

    // The key used to take a screenshot
    public KeyCode m_ScreenshotKey = KeyCode.F2;

    // The amount to scale the screenshot
    public int m_ScaleFactor = 1;

    #endregion

    #region Private Variables

    // The number of screenshots taken
    private int m_ImageCount;

    #endregion

    #region Constants

    // The key used to get/set the number of images
    private const string ImageCntKey = "IMAGE_CNT";

    private const string SCREENSHOTS_DIRECTORY = "Screenshots";

    private const string EXTENSION = "png";

    private const string SCREENSHOT_SOUND_NAME = "Snapshot";

    private AudioClip screenshotSound;

    #endregion

    /// <summary>
    /// Lets the screenshot utility persist through scenes.
    /// </summary>
    private void OnEnable() 
    {
        Debug.Log("Initialized Screenshotter");
        
        screenshotSound = EditorGUIUtility.Load($"Assets/Audio/{SCREENSHOT_SOUND_NAME}.wav") as AudioClip;
        
        if (screenShotUtility == null)
        {
            // get image count from player prefs for indexing of filename
            m_ImageCount = PlayerPrefs.GetInt(ImageCntKey);
        }

        // if there is not a "Screenshots" directory in the Project folder, create one
        if (!Directory.Exists(SCREENSHOTS_DIRECTORY))
        {
            Directory.CreateDirectory(SCREENSHOTS_DIRECTORY);
        }
        
        EditorApplication.update += Update;
    }

    private void OnDisable()
    {
        EditorApplication.update -= Update;
    }

    private void Update()
    {
        // Checks for input
        if (!Input.GetKeyDown(m_ScreenshotKey)) return;
        
        PlayClip(screenshotSound);

        // Saves the current image count
        PlayerPrefs.SetInt(ImageCntKey, ++m_ImageCount);

        // Adjusts the height and width for the file name
        int width = Screen.width * m_ScaleFactor;
        int height = Screen.height * m_ScaleFactor;

        string screenshotName = $"{SCREENSHOTS_DIRECTORY}/Screenshot_{width}x{height}_{m_ImageCount}.{EXTENSION}";

        // Takes the screenshot with filename "Screenshot_WIDTHxHEIGHT_IMAGECOUNT.png"
        // and save it in the Screenshots folder
        ScreenCapture.CaptureScreenshot(screenshotName, m_ScaleFactor);

        Debug.Log($"Screenshot: {screenshotName} Successfully Captured.");
    }

    public static void PlayClip(AudioClip clip)
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "PlayClip",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new [] { typeof(AudioClip) },
            null
        );
        method?.Invoke(null, new object[] { clip });
    }

    public static void StopAllClips()
    {
        Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo method = audioUtilClass.GetMethod(
            "StopAllClips",
            BindingFlags.Static | BindingFlags.Public,
            null,
            new Type[] { },
            null
        );
        method?.Invoke(
            null,
            new object[] { }
        );
    }
}

#endif