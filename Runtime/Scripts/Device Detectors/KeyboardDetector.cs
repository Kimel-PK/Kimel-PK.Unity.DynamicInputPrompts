using UnityEngine;
using UnityEngine.InputSystem;

namespace KimelPK.DynamicInputPrompts {
	
	[CreateAssetMenu (fileName = "KeyboardDetector", menuName = "Kimel-PK/DynamicInputPrompts/DeviceDetectors/KeyboardDetector")]
	public class KeyboardDetector : DeviceDetector {

		public override bool DetectDevice (InputDevice inputDevice) {
			return inputDevice is Keyboard;
		}
	}
}