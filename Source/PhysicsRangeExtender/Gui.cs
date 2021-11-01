/*
	This file is part of Physics Range Extender /L Unleashed
		© 2018-2021 Lisias T : http://lisias.net <support@lisias.net>
		© 2017 jrodrigues

	CrewLight is double licensed, as follows:

		* SKL 1.0 : https://ksp.lisias.net/SKL-1_0.txt
		* GPL 2.0 : https://www.gnu.org/licenses/gpl-2.0.txt

	And you are allowed to choose the License that better suit your needs.

	Physics Range Extender /L Unleashed is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the SKL Standard License 1.0
	along with Physics Range Extender /L Unleashed.
	If not, see <https://ksp.lisias.net/SKL-1_0.txt>.

	You should have received a copy of the GNU General Public License 2.0
	along with Physics Range Extender /L Unleashed.
	If not, see <https://www.gnu.org/licenses/>.

*/
using System;
using System.Globalization;
using UnityEngine;

using KSP.UI.Screens;

using Asset = KSPe.IO.Asset<PhysicsRangeExtender.Startup>;

using Toolbar = KSPe.UI.Toolbar;
using GUI = KSPe.UI.GUI;
using GUILayout = KSPe.UI.GUILayout;
using KSPe.Annotations;


// ReSharper disable NotAccessedField.Local

