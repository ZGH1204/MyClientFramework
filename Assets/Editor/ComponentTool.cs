using UnityEditor;
using UnityEngine;

public class ComponentTool : MonoBehaviour
{
    [MenuItem("Tools/Component Tool/Add MeshCollider Deep")]
    private static void AddMeshCollider()
    {
        Transform[] arr = UnityEditor.Selection.GetTransforms(UnityEditor.SelectionMode.Deep);
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].GetComponent<MeshRenderer>() != null)
            {
                if (arr[i].gameObject.GetComponent<MeshCollider>() == null)
                {
                    arr[i].gameObject.AddComponent<MeshCollider>();
                }
            }
        }
    }

    [MenuItem("Tools/Component Tool/Delect MeshCollider Deep")]
    private static void DelectMeshCollider()
    {
        Transform[] arr = UnityEditor.Selection.GetTransforms(UnityEditor.SelectionMode.Deep);
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].gameObject.GetComponent<MeshCollider>() != null)
            {
                GameObject.DestroyImmediate(arr[i].gameObject.GetComponent<MeshCollider>());
            }
        }
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}