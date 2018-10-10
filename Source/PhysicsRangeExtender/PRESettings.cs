using System;
using UnityEngine;

namespace PhysicsRangeExtender
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class PreSettings : MonoBehaviour
    {
		private static readonly KSPe.PluginConfig SETTINGS = KSPe.PluginConfig.ForType<PhysicsRangeExtender>("PreSettings", "settings.cfg");
        
        public static int GlobalRange { get; set; } 

        public static bool ConfigLoaded { get; set; } = false;
        void Awake()
        {
            LoadConfig();
            ConfigLoaded = true;
        }

        public static void LoadConfig()
        {
            try
            {
                Debug.Log("Loading settings.cfg ==");

				ConfigNode settings = SETTINGS.Load().Node;
                GlobalRange = int.Parse(settings.GetValue("GlobalRange"));
            }
            catch (Exception ex)
            {
                Debug.Log("Failed to load settings config:" + ex.Message);
            }
        }

        public static void SaveConfig()
        {
            try
            {
                Debug.Log("Saving settings.cfg ==");

				ConfigNode settings = SETTINGS.Load().Node;
                settings.SetValue("GlobalRange", GlobalRange);
                SETTINGS.Save();
            }
            catch (Exception ex)
            {
                Debug.Log("Failed to save settings config:" + ex.Message); throw;
            }
        }


    }
}
