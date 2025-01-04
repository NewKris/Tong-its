using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace NordicBibo.Runtime.Common.Settings.Graphics {
    public class ResolutionDropdown : MonoBehaviour {
        public ResolutionSetting[] settings;
        public TMP_Dropdown dropdown;

        public Vector2Int Value => settings[dropdown.value].Dimensions;

        private void OnEnable() {
            dropdown.ClearOptions();
            dropdown.AddOptions(settings.Select(x => x.DisplayText()).ToList());
        }
    }
}