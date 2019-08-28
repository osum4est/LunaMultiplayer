using LmpClient.Base;
using LmpClient.Localization;
using LmpClient.Systems.Quicksave;
using LmpClient.VesselUtilities;
using LmpCommon.Enums;
using UnityEngine;

namespace LmpClient.Windows.Quicksave
{
    public partial class QuicksaveWindow : SystemWindow<QuicksaveWindow, QuicksaveSystem>
    {
        private static bool _display;

        public override bool Display
        {
            get => base.Display && _display && MainSystem.NetworkState >= ClientState.Running &&
                   HighLogic.LoadedSceneIsFlight && !VesselCommon.IsSpectating;
            set
            {
                var prevDisplay = _display;
                base.Display = _display = value;
                if (Display && !prevDisplay)
                    Refresh();
            }
        }

        private const float WindowHeight = 400;
        private const float WindowWidth = 400;

        private static Vector2 _scrollPos;
        
        private static string _quicksaveName;

        private static GUIStyle _quicksaveStyle;
        private static GUIStyle _buttonStyle;

        protected override bool Resizable => false;

        public override void SetStyles()
        {
            WindowRect = new Rect(Screen.width / 10f, Screen.height / 2f - WindowHeight / 2f, WindowWidth,
                WindowHeight);
            MoveRect = new Rect(0, 0, int.MaxValue, TitleHeight);

            _scrollPos = new Vector2(0, 0);
            _quicksaveStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Normal, stretchWidth = true, wordWrap = false
            };

            _buttonStyle = new GUIStyle(Skin.button)
            {
                fontStyle = FontStyle.Normal, stretchWidth = false, wordWrap = false,
            };
        }

        protected override void DrawGui()
        {
            WindowRect = FixWindowPos(GUILayout.Window(6726 + MainSystem.WindowOffset, WindowRect, DrawContent,
                LocalizationContainer.QuicksaveWindowText.Title));
        }

        private static void Refresh()
        {
            System.MessageSender.SendQuicksaveListRequestMsg();
        }
    }
}