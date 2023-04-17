using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static UnityEngine.UI.Image;

namespace NodeCanvas.Tasks.Actions{

	public class SetJumpActionTask : ActionTask{

        /// <summary>
        /// Sets an arc to travel at given the positions of the boss, the player, and a height we want to jump at
        /// </summary>

        //Variables
        public float gravity;
        public float maxJumpHeight;
		public float jumpTimeToComplete;

        

        private Vector2 playerPos;
		private Vector2 bossPos;

		private bool isJumping;
		private float currentTime;

		Vector2 speed;
        CircleCollider2D col;
        protected override string OnInit(){
            col = agent.GetComponent<CircleCollider2D>();
            return null;
		}

		protected override void OnExecute(){

			//make sure we're only setting the velocity once
			isJumping = blackboard.GetVariableValue<bool>("isJumping");
			if (isJumping)
				return;

			blackboard.SetVariableValue("isJumping", true);

            //forum that helped me: https://forum.unity.com/threads/jump-to-a-specific-position-without-rigidbody.922592/
            playerPos = blackboard.GetVariableValue<Vector2>("player_position");
			bossPos = agent.transform.position;

			

			CalculateSpeed();
        }
        protected override void OnUpdate(){
            //increase the time for the time variable in equation
            currentTime += Time.deltaTime;

            //update position
			Vector2 pos = agent.transform.position;

            pos = new Vector2(bossPos.x + speed.x * currentTime, bossPos.y + speed.y * currentTime + 0.5f * gravity * Mathf.Pow(currentTime, 2));

			agent.transform.position = pos;

            //check to see if they landed on the ground and not right when the script activated (hence the 1)
            if(CheckGrounded() && currentTime > 1)
            {
                currentTime = 0;
                blackboard.SetVariableValue("isJumping", false);
                EndAction(true);
			}
        }
        private void CalculateSpeed()
        {
			Vector2 diff = playerPos - bossPos;

            float speedY = Mathf.Sqrt(-2 * gravity * maxJumpHeight);

            speed = new Vector2 ( diff.x * gravity / (-speedY - Mathf.Sqrt(Mathf.Abs(Mathf.Pow(speedY, 2) + 2 * gravity * diff.y))), speedY);
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