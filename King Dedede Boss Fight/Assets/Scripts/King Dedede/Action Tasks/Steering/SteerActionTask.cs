using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace NodeCanvas.Tasks.Actions{

	public class SteerActionTask : ActionTask{

		//Components
		private Rigidbody2D rb;
		private CircleCollider2D col;

        //Variables
		public float launchUpForce;
		public float maxSpeed;
        public float speed;
		public float accelX;
		public float gravity;
		public float maxHeightY;
        public Vector2 velocity;

        private float currentPuffTime;
		private float maxPuffTime;
		private float speedMultiplier;

		private bool once;

        protected override string OnInit(){
			rb = agent.GetComponent<Rigidbody2D>();
			col = agent.GetComponent<CircleCollider2D>();
			maxPuffTime = blackboard.GetVariableValue<float>("maxPuffTime");

            return null;
		}

		protected override void OnExecute(){
			velocity.y = 0;
			velocity.x = 0;
            speedMultiplier = blackboard.GetVariableValue<float>("auto_speed");
			once = false;
        }

		protected override void OnUpdate(){

			if (!once)
			{
                currentPuffTime = 0;
				once = true;
            }
				

            currentPuffTime += Time.deltaTime;

            //Move Boss based on velocity from approach task
            int direction = blackboard.GetVariableValue<int>("dedede_directionFacing");

			Vector2 pos = agent.transform.position;

			//adjust velocity based on acceleration in X and Y direction
			velocity.x += accelX * speedMultiplier * direction * Time.deltaTime;
			velocity.y -= gravity * Time.deltaTime;

			//clamp speed so King Dedede doesn't keep getting faster
			velocity.x = Mathf.Clamp(velocity.x, -maxSpeed * speedMultiplier, maxSpeed * speedMultiplier);
            velocity.y = Mathf.Clamp(velocity.y, -maxHeightY, maxHeightY);

            //set the position of King Dedede
            pos += velocity * Time.deltaTime;
			agent.transform.position =  pos;


            if (CheckGrounded())
			{
				//Add Force Upwards
				velocity.y += launchUpForce * Time.deltaTime;
			}

			if(CheckHitWall())
			{
				//Flips the velocity in the X
				velocity.x *= -1;
            }

			float groundHeight = blackboard.GetVariableValue<float>("groundHeight");
            bool completeTime = currentPuffTime >= maxPuffTime;
			bool velocityThreshold = velocity.y > -0.2f && velocity.y < 0.2f;
			bool aboveGround = agent.transform.position.y > groundHeight + 0.2f;

			if(completeTime && velocityThreshold && aboveGround)
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

		private bool CheckHitWall()
		{
            LayerMask groundLayer = LayerMask.GetMask("Environment");
            bool overlap = Physics2D.Linecast(agent.transform.position, (Vector2)agent.transform.position + Vector2.right * (col.radius + 0.08f), groundLayer);

            return overlap;
        }
        protected override void OnStop(){
			once = false;

        }

		protected override void OnPause(){
			
		}
	}
}