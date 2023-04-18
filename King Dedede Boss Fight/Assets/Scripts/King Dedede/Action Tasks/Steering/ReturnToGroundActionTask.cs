using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	public class ReturnToGroundActionTask : ActionTask{

		//Components
		private CircleCollider2D col;
		private Rigidbody2D rb;

		//Variables
		public float rbGravity;

		protected override string OnInit(){
			col = agent.GetComponent<CircleCollider2D>();
			rb = agent.GetComponent<Rigidbody2D>();
			return null;
		}

		protected override void OnExecute(){
			rb.bodyType = RigidbodyType2D.Dynamic;
			rb.gravityScale = rbGravity;

			AudioManager.Instance.PlaySound("k_exhale");
		}

		protected override void OnUpdate(){

            if (CheckGrounded())
            {
				rb.velocity = Vector2.zero;
				rb.bodyType = RigidbodyType2D.Kinematic;
                EndAction(true);
            }
            
        }

        private bool CheckGrounded()
        {
            LayerMask groundLayer = LayerMask.GetMask("Ground");
            bool overlap = Physics2D.Linecast(agent.transform.position, (Vector2)agent.transform.position + Vector2.down * (col.radius + 0.08f), groundLayer);

            return overlap;
        }
        
		protected override void OnStop(){
			
		}

		protected override void OnPause(){
			
		}
	}
}