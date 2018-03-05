using UnityEngine;
using UnityEditor;

public class BloodSliderEditor : Editor {

    [MenuItem("GameObject/UI/BloodSlider")]
    static void CreateBloodSlider()
    {
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas == null)
        {
            EditorUtility.DisplayDialog("提示", "场景中不存在至少一个画布！", "确定");
            return;
        }

        GameObject bloodSlider = Instantiate(Resources.Load("BloodSlider") as GameObject);
        bloodSlider.name = "BloodSlider";
        bloodSlider.transform.SetParent(canvas.transform);
        bloodSlider.transform.localPosition = Vector3.zero;
        bloodSlider.transform.localScale = Vector3.one;
    }
}
