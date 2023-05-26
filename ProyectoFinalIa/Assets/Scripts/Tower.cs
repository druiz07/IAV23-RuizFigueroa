using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum attackType { FIRST = 0, STRONG = 1 }

public class Tower : MonoBehaviour
{

    public attackType type = attackType.FIRST;
    public GameObject proyectil;

    public float shootCooldown;
    public float shootSpeed;
    public float enemVelocity;
    public int damage = 1;

    bool updatedLength = false, updatedDamage = false;
    
    private List<GameObject> rangeEnemies;
    private GameObject attackedEnemy;
    private bool readyShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        rangeEnemies = new List<GameObject>();
        Invoke("Recharge", shootCooldown);
    }
    
    // Update is called once per frame
    void Update()
    {

        if (readyShoot)
            if (rangeEnemies.Count > 0)
                switch (type)
                {
                    case attackType.FIRST:
                        Shoot(rangeEnemies[0]);
                        attackedEnemy = rangeEnemies[0];
                        break;
                    case attackType.STRONG:
                        GameObject attacked = selectStronger();
                        if (attacked)
                        {
                            Shoot(attacked);
                            attackedEnemy = attacked;
                        }
                        break;

                }
    }


    public bool getUpdatedLength()
    {
        return updatedLength;
    }

    public bool getUpdatedDamage()
    {
        return updatedDamage;
    }

    private GameObject selectStronger()
    {

        List<int> strongerIndex = new List<int>(3);
        for (int x = 0; x < 3; x++)
        {
            strongerIndex.Add(-1);
        }
        for (int x = 0; x < rangeEnemies.Count; x++)
        {
            Enemigo e = rangeEnemies[x].GetComponent<Enemigo>();
            if (e && strongerIndex[2] == -1 && e.tipoEnem == TipoEnemigo.FUERTE)
                strongerIndex[2] = x;
            else if (e && strongerIndex[1] == -1 && e.tipoEnem == TipoEnemigo.DEFAULT)
                strongerIndex[1] = x;
            else if (e && strongerIndex[0] == -1 && e.tipoEnem == TipoEnemigo.RAPIDO)
                strongerIndex[0] = x;
        }

        for (int x = strongerIndex.Count - 1; x >= 0; x--)
        {
            if (strongerIndex[x] > -1)
                return rangeEnemies[strongerIndex[x]];
        }


        return null;
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyMovement>() != null)
            rangeEnemies.Add(other.gameObject);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnemyMovement>() != null)
            rangeEnemies.Remove(other.gameObject);
    }

    private void Shoot(GameObject target)
    {
        if(target != null)
        {
            readyShoot = false;

            GameObject p = proyectil;
            float speed_ = shootSpeed;
            float predictSpeed = shootSpeed;
            float rechargeTime = shootCooldown;

            Invoke("Recharge", rechargeTime);

            GameObject pShoot = Instantiate(p, this.transform);
            pShoot.GetComponent<Proyectil>().setDamage(damage);
            pShoot.GetComponent<Proyectil>().setRadio(this.GetComponent<DrawRadius>().getRadio());
            pShoot.transform.position = this.transform.position;

            Vector3 posPredEnemigo = predictedEnemyPosition(target.transform.position, target.GetComponent<Rigidbody>().velocity, predictSpeed);
            Vector3 shootDir = posPredEnemigo - this.transform.position;

            
            pShoot.GetComponent<Rigidbody>().velocity = finalSpeed(shootDir, speed_);
            
            pShoot.transform.rotation = Quaternion.LookRotation(shootDir);
            pShoot.transform.Rotate(new Vector3(90, 0, 0));
        }
    }
    private Vector3 predictedEnemyPosition(Vector3 targetPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        Vector3 displacement = targetPosition - this.transform.position;
        float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;

        if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed && Mathf.Sin(targetMoveAngle) /
            projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
            return targetPosition;

        float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);
        return targetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle)
            * Mathf.Sin(shootAngle) / targetVelocity.magnitude;
    }

    Vector3 finalSpeed(Vector3 dirDestino, float velAUsar)
    {
        float magnitud = Mathf.Sqrt(Mathf.Pow(dirDestino.x, 2) + Mathf.Pow(dirDestino.y, 2) + Mathf.Pow(dirDestino.z, 2));
        float a = velAUsar / magnitud;
        return new Vector3(a * dirDestino.x, a * dirDestino.y, a * dirDestino.z);
    }
    private void Recharge()
    {
        readyShoot = true;
    }

    public void addLength()
    {
        GetComponent<SphereCollider>().radius = 16;
        GetComponent<DrawRadius>().radius = 16;
        GetComponent<DrawRadius>().CreatePoints();

        updatedLength = true;
    }

    public void addDamage(int damage_)
    {
        damage = damage_;
        updatedDamage = true;
    }

    public void removeEnemyFromList(GameObject gO )
    {
        rangeEnemies.Remove(gO);
    }


}
