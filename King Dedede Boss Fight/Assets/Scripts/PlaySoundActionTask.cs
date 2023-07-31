using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions{

	public class PlaySoundActionTask : ActionTask{

		public string soundClipName;
		protected override string OnInit(){
			return null;
		}

		protected override void OnExecute(){
            AudioManager.Instance.StopSound(soundClipName);
            AudioManager.Instance.PlaySound(soundClipName);
			EndAction(true);
		}

		protected override void OnUpdate(){
			
		}

		protected override void OnStop()
		{
		}

		protected override void OnPause(){
			
		}
	}
}