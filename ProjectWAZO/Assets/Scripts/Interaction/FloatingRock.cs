using _3C;
using DG.Tweening;
using UnityEngine;

namespace Interaction
{
    public class FloatingRock : MonoBehaviour
    {
        [SerializeField] private float heightToMove;
        [SerializeField] private float moveDuration;
        
        private bool _birdOnRock;
        private Tweener _currentTween;

        private void Awake()
        {
            _inputAction = new PlayerControls();
            _inputAction.Player.Jump.performed += ctx => JumpOut();
        }
        
        private PlayerControls _inputAction;
        private void OnEnable()
        {
            _inputAction.Player.Enable();
        }
        private void OnDisable()
        {
            _inputAction.Player.Disable();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer != 6) return;
            if (_birdOnRock) return;

            _birdOnRock = true;
            Controller.instance.transform.parent = transform;
            _currentTween?.Kill();
            _currentTween = transform.DOMove(transform.position - new Vector3(0, heightToMove, 0), moveDuration);
        }

        private void JumpOut()
        {
            if (!_birdOnRock) return;

            _birdOnRock = false;
            Controller.instance.transform.parent = null;
            _currentTween?.Kill();
            _currentTween = transform.DOMove(transform.position + new Vector3(0, heightToMove, 0), moveDuration);
        }
    }
}
