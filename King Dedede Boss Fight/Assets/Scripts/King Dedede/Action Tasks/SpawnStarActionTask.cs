using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	public class SpawnStarActionTask : ActionTask{

		//Variables
		public int spawnStarCount;
		public Transform spawnStarTransform;

		private GameObject fallenStarPrefab;

		protected override string OnInit(){
			//Get the star prefab
			fallenStarPrefab = blackboard.GetVariableValue<GameObject>("starPrefab");
			return null;
		}

		protected override void OnExecute(){
			
			//Spawn a star or two depending on attack
			if(spawnStarCount == 1)
			{
                GameObject starPrefab = GameObject.Instantiate(fallenStarPrefab, spawnStarTransform.position, Quaternion.identity);
                RotateStar rotStar = starPrefab.GetComponentInChildren<RotateStar>();

                //Get direction of boss
                int dir = blackboard.GetVariableValue<int>("dedede_directionFacing");

                rotStar.direction = dir;
            }
			else if(spawnStarCount == 2)
			{
				for(int i = 0; i < spawnStarCount; i++)
				{
                    GameObject starPrefab = GameObject.Instantiate(fallenStarPrefab, spawnStarTransform.position, Quaternion.identity);
                    RotateStar rotStar = starPrefab.GetComponentInChildren<RotateStar>();

					//spawn 2 stars that go in opposite directions
					if(i == 0)
						rotStar.direction = -1;
					else
						rotStar.direction = 1;
                }
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