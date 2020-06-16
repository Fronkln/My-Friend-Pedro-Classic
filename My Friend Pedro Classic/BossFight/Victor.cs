using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MFPClassic
{

    public class Victor : MonoBehaviour
    {
        public enum VictorAttacks
        {
            LittleHeads,
            BulletMouth,
            Spatula,
            Shotgun
        }

        public VictorAttacks currentAttack = VictorAttacks.LittleHeads;

        private RootScript root;

        [HideInInspector] public Transform headPivot;
        [HideInInspector] public Transform head;
        [HideInInspector] public Transform headLerpPoint;
        [HideInInspector] public Transform jaw;
        [HideInInspector] public PlayerScript thePlayer;

        [HideInInspector] public Transform shootNode;

        private Quaternion defaultHeadRotation;
        public Vector3 debugPos;

        private Vector3 lerpDefaultPos;

        private float currentJawIntensity = 1.128f;

        public float jawSpeed = 0.03f;

        public readonly float jawMin = 1.128f;
        public readonly float jawMax = 1.994f;

        public bool jawMoving = false;

        public Animator victorAnimator;
        public AudioSource audioSource;

        public float attackDelay = 0;


        public bool lookAt = false;
        public bool attacking = false;
        public bool canAttack = false;

        public bool waiting = false;

        [HideInInspector] public List<Transform> victorHeads = new List<Transform>();



        public void Start()
        {
            root = MFPClassicAssets.root;

            thePlayer = MFPClassicAssets.player;
            headPivot = transform.Find("body/headPivot");
            head = headPivot.Find("head");
            headLerpPoint = headPivot.Find("jawLerp");
            jaw = headPivot.Find("jaw");
            shootNode = head.Find("shootNode");



            defaultHeadRotation = headPivot.rotation;
            lerpDefaultPos = headLerpPoint.transform.localPosition;

            victorAnimator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }


        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(debugPos, 0.4f);
        }


        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
                StartCoroutine(MoveJaw(1.7f));

            if (Input.GetKeyDown(KeyCode.N))
                StartCoroutine(DoAttack(VictorAttacks.LittleHeads));


            if (lookAt)
            {
                Vector3 dir = (thePlayer.transform.position + new Vector3(60, 10, -25f)) - transform.position;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                headPivot.transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);


                debugPos = dir;
            }
        }


        public IEnumerator DoAttack(VictorAttacks attackType)
        {
            if (attacking)
                yield break;

            attacking = true;
            currentAttack = attackType;



            switch (attackType)
            {
                case VictorAttacks.LittleHeads:
                    lookAt = true;
                    StartCoroutine(MoveJaw(2));

                    while (jawMoving)
                        yield return null;

                    MFPEditorUtils.Log("My jaw is open, time to attack!");

                    int nrOfHeads = 0;

                    switch (root.difficultyMode)
                    {
                        case 0: nrOfHeads = Random.Range(4, 7); break;
                        case 1: nrOfHeads = Random.Range(5, 9); break;
                        case 2: nrOfHeads = Random.Range(6, 13); break;
                    }


                    //TODO: Maybe pool them?
                    while(nrOfHeads != 0)
                    {
                        GameObject miniVic = Instantiate(MFPClassicAssets.miniVictor);
                        miniVic.transform.position = new Vector3(shootNode.transform.position.x, shootNode.transform.position.y, miniVic.transform.position.z);

                        nrOfHeads--;

                        yield return new WaitForSeconds(0.5f);
                    }

                    break;
            }

            yield return null;
        }

        public IEnumerator MoveJaw(float target)
        {

            float targetClamped = target;

            if (targetClamped < jawMin)
                targetClamped = jawMin;
            if (targetClamped > jawMax)
                targetClamped = jawMax;

            if (targetClamped == currentJawIntensity)
            {
                MFPEditorUtils.Log("No.");
                yield break;
            }

            jawMoving = true;


            MFPEditorUtils.Log(targetClamped.ToString());

            if (targetClamped > currentJawIntensity)
            {
                while (currentJawIntensity < targetClamped)
                {
                    float yAmount = Mathf.Clamp(head.transform.localPosition.y + jawSpeed, jawMin, jawMax);
                    head.transform.localPosition = new Vector3(head.transform.localPosition.x, yAmount, head.transform.localPosition.z);
                    currentJawIntensity = yAmount;

                    yield return null;
                }
            }
            else
            {

                while (currentJawIntensity > targetClamped)
                {
                    float yAmount = Mathf.Clamp(head.transform.localPosition.y - jawSpeed, jawMin, jawMax);
                    head.transform.localPosition = new Vector3(head.transform.localPosition.x, yAmount, head.transform.localPosition.z);
                    currentJawIntensity = yAmount;

                    yield return null;
                }
            }

            head.transform.localPosition = new Vector3(head.transform.localPosition.x, targetClamped, head.transform.localPosition.z);
            currentJawIntensity = targetClamped;

            jawMoving = false;
        }




        /*    public void AttackBehaviour()
             {
                 if (!attacking || jawMoving)
                     return;

                 switch (currentAttack)
                 {
                     #region Bullet Mouth
                     case VictorAttacks.BulletMouth:
                         lookAt = true;
                         shootNode.transform.LookAt(thePlayer.transform.position);

                         if (!canAttack && !waiting)
                         {
                             attackDelay = (thePlayer.dodgingCoolDown <= 0 ? UnityEngine.Random.Range(0.5f, 1.7f) : thePlayer.dodgingCoolDown / 30);
                             waiting = true;
                             break;
                         }

                         if (waiting && !canAttack)
                             if (attackDelay <= 0)
                             {
                                 canAttack = true;
                                 waiting = false;
                             }

                         if (canAttack)
                             if (attackDelay <= 0)
                             {
                                 if (nrOfAttacks != 0)
                                 {
                                     root.getBullet(shootNode.transform.position, Quaternion.LookRotation(shootNode.transform.forward));

                                     BulletScript bulletScript = this.root.getBulletScript();
                                     bulletScript.bulletStrength = 0.35f;
                                     bulletScript.bulletSpeed = 10f;
                                     bulletScript.friendly = false;
                                     bulletScript.doPostSetup();

                                     nrOfAttacks--;

                                     switch (root.difficultyMode)
                                     {
                                         case 0:
                                             attackDelay = 0.2f;
                                             break;
                                         case 1:
                                             goto case 0;
                                         case 2:
                                             attackDelay = 0.1f;
                                             break;
                                     }
                                     break;
                                 }
                                 else
                                 {
                                     attacking = false;
                                     lookAt = false;
                                     SetJaw(-currentJawIntensity);
                                     break;
                                 }
                             }

                         break;
                     #endregion
                     #region Little Heads
                     case VictorAttacks.LittleHeads:
                         lookAt = true;
                         shootNode.transform.LookAt(thePlayer.transform.position);

                         if (!canAttack && !waiting)
                         {
                             attackDelay = 2;
                             waiting = true;
                             break;
                         }

                         if (waiting && !canAttack)
                             if (attackDelay <= 0)
                             {
                                 canAttack = true;
                                 waiting = false;
                             }

                         if (canAttack)
                         {
                             if (attackDelay <= 0)
                             {
                                 if (nrOfAttacks != 0)
                                 {
                                     GameObject miniVic = Instantiate(MFPClassicAssets.miniVictor);
                                     miniVic.transform.position = new Vector3(shootNode.transform.position.x, shootNode.transform.position.y, miniVic.transform.position.z);

                                     victorHeads.Add(miniVic.transform);

                                     attackDelay = 0.5f;
                                     nrOfAttacks--;
                                 }
                                 else
                                 {
                                     lookAt = false;
                                     attacking = false;
                                     SetJaw(-currentJawIntensity);
                                     break;
                                 }
                             }
                         }


                         break;
                         #endregion

                 }
             }

             public void Update()
             {

                 if (Input.GetKeyDown(KeyCode.M))
                     NewAttack(VictorAttacks.LittleHeads);
                 if (Input.GetKeyDown(KeyCode.V))
                     StartCoroutine(MoveJawSmooth(0));

                 /*  if (!forceJawInstant)
                       head.transform.position = new Vector3(head.transform.position.x, Vector2.Lerp(head.transform.position, headLerpPoint.transform.position, 0.05f).y, head.transform.position.z);
                   else
                   {
                       bool wasLookat = lookAt;

                       forceJawInstant = false;
                       lookAt = false;
                       head.transform.position = new Vector3(head.transform.position.x, headLerpPoint.transform.position.y, head.transform.position.z);

                       if(wasLookat)
                           lookAt = true;
                   }
                   */


        /*
        if (currentJawIntensity != targetJawIntensity)
        {
            float diff = targetJawIntensity - currentJawIntensity;

            if (diff > -0.15f && diff < 0.15f)
            {
                jawMoving = false;
                Move(diff);
            }
            else
                jawMoving = true;

            float moveAmount = (targetJawIntensity <= 0.15f ? 0.05f : 0.1f); /*(targetJawIntensity <= 0.15f ? 0.1f : targetJawIntensity / 24);



            if (jawMoving)
            {
                headPivot.transform.rotation = defaultHeadRotation;

                if (currentJawIntensity < targetJawIntensity)
                    Move(moveAmount);

                if (currentJawIntensity > targetJawIntensity)
                    Move(-moveAmount);
            }
            MFPEditorUtils.Log(currentJawIntensity.ToString() + " " + targetJawIntensity.ToString() + " " + moveAmount.ToString() + " " + diff.ToString());

        }
        else
            jawMoving = false;



        if (lookAt)
        {
            Vector3 dir = (thePlayer.transform.position + new Vector3(60, 10, -25f)) - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            headPivot.transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);


            debugPos = dir;
        }

    }
    */


        /*     public void FixedUpdate()
             {
                 if (attacking)
                     attackDelay -= Time.deltaTime;
                 else
                     attackDelay = 0;

             }
             */


    }
}