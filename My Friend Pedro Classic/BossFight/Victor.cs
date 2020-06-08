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

        public float targetJawIntensity = 0;
        private float currentJawIntensity = 0;

        public bool jawMoving = false;

        public Animator victorAnimator;
        public AudioSource audioSource;

        public float attackDelay = 0;


        public bool lookAt = false;
        public bool attacking = false;
        public bool canAttack = false;

        public bool waiting = false;

        public int nrOfAttacks = 0;

        [HideInInspector] public List<Transform> victorHeads = new List<Transform>();

        #region Checkpoint Variables
        private float currentJawIntensityS = 0;
        private float targetJawIntensityS = 0;
        private bool jawMovingS = false;
        private bool lookAtS = false;
        private bool attackingS = false;
        private float attackDelayS = 0;
        private int nrOfAttackS = 0;
        private bool canAttackS = false;
        private bool waitingS = false;

        private bool forceJawInstant = false;
        
        private List<Transform> victorHeadsS = new List<Transform>();
        #endregion



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


        public void saveState()
        {
            currentJawIntensityS = currentJawIntensity;
            targetJawIntensityS = targetJawIntensity;
            jawMovingS = jawMoving;
            lookAtS = lookAt;
            attackingS = attacking;
            attackDelayS = attackDelay;
            nrOfAttackS = nrOfAttacks;
            canAttackS = canAttack;
            waitingS = waiting;
            victorHeadsS = victorHeads.ToList();
        }

        public void loadState()
        {
            forceJawInstant = true;

            foreach (Transform miniVic in victorHeads)
            {
                if (!victorHeadsS.Contains(miniVic))
                    Destroy(miniVic);
                else
                    MFPEditorUtils.Log("miniVic is in victorHeadsS array");
            }

            MFPEditorUtils.Log(currentJawIntensity + " " + currentJawIntensityS);


        //    SetJaw(currentJawIntensity - currentJawIntensityS, true);

            currentJawIntensity = currentJawIntensityS;
            targetJawIntensity = targetJawIntensityS;
            jawMoving = jawMovingS;
            lookAt = lookAtS;
            attacking = attackingS;
            attackDelay = attackDelayS;
            nrOfAttacks = nrOfAttackS;
            canAttack = canAttackS;
            waiting = waitingS;
            victorHeads = victorHeadsS.ToList();

            //Move(targetJawIntensity - currentJawIntensity);

        }

        public void LateUpdate()
        {

            if (root.doCheckpointSave)
            {
                MFPEditorUtils.Log("SAVE CALLED");
                saveState();
            }
            if (root.doCheckpointLoad)
                loadState();

        }

        public void NewAttack(VictorAttacks newAttack)
        {
            currentAttack = newAttack;

            switch (currentAttack)
            {
                case VictorAttacks.BulletMouth:
                    //targetJawIntensity = 5;
                    SetJaw(5f, true);
                    

                    switch (root.difficultyMode)
                    {
                        case 0:
                            nrOfAttacks = UnityEngine.Random.Range(6, 11);
                            break;
                        case 1:
                            goto case 0;
                        case 2:
                            nrOfAttacks = UnityEngine.Random.Range(8, 13);
                            break;
                    }
                    break;

                case VictorAttacks.LittleHeads:
                    //targetJawIntensity = 5;
                    SetJaw(5f, true);

                    switch (root.difficultyMode)
                    {
                        case 0:
                            nrOfAttacks = UnityEngine.Random.Range(4, 7);
                            break;
                        case 1:
                            nrOfAttacks = UnityEngine.Random.Range(6, 8);
                            break;
                        case 2:
                            nrOfAttacks = UnityEngine.Random.Range(7, 10);
                            break;
                    }
                    break;
            }

            attacking = true;

            waiting = false;
            canAttack = false;

        }

        public void AttackBehaviour()
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
                SetJaw(-currentJawIntensity);

            if (!forceJawInstant)
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

            float diff = head.transform.position.y - headLerpPoint.transform.position.y;

            if (diff < 0.05f && diff > -0.05f)
                jawMoving = false;
            else
                jawMoving = true;

            

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
                */
            

            if (lookAt)
            {
                Vector3 dir = (thePlayer.transform.position + new Vector3(60, 10, -25f)) - transform.position;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                headPivot.transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);


                debugPos = dir;
            }

        }

        public void FixedUpdate()
        {
            if (attacking)
                attackDelay -= Time.deltaTime;
            else
                attackDelay = 0;

            AttackBehaviour();
        }


        public void SetJaw(float yPos, bool checkEquals = false)
        {
            if (checkEquals)
                if(yPos != 0)
                if (currentJawIntensity == yPos)
                    return;

            lookAt = false;
            headPivot.transform.rotation = defaultHeadRotation;

            if (yPos != 0)
                headLerpPoint.transform.position += new Vector3(0, yPos, 0);
            else
                headLerpPoint.transform.localPosition = lerpDefaultPos;

            currentJawIntensity += yPos;

           // if (yPos == 0)
           //  headLerpPoint.transform.position = lerpDefaultPos;
           // else
           // headLerpPoint.transform.position = new Vector3(headLerpPoint.transform.position.x, yPos, headLerpPoint.transform.position.z);
        }

        /*
        public IEnumerator DoAttack()
        {
            if (attacking)
                yield break;

            attacking = true;

            switch (currentAttack)
            {
                #region Bullet Mouth
                case VictorAttacks.BulletMouth:
                    targetJawIntensity = 0;
                    while (jawMoving)
                    {
                        yield return null;
                    }
                    MFPEditorUtils.Log("My jaw is open, time to attack!");
                    lookAt = true;

                    int nrOfAttacks = UnityEngine.Random.Range(8, 14);


                    float shootDelay = (thePlayer.dodgingCoolDown <= 0 ? UnityEngine.Random.Range(1, 2) : thePlayer.dodgingCoolDown / 30);

                    yield return new WaitForSeconds(shootDelay);

                    while (nrOfAttacks != 0)
                    {
                        root.getBullet(shootNode.transform.position, Quaternion.LookRotation(shootNode.transform.forward));

                        BulletScript bulletScript = this.root.getBulletScript();
                        bulletScript.bulletStrength = 0.35f;
                        bulletScript.bulletSpeed = 10f;
                        bulletScript.friendly = false;
                        bulletScript.doPostSetup();

                        nrOfAttacks--;

                        yield return new WaitForSeconds(0.1f);
                    }

                    break;
                #endregion
                #region Little Heads
                case VictorAttacks.LittleHeads:

                    StartCoroutine(MoveJawSmooth(5));
                    while (jawMoving)
                    {
                        yield return null;
                    }
                    MFPEditorUtils.Log("My jaw is open, time to attack!");
                    lookAt = true;

                    yield return new WaitForSeconds(2);

                    int nrofHeads = UnityEngine.Random.Range(3, 7);

                    while (nrofHeads != 0)
                    {
                        GameObject miniVic = Instantiate(MFPClassicAssets.miniVictor);
                        miniVic.transform.position = new Vector3(shootNode.transform.position.x, shootNode.transform.position.y, miniVic.transform.position.z);

                        victorHeads.Add(miniVic.transform);

                        nrofHeads--;

                        yield return new WaitForSeconds(0.5f);
                    }
                    break;
                    #endregion
            }

            attacking = false;
            yield return null;
        }
        */

        /*
        public IEnumerator MoveJawSmooth(float target)
        {

            MFPEditorUtils.Log("TICK");

            if (currentJawIntensity == target)
                yield break;

            bool wasAtLookAt = lookAt;
            lookAt = false;

            headPivot.transform.rotation = defaultHeadRotation;

            float moveAmount = (target <= 0.15f ? 0.1f : target / 24);
            jawMoving = true;


            if (target > currentJawIntensity)
            {
                while (currentJawIntensity < target)
                {
                    if (!root.dead)
                        if (!root.paused)
                        {

                            Move(moveAmount);
                            //  Move(0.1f);

                            //  currentJawIntensity += Time.deltaTime;
                            currentJawIntensity += moveAmount;
                        }
                    yield return null;
                }

                currentJawIntensity = target;
            }
            else
            {
                while (currentJawIntensity > target)
                {
                    if (!root.dead)
                        if (!root.paused)
                        {
                            Move(-moveAmount);
                            // Move(-0.1f);

                            //   currentJawIntensity -= Time.deltaTime;
                            currentJawIntensity -= moveAmount;
                        }
                    yield return null;
                }

                currentJawIntensity = target;
            }

            if (wasAtLookAt)
                lookAt = true;

            jawMoving = false;

            yield return null;
        }
        */

            /*
        public void Move(float moveIntensity)
        {
            jaw.localScale += new Vector3(0, moveIntensity / 6, 0);
            head.transform.position += new Vector3(0, moveIntensity, 0);

            currentJawIntensity += moveIntensity;

        }
        */

    }
}