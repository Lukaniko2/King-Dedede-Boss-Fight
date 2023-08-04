using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace NodeCanvas.Tasks.Actions{

	public class DiveActionTask : ActionTask{

		//Variables
		public float maxDiveTime;
		public float decrementSpeed;
		public float diveSpeed;

		private Vector2 playerTransform;
		private float currentDiveSpeed;

		private bool isDiving;
		private int directionOfTravel;
		private float speedMultiplier;

        protected override string OnInit(){
			
			isDiving = blackboard.GetVariableValue<bool>("isDiving");

			
			return null;
		}

		protected override void OnExecute(){

            speedMultiplier = blackboard.GetVariableValue<float>("auto_speed");

            //Only call this once, like a void start
            if (!isDiving)
            {
				playerTransform = blackboard.GetVariableValue<Vector2>("player_position");
				currentDiveSpeed = diveSpeed * speedMultiplier;
				blackboard.SetVariableValue("isDiving", true);

                Vector2 dir = playerTransform - (Vector2)agent.transform.position;
                directionOfTravel = (int)Mathf.Sign(dir.x);
            }
		}

		protected override void OnUpdate(){
			//lunge at player
			Vector2 pos = agent.transform.position;

			//decrement the speed for a slower stop
			currentDiveSpeed = Mathf.Lerp(currentDiveSpeed, 0, Time.deltaTime / maxDiveTime);

			//Move the player
			pos += new Vector2(directionOfTravel * currentDiveSpeed * Time.deltaTime, 0);

			agent.transform.position = pos;

            //if the dive ends, then reset values
            bool diveTimeEnded = currentDiveSpeed < 0.1f;
			if (diveTimeEnded)
            {
				currentDiveSpeed = diveSpeed;
				blackboard.SetVariableValue("isDiving", false);
				EndAction(true);
            }

		}

		protected override void OnStop(){
			
		}

		protected override void OnPause(){
			
		}
	}
}