using NordicBibo.Runtime.Gameplay.Cards;
using UnityEngine;

namespace NordicBibo.Runtime.Gameplay.Utility {
    public class PivotShifter : MonoBehaviour {
        public PivotMaster pivotMaster;
        public CardStack cardStack;

        public void EnableCollisions() {
            foreach (GameObject pivot in pivotMaster.Pivots) {
                
            }
        }

        public void ShiftStack(GameObject fromPivot, GameObject toPivot) {
            
        }
    }
}