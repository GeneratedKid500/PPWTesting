using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attatch to the main player - NOTE: THIS SCRIPT ALSO GETS INPUT FOR THE MELEE ATTACK
public class WeaponSwitch : MonoBehaviour
{
    ////editor vars
    [Header("Weapon Switching")]
    public GameObject[] WEAPONS; //constructed in the editor
    [Header("Melee Strike")]
    public GameObject meleeWeapon;
    public int rotationSpeed = 10;

    ////system vars
    //weapon switching
    GameObject decorStaff;
    [HideInInspector] public GameObject current;
    int x; //pointer
    //melee
    AudioSource audioSource;
    Transform originalPos;  
    int counter = 0;
    float countLimit = 90;
    bool striking = false, retracting = false;
    bool firstUpdate = true;

    void Start()
    {
        x = 0; 
        current = WEAPONS[x];
        //sets the first weapon in the list to be the first one equipped

        decorStaff = GameObject.FindGameObjectWithTag("Weapon/Staff");
        decorStaff.SetActive(false);
        //gets and disables the staff seen in the editor. dunno why this isnt just dragged in honestly

        WEAPONS[WEAPONS.Length - 1] = decorStaff; //always sets the last item in an array to be a melee stick

        for (int i = 0; i < WEAPONS.Length; i++)
        {
            WEAPONS[i].SetActive(true);
        }

        //melee
        audioSource = GetComponent<AudioSource>();
        countLimit /= rotationSpeed;
        meleeWeapon.SetActive(true);
        originalPos = meleeWeapon.transform; //gets the current position of the melee weapon so that it can be returned to that positon
        meleeWeapon.SetActive(false);
    }

    void Update()
    {
        if (firstUpdate)
        {
            firstUpdate = false;
            for (int i = 0; i < WEAPONS.Length; i++)
            {
                WEAPONS[i].SetActive(false);
            }
            WEAPONS[x].SetActive(true);
        }

        if (!(striking || retracting)) //if the player is not using a melee attack
        {
            ViaScrollWheel();
            ViaNumKeys();
        }
    }

    void LateUpdate() //stops weapon and melee being active at once by explotitive players
    {
        //melee
        MeleeStrike();
    }

    void ViaScrollWheel() //change via scroll wheel
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) //if scroll wheel is scrolled up
        {
            WEAPONS[x].SetActive(false); //sets current weapon to be inactive
            if (x != WEAPONS.Length - 1) //if x isnt the same size as the weapons list
                x++; //adds value
            else
                x = 0; //if it is the same size then it loops back to 0
            current = WEAPONS[x]; //updates current
            WEAPONS[x].SetActive(true); //enables new weapon
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) //if scroll wheel is scrolled down
        {
            WEAPONS[x].SetActive(false); //sets current weapon to be inactive
            if (x != 0) //if x isnt 0 (aka end of the list)
                x--; //minus value
            else
                x = WEAPONS.Length - 1; //if it is, loops around to end of list
            current = WEAPONS[x]; //updates var current
            WEAPONS[x].SetActive(true); //enables new weapon
        }
    }

    void ViaNumKeys() //gets number key input
    {
        //calls additional function after input is taken
        if (Input.GetKeyDown("1")) NumKeySwitch(0); 
        if (Input.GetKeyDown("2")) NumKeySwitch(1);
        if (Input.GetKeyDown("3")) NumKeySwitch(2);
        if (Input.GetKeyDown("4")) NumKeySwitch(3);
        if (Input.GetKeyDown("5")) NumKeySwitch(4);
        if (Input.GetKeyDown("6")) NumKeySwitch(5);
        //theres probably a more efficient way to do this (ie with less writing)
    }

    void NumKeySwitch(int weapon) //switches via number key
    {

#pragma warning disable CS0642 // disables "Possible mistaken empty statement" warning
        try { if (WEAPONS[weapon]) ; }
        catch { Debug.LogWarning(weapon + 1 + " is not in inventory"); return; }
        //^^ checks if key pressed is in array

        WEAPONS[x].SetActive(false); //sets current weapon inactive
        x = weapon; //makes x = the button pressed
        current = WEAPONS[x]; //updates current
        WEAPONS[x].SetActive(true); //updates active weapon
    }

    void MeleeStrike()
    {
        if (striking) //moving melee weapon forward
        {
            counter++; //starts adding to counter
            meleeWeapon.transform.Rotate(Vector3.up, rotationSpeed); //starts moving weapon forward
            if (counter >= countLimit) 
            {
                striking = false;
                audioSource.Play();
                retracting = true; //stops moving weapon forwrard once timer reaches above value
            }
        }
        else if (retracting) //moving melee weapon backwards
        {
            counter--; //starts removing to the counter
            meleeWeapon.transform.Rotate(Vector3.up, -rotationSpeed); //starts moving weapon backward 
            if (counter <= 0) //if counter is less than 0
            {
                counter = 0; //resets counter at 0
                retracting = false; //disables melee system
                meleeWeapon.transform.position = originalPos.position; //resets position of weapon incase anything small has chnaged
                meleeWeapon.SetActive(false); //disables melee weapon
                WEAPONS[x].SetActive(true); //re-enables players old weapon
            }
        }
        else //neither is happening so is detecting input
        {
            //checks for input either on F key at any time or if the melee weapon is the current equipped weapon, then just the shoot button 
            //also cant melee attack if melee attack is already happening
            if ((Input.GetKeyDown(KeyCode.F) || ((Input.GetMouseButtonDown(0) && WEAPONS[x] == decorStaff))) && (striking == false || retracting == false))
            {
                WEAPONS[x].SetActive(false); //disables current  weapon
                meleeWeapon.SetActive(true); //enables melee weapon
                originalPos = meleeWeapon.transform; //ensures proper position is gathered
                meleeWeapon.GetComponent<MeleeStrike>().hitCounter = 0; //resets the hitcounter
                //see MeleeStrike.cs for more
                striking = true; //starts the striking process
            }
        }
    }
}
