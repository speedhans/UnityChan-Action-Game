using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SkinnedMeshLinkEditor : EditorWindow
{
    static GameObject m_LinkTargetObject;
    static GameObject m_SourceObject1;
    static GameObject m_SourceObject2;
    static GameObject m_SourceObject3;

    [MenuItem("CustomEditor/SkinnedMeshLink")]
    static void Initialize()
    {
        m_LinkTargetObject = null;
        m_SourceObject1 = null;
        m_SourceObject2 = null;
        m_SourceObject3 = null;
        SkinnedMeshLinkEditor window = (SkinnedMeshLinkEditor)GetWindow(typeof(SkinnedMeshLinkEditor));
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("MeshLink");
        EditorGUILayout.Space(10);

        m_LinkTargetObject = EditorGUILayout.ObjectField("LinkTargetObject", m_LinkTargetObject, typeof(GameObject), true) as GameObject;
        m_SourceObject1 = EditorGUILayout.ObjectField("SourceObject1", m_SourceObject1, typeof(GameObject), true) as GameObject;
        m_SourceObject2 = EditorGUILayout.ObjectField("SourceObject2", m_SourceObject2, typeof(GameObject), true) as GameObject;
        m_SourceObject3 = EditorGUILayout.ObjectField("SourceObject3", m_SourceObject3, typeof(GameObject), true) as GameObject;

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Link"))
        {
            MeshLink(m_LinkTargetObject, m_SourceObject1);
            MeshLink(m_LinkTargetObject, m_SourceObject2);
            MeshLink(m_LinkTargetObject, m_SourceObject3);
        }
    }

    void MeshLink(GameObject _Dst, GameObject _Source)
    {
        if (!_Dst || !_Source) return;

        SkinnedMeshRenderer[] renderer = _Source.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < renderer.Length; ++i)
        {
            bool parentisroot = false;
            if (renderer[i].transform.name.Contains(renderer[i].transform.parent.name))
            {
                parentisroot = true;
            }
            else
            {
                parentisroot = false;
            }

            Transform[] bones = FindBones(renderer[i], _Dst.transform);
            Transform rootbone = renderer[i].rootBone ? FindRootBone(bones, renderer[i].rootBone.name) : null;

            Vector3 pos = renderer[i].transform.localPosition;
            Quaternion rot = renderer[i].transform.localRotation;
            Vector3 scale = renderer[i].transform.localScale;

            renderer[i].bones = bones;
            renderer[i].rootBone = rootbone;

            if (parentisroot)
            {
                renderer[i].transform.SetParent(_Dst.transform);
            }
            else
            {
                string path = FindPath(renderer[i].transform);
                renderer[i].transform.SetParent(_Dst.transform.Find(path));
            }

            renderer[i].transform.localPosition = pos;
            renderer[i].transform.localRotation = rot;
            renderer[i].transform.localScale = scale;
        }
    }

    Transform[] FindBones(SkinnedMeshRenderer _SrcRenderer, Transform _DstTransform)
    {
        Transform[] transforms = new Transform[_SrcRenderer.bones.Length];

        for (int i = 0; i < transforms.Length; ++i)
        {
            transforms[i] = FindBone(_DstTransform, _SrcRenderer.bones[i].name);
        }

        return transforms;
    }

    Transform FindBone(Transform _Transform, string _BoneName)
    {
        if (_Transform.name.Contains(_BoneName)) return _Transform;

        for (int i = 0; i < _Transform.childCount; ++i)
        {
            Transform t = FindBone(_Transform.GetChild(i), _BoneName);
            if (t)
            {
                return t;
            }
        }

        return null;
    }

    Transform FindRootBone(Transform[] _Transforms, string _BoneName)
    {
        for (int i = 0; i < _Transforms.Length; ++i)
        {
            if (_Transforms[i].name.Contains(_BoneName))
            {
                return _Transforms[i];
            }
        }

        return null;
    }

    string FindPath(Transform _Source)
    {
        List<string> pathlist = new List<string>();
        FindPath_s(_Source, ref pathlist);
        string path = "";
        for (int i = 1; i < pathlist.Count; ++i)
        {
            if (i <= pathlist.Count - 1)
                path += pathlist[i] + "/";
            else
                path += pathlist[i];
        }

        return path;
    }

    void FindPath_s(Transform _Source, ref List<string> _PathList)
    {
        if (_Source.parent)
        {
            _PathList.Insert(0, _Source.parent.name);
            FindPath_s(_Source.parent, ref _PathList);
        }
    }
}
