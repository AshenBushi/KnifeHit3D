using UnityEngine;

namespace KnifeFest
{
    public class WallCutscene : MonoBehaviour
    {
        [SerializeField] private int _multiplierWeight = 15;

        public int MultiplierWeight => _multiplierWeight;
        public bool IsEndWall { get; private set; } = false;

        public void SetEndWall()
        {
            IsEndWall = true;
        }
    }
}
