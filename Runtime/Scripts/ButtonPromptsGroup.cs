using System.Collections.Generic;
using UnityEngine;

namespace KimelPK.DynamicInputPrompts {
    public class ButtonPromptsGroup : MonoBehaviour {
        
        [SerializeField] private List<ButtonSpritesInjector> buttonSpritesInjectors = new();

        [field: SerializeField] public bool ShowOnStart { get; set; }

        private void Start () {
            if (ShowOnStart)
                Show();
        }

        public void Show () {}

        public void Hide () {}
    }
}