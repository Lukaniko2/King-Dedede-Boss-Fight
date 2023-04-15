using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.InputSystem.Android;

namespace NodeCanvas.Tasks.Actions{

	public class SwingHammerActionTask : ActionTask{

		public enum EnabledType
		{
			Enable,
			Disable,
			Toggle
		}
		public EnabledType enabledType;

		//Components
		private BoxCollider2D hammerCollider;

		protected override string OnInit(){
			hammerCollider = blackboard.GetVariableValue<BoxCollider2D>("hammerCollider");
			return null;
		}

		protected override void OnExecute(){

			//activating or deactivating the hammer's collider based on choice
			switch(enabledType)
			{
				case EnabledType.Enable:
                    hammerCollider.enabled = true;
                    break;

					case EnabledType.Disable:
					hammerCollider.enabled = false;
                    break;

				case EnabledType.Toggle:
                    hammerCollider.enabled = !hammerCollider.enabled;
                    break;
			}
			
			EndAction(true);
		}

		protected override void OnUpdate(){
			
		}

		protected override void OnStop(){
			
		}

		protected override void OnPause(){
			
		}
	}
}