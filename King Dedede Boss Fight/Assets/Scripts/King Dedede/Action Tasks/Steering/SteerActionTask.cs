using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace NodeCanvas.Tasks.Actions{

	public class SteerActionTask : ActionTask{

		//Components
		private Rigidbody2D rb;
		private CircleCollider2D col;

        //Variables
        public float maxPuffTime;
		public float launchUpForce;
		public float maxSpeed;
        public float speed;
		public float accelX;
		public float gravity;

		private float currentPuffTime;
		private Vector2 velocity;
        protected override string OnInit(){
			rb = agent.GetComponent<Rigidbody2D>();
			col = agent.GetComponent<CircleCollider2D>();
			return null;
		}

		protected override void OnExecute(){
            currentPuffTime += Time.deltaTime;
			
        }

		protected override void OnUpdate(){
            
            //Move Boss based on velocity from approach task
            Vector2 direction = blackboard.GetVariableValue<Vector2>("puffSteerVel");

			Vector2 pos = agent.transform.position;

			velocity.x += accelX * direction.x * Time.deltaTime;
			velocity.y -= gravity * Time.deltaTime;

			velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);


            pos += velocity * Time.deltaTime;

			agent.transform.position =  pos;




            if (CheckGrounded())
			{
				//Add Force Upwards
				velocity.y += launchUpForce * Time.deltaTime;
			}


            bool completeTime = currentPuffTime >= maxPuffTime;
			if(completeTime)
			{
				currentPuffTime = 0;
				EndAction(true);

			}
        }
	
        private bool CheckGrounded()
        {
            LayerMask groundLayer = LayerMask.GetMask("Ground");
            bool overlap = Physics2D.Linecast(agent.transform.position, (Vector2)agent.transform.position + Vector2.down * (col.radius + 0.08f), groundLayer);

            return overlap;
        }
        protected override void OnStop(){
			
		}

		protected override void OnPause(){
			
		}
	}
}