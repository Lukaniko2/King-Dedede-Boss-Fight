using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	public class PuffUpStartActionTask : ActionTask{
		/// <summary>
		/// Increase the position of King Dedede to a certain point
		/// Once it reaches that point, continue
		/// </summary>

		public float targetHeightAboveBoss;
		public float riseSpeed;
		private float groundHeight;

		protected override string OnInit(){
			return null;
		}


		protected override void OnExecute(){
			bool isPuffingUp = blackboard.GetVariableValue<bool>("isPuffingUp");
			if(!isPuffingUp)
			{
				groundHeight = agent.transform.position.y;
				blackboard.SetVariableValue("isPuffingUp", true);
			}

		}

		protected override void OnUpdate(){
            Vector2 pos = agent.transform.position;
			pos += Vector2.up * targetHeightAboveBoss * riseSpeed * Time.deltaTime;
			agent.transform.position = pos;

			bool reachedTargetPos = agent.transform.position.y >= groundHeight + targetHeightAboveBoss;
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