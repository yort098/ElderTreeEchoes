using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private GameObject lightBullet;
    [SerializeField] private GameObject waterShot;
    [SerializeField] private GameManager manager;
    private List<GameObject> lightShots = new List<GameObject>();
    private List<GameObject> waterShots = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (lightShots.Count > 5)
        {
            Destroy(lightShots[0]);
            lightShots.RemoveAt(0);
        }

        if (waterShots.Count > 10)
        {
            Destroy(waterShots[0]);
            waterShots.RemoveAt(0);
        }
    }

    public void GenerateLightAttack(Vector2 position, Vector2 mouseScreenLocation)
    {
        if (manager.LightEnergy >= 12)
        {
            GameObject shot = Instantiate(lightBullet, position, Quaternion.identity);
            shot.GetComponent<Projectile>().setDirectionMouse(mouseScreenLocation);
            shot.GetComponent<Projectile>().Fire();
            lightShots.Add(shot);
            manager.DepleteEnergy(ProjectileType.Light, 12);
        }
    }

    public void GenerateWaterShot(Vector2 position, Vector2 mouseScreenLocation)
    {
        if (manager.WaterEnergy >= 8)
        {
            GameObject shot = Instantiate(waterShot, position, Quaternion.identity);
            shot.GetComponent<Projectile>().setDirectionMouse(mouseScreenLocation);
            shot.GetComponent<Projectile>().Fire();
            waterShots.Add(shot);
            manager.DepleteEnergy(ProjectileType.Water, 8);
        }
    }
}
