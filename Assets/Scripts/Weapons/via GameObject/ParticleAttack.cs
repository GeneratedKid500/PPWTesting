using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attatch to particle firing weapon
public class ParticleAttack : MonoBehaviour
{
    //editor vars
    public ParticleSystem particles;
    public Light[] fuelLights;
    public Material dark;

    [Header ("Attack Variables")]
    public float fuel = 100;
    public float decreaseRate = 0.2f;

    //system vars
    AudioSource audioSource;
    Material red;
    int cFuelLight;
    float maxFuel;
    float[] lightOutVals;
    bool lockout;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        maxFuel = fuel; //sets maximum fuel value to current amount of fuel
        lockout = false;
        cFuelLight = 0; //pointer variable
        red = fuelLights[cFuelLight+1].GetComponent<Renderer>().material; //gets material colour of an object

        //assigns the array values of when the lights should be activated / deactivated. 
        lightOutVals = new float[4] { maxFuel, maxFuel / 2, maxFuel / 4, 1 };
    }

    void Update()
    {
        CheckFuel();
        AlterLights();
    }

    void FixedUpdate() //plays particles, physics play better in fixedUpdate
    {
        if (Input.GetMouseButton(0) && fuel > 0 && !lockout && !particles.isPlaying)
        {
            particles.Play();
            audioSource.Play();
            audioSource.loop = true;
        }
        else if (!Input.GetMouseButton(0))
        {
            particles.Stop();
            audioSource.Stop();
        }
        //using GetMouseButton rather than GetMouseButtonDown 
        //offers more reliable click detection
    }

    void CheckFuel() //deals with all fuel related issues
    {
        if (particles.isEmitting)
        {
            fuel -= decreaseRate; //decreases fuel levels
        }
        else if (fuel < maxFuel) //if isEmitting = false AND if fuel is lower than max
        {
            fuel += decreaseRate; //increases fuel levels
        }

        //bool lockout stops the player from shooting
        //until fuel is full again as a sort of punishment and learning curve
        if (fuel <= 0)
        {
            particles.Stop(); //stops particles
            lockout = true;
        }
        else if (fuel >= maxFuel && lockout)
            lockout = false;
    }

    void AlterLights() //deals with all of the lights on the side of the weapon
    {
        if (fuel < lightOutVals[cFuelLight]) //if fuel val is smaller than the x'th value in the array
        {
            if (cFuelLight != 0) //if the current value of pointer is NOT 0
            {
                //(-1 as there are only 3 lights)
                fuelLights[cFuelLight-1].enabled = false; //disables current light 
                fuelLights[cFuelLight-1].GetComponent<Renderer>().material = dark; //changes material to turn light out
            }
            if (cFuelLight != 3)
                cFuelLight++; //increases value of pointer if current value of pointer is NOT 3
        }
        else if (fuel >= lightOutVals[cFuelLight]) //if fuel val is greater than the x'th value in the array - inverse of above
        {
            if (cFuelLight != 3) // if current value of pointer is NOT 3
            {
                fuelLights[cFuelLight].enabled = true; //enables current light 
                fuelLights[cFuelLight].GetComponent<Renderer>().material = red; //changes material to put light on
            }
            if (cFuelLight != 0)
                cFuelLight--; //decreases value of pointer if current value of pointer is NOT 0
        }
        
        if (fuel >= maxFuel && fuelLights[0].enabled != true)
        {
            fuelLights[0].enabled = true;
            fuelLights[cFuelLight].GetComponent<Renderer>().material = red;  
        } 
        //this is just a temporary debug fix as the final light in the array will sometimes not enable 
    }
}
