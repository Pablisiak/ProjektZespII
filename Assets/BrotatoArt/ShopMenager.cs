using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenager : MonoBehaviour
{
    public List<Item> Items;
    public List<Shop> Shops;

    public void Roll()
    {
        foreach(Shop shop in Shops)
        {
            shop.gameObject.SetActive(true);
            shop.Roll();
            
        }
    }
}
