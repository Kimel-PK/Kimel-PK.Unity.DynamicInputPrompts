using UnityEngine;
using UnityEngine.InputSystem;

namespace KimelPK.DynamicInputPrompts {
	
	[CreateAssetMenu (fileName = "SteamDeckDetector", menuName = "Kimel-PK/DynamicInputPrompts/DeviceDetectors/SteamDeckDetector")]
	public class SteamDeckDetector : DeviceDetector {

		public override bool DetectDevice (InputDevice inputDevice) {
			return inputDevice.displayName.ToLower ().Contains ("steam");
		}
	}
}