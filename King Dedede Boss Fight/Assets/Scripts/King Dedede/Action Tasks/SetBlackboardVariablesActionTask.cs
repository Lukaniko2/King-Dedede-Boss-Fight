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

        protected override string OnInit(){
			SO_BossParameters bossParameters = blackboard.GetVariableValue<SO_BossParameters>("bossParams");
            playerTransform = GameObject.FindObjectOfType<PlayerInput>().transform;

            blackboard.SetVariableValue("auto_playerLocation", playerTransform);
            blackboard.SetVariableValue("auto_speed", bossParameters.bossSpeed);

			return null;
        }
        protected override void OnExecute()
        {
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