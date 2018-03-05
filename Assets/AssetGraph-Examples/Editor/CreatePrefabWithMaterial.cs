using UnityEngine;
using UnityEditor;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine.AssetGraph;
using Model=UnityEngine.AssetGraph.DataModel.Version2;

[CustomPrefabBuilder("[Example]Create Prefab with Material", "v1.0", 1)]
public class CreatePrefabWithMaterial : IPrefabBuilder {

    [SerializeField] private GameObjectReference referencePrefab;

    public void OnValidate () {
        if (referencePrefab == null || referencePrefab.Object == null) {
            throw new NodeException ("Reference Prefab is Empty", "Select Reference Prefab from inspector.");
        }

        var r = referencePrefab.Object.GetComponentInChildren<MeshRenderer> (true);
        if (r == null) {
            throw new NodeException ("Reference Prefab does not contain MeshRenderer", "Add MeshRenderer to selected prefab, or select prefab with MeshRenderer.");
        }
    }

	/**
		 * Test if prefab can be created with incoming assets.
		 * @result Name of prefab file if prefab can be created. null if not.
		 */
    public string CanCreatePrefab (string groupKey, List<UnityEngine.Object> objects, UnityEngine.GameObject previous) {

        var mat = (Material)objects.Find(o => o.GetType() == typeof(UnityEngine.Material));

        if (mat == null) {
            return null;
        }

        return string.Format("{0}", groupKey);
	}

	/**
	 * Create Prefab.
	 */ 
    public UnityEngine.GameObject CreatePrefab (string groupKey, List<UnityEngine.Object> objects, UnityEngine.GameObject previous) {

        GameObject go = GameObject.Instantiate (referencePrefab.Object);
        go.name = groupKey;

        var mat = (Material)objects.Find(o => o.GetType() == typeof(UnityEngine.Material));

        var r = go.GetComponentInChildren<MeshRenderer> (true);

        r.material = mat;

		return go;
	}

	/**
	 * Draw Inspector GUI for this PrefabBuilder.
	 */ 
	public void OnInspectorGUI (Action onValueChanged) {

        if (referencePrefab == null) {
            referencePrefab = new GameObjectReference ();
        }

        var refObj = (GameObject)EditorGUILayout.ObjectField ("Reference Prefab", referencePrefab.Object, typeof(GameObject), false);
        if (refObj != referencePrefab.Object) {
            referencePrefab.Object = refObj;
            onValueChanged ();
        }
	}
}
