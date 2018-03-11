using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShotTrajectory))]
public class Shooting : MonoBehaviour {

    public bool canShoot = true;
    public Transform shootingFrom;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float minShootVectorLenght = 0.1f;
    public float bulletCount = 0;

    private bool hold;
    private Vector3 lastMousePosition;
    private ShotTrajectory shotTrajectory;

    private void Awake()
    {
        shotTrajectory = gameObject.GetComponent<ShotTrajectory>();
    }

    private void Update()
    {

        if (Input.GetMouseButtonUp(0) && bulletCount > 0)
        {
            if(hold)
            {
                Vector3 shotVector = lastMousePosition - GetNormalizedMousePosition();
                if (shotVector.magnitude > minShootVectorLenght)
                {
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<Rigidbody2D>().velocity = shotVector * bulletSpeed;
                    shotTrajectory.ClearTrajectoryLine();
                    bulletCount--;
                }
            }
            hold = false;
        }

        if (Input.GetMouseButtonDown(0) && bulletCount > 0)
        {
            hold = true;
            lastMousePosition = GetNormalizedMousePosition();
        }

        if(hold && bulletCount > 0)
        {
            Vector3 shotVector = lastMousePosition - GetNormalizedMousePosition();
            if(shotVector.magnitude > minShootVectorLenght)
            {
                DrawShootingLine();
            }
        }
    }

    private Vector3 GetNormalizedMousePosition()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        return mousePos;
    }

    private void DrawShootingLine()
    {
        Vector3 shotVector = lastMousePosition - GetNormalizedMousePosition();
        shotTrajectory.DrawTrajectory(transform.position, shotVector * bulletSpeed);
    }

    public void RockPick()
    {
        bulletCount++;
    }
}
