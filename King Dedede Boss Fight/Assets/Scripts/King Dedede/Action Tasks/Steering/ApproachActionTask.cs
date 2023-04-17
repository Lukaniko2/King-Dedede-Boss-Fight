using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	public class ApproachActionTask : ActionTask{

		//Variables
		private Vector2 targetPositions;

		protected override string OnInit(){

			return null;
		}

		protected override void OnExecute(){
			
			
		}

		protected override void OnUpdate(){
			GettingPlayerLocation();
            targetPositions = blackboard.GetVariableValue<Vector2>("player_position");
			

			Vector2 directionToTarget = (targetPositions - (Vector2)agent.transform.position);
			

			blackboard.SetVariableValue("puffSteerVel", directionToTarget.normalized);
           
		}

		private void GettingPlayerLocation()
		{
            //Get the player's current position when this is called
            Vector2 playerPos = blackboard.GetVariableValue<Transform>("auto_playerLocation").position;
            blackboard.SetVariableValue("player_position", playerPos);


            //Calculate direction to player
            Vector2 playerPosition = blackboard.GetVariableValue<Vector2>("player_position");
            Vector2 pos = agent.transform.position;

            Vector2 dir = playerPosition - pos;

            //Get direction as an int and set it
            int directionOfTravel = (int)Mathf.Sign(dir.x);
            blackboard.SetVariableValue("dedede_directionFacing", directionOfTravel);

            //Flip Sprite based on direction
            Vector2 currentScale = agent.transform.localScale;
            agent.transform.localScale = new Vector2((Mathf.Abs(currentScale.x)) * directionOfTravel, currentScale.y);
        }
		protected override void OnStop(){
			
		}

		protected override void OnPause(){
			
		}
	}
}