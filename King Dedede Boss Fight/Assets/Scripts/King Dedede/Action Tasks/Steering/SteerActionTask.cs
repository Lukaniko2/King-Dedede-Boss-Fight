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
		public float launchUpForce;
		public float maxSpeed;
        public float speed;
		public float accelX;
		public float gravity;
		public float maxHeightY;
        public Vector2 velocity;

        private float currentPuffTime;
		private float maxPuffTime;
		private bool goingUp;
        protected override string OnInit(){
			rb = agent.GetComponent<Rigidbody2D>();
			col = agent.GetComponent<CircleCollider2D>();
			maxPuffTime = blackboard.GetVariableValue<float>("maxPuffTime");

            return null;
		}

		protected override void OnExecute(){
			velocity.y = 0;
			velocity.x = 0;
        }

		protected override void OnUpdate(){
            currentPuffTime += Time.deltaTime;

            //Move Boss based on velocity from approach task
            int direction = blackboard.GetVariableValue<int>("dedede_directionFacing");

			Vector2 pos = agent.transform.position;

			//adjust velocity based on acceleration in X and Y direction
			velocity.x += accelX * direction * Time.deltaTime;
			velocity.y -= gravity * Time.deltaTime;

			//clamp speed so King Dedede doesn't keep getting faster
			velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
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

            bool completeTime = currentPuffTime >= maxPuffTime;
			bool velocityThreshold = velocity.y > -0.2f && velocity.y < 0.2f;
			if(completeTime && velocityThreshold)
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
			
		}

		protected override void OnPause(){
			
		}
	}
}