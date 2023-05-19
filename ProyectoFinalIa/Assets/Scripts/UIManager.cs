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

                ActiveButtons();
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

    void DesactiveButtons()
    {
        DesactiveSell();
        DesactiveLengthUpdate();
        DesactiveDamageUpdate();
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
        damage.SetActive(true);
        damageCost.SetActive(true);
        if (int.Parse(moneyText.text) >= int.Parse(damageCost.GetComponent<Text>().text))
            damageCost.GetComponent<Text>().color = Color.green;
        else damageCost.GetComponent<Text>().color = Color.red;
    }

    void DesactiveDamageUpdate()
    {
        damageCost.SetActive(false);
        damage.SetActive(false);
    }

    void ActiveLengthUpdate()
    {
        length.SetActive(true);
        lengthCost.SetActive(true);
        if (int.Parse(moneyText.text) >= int.Parse(lengthCost.GetComponent<Text>().text))
            lengthCost.GetComponent<Text>().color = Color.green;
        else lengthCost.GetComponent<Text>().color = Color.red;

    }
    void DesactiveLengthUpdate()
    {
        length.SetActive(false);
        lengthCost.SetActive(false);
    }

    public void Reset()
    {
        SceneManager.LoadScene("EscenaFinal");
    }


}
