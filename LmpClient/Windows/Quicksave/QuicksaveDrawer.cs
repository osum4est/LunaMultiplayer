using LmpClient.Systems.Quicksave;
using UnityEngine;

namespace LmpClient.Windows.Quicksave
{
    public partial class QuicksaveWindow
    {
        private static int _prevNumQuicksaves = -1;

        protected override void DrawWindowContent(int windowId)
        {
            GUILayout.BeginVertical();
            GUI.DragWindow(MoveRect);

            DrawQuicksaveList();
            DrawQuicksaveButtons();

            GUILayout.EndVertical();
        }

        private static void DrawQuicksaveList()
        {
            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            foreach (var quicksave in System.Quicksaves)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{quicksave.Name} ({quicksave.Date:g})", _quicksaveStyle);
                if (GUILayout.Button(QuicksaveLoadIcon, _buttonStyle))
                    System.MessageSender.SendQuicksaveLoadRequestMsg(quicksave);

                if (GUILayout.Button(DeleteIcon, _buttonStyle))
                    System.MessageSender.SendQuicksaveRemoveRequestMsg(quicksave);

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private static void DrawQuicksaveButtons()
        {
            GUILayout.BeginHorizontal();

            _quicksaveName = UpdateQuicksaveName(GUILayout.TextArea(_quicksaveName));

            if (GUILayout.Button(QuicksaveSaveIcon, _buttonStyle))
                System.MessageSender.SendQuicksaveSaveRequestMsg(_quicksaveName);

            GUILayout.EndHorizontal();
        }

        private static string UpdateQuicksaveName(string newName)
        {
            var defaultName = $"Quicksave #{System.Quicksaves.Count + 1}";

            if (_prevNumQuicksaves == System.Quicksaves.Count)
                return newName;

            _prevNumQuicksaves = System.Quicksaves.Count;
            return defaultName;
        }
    }
}