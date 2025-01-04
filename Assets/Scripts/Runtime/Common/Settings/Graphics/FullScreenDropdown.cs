using System;
using TMPro;
using UnityEngine;

namespace NordicBibo.Runtime.Common.Settings.Graphics {
    public class FullScreenDropdown : MonoBehaviour {
        public TMP_Dropdown dropdown;

        public FullScreenMode Value => ToFullScreenMode(dropdown.value);

        private FullScreenMode ToFullScreenMode(int index) {
            return index switch {
                0 => FullScreenMode.FullScreenWindow,
                1 => FullScreenMode.MaximizedWindow,
                2 => FullScreenMode.Windowed,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}