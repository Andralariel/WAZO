using UnityEngine;

namespace Sound
{
    public class StepsSoundVariator : MonoBehaviour
    {
        [SerializeField] private AudioSource stepSource;
        [SerializeField] private AudioClip stepSound;

        [SerializeField] private float pitchLowerBound;
        [SerializeField] private float pitchUpperBound;
        [SerializeField] private int incrementAmount; //amount of increment between lower and upper bound
        [SerializeField] private bool useFiniteIncrement;

        public void PlayAStep()
        {
            stepSource.PlayOneShot(stepSound,RandomPitch());
        }

        private float RandomPitch()
        {
            if (useFiniteIncrement)
            {
                var increment = (pitchUpperBound - pitchLowerBound) / incrementAmount;
                return pitchLowerBound + increment*Random.Range(0, incrementAmount);
            }
            else
            {
                return Random.Range(pitchLowerBound, pitchUpperBound);
            }
        }
    }
}
