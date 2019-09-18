using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrassLOD : MonoBehaviour
{
    public Terrain terrainObject1;
    public Terrain terrainObject2;
    public Slider sliderObject;

    void Update ()
    {
        terrainObject1.detailObjectDensity = sliderObject.value;
        terrainObject2.detailObjectDensity = sliderObject.value;
	}
}