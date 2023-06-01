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
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer != 6) return;
            if (_birdOnRock) return;

            _birdOnRock = true;
            _currentTween?.Kill();
            _currentTween = transform.DOMove(transform.position - new Vector3(0, heightToMove, 0), moveDuration);
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.layer != 6) return;
            if (!_birdOnRock) return;

            _birdOnRock = false;
            _currentTween?.Kill();
            _currentTween = transform.DOMove(transform.position + new Vector3(0, heightToMove, 0), moveDuration);
        }
    }
}
