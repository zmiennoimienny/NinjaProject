using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPick : MonoBehaviour {


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetButtonDown("RockPick"))
            {
                collision.gameObject.SendMessage("RockPick");
                Destroy(gameObject);
            }
        }
    }


}
