using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MFPClassic
{

    public class Victor : MonoBehaviour
    {
        public enum VictorAttacks
        {
            None, //used for first move

            LittleHeads,
            BulletMouth,
            Spatula,
            Shotgun //could be cool, maybe add it idk
        }


        private bool debugAuto = true;

        public VictorAttacks currentAttack = VictorAttacks.None;

        public float health = 1;

        private RootScript root;
        private PlayerScript player;

        [HideInInspector] public Transform headPivot;
        [HideInInspector] public Transform head;
        [HideInInspector] public Transform headLerpPoint;
        [HideInInspector] public Transform jaw;
        [HideInInspector] public Transform arm;

        [HideInInspector] public Transform shootNode;

        private Quaternion defaultHeadRotation;
        public Vector3 debugPos;

        private Vector3 lerpDefaultPos;
        private Transform spatulaArmStartPoint;
        private Vector3 spatulaArmEndPoint;

        private float currentJawIntensity = 1.128f;

        public float jawSpeed { get { return 0.03f * root.timescale; } }

        public readonly float jawMin = 1.128f;
        public readonly float jawMax = 1.994f;

        public float swingSpeed { get { return 1.2f * root.timescale; } }

        public bool jawMoving = false;

        public Animator victorAnimator;
        public AudioSource audioSource;

        public bool lookAt = false;
        public bool attacking = false;

        private int burstAmount = 0; //for repeating stuff like shooting
        private bool bursting = false;


        private bool cancelJawDoOnce = false;

        public Image healthBar;

        private VictorSpatula spatula;

        public void Start()
        {
            //eğer boss aniden çalışmamaya başlarsa burdaki koda ilk göz at, belki isim değiştirmişsindir.

            root = MFPClassicAssets.root;

            player = MFPClassicAssets.player;
            headPivot = transform.Find("body/headPivot");
            head = headPivot.Find("head");
            headLerpPoint = headPivot.Find("jawLerp");
            jaw = headPivot.Find("jaw");
            shootNode = head.Find("shootNode");

            arm = transform.Find("body/armPivot");
            spatulaArmStartPoint = transform.Find("body/spatulaArmStartPoint");
            spatulaArmEndPoint = arm.localPosition;
            arm.transform.position = spatulaArmStartPoint.position;
            arm.gameObject.SetActive(false);


            spatula = transform.Find("body/armPivot/spatulaColl").gameObject.AddComponent<VictorSpatula>();


            defaultHeadRotation = headPivot.rotation;
            lerpDefaultPos = headLerpPoint.transform.localPosition;

            victorAnimator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            transform.Find("body").gameObject.AddComponent<VictorLimb>();
            arm.transform.Find("handColl").gameObject.AddComponent<VictorLimb>();
            arm.transform.Find("armColl").gameObject.AddComponent<VictorLimb>();
            head.transform.Find("headCollider").gameObject.AddComponent<VictorLimb>();

            healthBar = FinalBattleController.inst.bossfightUI.transform.Find("HealthOutline/Health").GetComponent<Image>();
        }


        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(debugPos, 0.4f);
        }


        public void Hurt(BulletScript bullet)
        {

            if (health <= 0)
                return;

            health -= (bullet.bulletStrength * 0.01f);
            healthBar.fillAmount = health;

            if (health <= 0)
                FinalBattleController.inst.OnVictorDeath();
        }


        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
                StartCoroutine(DoAttack(VictorAttacks.BulletMouth));

            if (Input.GetKeyDown(KeyCode.N))
                StartCoroutine(DoAttack(VictorAttacks.LittleHeads));

            if (Input.GetKeyDown(KeyCode.M))
                StartCoroutine(DoAttack(VictorAttacks.Spatula));

            if (debugAuto)
                if (!attacking && !bursting && FinalBattleController.inst.battleStarted)
                {
                    switch (currentAttack) //attack order
                    {
                        case VictorAttacks.None:
                            StartCoroutine(DoAttack(VictorAttacks.BulletMouth));
                            break;
                        case VictorAttacks.BulletMouth:
                            StartCoroutine(DoAttack(VictorAttacks.Spatula));
                            break;
                        case VictorAttacks.Spatula:
                            StartCoroutine(DoAttack(VictorAttacks.LittleHeads));
                            break;
                        case VictorAttacks.LittleHeads:
                            StartCoroutine(DoAttack(VictorAttacks.BulletMouth));
                            break;
                    }

                    MFPEditorUtils.Log("requesting new attack");
                }


            if (lookAt)
            {
                Vector3 dir = (player.transform.position + new Vector3(60, 10, -25f)) - transform.position;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                headPivot.transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);

                shootNode.transform.LookAt(player.transform);


                debugPos = dir;
            }
            else
                headPivot.transform.rotation = Quaternion.Lerp(headPivot.transform.rotation, defaultHeadRotation, 0.2f);
        }


        public IEnumerator DoAttack(VictorAttacks attackType)
        {
            if (attacking)
            {
                MFPEditorUtils.Log("already attacking");
                yield break;
            }

            attacking = true;
            currentAttack = attackType;
            switch (attackType)
            {


                case VictorAttacks.BulletMouth:


                    if (!bursting)
                    {
                        burstAmount = UnityEngine.Random.Range(1, 3);
                        bursting = true;
                    }

                    lookAt = true;
                    MoveJaw(2, true);

                    while (jawMoving)
                        yield return null;

                    yield return new WaitForSeconds((player.dodgingCoolDown <= 0 ? UnityEngine.Random.Range(0.5f, 1.7f) : player.dodgingCoolDown / 30));


                    int nrOfBullets = 0;

                    float attackDelay = 0;

                    switch (root.difficultyMode)
                    {
                        case 0:
                            nrOfBullets = UnityEngine.Random.Range(6, 11);
                            attackDelay = 0.2f;
                            break;
                        case 1:
                            goto case 0;
                        case 2:
                            nrOfBullets = UnityEngine.Random.Range(8, 13);
                            attackDelay = 0.1f;
                            break;
                    }

                    while (nrOfBullets != 0)
                    {
                        root.getBullet(shootNode.transform.position, Quaternion.LookRotation(shootNode.transform.forward));

                        BulletScript bulletScript = root.getBulletScript();
                        bulletScript.bulletStrength = 0.35f;
                        bulletScript.bulletSpeed = 10f;
                        bulletScript.friendly = false;
                        bulletScript.doPostSetup();

                        nrOfBullets--;

                        yield return new WaitForSeconds(attackDelay);
                    }


                    if (bursting && burstAmount == 0)
                        bursting = false;

                    if (bursting && burstAmount > 0)
                    {
                        attacking = false;
                        burstAmount--;
                        StartCoroutine(DoAttack(currentAttack));

                        yield break;
                    }


                    break;

                case VictorAttacks.LittleHeads:
                    lookAt = true;
                    MoveJaw(2);

                    while (jawMoving)
                        yield return null;

                    if (!bursting)
                    {
                        burstAmount = 1;
                        bursting = true;
                    }


                    yield return new WaitForSeconds(1.3f);
                    int nrOfHeads = 0;

                    switch (root.difficultyMode)
                    {
                        case 0: nrOfHeads = Random.Range(2, 5); break;
                        case 1: nrOfHeads = Random.Range(3, 6); break;
                        case 2: nrOfHeads = Random.Range(5, 7); break;
                    }


                    //TODO: Maybe pool them?
                    while (nrOfHeads != 0)
                    {
                        if (player.health <= 0)
                            yield break;

                        GameObject miniVic = Instantiate(MFPClassicAssets.miniVictor);
                        miniVic.transform.position = new Vector3(shootNode.transform.position.x, shootNode.transform.position.y, miniVic.transform.position.z);

                        nrOfHeads--;

                        yield return new WaitForSeconds(0.5f);
                    }


                    if (bursting && burstAmount > 0)
                    {
                        attacking = false;
                        burstAmount--;
                        yield return new WaitForSeconds(1);
                        StartCoroutine(DoAttack(currentAttack));

                        yield break;
                    }


                    if (bursting && burstAmount == 0) //some shitty bug keeps LittleHeads from iterating properly
                    {
                        attacking = false;
                        lookAt = false;
                        bursting = false;

                        yield break;
                    }


                    break;

                case VictorAttacks.Spatula:

                    arm.gameObject.SetActive(true);


                    Quaternion armStartRot = arm.rotation;


                    float aimTime = 0;
                    float curTime = 0;

                    switch (root.difficultyMode)
                    {
                        case 0: aimTime = Random.Range(1.4f, 2.5f); break;
                        case 1: aimTime = Random.Range(1.4f, 2.3f); break;
                        case 2: aimTime = Random.Range(1.1f, 2f); break;
                    }

                    while (curTime < aimTime)
                    {
                        if (root.paused || root.dead)
                            yield return null;

                        Vector3 dir2 = (player.transform.position + new Vector3(60, 100, -25f)) - transform.position;

                        float angle2 = Mathf.Atan2(dir2.y, dir2.x) * Mathf.Rad2Deg;


                        arm.transform.localPosition = Vector3.Lerp(arm.transform.localPosition, spatulaArmEndPoint, 0.03f);
                        arm.transform.rotation = Quaternion.Lerp(armStartRot, Quaternion.AngleAxis(angle2, Vector3.forward), 1);

                        curTime += Time.deltaTime;

                        yield return null;
                    }

                    float targetSwingBackTime = 0.3f;
                    float curSwingBackTime = 0;

                    while (curSwingBackTime < targetSwingBackTime)
                    {
                        if (root.paused || root.dead)
                            yield return null;

                        arm.Rotate(transform.forward * swingSpeed);

                        curSwingBackTime += Time.deltaTime;
                        yield return null;
                    }


                    yield return new WaitForSeconds(0.2f);

                    float targetSwingTime = 0.3f;
                    float curSwingTime = 0;

                    spatula.swinging = true;

                    while (curSwingTime < targetSwingTime)
                    {
                        if (root.paused || root.dead)
                            yield return null;

                        arm.transform.position = Vector3.Lerp(arm.transform.position, arm.transform.position + (arm.transform.forward * 1.1f), 0.3f);
                        arm.Rotate(-transform.forward * (swingSpeed * 6));

                        curSwingTime += Time.deltaTime;
                        yield return null;
                    }

                    spatula.swinging = false;


                    yield return new WaitForSeconds(2);

                    arm.transform.rotation = armStartRot;
                    arm.transform.position = spatulaArmStartPoint.position;
                    arm.gameObject.SetActive(false);

                    break;
            }


            attacking = false;
            lookAt = false;
            MoveJaw(1);

            yield return new WaitForSeconds(2);

            yield return null;
        }


        public void MoveJaw(float target, bool forceDo = false)
        {
            float targetClamped = target;

            if (targetClamped < jawMin)
                targetClamped = jawMin;
            if (targetClamped > jawMax)
                targetClamped = jawMax;

            if (targetClamped == currentJawIntensity)
                return;

            if (!jawMoving)
            {
                if(forceDo)
                {
                    if (jawMoving)
                    {
                        cancelJawDoOnce = true;
                        while (jawMoving)
                        {
                            return;
                        }
                    }
                }

                StartCoroutine(MoveJawCoroutine(targetClamped));
            }
        }

        public IEnumerator MoveJawCoroutine(float target)
        {

            jawMoving = true;


            MFPEditorUtils.Log(target.ToString());

            if (target> currentJawIntensity)
            {
                while (currentJawIntensity < target)
                {
                    if (root.paused || root.dead)
                        yield return null;

                    if(cancelJawDoOnce)
                    {
                        jawMoving = false;
                        cancelJawDoOnce = false;
                        yield break;
                    }

                    float yAmount = Mathf.Clamp(head.transform.localPosition.y + jawSpeed, jawMin, jawMax);
                    head.transform.localPosition = new Vector3(head.transform.localPosition.x, yAmount, head.transform.localPosition.z);
                    currentJawIntensity = yAmount;

                    yield return null;
                }
            }
            else
            {

                while (currentJawIntensity > target)
                {
                    if (root.paused || root.dead)
                        yield return null;


                    if (cancelJawDoOnce)
                    {
                        jawMoving = false;
                        cancelJawDoOnce = false;
                        yield break;
                    }


                    float yAmount = Mathf.Clamp(head.transform.localPosition.y - jawSpeed, jawMin, jawMax);
                    head.transform.localPosition = new Vector3(head.transform.localPosition.x, yAmount, head.transform.localPosition.z);
                    currentJawIntensity = yAmount;

                    yield return null;
                }
            }

            head.transform.localPosition = new Vector3(head.transform.localPosition.x, target, head.transform.localPosition.z);
            currentJawIntensity = target;

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