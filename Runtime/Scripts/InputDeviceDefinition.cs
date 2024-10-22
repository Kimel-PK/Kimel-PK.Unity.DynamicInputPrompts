using TMPro;
using UnityEngine;

namespace KimelPK.DynamicInputPrompts {
	[CreateAssetMenu (fileName = "InputDeviceDefinition", menuName = "KimelPK/DynamicInputPrompts/InputDeviceDefinition")]
	public class InputDeviceDefinition : ScriptableObject {
		[field: SerializeField] public TMP_SpriteAsset ButtonSpritesAsset { get; private set; }
	}
}