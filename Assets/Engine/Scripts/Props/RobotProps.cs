using UnityEngine;
using Engine.Mechanics;

namespace Engine.Props
{
    public class RobotProps : MonoBehaviour
    {
        public PatrolPath PatrolPath;

        [Tooltip("The random hit damage effects")]
        public ParticleSystem[] RandomHitSparks;

        public ParticleSystem[] OnDetectVfx;
        public AudioClip OnDetectSfx;

        [Header("Sound")] public AudioClip MovementSound;
    }
}