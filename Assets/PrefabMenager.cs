using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabMenager : MonoBehaviour
{
    public static PrefabMenager prefabMenager;

    public GameObject PopUp;

    void Awake()
    {
        prefabMenager = this;
    }


}
