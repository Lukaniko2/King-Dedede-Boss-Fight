using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	public class DiveActionTask : ActionTask{

		//Variables
		public float maxDiveTime;
		public float decrementSpeed;
		public float diveSpeed;

		private Vector2 playerTransform;
		private float currentDiveTime;
		private float currentDiveSpeed;

		private bool isDiving;

		protected override string OnInit(){
			
			isDiving = blackboard.GetVariableValue<bool>("isDiving");

			
			return null;
		}

		protected override void OnExecute(){
			//Only call this once, like a void start
			if(!isDiving)
            {
				playerTransform = blackboard.GetVariableValue<Vector2>("player_position");
				currentDiveSpeed = diveSpeed;
				blackboard.SetVariableValue("isDiving", true);
			}
		}

		protected override void OnUpdate(){
			currentDiveTime += Time.deltaTime;
			
			//lunge at player
			Vector2 pos = agent.transform.position;
			Vector2 dir = playerTransform - pos;

			//decrement the speed for a slower stop
			currentDiveSpeed -= decrementSpeed * Time.deltaTime;

			//Move the player
			int directionOfTravel = (int)Mathf.Sign(dir.x);
			pos += new Vector2(directionOfTravel * currentDiveSpeed * Time.deltaTime, 0);

			agent.transform.position = pos;
			

			//if the dive ends, then reset values
			bool diveTimeEnded = currentDiveTime >= maxDiveTime;
			if (diveTimeEnded)
            {
				Debug.Log(currentDiveSpeed);
				currentDiveTime = 0;
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