using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static List<GameObject> towers = new List<GameObject>();
    public static List<GameObject> enemies = new List<GameObject>();


    [SerializeField]
    private float cameraSensX=3f;
    [SerializeField]
    private float cameraSensY=3f;

    float cont;
    float moneyTime=0.4f;

    private Color c;

    public UIManager uiManager;
    GameObject selectCell;
    private int money = 250;
    private int lives = 100;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                _instance = go.AddComponent<GameManager>();
                
            }
            return _instance;
        }
    }

    public void UpdateCameraInfo(float newSensX, float newSensY)
    {
        cameraSensX = newSensX;
        cameraSensY = newSensY;
    }

    public Vector2 GetCameraInfo()
    {
        return new Vector2(cameraSensX, cameraSensY);
    }

    public GameObject getSelectCell() { 
        return selectCell;
    }
    private void Update()
    {
        if (cont >= moneyTime)
        {
            AddMoney(1);
            cont = 0f;
        }
        else cont += Time.deltaTime;
    }

    public UIManager getUIManager()
    {
        return uiManager;
    }

    public void deselectCell()
    {
        GameObject.FindGameObjectsWithTag("Particulas")[0].GetComponentInChildren<ParticleSystem>().Stop();
        if (selectCell != null)
        {
            selectCell.GetComponent<MeshRenderer>().material.color = c;
            showRadius(false);

        }

        selectCell = null;
        uiManager.addUI();

    }


    public void setSelectCell(GameObject cell)
    {
        GameObject.FindGameObjectsWithTag("Particulas")[0].GetComponentInChildren<ParticleSystem>().Play();

        if (selectCell != null)
        {
            showRadius(false);
            selectCell.GetComponent<MeshRenderer>().material.color = c;
        }

        uiManager = GameObject.FindWithTag("Canvas").GetComponent<UIManager>();

        selectCell = cell;
        c = selectCell.GetComponent<MeshRenderer>().material.color; ;
        selectCell.GetComponent<MeshRenderer>().material.color = Color.blue;
        GameObject.FindGameObjectsWithTag("Particulas")[0].transform.position = selectCell.transform.position;
        showRadius(true);
        uiManager.addUI();



    }


    public void setUIManger(UIManager ui)
    {
        uiManager = ui;
        uiManager.updateMoney(money);
        uiManager.updateVidas(lives);
    }

    public bool enoughMoney(int money_)
    {
        return money >= money_;
    }
    public void AddMoney(int money_)
    {
        money += money_;
        uiManager.updateMoney(money);

    }

    public void subLives(int lives_)
    {
        lives -= lives_;
        uiManager.updateVidas(lives);
        if (lives <= 0)
        {
            SceneManager.LoadScene("EscenaFinal");
        }
    }
    public void showRadius( bool activar)
    {
        if (selectCell.transform.childCount > 0)
        {
            LineRenderer lr = selectCell.transform.GetChild(0).GetComponent<LineRenderer>();
            DrawRadius dr = selectCell.transform.GetChild(0).GetComponent<DrawRadius>();


            if(lr)lr.enabled = activar;
            if(dr)dr.enabled = activar;
        }
       
    }

    public void removeFromTowers(GameObject gO)
    {
        for(int x = 0; x < towers.Count; x++)
        {
           if(towers[x]) 
                towers[x].GetComponent<Tower>().removeEnemyFromList(gO);
        }
    }
    public void addTower(GameObject t) {
        towers.Add(t);
    } 
    public void removeTower(GameObject t) { 
        towers.Remove(t);
    } 
}
