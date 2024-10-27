using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

namespace KimelPK.DynamicInputPrompts {
	
	[CreateAssetMenu (fileName = "SteamDeckDetector", menuName = "Kimel-PK/DynamicInputPrompts/DeviceDetectors/SteamDeckDetector")]
	public class SteamDeckDetector : DeviceDetector {

		public override bool DetectDevice (InputDevice inputDevice) {
			return inputDevice is XInputController && inputDevice.name.Contains("Steam");
		}
	}
}