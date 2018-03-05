using UnityEditor;
using ZFramework.Network;
using ZFramework.Runtime;

namespace ZFramework.Editor
{
    [CustomEditor(typeof(NetworkComponent))]
    internal sealed class NetworkComponentInspector : ZFrameworkInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            NetworkComponent t = (NetworkComponent)target;

            if (PrefabUtility.GetPrefabType(t.gameObject) != PrefabType.Prefab)
            {
                EditorGUILayout.LabelField("Network Channel Count", t.NetworkChannelCount.ToString());

                INetworkChannel[] networkChannels = t.GetAllNetworkChannels();
                foreach (INetworkChannel networkChannel in networkChannels)
                {
                    DrawNetworkChannel(networkChannel);
                }
            }

            Repaint();
        }

        private void OnEnable()
        {
        }

        private void DrawNetworkChannel(INetworkChannel networkChannel)
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.LabelField(networkChannel.Name, networkChannel.Connected ? "Connected" : "Disconnected");
                EditorGUILayout.LabelField("Network Type", networkChannel.NetworkType.ToString());
                EditorGUILayout.LabelField("Local Address", networkChannel.Connected ? string.Format("{0}:{1}", networkChannel.LocalIPAddress.ToString(), networkChannel.LocalPort.ToString()) : "Unknown");
                EditorGUILayout.LabelField("Remote Address", networkChannel.Connected ? string.Format("{0}:{1}", networkChannel.RemoteIPAddress.ToString(), networkChannel.RemotePort.ToString()) : "Unknown");
                EditorGUILayout.LabelField("Heart Beat Interval", networkChannel.HeartBeatInterval.ToString());
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
        }
    }
}