using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text moneyText;
    public Text livesText;

    public GameObject buy;
    public GameObject buyCost;
    public GameObject sell;
    public GameObject sellCost;
    public GameObject length;
    public GameObject lengthCost;
    public GameObject damage;
    public GameObject damageCost;
    public GameObject money;
    public GameObject moneyCost;


    void Start()
    {
        GameManager.Instance.setUIManger(this);
    }

    public void updateMoney(int money)
    {
        moneyText.text = money.ToString();
        addUI();
    }

    public void updateVidas(int vidas)
    {
        livesText.text = vidas.ToString();
    }

    public void addUI()
    {
        GameObject towerSelected = null;
        if (GameManager.Instance.getSelectCell() && GameManager.Instance.getSelectCell().transform.childCount > 0)
        {
            towerSelected = GameManager.Instance.getSelectCell().transform.GetChild(0).gameObject;
            if (towerSelected.GetComponent<Tower>())
            {

                buy.SetActive(false);
                buyCost.SetActive(false);

                DesactiveButtons();

                ActiveButtons();
            }
            else if (towerSelected.GetComponent<PanelSolar>())
            {
                buy.SetActive(false);
                buyCost.SetActive(false);

                DesactiveButtons();

                ActivePanelButtons();
            }
            else
            {
                ActiveBougth();
                DesactiveButtons();
            }
        }
        else
        {
            ActiveBougth();
            DesactiveButtons();
        }
    }
    

    void ActiveBougth()
    {
        buy.SetActive(true);
        buyCost.SetActive(true);
        if (int.Parse(moneyText.text) >= int.Parse(buyCost.GetComponent<Text>().text))
            buyCost.GetComponent<Text>().color = Color.green;
        else buyCost.GetComponent<Text>().color = Color.red;
    }

    void ActiveButtons()
    {
        ActiveSell();
        ActiveLengthUpdate();
        ActiveDamageUpdate();
    }

    void ActivePanelButtons()
    {
        ActiveSell();
        ActiveMoneyUpdate();
    }

    void DesactiveButtons()
    {
        DesactiveSell();
        DesactiveLengthUpdate();
        DesactiveDamageUpdate();
        DesactiveMoneyUpdate();
    }

    void ActiveSell()
    {
        sell.SetActive(true);
        sellCost.SetActive(true);
        sellCost.GetComponent<Text>().color = Color.red;

    }

    void DesactiveSell()
    {
        sell.SetActive(false);
        sellCost.SetActive(false);
    }

    void ActiveDamageUpdate()
    {
        GameObject towerSelected = GameManager.Instance.getSelectCell().transform.GetChild(0).gameObject;
        if (!towerSelected.GetComponent<Tower>().getUpdatedDamage())
        {
            damage.SetActive(true);
            damageCost.SetActive(true);
            if (int.Parse(moneyText.text) >= int.Parse(damageCost.GetComponent<Text>().text))
                damageCost.GetComponent<Text>().color = Color.green;
            else damageCost.GetComponent<Text>().color = Color.red;
        }
        else DesactiveDamageUpdate();
    }

    void DesactiveDamageUpdate()
    {
        damageCost.SetActive(false);
        damage.SetActive(false);
    }


    void ActiveLengthUpdate()
    {
        GameObject towerSelected = GameManager.Instance.getSelectCell().transform.GetChild(0).gameObject;
        if (!towerSelected.GetComponent<Tower>().getUpdatedLength())
        {
            length.SetActive(true);
            lengthCost.SetActive(true);
            if (int.Parse(moneyText.text) >= int.Parse(lengthCost.GetComponent<Text>().text))
                lengthCost.GetComponent<Text>().color = Color.green;
            else lengthCost.GetComponent<Text>().color = Color.red;
        }
        else DesactiveLengthUpdate();

    }


    void ActiveMoneyUpdate()
    {
        GameObject towerSelected = GameManager.Instance.getSelectCell().transform.GetChild(0).gameObject;
        if (!towerSelected.GetComponent<PanelSolar>().getUpdateMoney())
        {
            money.SetActive(true);
            moneyCost.SetActive(true);
            if (int.Parse(moneyText.text) >= int.Parse(moneyCost.GetComponent<Text>().text))
                moneyCost.GetComponent<Text>().color = Color.green;
            else moneyCost.GetComponent<Text>().color = Color.red;
        }
        else DesactiveMoneyUpdate();

    }

    void DesactiveLengthUpdate()
    {
        length.SetActive(false);
        lengthCost.SetActive(false);
    }
    void DesactiveMoneyUpdate()
    {
        money.SetActive(false);
        moneyCost.SetActive(false);
    }

    public void Reset()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
