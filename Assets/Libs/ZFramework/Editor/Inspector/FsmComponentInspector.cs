using UnityEditor;
using ZFramework.Fsm;
using ZFramework.Runtime;

namespace ZFramework.Editor
{
    [CustomEditor(typeof(FsmComponent))]
    internal sealed class FsmComponentInspector : ZFrameworkInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            FsmComponent t = (FsmComponent)target;

            if (PrefabUtility.GetPrefabType(t.gameObject) != PrefabType.Prefab)
            {
                EditorGUILayout.LabelField("FSM Count", t.Count.ToString());

                FsmBase[] fsms = t.GetAllFsms();
                foreach (FsmBase fsm in fsms)
                {
                    DrawFsm(fsm);
                }
            }

            Repaint();
        }

        private void OnEnable()
        {
        }

        private void DrawFsm(FsmBase fsm)
        {
            EditorGUILayout.LabelField(Utility.Text.GetFullName(fsm.OwnerType, fsm.Name), fsm.IsRunning ? string.Format("{0}, {1} s", fsm.CurrentStateName, fsm.CurrentStateTime.ToString("F1")) : (fsm.IsDestroyed ? "Destroyed" : "Not Running"));
        }
    }
}