using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	public class PuffUpStartActionTask : ActionTask{
		/// <summary>
		/// Increase the position of King Dedede to a certain point
		/// Once it reaches that point, continue
		/// </summary>

		//variables
		public float maxHeightFromGround;
		public float riseSpeed;
		private float groundHeight;
		private float currentAnimTime;

		protected override string OnInit(){
			return null;
		}


		protected override void OnExecute(){

			//make sure they only get the ground height once and not repeatedly
			bool isPuffingUp = blackboard.GetVariableValue<bool>("isPuffingUp");

			if(!isPuffingUp)
			{
				groundHeight = agent.transform.position.y;
				blackboard.SetVariableValue("groundHeight", groundHeight);
				blackboard.SetVariableValue("isPuffingUp", true);
			}

		}

		protected override void OnUpdate(){

			//increase time for anim curve to evaluate the Y based on X (Time)
			currentAnimTime += Time.deltaTime;

            Vector2 pos = agent.transform.position;

			pos += Vector2.up * riseSpeed * Time.deltaTime;

			agent.transform.position = pos;

			

			bool reachedTargetPos = agent.transform.position.y > groundHeight + maxHeightFromGround;
			if(reachedTargetPos)
			{
				EndAction(true);
			}
        }


		protected override void OnStop(){
			
		}

		protected override void OnPause(){
			
		}
	}
}