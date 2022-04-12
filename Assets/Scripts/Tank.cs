using Risa;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public void Damage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            GameObject vfx = Instantiate(deathVfx, transform.position, Quaternion.identity);
            Destroy(vfx, 2f);

            gameObject.SetActive(false);

            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/destroy"), Camera.main.transform.position);
        }
        else AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/hit"), Camera.main.transform.position);
    }

    public void ResetHealth()
    {
        health = _initHealth;
    }

    public Value GetPosition(VM vm, Args args)
    {
        Vector2 position = transform.position;

        ValueObject vector = vm.CreateObject();
        vector.Set("x", vm.CreateFloat(position.x));
        vector.Set("y", vm.CreateFloat(position.y));

        return vector.ToValue();
    }

    public Value GetVelocity(VM vm, Args args)
    {
        Vector2 velocity = rb.velocity;

        ValueObject vector = vm.CreateObject();
        vector.Set("x", vm.CreateFloat(velocity.x));
        vector.Set("y", vm.CreateFloat(velocity.y));

        return vector.ToValue();
    }

    public Value GetRotation(VM vm, Args args)
    {
        float rotation = transform.eulerAngles.z;
        return vm.CreateFloat(rotation);
    }

    public Value GetBarrelRotation(VM vm, Args args)
    {
        return vm.CreateFloat(barrelRotation);
    }

    public Value IsAlive(VM vm, Args args)
    {
        return vm.CreateBool(gameObject.activeInHierarchy);
    }

    private void LateUpdate()
    {
        gunPivot.eulerAngles = new Vector3(0, 0, barrelRotation);
    }

    protected void Move(float dir)
    {
        if (dir > 1)
            dir = 1;
        if (dir < -1)
            dir = -1;

        _addedMovement += dir * movementSpeed;
    }

    float _addedRotation, _addedBarrelRotation, _addedMovement;
    protected void Rotate(float dir)
    {
        if (dir > 1)
            dir = 1;
        if (dir < -1)
            dir = -1;

        _addedRotation += rotationSpeed * dir * Time.deltaTime;
    }

    protected void RotateBarrel(float dir)
    {
        if (dir > 1)
            dir = 1;
        if (dir < -1)
            dir = -1;

        _addedBarrelRotation += barrelRotationSpeed * dir * Time.deltaTime;
    }

    protected void RotateTowards(float angle, float amplifier)
    {
        if (amplifier > 1f)
            amplifier = 1f;
        if (amplifier < -1f)
            amplifier = -1f;

        _addedRotation += Mathf.DeltaAngle(transform.eulerAngles.z, Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * amplifier * Time.deltaTime).eulerAngles.z);
    }

    protected void RotateBarrelTowards(float angle, float amplifier)
    {
        if (amplifier > 1f)
            amplifier = 1f;
        if (amplifier < -1f)
            amplifier = -1f;

        _addedBarrelRotation += Mathf.DeltaAngle(barrelRotation, Quaternion.RotateTowards(bulletPosition.rotation, Quaternion.Euler(0, 0, angle), barrelRotationSpeed * amplifier * Time.deltaTime).eulerAngles.z);
    }

    protected void Shoot()
    {
        if (!CanShoot)
            return;

        GameObject bullet = Instantiate(this.bullet);
        bullet.transform.position = bulletPosition.position;
        bullet.transform.up = bulletPosition.right;
        bullet.GetComponent<Rigidbody2D>().velocity = bulletPosition.right * bulletSpeed;

        gunBarrel.Play("shoot");
        gunParticles.Play();

        source.Play();
    }

    float _initHealth;
    AudioSource source;
    private void Awake()
    {
        _initHealth = health;

        rb = GetComponent<Rigidbody2D>();
        gunBarrel.speed = fireRate;

        barrelRotation = transform.eulerAngles.z;

        source = gameObject.AddComponent<AudioSource>();
        source.clip = Resources.Load<AudioClip>("Sounds/shoot");
    }

    bool CanShoot
    {
        get
        {
            return gunBarrel.GetCurrentAnimatorStateInfo(0).IsName("idle");
        }
    }

    protected virtual void Update()
    {
        if (_addedMovement > movementSpeed)
            _addedMovement = movementSpeed;
        else if (_addedMovement < -movementSpeed)
            _addedMovement = -movementSpeed;

        rb.AddForce(transform.right * _addedMovement);

        if (_addedRotation > barrelRotationSpeed * Time.deltaTime)
            _addedRotation = barrelRotationSpeed * Time.deltaTime;
        else if (_addedRotation < -barrelRotationSpeed * Time.deltaTime)
            _addedRotation = -barrelRotationSpeed * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, 0, _addedRotation);

        if (_addedBarrelRotation > barrelRotationSpeed * Time.deltaTime)
            _addedBarrelRotation = barrelRotationSpeed * Time.deltaTime;
        else if (_addedBarrelRotation < -barrelRotationSpeed * Time.deltaTime)
            _addedBarrelRotation = -barrelRotationSpeed * Time.deltaTime;
        barrelRotation += _addedBarrelRotation;
        barrelRotation %= 360;

        _addedMovement = _addedRotation = _addedBarrelRotation = 0;
    }

    [Header("Properties")]
    [SerializeField] float fireRate;
    [SerializeField] float bulletSpeed;
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float barrelRotationSpeed;
    [SerializeField] float health;

    [Space(20)]
    [Header("Objects")]
    [SerializeField] protected Transform gunPivot;
    [SerializeField] protected Animator gunBarrel;
    [SerializeField] protected Transform bulletPosition;
    [SerializeField] protected ParticleSystem gunParticles;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected GameObject deathVfx;

    Rigidbody2D rb;
    public float barrelRotation;
}
