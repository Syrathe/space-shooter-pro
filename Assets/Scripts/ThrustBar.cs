using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrustBar : MonoBehaviour
{
    public Slider slider;
    //This is just the variable in which I reference the bar itself.
    
    public void SetThrustBar(int _gas){
        slider.value = _gas;
    }
}
