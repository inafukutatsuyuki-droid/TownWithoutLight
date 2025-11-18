using UnityEngine;

namespace TownWithoutLight.Puzzles
{
    /// <summary>
    /// Solves when a sufficient amount of physics weight rests on the trigger volume.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class PressurePlatePuzzle : PuzzleBase
    {
        [SerializeField] private float requiredWeight = 5f;

        private float _currentWeight;

        private void Reset()
        {
            var collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            ModifyWeight(other, add: true);
        }

        private void OnTriggerExit(Collider other)
        {
            ModifyWeight(other, add: false);
        }

        private void ModifyWeight(Collider other, bool add)
        {
            if (IsSolved)
            {
                return;
            }

            float weight = 1f;
            if (other.attachedRigidbody != null)
            {
                weight = Mathf.Max(1f, other.attachedRigidbody.mass);
            }

            _currentWeight += add ? weight : -weight;
            _currentWeight = Mathf.Max(0f, _currentWeight);

            if (_currentWeight >= requiredWeight)
            {
                SolvePuzzle();
            }
        }
    }
}
