using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    void LateUpdate()
    {
        Vector3 newPosition=player.position;
        newPosition.y=transform.position.y;
        transform.position=newPosition;
    }
}
