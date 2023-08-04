using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NodeCanvas.Tasks.Actions{

	public class SetBlackboardVariablesActionTask : ActionTask{
        /// <summary>
        /// This script will automatically set some variables in the blackboard.
        /// Reason I have variables in the "bossParameters" script is because If I want to make more bosses in the future
        /// I can easily create a scriptable object and change the values there instead of setting them in the blackboard
        /// </summary>

        private Transform playerTransform;
        private SO_BossParameters bossParameters;
        private bool first = true; //need for setting health only second time because it starts off at 0

        protected override string OnInit(){
			bossParameters = blackboard.GetVariableValue<SO_BossParameters>("bossParams");
            playerTransform = GameObject.FindObjectOfType<PlayerInput>().transform;

            blackboard.SetVariableValue("auto_playerLocation", playerTransform);
            blackboard.SetVariableValue("auto_speed", bossParameters.bossSpeed);
            blackboard.SetVariableValue("bossHealth", 100f);
            return null;
        }
        protected override void OnExecute()
        {

            //Check to see which type of speed we should be using
            float currentHealth = agent.GetComponent<BossDetails>().health;

            if (first)
                first = false;
            else
                blackboard.SetVariableValue("bossHealth", currentHealth);


            if (currentHealth < 50)
            {
                blackboard.SetVariableValue("auto_speed", bossParameters.bossSpeedFast);
            }

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