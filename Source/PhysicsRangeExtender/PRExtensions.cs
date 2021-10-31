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
using System.Collections.Generic;
using System;
using UniLinq;
using UnityEngine;

namespace PhysicsRangeExtender
{
    public static class PRExtensions
    {
        public static bool _wasEnabled = false;

        public static void PreOn(string _modName)
        {
            if (!PreSettings.ModEnabled && _wasEnabled)
            {
                Debug.Log("[Physic Range Extender] === Being turned on by " + _modName);

                PreSettings.ModEnabled = true;
                Gui.Fetch.Apply();
                PreSettings.SaveConfig();
            }
        }

        public static void PreOff(string _modName)
        {
            if (PreSettings.ModEnabled)
            {
                _wasEnabled = true;
                Debug.Log("[Physic Range Extender] === Being turned off by " + _modName);
                PreSettings.ModEnabled = false;
                PhysicsRangeExtender.RestoreStockRanges();
                PreSettings.SaveConfig();
            }
        }
    }
}