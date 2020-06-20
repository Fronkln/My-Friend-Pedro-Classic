using UnityEngine;

namespace MFPClassic
{
    public class VictorSpatula : MonoBehaviour
    {
        public bool swinging = false;

        private bool recentlyHit = false;
        private float lastTimeSinceHit = 0;


        public void Update()
        {
            if (recentlyHit)
            {
                lastTimeSinceHit += Time.deltaTime;

                if (lastTimeSinceHit >= 1)
                {
                    recentlyHit = false;
                    lastTimeSinceHit = 0;
                }
            }
        }

        public void OnCollisionEnter(Collision other)
        {
            if (swinging && !recentlyHit)
            {
                if (other.transform.tag == "Player")
                {
                    recentlyHit = true;
                    PlayerScript player = PlayerScript.PlayerInstance;

                    Vector3 direction = other.contacts[0].point - player.transform.position;
                    direction = -direction;

                    player.xSpeed += direction.x * 10;
                    player.ySpeed -= direction.y * 10;

                    player.bulletHit = true;
                    player.bulletStrength = 0.7f;
                    player.bulletHitVel = Vector3.zero;
                    player.bulletHitRotation = transform.rotation;
                    player.bulletHitDoSound = true;
                }
            }
        }
    }
}

