using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace NovaEditor.Library.Utilities
{
    public class ScreenshotUtility : EditorWindow
    {
        private Camera camera;
        private RenderTexture renderTexture;

        private int resWidth = Screen.width * 4;
        private int resHeight = Screen.height * 4;
        private int scale = 1;

        private string path = "";

        private bool showPreview = true;
        private bool isTransparent;
        private bool takeHiResShot;

        private string lastScreenshot = "";

        private static readonly Vector2 Resolution2K = new Vector2(2560, 1440);
        private static readonly Vector2 Resolution4K = new Vector2(3840, 2160);
        private static readonly Vector2 Resolution8K = new Vector2(7680, 4320);

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Tools/Screenshot")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow editorWindow = GetWindow(typeof(ScreenshotUtility));
            editorWindow.autoRepaintOnSceneChange = true;
            editorWindow.Show();
            editorWindow.titleContent.text = "Screenshot";
        }

        private void OnGUI()
        {
            LabelField("Resolution", EditorStyles.boldLabel);
            resWidth = IntField("Width", resWidth);
            resHeight = IntField("Height", resHeight);

            Space();

            scale = IntSlider("Scale", scale, 1, 15);

            HelpBox(
                "The default mode of screenshot is crop - so choose a proper width and height. The scale is a factor " +
                "to multiply or enlarge the renders without loosing quality.", MessageType.None);

            Space();

            GUILayout.Label("Save Path", EditorStyles.boldLabel);

            BeginHorizontal();
            TextField(path, GUILayout.ExpandWidth(false));
            if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
                path = EditorUtility.SaveFolderPanel("Path to Save Images", path, Application.dataPath);

            EndHorizontal();

            HelpBox("Choose the folder in which to save the screenshots ", MessageType.None);
            Space();

            //isTransparent = Toggle(isTransparent,"Transparent Background");

            GUILayout.Label("Select Camera", EditorStyles.boldLabel);

            camera = ObjectField(camera, typeof(Camera), true, null) as Camera;

            if (camera == null)
            {
                camera = Camera.main;
            }

            isTransparent = Toggle("Transparent Background", isTransparent);

            HelpBox(
                "Choose the camera of which to capture the render. You can make the background transparent using the transparency option.",
                MessageType.None);

            Space();
            BeginVertical();
            LabelField("Default Options", EditorStyles.boldLabel);

            if (GUILayout.Button("Set To Screen Size"))
            {
                resHeight = (int) Handles.GetMainGameViewSize().y;
                resWidth = (int) Handles.GetMainGameViewSize().x;
            }

            if (GUILayout.Button("Default Size"))
            {
                resWidth = (int) Resolution2K.x;
                resHeight = (int) Resolution2K.y;
                scale = 1;
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("2K Size"))
            {
                resWidth = (int) Resolution2K.x;
                resHeight = (int) Resolution2K.y;
                scale = 1;
            }

            if (GUILayout.Button("4K Size"))
            {
                resWidth = (int) Resolution4K.x;
                resHeight = (int) Resolution4K.y;
                scale = 1;
            }

            if (GUILayout.Button("8K Size"))
            {
                resWidth = (int) Resolution8K.x;
                resHeight = (int) Resolution8K.y;
                scale = 1;
            }

            GUILayout.EndHorizontal();

            EndVertical();

            Space();
            LabelField($"Screenshot will be taken at {resWidth * scale}x{resHeight * scale} px",
                EditorStyles.boldLabel);

            if (GUILayout.Button("Take Screenshot", GUILayout.MinHeight(60)))
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = EditorUtility.SaveFolderPanel("Path to Save Images", path, Application.dataPath);
                    Debug.Log("Path Set");
                    TakeHiResShot();
                }
                else
                {
                    TakeHiResShot();
                }
            }

            Space();
            BeginHorizontal();

            if (GUILayout.Button("Open Last Screenshot", GUILayout.MaxWidth(160), GUILayout.MinHeight(40)))
            {
                if (string.IsNullOrEmpty(lastScreenshot))
                {
                    Application.OpenURL($"file://{lastScreenshot}");
                    Debug.Log($"Opening File {lastScreenshot}");
                }
            }

            if (GUILayout.Button("Open Folder", GUILayout.MaxWidth(100), GUILayout.MinHeight(40)))
            {
                Application.OpenURL($"file://{path}");
            }

            if (GUILayout.Button("More Assets", GUILayout.MaxWidth(100), GUILayout.MinHeight(40)))
            {
                Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/publisher/5951");
            }

            EndHorizontal();

            if (takeHiResShot)
            {
                int resWidthN = resWidth * scale;
                int resHeightN = resHeight * scale;
                RenderTexture rt = new RenderTexture(resWidthN, resHeightN, 24);
                if (camera != null)
                {
                    camera.targetTexture = rt;

                    TextureFormat tFormat = isTransparent ? TextureFormat.ARGB32 : TextureFormat.RGB24;

                    Texture2D screenShot = new Texture2D(resWidthN, resHeightN, tFormat, false);
                    camera.Render();
                    RenderTexture.active = rt;
                    screenShot.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);
                    camera.targetTexture = null;
                    RenderTexture.active = null;
                    byte[] bytes = screenShot.EncodeToPNG();
                    string filename = ScreenShotName(resWidthN, resHeightN);

                    System.IO.File.WriteAllBytes(filename, bytes);
                    Debug.Log($"Took screenshot to: {filename}");
                    Application.OpenURL(filename);
                }

                takeHiResShot = false;
            }

            HelpBox(
                "In case of any error, make sure you have Unity Pro as the plugin requires Unity Pro to work.",
                MessageType.Info);
        }

        private string ScreenShotName(int width, int height)
        {
            string strPath = $"{path}/screen_{width}x{height}_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
            lastScreenshot = strPath;
            return strPath;
        }

        public void TakeHiResShot()
        {
            Debug.Log("Taking Screenshot");
            takeHiResShot = true;
        }
    }
}