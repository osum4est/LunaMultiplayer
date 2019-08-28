using LmpClient.Systems.Quicksave;
using UnityEngine;

namespace LmpClient.Windows.Quicksave
{
    public partial class QuicksaveWindow
    {
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

            foreach (var quicksave in QuicksaveSystem.Singleton.Quicksaves)
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

            var name = GUILayout.TextArea($"Quicksave #{QuicksaveSystem.Singleton.Quicksaves.Count + 1}");
            if (GUILayout.Button(QuicksaveSaveIcon, _buttonStyle))
                System.MessageSender.SendQuicksaveSaveRequestMsg(name);

            GUILayout.EndHorizontal();
        }
    }
}