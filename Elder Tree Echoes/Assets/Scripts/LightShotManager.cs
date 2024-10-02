using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShotManager : MonoBehaviour
{
    [SerializeField] private GameObject lightBullet;
    //private LightShot light;
    private List<GameObject> shots = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (shots.Count > 5)
        {
            Destroy(shots[0]);
            shots.RemoveAt(0);
        }
    }

    public void GenerateLightAttack(Vector2 position, bool direction)
    {
        GameObject shot = Instantiate(lightBullet, position, Quaternion.identity);
        shot.GetComponent<LightShot>().setDirection(direction);
        shots.Add(shot);
    }
}
