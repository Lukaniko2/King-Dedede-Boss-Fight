using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NodeCanvas.Tasks.Actions{

	public class GetPlayerLocationActionTask : ActionTask{

        protected override string OnInit(){
			return null;
		}
		
		protected override void OnExecute(){

            //Only get the player's position once.
            //With behaviour trees, the execute is ran once a frame like an update
            bool havePlayerLocation = blackboard.GetVariableValue<Vector2>("player_position") != Vector2.zero;

			if (havePlayerLocation)
			{
				EndAction(true);
                return;
            }
				

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
			
            EndAction(true);
		}

        #region Nothing In Other Functions
        protected override void OnUpdate(){
			
		}

		
		protected override void OnStop(){
			
		}

		
		protected override void OnPause(){
			
		}
		#endregion
	}
}