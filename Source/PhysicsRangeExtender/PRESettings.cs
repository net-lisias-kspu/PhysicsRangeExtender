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
using UnityEngine;

namespace PhysicsRangeExtender
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class PreSettings : MonoBehaviour
    {
        private static readonly KSPe.IO.Data.ConfigNode SETTINGS = KSPe.IO.Data.ConfigNode.ForType<PhysicsRangeExtender>("PreSettings", "settings.cfg");       
        
        public static int GlobalRange { get; set; }
        public static bool FlickeringFixEnabled { get; set; }
        public static bool TerrainExtenderEnabled { get; set; }
        public static float CamFixMultiplier { get; set; }

        public static bool ConfigLoaded { get; set; } = false;

        internal static bool ModEnabled => FlickeringFixEnabled || TerrainExtenderEnabled;

        void Awake()
        {
            LoadConfig();
            ConfigLoaded = true;
        }

        public static void LoadConfig()
        {
            Debug.Log("[PhysicsRangeExtender]: Loading settings.cfg ==");
            try
            {
		        KSPe.IO.Asset.ConfigNode defaultSettings = KSPe.IO.Asset.ConfigNode.ForType<PhysicsRangeExtender>("PreSettings", "default.cfg");       
				LoadConfig(defaultSettings.Load());
				if (SETTINGS.IsLoadable)
					LoadConfig(SETTINGS.Load());
            }
            catch (Exception ex)
            {
                Debug.Log("[PhysicsRangeExtender]: Failed to load settings config:" + ex.Message);
            }
        }
        
        private static void LoadConfig(KSPe.IO.ReadableConfigNode configNode)
		{
			KSPe.ConfigNodeWithSteroids settings = KSPe.ConfigNodeWithSteroids.from(configNode.Node);
            GlobalRange = settings.GetValue<int>("GlobalRange", GlobalRange);
            CamFixMultiplier = settings.GetValue<float>("CamFixMultiplier", CamFixMultiplier);
            FlickeringFixEnabled = settings.GetValue<bool>("ModEnabled", FlickeringFixEnabled);
            TerrainExtenderEnabled = settings.GetValue<bool>("TerrainExtenderEnabled", TerrainExtenderEnabled);
		}

        public static void SaveConfig()
        {
            try
            {
                Debug.Log("Saving settings.cfg ==");

				SETTINGS.Clear();
                ConfigNode settings = SETTINGS.Node;
                settings.SetValue("GlobalRange", GlobalRange, true);
                settings.SetValue("CamFixMultiplier", CamFixMultiplier, true);
                settings.SetValue("FlickeringFixEnabled", FlickeringFixEnabled, true);
                settings.SetValue("TerrainExtenderEnabled", TerrainExtenderEnabled, true);
                SETTINGS.Save();
            }
            catch (Exception ex)
            {
                Debug.Log("[PhysicsRangeExtender]: Failed to save settings config:" + ex.Message); throw;
            }
        }
    }
}
