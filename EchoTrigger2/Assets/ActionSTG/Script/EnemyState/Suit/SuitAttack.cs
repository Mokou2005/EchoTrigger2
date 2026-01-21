using UnityEngine;
using UnityEngine.AI;

namespace StateMachineAI
{

    public class SuitAttack : State<EnemyAI>
    {
    
        public SuitAttack(EnemyAI owner) : base(owner) { }
        public override void Enter()
        {
    
            Call Cal=owner.GetComponent<Call>();
            if (Cal==null)
            {
                Debug.Log("Call‚ª‚È‚©‚Á‚½‚Ì‚ÅŽ©“®’Ç‰Á‚µ‚Ü‚·");
                Cal=owner.gameObject.AddComponent<Call>();
            }
         
        }

        public override void Stay()
        {

        }

        public override void Exit()
        {
        }
    }

}