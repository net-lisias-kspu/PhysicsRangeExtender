﻿using System;
using UnityEngine;

namespace PhysicsRangeExtender
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class PreSettings : MonoBehaviour
    {
        public static string SettingsConfigUrl = "GameData/PhysicsRangeExtender/settings.cfg";
        public static int GlobalRange { get; set; }
        public static bool FlickeringFix { get; set; }
        public static bool TerrainExtender { get; set; }
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
                Debug.Log("[PhysicsRangeExtender]: Loading settings.cfg ==");

                ConfigNode fileNode = ConfigNode.Load(SettingsConfigUrl);
                if (!fileNode.HasNode("PreSettings")) return;

                ConfigNode settings = fileNode.GetNode("PreSettings");
                GlobalRange = int.Parse(settings.GetValue("GlobalRange"));
                FlickeringFix = bool.Parse(settings.GetValue("FlickeringFix"));
                TerrainExtender = bool.Parse(settings.GetValue("TerrainExtender"));

            }
            catch (Exception ex)
            {
                Debug.Log("[PhysicsRangeExtender]: Failed to load settings config:" + ex.Message);
            }
        }

        public static void SaveConfig()
        {
            try
            {
                Debug.Log("[PhysicsRangeExtender]: Saving settings.cfg ==");
                ConfigNode fileNode = ConfigNode.Load(SettingsConfigUrl);
                if (!fileNode.HasNode("PreSettings")) return;
                ConfigNode settings = fileNode.GetNode("PreSettings");

                settings.SetValue("GlobalRange", GlobalRange);
                settings.SetValue("FlickeringFix", FlickeringFix);
                settings.SetValue("TerrainExtender", TerrainExtender);
                fileNode.Save(SettingsConfigUrl);
            }
            catch (Exception ex)
            {
                Debug.Log("[PhysicsRangeExtender]: Failed to save settings config:" + ex.Message); throw;
            }
        }


    }
}
