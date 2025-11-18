using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TownWithoutLight.Systems;

namespace TownWithoutLight.Interaction
{
    /// <summary>
    /// Simple hinge-style door that opens and closes based on interaction and optional progression requirements.
    /// </summary>
    public class DoorController : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform doorPivot;
        [SerializeField] private Vector3 closedEuler;
        [SerializeField] private Vector3 openEuler = new Vector3(0f, 90f, 0f);
        [SerializeField] private float openSpeed = 4f;
        [SerializeField] private string requiredFlag;
        [SerializeField] private int requiredClues;
        [SerializeField] private UnityEvent onOpened;
        [SerializeField] private UnityEvent onClosed;

        private Coroutine _animationRoutine;
        private bool _isOpen;

        public string GetDescription() => _isOpen ? "ドアを閉める" : "ドアを開ける";

        private void Start()
        {
            if (doorPivot != null)
            {
                doorPivot.localRotation = Quaternion.Euler(_isOpen ? openEuler : closedEuler);
            }
        }

        public bool CanInteract
        {
            get
            {
                if (doorPivot == null)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(requiredFlag) && (FlagManager.Instance == null || !FlagManager.Instance.HasFlag(requiredFlag)))
                {
                    return false;
                }

                if (requiredClues > 0 && (ClueManager.Instance == null || ClueManager.Instance.ClueCount < requiredClues))
                {
                    return false;
                }

                return true;
            }
        }

        public void Interact(Interactor interactor)
        {
            if (!CanInteract)
            {
                return;
            }

            SetOpen(!_isOpen);
        }

        public void SetOpen(bool open)
        {
            if (doorPivot == null || _isOpen == open)
            {
                return;
            }

            _isOpen = open;
            if (_animationRoutine != null)
            {
                StopCoroutine(_animationRoutine);
            }

            _animationRoutine = StartCoroutine(AnimateDoor(open));
        }

        private IEnumerator AnimateDoor(bool opening)
        {
            Quaternion start = doorPivot.localRotation;
            Quaternion target = Quaternion.Euler(opening ? openEuler : closedEuler);
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * openSpeed;
                doorPivot.localRotation = Quaternion.Slerp(start, target, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }

            doorPivot.localRotation = target;

            if (opening)
            {
                onOpened?.Invoke();
            }
            else
            {
                onClosed?.Invoke();
            }
        }
    }
}
