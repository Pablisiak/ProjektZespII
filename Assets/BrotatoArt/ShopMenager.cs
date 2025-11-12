using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenager : MonoBehaviour
{
    public List<Item> Items;
    public List<Shop> Shops;

    public void Roll()
    {
        foreach (Shop shop in Shops)
        {
            shop.gameObject.SetActive(true);
            shop.Roll();
        }
    }
    
    public void RollForPlayer(int playerIndex)
    {
        foreach (Shop shop in Shops)
        {
            if (shop.PlayerIndex == playerIndex)
            {
                shop.gameObject.SetActive(true);
                shop.Roll();
            }
        }
    }

}
