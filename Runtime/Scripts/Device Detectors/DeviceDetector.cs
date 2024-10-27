using UnityEngine;
using UnityEngine.InputSystem;

namespace KimelPK.DynamicInputPrompts {
	
	public abstract class DeviceDetector : ScriptableObject {

		public abstract bool DetectDevice (InputDevice inputDevice);
	}
}