using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	public class ReturnToGroundActionTask : ActionTask{

		public enum DescentType
		{
            Both,
			Stright_Down
		}
		public DescentType descentType;

		//Components
		private CircleCollider2D col;
		private Rigidbody2D rb;
        GameObject puffOutPrefab;
        //Variables
        public float rbGravity;
		public float maxTimeBeforeFalling;
		public float puffUpwardsSpeed;

		private float currentTime;
		private int directionFacing;
        private Vector2 speed;

		protected override string OnInit(){
			col = agent.GetComponent<CircleCollider2D>();
			rb = agent.GetComponent<Rigidbody2D>();
			puffOutPrefab = blackboard.GetVariableValue<GameObject>("doublePuffPrefab");

            return null;
		}

		protected override void OnExecute(){


            directionFacing = blackboard.GetVariableValue<int>("dedede_directionFacing");

			if(descentType == DescentType.Both)
			{
                //Puff Out
                GameObject puff = GameObject.Instantiate(blackboard.GetVariableValue<GameObject>("puffOutPrefab"), agent.transform.position, Quaternion.identity);
                puff.GetComponent<PuffOut>().PuffSetup(directionFacing);

                AudioManager.Instance.PlaySound("k_exhale");
            }

			currentTime = Time.time;
			speed.y = puffUpwardsSpeed;
        }

		protected override void OnUpdate(){

			Vector2 pos = agent.transform.position;
			Vector2 directionToPuffOut = new Vector2(0.5f, 1);

			//horizontal movement
			if(descentType == DescentType.Both)
                pos.x += directionToPuffOut.x * -directionFacing * Time.deltaTime;

            //add gravity
            if (Time.time - currentTime >= maxTimeBeforeFalling)
                speed.y -= directionToPuffOut.y * rbGravity * Time.deltaTime;

            //vertical recoil
            pos.y += directionToPuffOut.y * speed.y * Time.deltaTime;
			

            agent.transform.position = pos;


            if (CheckGrounded())
            {
				rb.velocity = Vector2.zero;
				rb.bodyType = RigidbodyType2D.Kinematic;

				//spawn double puffs if on the way down from the puff attack
				if(descentType == DescentType.Both)
					GameObject.Instantiate(puffOutPrefab, agent.transform.position + Vector3.down * 0.2f, Quaternion.identity);
                
				EndAction(true);
            }
            
        }

        private bool CheckGrounded()
        {
            LayerMask groundLayer = LayerMask.GetMask("Ground");
            bool overlap = Physics2D.Linecast(agent.transform.position, (Vector2)agent.transform.position + Vector2.down * (col.radius + 0.1f), groundLayer);

            return overlap;
        }
        
		protected override void OnStop(){
			
		}

		protected override void OnPause(){
			
		}
	}
}