namespace PhysicsRangeExtender
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Gui : MonoBehaviour
    {
        private const float WindowWidth = 250;
        private const float DraggableHeight = 40;
        private const float LeftIndent = 12;
        private const float ContentTop = 20;
        public static Gui Fetch;
        private Toolbar.Button button = null;
        public static bool GuiEnabled;
        private readonly float _incrButtonWidth = 26;
        private readonly float contentWidth = WindowWidth - 2 * LeftIndent;
        private readonly float entryHeight = 20;
        
        private bool _gameUiToggle;
        private string _guiGlobalRangeForVessels = String.Empty;

        private float _windowHeight = 250;
        private Rect _windowRect;
        private string _guiCamFixMultiplier;

        [UsedImplicitly]
        private void Awake()
        {
            if (Fetch)
                Destroy(Fetch);

            Fetch = this;
        }

        [UsedImplicitly]
        private void Start()
        {
            _windowRect = new Rect(Screen.width - WindowWidth - 40, 100, WindowWidth, _windowHeight);
            this.AddToolbarButton();
            GameEvents.onHideUI.Add(this.GameUiDisable);
            GameEvents.onShowUI.Add(this.GameUiEnable);
            _gameUiToggle = true;
            _guiGlobalRangeForVessels = PreSettings.GlobalRange.ToString();
            _guiCamFixMultiplier = PreSettings.CamFixMultiplier.ToString(CultureInfo.InvariantCulture);
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
            ToolbarController.Instance.Destroy();
            GameEvents.onHideUI.Remove(this.GameUiDisable);
            GameEvents.onShowUI.Remove(this.GameUiEnable);
        }

        [UsedImplicitly]
        private void OnGUI()
        {
            if (!PreSettings.ConfigLoaded) return;
            if (GuiEnabled && _gameUiToggle)
                _windowRect = GUI.Window(320, _windowRect, GuiWindow, "");
        }

        private void GuiWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, WindowWidth, DraggableHeight));
            float line = 0;

            DrawTitle();
            line++;

            if (PreSettings.ModEnabled)
            {
                DrawGlobalVesselRange(line);
                line++;
                DrawCamFixMultiplier(line);
                line++;
                DrawSaveButton(line);
                line++;
            }

            DisableMod(line);

            _windowHeight = ContentTop + line * entryHeight + entryHeight + entryHeight;
            _windowRect.height = _windowHeight;
        }

        private void DisableMod(float line)
        {
            Rect saveRect = new Rect(LeftIndent, ContentTop + line * entryHeight, contentWidth, entryHeight);

            if (!PreSettings.FlickeringFixEnabled)
            {
                if (GUI.Button(saveRect, "Disable flickering fix"))
                {
                    PreSettings.FlickeringFixEnabled = false;
                    PhysicsRangeExtender.RestoreStockRanges();
                    PreSettings.SaveConfig();
                }
            }
            else
            {
                if (GUI.Button(saveRect, "Enable flickering fix"))
                {
                    PreSettings.FlickeringFixEnabled = true;
                    Apply();
                    PreSettings.SaveConfig();
                }
            }
        }


        private void DrawGlobalVesselRange(float line)
        {
            GUIStyle leftLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperLeft,
                normal = {textColor = Color.white}
            };

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Global range:", leftLabel);
            float textFieldWidth = 42;
            Rect fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth - 3 * _incrButtonWidth,
                  ContentTop + line * entryHeight, textFieldWidth, entryHeight);
            _guiGlobalRangeForVessels = GUI.TextField(fwdFieldRect, _guiGlobalRangeForVessels);
          
        }

        private void DrawCamFixMultiplier(float line)
        {
            GUIStyle leftLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperLeft,
                normal = { textColor = Color.white }
            };

            GUI.Label(new Rect(LeftIndent, ContentTop + line * entryHeight, 60, entryHeight), "Cam fix multiplier:",
                leftLabel);
            float textFieldWidth = 42;
            Rect fwdFieldRect = new Rect(LeftIndent + contentWidth - textFieldWidth - 3 * _incrButtonWidth,
                ContentTop + line * entryHeight, textFieldWidth, entryHeight);

           this._guiCamFixMultiplier = GUI.TextField(fwdFieldRect, _guiCamFixMultiplier);
           
        }

        private void DrawSaveButton(float line)
        {
            Rect saveRect = new Rect(LeftIndent, ContentTop + line * entryHeight, contentWidth / 2, entryHeight);
            if (GUI.Button(saveRect, "Apply new range"))
            { 
                Apply();
                PreSettings.SaveConfig();
            }
        }

        internal void Apply()
        {
            if (int.TryParse(_guiGlobalRangeForVessels, out int parseGlobalRange))
            {
                PreSettings.GlobalRange = parseGlobalRange;
                _guiGlobalRangeForVessels = PreSettings.GlobalRange.ToString();
            }

            if (float.TryParse(_guiCamFixMultiplier, out float parseCamFix))
            {
                PreSettings.CamFixMultiplier = parseCamFix;
                _guiCamFixMultiplier = PreSettings.CamFixMultiplier.ToString(CultureInfo.InvariantCulture);
            }

            PreSettings.SaveConfig();
            PhysicsRangeExtender.UpdateRanges(true);
        }

        private void DrawTitle()
        {
            GUIStyle centerLabel = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter,
                normal = {textColor = Color.white}
            };
            GUIStyle titleStyle = new GUIStyle(centerLabel)
            {
                fontSize = 10,
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(new Rect(0, 0, WindowWidth, 20), "Physics Range Extender", titleStyle);
        }

        private void AddToolbarButton()
        {
            if (null == this.button)
            {
                Texture2D buttonTexture = Asset.Texture2D.LoadFromFile("Textures", "icon");
                this.button = Toolbar.Button.Create(this
                        , ApplicationLauncher.AppScenes.ALWAYS
                        , buttonTexture, buttonTexture
                        , Version.FriendlyName
                    );
                this.button.Toolbar.Add(Toolbar.Button.ToolbarEvents.Kind.Active, new Toolbar.Button.Event(this.EnableGui, this.DisableGui));
                ToolbarController.Instance.Add(this.button);
            }
        }

        private void EnableGui()
        {
            GuiEnabled = true;
            Log.trace("Showing PRE GUI");
        }

        private void DisableGui()
        {
            GuiEnabled = false;
            Log.trace("Hiding PRE GUI");
        }

        private void GameUiEnable()
        {
            _gameUiToggle = true;
        }

        private void GameUiDisable()
        {
            _gameUiToggle = false;
        }
    }
}