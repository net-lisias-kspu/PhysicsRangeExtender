/*
	This file is part of Physics Range Extender /L Unleashed
		© 2018-2021 Lisias T : http://lisias.net <support@lisias.net>
		© 2017 jrodrigues

	Physics Range Extender is double licensed, as follows:

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
using System.Collections.Generic;
using System.Linq;
using KSPe.Annotations;
using UnityEngine;

namespace PhysicsRangeExtender
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class TerrainExtender : MonoBehaviour
    {
        public enum LandedVesselsStates
        {
            NotFocused,
            Focusing,
            Focused,
            Lifted,
            Landed
        }

        private static bool _crashDamage;
        private static bool _joints;
        private bool _initialLoading;
        private bool _loading;
        private Vessel _tvel;

        public static List<VesselLandedState> VesselsLandedToLoad { get; set; } = new List<VesselLandedState>();

        public static void UpdateSphere()
        {
            PQS pqs = FlightGlobals.currentMainBody.pqsController;

            pqs.detailAltitudeMax = Mathf.Max(PreSettings.GlobalRange * 1000f, 100000);
            pqs.visRadAltitudeMax = Mathf.Max(PreSettings.GlobalRange * 1000f, 100000);
            pqs.collapseAltitudeMax = Mathf.Max(PreSettings.GlobalRange * 1000f, 100000) * 10;
            pqs.detailSeaLevelQuads = 0;
            pqs.detailAltitudeQuads = 0;
            pqs.maxQuadLenghtsPerFrame = 0.03f;
            pqs.visRadSeaLevelValue = 200;
            pqs.collapseSeaLevelValue = 200;
            pqs.StartUpSphere();
        }

        private void FixedUpdate()
        {
            if (!PreSettings.ConfigLoaded) return;
            if (!PreSettings.ModEnabled) return;
            if (!PreSettings.TerrainExtenderEnabled) return;
            if (!FlightGlobals.ready) return;
            if (FlightGlobals.currentMainBody == null) return;
            if (FlightGlobals.currentMainBody.pqsController == null) return;
            if (FlightGlobals.currentMainBody.pqsController.isBuildingMaps) return;

            ExtendTerrainForLandedVessels();
        }

        private void ExtendTerrainForLandedVessels()
        {
            InitialFetch();

            VesselsLandedToLoad.RemoveAll(x => x.Vessel == null);

            if (VesselsLandedToLoad.Count == 0) return;

            if (!_loading)
            {
                _loading = true;
                ActivateNoCrashDamage();

                _tvel = FlightGlobals.ActiveVessel;
            }

            foreach (VesselLandedState currentVesselData in VesselsLandedToLoad)
            {
                Vessel currentVessel = currentVesselData.Vessel;

                if (currentVessel == null) continue;
                if (!SortaLanded(currentVessel)) continue;

                switch (currentVesselData.LandedState)
                {
                    case LandedVesselsStates.NotFocused:

                       
                        if (currentVesselData.Vessel != _tvel)
                        {
                            if (VesselsLandedToLoad.Any(x =>
                                x.Vessel != currentVessel && x.LandedState == LandedVesselsStates.Focusing)) return;

                            if (InternalCamera.Instance.isActiveAndEnabled)
                            {
                                InternalCamera.Instance.DisableCamera();
                                CameraManager.Instance.SetCameraFlight();
                            }

                            MakingVesselPartsIndestructible(currentVessel);

                            FlightGlobals.ForceSetActiveVessel(currentVessel);
                            CameraManager.Instance.SetCameraFlight();

                            //UpdateSphere();
                            currentVesselData.LandedState = LandedVesselsStates.Focusing;
                            currentVesselData.TimeOfState = Time.time;
                        }
                        else
                        {
                            currentVesselData.LandedState = LandedVesselsStates.Focused;
                        }

                        currentVessel.SetPosition(currentVesselData.InitialPosition + currentVessel.up.normalized * CalculateVesselSize(currentVessel));
                        currentVessel.SetWorldVelocity(Vector3.zero);
                        break;
                    case LandedVesselsStates.Focusing:

                        if (Time.time - currentVesselData.TimeOfState > 2)
                        {
                            currentVesselData.LandedState = LandedVesselsStates.Focused;

                            foreach (VesselLandedState vesselLandedState in VesselsLandedToLoad.Where(x =>x.LandedState == LandedVesselsStates.NotFocused && Vector3.Distance(currentVessel.CoM, x.Vessel.CoM) < 2500))
                            {
                                vesselLandedState.LandedState = LandedVesselsStates.Focused;
                            }
                        }

                        currentVessel.SetPosition(currentVesselData.InitialPosition + currentVessel.up.normalized * CalculateVesselSize(currentVessel));
                        currentVessel.SetWorldVelocity(Vector3.zero);
                        break;
                    case LandedVesselsStates.Focused:

                        currentVessel.SetPosition(currentVesselData.InitialPosition + currentVessel.up.normalized * CalculateVesselSize(currentVessel));
                        currentVessel.SetWorldVelocity(Vector3.zero);
                        currentVessel.UpdateLandedSplashed();
                            currentVesselData.LandedState = LandedVesselsStates.Lifted;
                        
                        break;
                    case LandedVesselsStates.Lifted:

                        if (!currentVessel.Landed)
                        {
                            currentVessel.SetWorldVelocity(currentVessel.gravityForPos.normalized * 20.0f * Time.fixedDeltaTime);

                            currentVessel.UpdateLandedSplashed();
                        }
                        else
                        {
                            currentVessel.SetWorldVelocity(Vector3.zero);
                            currentVesselData.LandedState = LandedVesselsStates.Landed;
                        }

                        break;
                    case LandedVesselsStates.Landed:
                        currentVessel.SetWorldVelocity(Vector3.zero);
                        break;
                  
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            VesselsLandedToLoad.RemoveAll(x => x.LandedState == LandedVesselsStates.Landed);

            if (FlightGlobals.ActiveVessel != _tvel && VesselsLandedToLoad.All(x => x.LandedState != LandedVesselsStates.NotFocused && x.LandedState != LandedVesselsStates.Focusing))
            {
                FlightGlobals.ForceSetActiveVessel(_tvel);
                CameraManager.Instance.SetCameraFlight();
            }

            if (VesselsLandedToLoad.Count == 0)
            {
                DeactivateNoCrashDamage();
                _loading = false;
            }
        }

		private static void MakingVesselPartsIndestructible(Vessel currentVessel)
        {
            foreach (Part currentVesselPart in currentVessel.parts)
            {
                currentVesselPart.crashTolerance = Math.Max(currentVesselPart.crashTolerance, 1000);
            }
        }

        private void InitialFetch()
        {
            if (_initialLoading && FlightGlobals.VesselsLoaded.Count >= 1)
            {
                foreach (Vessel vessel in FlightGlobals.VesselsLoaded)
                {
                    if (vessel.isActiveVessel) continue;

                    if ((vessel.Landed || SortaLanded(vessel)) &&
                        vessel.vesselType != VesselType.Debris)
                        VesselsLandedToLoad.Add(new VesselLandedState
                        {
                            InitialAltitude = vessel.altitude,
                            LandedState = LandedVesselsStates.NotFocused,
                            Vessel = vessel
                        });
                }

                _initialLoading = false;
            }
        }

        private void Update()
        {
            if (!PreSettings.ConfigLoaded) return;
            if (!PreSettings.ModEnabled) return;
            if (!PreSettings.TerrainExtenderEnabled) return;

            ShowMessageTerrainStatus();
        }

        private void ShowMessageTerrainStatus()
        {
            if (VesselsLandedToLoad.Count == 0) return;

            LandedVesselsStates overallStatus = LandedVesselsStates.NotFocused;

            if (VesselsLandedToLoad.Any(x => x.LandedState == LandedVesselsStates.NotFocused))
                overallStatus = LandedVesselsStates.NotFocused;
            else if (VesselsLandedToLoad.Any(x => x.LandedState == LandedVesselsStates.Focusing))
                overallStatus = LandedVesselsStates.Focusing;
            else if (VesselsLandedToLoad.Any(x => x.LandedState == LandedVesselsStates.Focused))
                overallStatus = LandedVesselsStates.Focused;
            else if (VesselsLandedToLoad.Any(x => x.LandedState == LandedVesselsStates.Lifted))
                overallStatus = LandedVesselsStates.Lifted;
            else if (VesselsLandedToLoad.Any(x => x.LandedState == LandedVesselsStates.Landed))
                overallStatus = LandedVesselsStates.Landed;

            switch (overallStatus)
            {
                case LandedVesselsStates.NotFocused:
                    ScreenMessages.PostScreenMessage(
                        "[PhysicsRangeExtender]Extending terrain: focusing landed vessels.", 3f,
                        ScreenMessageStyle.UPPER_CENTER);
                    break;
                case LandedVesselsStates.Focusing:
                    ScreenMessages.PostScreenMessage(
                        "[PhysicsRangeExtender]Extending terrain: focusing landed vessels.", 3f,
                        ScreenMessageStyle.UPPER_CENTER);
                    break;
                case LandedVesselsStates.Focused:
                    ScreenMessages.PostScreenMessage(
                        "[PhysicsRangeExtender]Extending terrain: lifting vessels.", 3f,
                        ScreenMessageStyle.UPPER_CENTER);
                    break;
                case LandedVesselsStates.Lifted:
                    ScreenMessages.PostScreenMessage(
                        "[PhysicsRangeExtender]Extending terrain: landing vessels.", 3f,
                        ScreenMessageStyle.UPPER_CENTER);
                    break;
                case LandedVesselsStates.Landed:
                    ScreenMessages.PostScreenMessage(
                        "[PhysicsRangeExtender]Extending terrain: switching to previous vessel.", 3f,
                        ScreenMessageStyle.UPPER_CENTER);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void DeactivateNoCrashDamage()
        {
            CheatOptions.NoCrashDamage = _crashDamage;
            CheatOptions.UnbreakableJoints = _joints;
        }


        private void Start()
        {
            if (!PreSettings.ModEnabled) return;
            if (!PreSettings.TerrainExtenderEnabled) return;

            _initialLoading = true;
        }

        public static void ActivateNoCrashDamage()
        {
            _crashDamage = CheatOptions.NoCrashDamage;
            _joints = CheatOptions.UnbreakableJoints;
            CheatOptions.NoCrashDamage = true;
            CheatOptions.UnbreakableJoints = true;
        }

        public static bool SortaLanded(Vessel v)
        {
            if (v.Splashed) return false;

            return v.mainBody.GetAltitude(v.CoM) - Math.Max(v.terrainAltitude, 0) < 100;
        }

        public class VesselLandedState
        {
            public Vessel Vessel { get; set; }
            public LandedVesselsStates LandedState { get; set; }
            public double InitialAltitude { get; set; }

            public double TimeOfState { get; set; }

            public Vector3d InitialPosition { get; set; }
        }

#if !KSP_15
		private static readonly Dictionary<Guid, float> VESSEL_SIZES = new Dictionary<Guid, float>();

		[UsedImplicitly]
		private void Awake()
		{
			VESSEL_SIZES.Clear();
			GameEvents.onVesselDestroy.Add(this.OnVesselDestroy);
			GameEvents.onVesselChange.Add(this.OnVesselChange);
		}

		[UsedImplicitly]
		private void OnDestroy()
		{
			GameEvents.onVesselChange.Remove(this.OnVesselChange);
			GameEvents.onVesselDestroy.Remove(this.OnVesselDestroy);
			VESSEL_SIZES.Clear();
		}

		private void OnVesselChange(Vessel vessel)
		{
			VESSEL_SIZES.Remove(vessel.id);
		}

		private void OnVesselDestroy(Vessel vessel)
		{
			VESSEL_SIZES.Remove(vessel.id);
		}

		private float CalculateVesselSize(Vessel vessel)
		{
			if (!VESSEL_SIZES.ContainsKey(vessel.id)) return VESSEL_SIZES[vessel.id];

			List<Bounds> list = new List<Bounds>(vessel.Parts.Count);
			foreach (Part p in vessel.Parts)
				list.Add(PartGeometryUtil.GetPartRendererBound(p));
			Bounds b = PartGeometryUtil.MergeBounds(list.ToArray(), vessel.transform);
			return VESSEL_SIZES[vessel.id] = b.size.magnitude;
		}
#else
		private float CalculateVesselSize(Vessel vessel)
		{
			return vessel.vesselSize.magnitude;
		}
#endif
    }
}