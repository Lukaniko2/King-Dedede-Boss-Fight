using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeatedFlicker : MonoBehaviour
{
    UIManager ui;
    SpriteRenderer sprite;
    [SerializeField] GameObject star;
    [SerializeField] GameObject cam;
    Color[] color = { Color.white, Color.red, Color.black };
    
    
    //beginning flickering variables
    private float currentTime;
    [SerializeField] float flickerTime; //how long does each flicker last
    private int amountOfFlickers; //records how many flickers have happened
    [SerializeField] int maxFlickers; //the max amount of flickers we want to happen
    private int j; //increasing the index of the color array

    // Start is called before the first frame update
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        ui = GameObject.Find("Canvas").GetComponent<UIManager>();

        //set the alpha value of each of these colors to 0.30
        for (int i = 0; i < color.Length; i++)
        {
            color[i].a = 0.30f;
        }

        currentTime = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        //if the boss is defeated and enough time has elapsed, play the next flicker
        if(GameManager.bossIsDefeated && Time.time - currentTime >= flickerTime && amountOfFlickers <= maxFlickers)
        {
            //j is in charge of changing the color of the flicker in the color array
            j++;
            j %= 3;
            sprite.color = color[j];

            amountOfFlickers++;
            currentTime = Time.time;
        }
        else if(amountOfFlickers >= maxFlickers)
        {
            //if the flickers reached it's max, lower the volume of the battle music andmake the victory star visible
            cam.SendMessage("LowerVolume");
            star.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
