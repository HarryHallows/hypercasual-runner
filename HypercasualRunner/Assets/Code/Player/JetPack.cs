using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float offset;
    // Start is called before the first frame update
    void Start()
    {
        //player = GetComponent<PlayerController>().transform.parent.gameObject;    
    }

    // Update is called once per frame
    void Update()
    {
        //Rotation();
    }

    private void Rotation()
    {
        Vector3 _eulerRotation = new Vector3(player.transform.eulerAngles.x, player.transform.eulerAngles.y, player.transform.eulerAngles.z);
        Quaternion _rotation = Quaternion.Euler(_eulerRotation);
        transform.rotation = _rotation;
    }

    public void Scale(float _fuel)
    {
        //increase scale == to amount of fuel if < fuelcapacity
        transform.localScale = new Vector3(_fuel / 2, _fuel / 2, _fuel / 2);
    }
}
