using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MFPClassic
{
    public class MiniVictor : MonoBehaviour
    {
        private PlayerScript thePlayer;
        private Vector3 startScale;

        private RootScript root;

        public void Awake()
        {
            thePlayer = MFPClassicAssets.player;
            startScale = transform.localScale;
            root = MFPClassicAssets.root;
        }

        public void Update()
        {
            if (!root.dead)
                if (!root.paused)
                {
                    float speed = 0;

                    switch(root.difficultyMode)
                    {
                        case 0:
                            speed = 0.33f;
                            break;
                        case 1:
                            speed = 0.55f;
                            break;
                        case 2:
                            speed = 0.66f;
                            break;
                    }

                    transform.position = Vector2.MoveTowards(transform.position, thePlayer.transform.position, speed);
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
                if(other.tag == "Bullet")
                {
                    GameObject.Find("Main Camera/BloodDropsParticle").GetComponent<ParticleSystem>().Emit(MFPClassicAssets.root.generateEmitParams(transform.position, new Vector3((float)(-(double)this.transform.forward.x * 3.5) + (float)UnityEngine.Random.Range(-4, 4), (float)(-(double)this.transform.forward.y * 3.5) + (float)UnityEngine.Random.Range(1, 6), UnityEngine.Random.Range(-0.5f, 0.5f)), UnityEngine.Random.Range(0.3f, 0.6f), UnityEngine.Random.Range(0.8f, 1.3f), !MFPClassicAssets.root.doGore ? new Color(0.0f, 0.0f, 0.0f, 1f) : new Color(1f, 1f, 1f, 1f)), 1);

                    other.gameObject.SetActive(false);

                    if (transform.localScale == startScale)
                        transform.localScale = startScale / 2;
                    else
                    {
                        GameObject.FindObjectOfType<Victor>().victorHeads.Remove(transform);


                        bool dropWeapon = UnityEngine.Random.Range(0, 4) == 1;

                        if (dropWeapon)
                        {
                            GameObject droppedWeapon = Instantiate(MFPClassicAssets.WeaponPickerSample);
                            droppedWeapon.layer = 22;

                            if (droppedWeapon.transform.parent != null)
                            {
                                Transform parent = droppedWeapon.transform.parent;
                                droppedWeapon.transform.parent = null;

                                Destroy(parent.gameObject);
                            }

                            droppedWeapon.transform.position = transform.position;

                            WeaponPickupScript weaponPickupScript = droppedWeapon.GetComponent<WeaponPickupScript>();

                            int rndWeapon = UnityEngine.Random.Range(3, 7);

                            weaponPickupScript.weapon = rndWeapon;

                            weaponPickupScript.enabled = true;
                            weaponPickupScript.doSetup();

                            droppedWeapon.AddComponent<SkyfallObjectScript>().skyfallYPos = transform.position.y;


                            Destroy(gameObject);
                        }
                    }
                }
            }
        }

    }
}
