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

        protected override string OnInit(){
            //only if they need the boss params should it run. Else it's a custom time 
            if(bossParameter.value != null)
                currentIdleTime = bossParameter.value.maxIdleTime;

            currentCustomTime = customTimeout;
            
            return null;
		}

        protected override void OnEnable(){}

        protected override bool OnCheck(){

            //Logic for both types of timeout
            switch (typeOfTimer)
            {
                case TypeOfTimer.Idle:

                    //decrease time and set variable
                    currentIdleTime -= Time.deltaTime;

                    bool timeoutCompleted = currentIdleTime <= 0;

                    if (timeoutCompleted)
                        currentIdleTime = bossParameter.value.maxIdleTime;

                    return timeoutCompleted;

                case TypeOfTimer.Custom:

                    //decrease time
                    currentCustomTime -= Time.deltaTime;

                    bool timeoutCustomCompleted = currentCustomTime <= 0;

                    if (timeoutCustomCompleted)
                        currentCustomTime = customTimeout;

                    return timeoutCustomCompleted;

                default:
                    return false;
            }

		}

        protected override void OnDisable(){ }
    }
}