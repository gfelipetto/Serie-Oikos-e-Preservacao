using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalType : MonoBehaviour
{
    public AnimalTypeEnum animalTypeVar;
    public enum AnimalTypeEnum
    {
        Null,
        Fish,
        Snake,
        Alligator
    }
}
