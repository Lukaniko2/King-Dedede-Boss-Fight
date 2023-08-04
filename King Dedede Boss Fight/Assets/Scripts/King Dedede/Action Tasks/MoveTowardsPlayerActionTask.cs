using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	public class MoveTowardsPlayerActionTask : ActionTask{
	
        //Variables
        public float distanceThreshold;
        private Vector2 playerTransform = Vector2.zero;
		public float speed;
		private float speedMultiplier;

		protected override string OnInit(){
			return null;
		}


		protected override void OnExecute(){

			playerTransform = blackboard.GetVariableValue<Vector2>("player_position");
            speedMultiplier = blackboard.GetVariableValue<float>("auto_speed");

		}


		protected override void OnUpdate(){
			//Check as an alternative to seeing if the value is null. Is set to zero once reached player
			if (playerTransform == Vector2.zero)
				return;

			Vector2 pos = agent.transform.position;

			Vector2 dir = playerTransform - pos;

			float distanceX = Mathf.Abs(dir.x);

            int directionOfTravel = (int)Mathf.Sign(dir.x);

			pos.x += speed * speedMultiplier * directionOfTravel * Time.deltaTime;

			agent.transform.position = pos;

			CheckDistanceWithinThreshold(distanceX);
		}

		protected override void OnStop(){
			
		}

		//Called when the task is paused.
		protected override void OnPause(){
			
		}

		private void CheckDistanceWithinThreshold(float distance)
		{
            bool inThreshold = distance < distanceThreshold;

			if (inThreshold)
				EndAction(true);
        }
	}
}