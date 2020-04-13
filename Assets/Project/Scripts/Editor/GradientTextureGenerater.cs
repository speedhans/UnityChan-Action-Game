using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GradientTextureGenerater : EditorWindow
{
    static Gradient m_Gradient;
    static bool m_HDR;
    static Texture2D m_Texture2D;

    [MenuItem("CustomEditor/GradientGenerater")]
    static void Initialize()
    {
        if (m_Gradient == null)
            m_Gradient = new Gradient();
        GradientTextureGenerater window = (GradientTextureGenerater)GetWindow(typeof(GradientTextureGenerater));
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("GradientGenerater");
        EditorGUILayout.Space(10);

        var oldColor = GUI.backgroundColor;
        GUI.backgroundColor = m_HDR ? Color.red : Color.white;
        if (GUILayout.Button("HDR"))
        {
            m_HDR = !m_HDR;
        }
        GUI.backgroundColor = oldColor;

        GUILayoutOption[] options = new[] { GUILayout.Width(300), GUILayout.Height(50) };
        m_Gradient = EditorGUILayout.GradientField(new GUIContent() { text = "Field" }, m_Gradient, m_HDR, options);

        if (GUILayout.Button("Generate Texture"))
        {
            m_Texture2D = new Texture2D(32, 4, TextureFormat.ARGB32, false);

            Color[] colors = new Color[32 * 4];
            for (int y = 0; y < 4; ++y)
            {
                for (int x = 0; x < 32; ++x)
                {
                    colors[(y * 32) + x] = m_Gradient.Evaluate((x + 1.0f) / 32.0f);
                }
            }

            m_Texture2D.SetPixels(colors);
            m_Texture2D.Apply();
        }

        if (m_Texture2D != null)
        {
            EditorGUI.DrawPreviewTexture(new Rect(90, 170, 150, 20), m_Texture2D);

            if (GUILayout.Button("Export"))
            {
                SaveTextureAsPNG(m_Texture2D, "Assets/Project/Graphic/Textures/GradientTex.png");
            }
        }
        else
            EditorGUI.DrawRect(new Rect(90, 170, 150, 20), Color.white);
    }

    public static void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
    {
        byte[] _bytes = _texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(_fullPath, _bytes);
        Debug.Log(_bytes.Length / 1024 + "Kb was saved as: " + _fullPath);
    }
}
