using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankReset : MonoBehaviour
{
    private void Start()
    {
        instance = this;

        for(int i=0;i<tanks.Length;i++)
        {
            tankData.Add(new TankData(tanks[i].transform.position, tanks[i].transform.eulerAngles.z, tanks[i].barrelRotation, tanks[i].enabled));
        }
    }

    public void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            risaTank.Running = false;
        }
    }

    public void ResetGame()
    {
        if (Timer.instance != null)
            Timer.Stop();

        if (risaTank.Running)
            risaTank.Run();

        foreach (Bullet bullet in FindObjectsOfType<Bullet>())
            Destroy(bullet.gameObject);

        for(int i=0;i<tanks.Length;i++)
        {
            tanks[i].gameObject.SetActive(true);
            tanks[i].enabled = tankData[i].enabled;
            tanks[i].ResetHealth();
            tanks[i].transform.position = tankData[i].position;
            tanks[i].transform.eulerAngles = new Vector3(0, 0, tankData[i].rotation);
            tanks[i].barrelRotation = tankData[i].barrelRotation;

            Rigidbody2D tankRb = tanks[i].GetComponent<Rigidbody2D>();
            tankRb.velocity = Vector2.zero;
            tankRb.angularVelocity = 0;
        }
    }

    List<TankData> tankData = new List<TankData>();
    [SerializeField] RisaTankController risaTank;
    [SerializeField] Tank[] tanks;

    public static TankReset instance;
}

public class TankData
{
    public Vector2 position;
    public float rotation;
    public float barrelRotation;
    public bool enabled;

    public TankData(Vector2 position, float rotation, float barrelRotation, bool enabled)
    {
        this.position = position;
        this.rotation = rotation;
        this.barrelRotation = barrelRotation;
        this.enabled = enabled;
    }
}
