using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    UIManager ui;
    //BossAttacks boss;
    AudioSource audio;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject kirby;
    public bool isFollowing = true; //if the camera is following kirby
    private bool firstTime = false;

    private void Awake()
    {
        ui = GameObject.Find("Canvas").GetComponent<UIManager>();
        //boss = GameObject.Find("WhispyWoods").GetComponent<BossAttacks>();
        audio = GameObject.Find("Main Camera").GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        //Make the camera follow Kirby's vertical movement with smoothing
        //if Kirby enters the  trigger to start the boss fight, then set the camera to a fixed position (gameObject that contains the trigger);
        if (isFollowing)
        {
            Vector3 diff = (kirby.transform.position - camera.transform.position);
            camera.transform.Translate(Vector3.up * diff.y);
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Kirby")
        {
            //if kirby enters the trigger where the boss area is, initiate the boss fight and lock the camera position 
            isFollowing = false;
            ui.bossBarVisible = true;
            camera.transform.position = new Vector3(0,0,-10);

            if(!firstTime)
            {
                //start the time for the boss to start 
                //boss.bossCurrentTime = Time.time;
                firstTime = true;
            }
            
        }
    }
    void LowerVolume()
    {
        for(int i = 0; i < 10; i++)
            audio.volume -= 0.1f;
    }
}
