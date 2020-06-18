using UnityEngine;

namespace MFPClassic
{
    public class VictorLimb : MonoBehaviour
    {
        private Victor victor;


        public void Awake()
        {
            victor = FinalBattleController.inst.victorBoss;
        }

        public void OnCollisionEnter(Collision coll)
        {
            MFPEditorUtils.Log("hit registered " + coll.transform.name);

            if (coll.transform.tag == "Bullet")
                victor.Hurt(coll.gameObject.GetComponent<BulletScript>());

        }
    }
}
