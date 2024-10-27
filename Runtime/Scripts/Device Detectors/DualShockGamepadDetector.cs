using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

namespace KimelPK.DynamicInputPrompts {
	
	[CreateAssetMenu (fileName = "DualShockGamepadDetector", menuName = "Kimel-PK/DynamicInputPrompts/DeviceDetectors/DualShockGamepadDetector")]
	public class DualShockGamepadDetector : DeviceDetector {

		public override bool DetectDevice (InputDevice inputDevice) {
			return inputDevice is DualShockGamepad;
		}
	}
}