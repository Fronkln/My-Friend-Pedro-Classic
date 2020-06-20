
using System.Collections;
using UnityEngine;

namespace MFPClassic
{
    public class MiniVictor : MonoBehaviour
    {
        private PlayerScript thePlayer;
        private Vector3 startScale;

        private RootScript root;
        private Collider collider;

        private bool wasShot = false;

        IEnumerator ScaleUp()
        {
            collider.enabled = false;

            while (transform.localScale.y < startScale.y)
            {

                if (root.paused || root.dead)
                    yield return null;

                float incrementAmount = Time.deltaTime * 12;

                transform.localScale += new Vector3(incrementAmount, incrementAmount, incrementAmount);
                yield return null;
            }

            transform.localScale = startScale;
            collider.enabled = true;
        }

        public void Awake()
        {
            thePlayer = MFPClassicAssets.player;
            startScale = transform.localScale;
            root = MFPClassicAssets.root;
            collider = GetComponent<Collider>();

            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            StartCoroutine(ScaleUp());
        }

        public void Update()
        {
            if (!root.dead)
                if (!root.paused)
                {
                    float speed = 0;

                    switch (root.difficultyMode)
                    {
                        case 0:
                            speed = 0.33f;
                            break;
                        case 1:
                            speed = 0.44f;
                            break;
                        case 2:
                            goto case 1;
                    }

                    transform.position = Vector2.MoveTowards(transform.position, thePlayer.transform.position, (speed * root.timescale));
                }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerScript>())
            {
                thePlayer.bulletHit = true;
                thePlayer.bulletStrength = 0.5f;
                thePlayer.bulletHitVel = GetComponent<Rigidbody>().velocity;
                thePlayer.bulletHitRotation = transform.rotation;
                thePlayer.bulletHitDoSound = true;

                Destroy(gameObject);
            }
            else
            {
                if (other.tag == "Bullet")
                {
                    GameObject.Find("Main Camera/BloodDropsParticle").GetComponent<ParticleSystem>().Emit(MFPClassicAssets.root.generateEmitParams(transform.position, new Vector3((float)(-(double)this.transform.forward.x * 3.5) + (float)UnityEngine.Random.Range(-4, 4), (float)(-(double)this.transform.forward.y * 3.5) + (float)UnityEngine.Random.Range(1, 6), UnityEngine.Random.Range(-0.5f, 0.5f)), UnityEngine.Random.Range(0.3f, 0.6f), UnityEngine.Random.Range(0.8f, 1.3f), !MFPClassicAssets.root.doGore ? new Color(0.0f, 0.0f, 0.0f, 1f) : new Color(1f, 1f, 1f, 1f)), 1);

                    if (!wasShot)
                    {
                        transform.localScale = startScale / 2;
                        wasShot = true;
                    }
                    else
                    {
                        bool dropWeapon = UnityEngine.Random.Range(0, 2) == 1;

                        if (dropWeapon)
                        {

                            int rndWeapon = UnityEngine.Random.Range(1, 7);

                            switch(rndWeapon)
                            {
                                case 2: rndWeapon = 1; break;
                                case 4: rndWeapon = 3; break;
                            }

                            PlayerScript.PlayerInstance.pickedUpWeapon(rndWeapon);

                        }
                        Destroy(gameObject);
                    }

                    other.gameObject.SetActive(false);
                }
            }
        }

    }
}
