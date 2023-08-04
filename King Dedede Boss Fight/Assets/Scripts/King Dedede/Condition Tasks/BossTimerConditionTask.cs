using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions{

	public class BossTimerConditionTask : ConditionTask{

        //For multiple cases of timers, we need to get certain variables from bossParameters script
        public enum TypeOfTimer
        {
            Idle,
            Custom
        }
        
		//Variables
        public BBParameter<SO_BossParameters> bossParameter;

		public TypeOfTimer typeOfTimer;

        public float customTimeout;

        private float currentIdleTime;
        private float currentCustomTime;
        private float currentHealth;

        protected override string OnInit(){
            //only if they need the boss params should it run. Else it's a custom time 
            if(bossParameter.value != null)
                currentIdleTime = bossParameter.value.maxIdleTime;

            currentCustomTime = customTimeout;
            
            return null;
		}

        protected override void OnEnable(){
            currentHealth = blackboard.GetVariableValue<float>("bossHealth");
        }

        protected override bool OnCheck(){

            //Logic for both types of timeout
            switch (typeOfTimer)
            {
                case TypeOfTimer.Idle:

                    //decrease time and set variable
                    currentIdleTime -= Time.deltaTime;

                    bool timeoutCompleted = currentIdleTime <= 0;

                    if (timeoutCompleted)
                        currentIdleTime = SetCurrentIdleTime();

                    return timeoutCompleted;

                case TypeOfTimer.Custom:

                    //decrease time
                    if(currentHealth > 50)
                        currentCustomTime -= Time.deltaTime * bossParameter.value.bossSpeed;
                    else
                        currentCustomTime -= Time.deltaTime * bossParameter.value.bossSpeedFast;

                    bool timeoutCustomCompleted = currentCustomTime <= 0;

                    if (timeoutCustomCompleted)
                        currentCustomTime = customTimeout;

                    return timeoutCustomCompleted;

                default:
                    return false;
            }

		}

        protected override void OnDisable()
        {
        }

        private float SetCurrentIdleTime()
        {
            //setting it back to the default value based on the boss' health
            if (currentHealth > 50)
                return bossParameter.value.maxIdleTime;
            else
                return bossParameter.value.minIdleTime;
        }
    }
}