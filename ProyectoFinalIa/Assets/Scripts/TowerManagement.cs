using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManagement : MonoBehaviour
{

    public List<GameObject> towers;
    public List<int> towersCosts;
    public GameObject addArrow;
    public GameObject subArrow;
    public GameObject buyCost;
    public Text towerText;
    private int towerIndex = 0;


    public void BuildTower()
    {
        if (GameManager.Instance.enoughMoney(towersCosts[towerIndex]))
        {
            GameObject selected = GameManager.Instance.getSelectCell();
            if (selected != null && selected.transform.childCount == 0)
            {
                GameObject tower = GameObject.Instantiate(towers[towerIndex], selected.transform);
                GameManager.Instance.AddMoney(-towersCosts[towerIndex]);

                if(towerIndex!=1) GameManager.Instance.addTower(tower);

                tower.transform.position = selected.transform.position;
                tower.transform.Translate(new Vector3(0, 0.5f, 0));
                if (towerIndex != 1) GameManager.Instance.showRadius(true);
            }
            if (GameManager.Instance.getUIManager()) GameManager.Instance.getUIManager().addUI();
        }
        
    }


    public void changeTower()
    {
        GameObject towerSelected = null;

        if (GameManager.Instance.getSelectCell().transform.childCount > 0)
        {
            towerSelected = GameManager.Instance.getSelectCell().transform.GetChild(0).gameObject;
            if (towerSelected.GetComponent<Tower>()) GameManager.Instance.uiManager.addUI();
        }
         
    }


    public void updateTowerDamage()
    {
        if (!GameManager.Instance.enoughMoney(80)) return;



        GameObject towerSelected = null;

        if (GameManager.Instance.getSelectCell().transform.childCount > 0)
        {
            towerSelected = GameManager.Instance.getSelectCell().transform.GetChild(0).gameObject;

            //TO DO
        }
    }


    public void AddIndex()
    {
        if (towerIndex < towers.Count - 1)
        {
            towerIndex++;                                                           
            towerText.text = towers[towerIndex].name;
            buyCost.GetComponent<Text>().text = towersCosts[towerIndex].ToString();
            subArrow.SetActive(true);
        }
        if (towerIndex == towers.Count - 1) addArrow.SetActive(false); 
    }

    public void subIndex()
    {
        if (towerIndex > 0)
        {
            towerIndex--;
            towerText.text = towers[towerIndex].name;
            buyCost.GetComponent<Text>().text = towersCosts[towerIndex].ToString();
            addArrow.SetActive(true);
        }
        if (towerIndex == 0) subArrow.SetActive(false);
    }

    public void updateTowerLength()
    {
        if (!GameManager.Instance.enoughMoney(100)) return;

        GameObject towerSelected = null;

        if (GameManager.Instance.getSelectCell().transform.childCount > 0)
        {
            towerSelected = GameManager.Instance.getSelectCell().transform.GetChild(0).gameObject;
            //TO DO
        }
    }

    public void DeleteTower()
    {
        GameObject selected = GameManager.Instance.getSelectCell();
        if (selected != null)
        {
            GameManager.Instance.showRadius(false);
            if (selected.transform.childCount > 0 && selected.transform.GetChild(0).GetComponent<Tower>())
            {
                GameObject tower = selected.transform.GetChild(0).gameObject;
                if (tower != null)
                {
                    GameManager.Instance.removeTower(tower);
                    Destroy(tower);
                    GameManager.Instance.getSelectCell().GetComponent<MeshRenderer>().material.color = Color.green;
                    GameManager.Instance.AddMoney(+80);

                }
            }
        }
        if (GameManager.Instance.getUIManager()) GameManager.Instance.getUIManager().addUI();
    }
}
