using UnityEngine;

namespace KnifeFest
{
    public class WallCutscene : MonoBehaviour
    {
        [SerializeField] private int _multiplierWeight = 15;

        public StepCutscene ParentStep { get; private set; }
        public int MultiplierWeight => _multiplierWeight;
        public bool IsEndWall { get; private set; } = false;

        private void Start()
        {
            ParentStep = GetComponentInParent<StepCutscene>();
        }

        public void SetEndWall()
        {
            IsEndWall = true;
        }
    }
}
