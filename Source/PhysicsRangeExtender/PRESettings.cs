using System;
using UnityEngine;

namespace PhysicsRangeExtender
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class PreSettings : MonoBehaviour
    {
        private static readonly KSPe.IO.Data.ConfigNode SETTINGS = KSPe.IO.Data.ConfigNode.ForType<PhysicsRangeExtender>("PreSettings", "settings.cfg");       
        
        public static int GlobalRange { get; set; }

        public static float CamFixMultiplier { get; set; }

        public static bool ConfigLoaded { get; set; } = false;
        public static bool ModEnabled { get; set; }

        public void Awake()
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
                Debug.Log("[PhysicsRangeExtender]: ModEnabled:" + ModEnabled);
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
            ModEnabled = settings.GetValue<bool>("ModEnabled", ModEnabled);
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
                settings.SetValue("ModEnabled", ModEnabled, true);
                SETTINGS.Save();
            }
            catch (Exception ex)
            {
                Debug.Log("[PhysicsRangeExtender]: Failed to save settings config:" + ex.Message); throw;
            }
        }


    }
}
