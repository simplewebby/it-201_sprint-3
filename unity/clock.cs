using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class clock : MonoBehaviour
{
    const float
         degreesPerHour = 30f,
         degreesPerMinute = 6f,
         degreesPerSecond = 6f,
        degreePerDayCycle = 15f;

    public Transform hoursTransform;
    public Transform minutesTransform;
    public Transform secondsTransform;
    private DateTime timeDiscrete;
    private TimeSpan timeCont;
    public bool continuous = true;

    public Transform dayCycle;
   // [Range(0, 23)]

    public Text textAMPM;
    private bool isAM;


    private float hours, minutes, hoursTemp;


    void Awake()
    {
        timeDiscrete = DateTime.Now;
        timeCont = DateTime.Now.TimeOfDay;

        hours = (float)timeCont.TotalHours;
        minutes = (float)timeCont.TotalMinutes;

        hoursTransform.localRotation = Quaternion.Euler(0f, hours * degreesPerHour, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, hours * degreesPerMinute, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, (float)timeCont.TotalSeconds * degreesPerSecond, 0f);

        if(hours< 12)
        {
            isAM = true;
            textAMPM.text = "AM";

        }
        else
        {
            isAM = false;
            textAMPM.text = "PM";
        }


    }


    void Update()
    {
        timeCont = DateTime.Now.TimeOfDay;
        minutes = (float)timeCont.TotalMinutes;
        minutesTransform.localRotation = Quaternion.Euler(0f, minutes * degreesPerMinute, 0f);
        hoursTransform.localRotation = Quaternion.Euler( hours * degreePerDayCycle, 0f, 0f);


        if (continuous)
        {
            UpdateContinuous();
        }
        else
        {
            UpdateDiscrete();
        }
    }



    void UpdateContinuous() {
        secondsTransform.localRotation = Quaternion.Euler(0f, (float)timeCont.TotalSeconds * degreesPerSecond, 0f);

}




void UpdateDiscrete()
{
        timeDiscrete = DateTime.Now;
        secondsTransform.localRotation = Quaternion.Euler(0f, timeDiscrete.Second * degreesPerSecond, 0f);
}




    public void UpdateTime(float clickedHoursRotation)
    {
        hoursTemp = ((float)timeCont.TotalHours - (int)timeCont.TotalHours) + (clickedHoursRotation / degreesPerHour);
        if (!isAM) hoursTemp += 12f;
        if ((int)hoursTemp < (int)hours)
        {
            hours = hoursTemp;
            UpdateAMPM();

        }
        else hours = hoursTemp;
    }


    public void UpdateAMPM()
    {
        if (isAM)
        {
            isAM = false;
            textAMPM.text = "PM";
            hours += 12f;


        }
        else
        {
            isAM = true;
            textAMPM.text = "AM";
            hours -= 12f;
        }
    }


    public float getHours()
    {
        return hours;
    }
}
