using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool spotTaken = false;

    public bool SpotTaken(bool _taken)
    {
        if (_taken)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }

        return spotTaken = _taken;
    }

}
