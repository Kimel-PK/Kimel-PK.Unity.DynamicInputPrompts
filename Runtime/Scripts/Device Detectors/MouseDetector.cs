using UnityEngine;
using UnityEngine.InputSystem;

namespace KimelPK.DynamicInputPrompts {
	
	[CreateAssetMenu (fileName = "MouseDetector", menuName = "Kimel-PK/DynamicInputPrompts/DeviceDetectors/MouseDetector")]
	public class MouseDetector : DeviceDetector {

		public override bool DetectDevice (InputDevice inputDevice) {
			return inputDevice is Mouse;
		}
	}
}