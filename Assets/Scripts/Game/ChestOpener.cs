using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestOpener : MonoBehaviour
{
    [SerializeField] private EnergyProbability mEnergyProbability;
    [SerializeField] private CoinProbability mCoinProbability;
    [SerializeField] private List<GameObject> mEnergyChests;
    [SerializeField] private List<GameObject> mCoinChests;
    [SerializeField] private GameManager mGameManager;

    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private GameObject RewardDisplayPanel;

    private void Start()
    {
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out raycastHit))
            {
                if (raycastHit.transform.gameObject.tag == "EnergyChestBox")
                {
                    //Get the Energy value of probability
                    int energyValue = mEnergyProbability.DisplayTheFinalElementBasedOnRandomValueGenerated();
                    rewardText.text = energyValue.ToString();
                    RewardDisplayPanel.SetActive(true);

                    //Changing the Energy value in Gamemanager
                    mGameManager._energy += energyValue;

                    //Assign it to chest which player clicks on and pass the values
                    ChestValue Chest = raycastHit.transform.gameObject.GetComponent<ChestValue>();
                    Chest._value = energyValue;
                    
                    EnergyListShuffle(mEnergyProbability._energies);

                    //Assign other values from Energy's Array to Chest Value apart from the Value that we got above
                    for (int i = 0; i < mEnergyChests.Count; i++)
                    {
                        if (mEnergyChests[i].GetComponent<ChestValue>()._value == energyValue)
                        {
                            mEnergyChests.Remove(mEnergyChests[i]);
                        }
                        if (mEnergyProbability._energies[i]._energyAmount == energyValue)
                        {
                            mEnergyProbability._energies.Remove(mEnergyProbability._energies[i]);
                        }
                        mEnergyChests[i].GetComponent<ChestValue>()._value = mEnergyProbability._energies[i]._energyAmount;
                    }
                    //Invoke(nameof(BackToMainScene), 1f);
                }
                if (raycastHit.transform.gameObject.tag == "CoinChestBox")
                {
                    //Get the Coin value of probability
                    int coinValue = mCoinProbability.DisplayTheFinalElementBasedOnRandomValueGenerated();
                    rewardText.text = coinValue.ToString();
                    RewardDisplayPanel.SetActive(true);

                    //Changing the Coinvalue
                    mGameManager._coins += coinValue;

                    //Assign it to chest which player clicks on and pass the values
                    ChestValue Chest = raycastHit.transform.gameObject.GetComponent<ChestValue>();
                    Chest._value = coinValue;
                    
                    CoinListShuffle(mCoinProbability._coins);

                    //Assign other values from Coin's Array to Chest Value apart from the Value that we got above
                    for (int i = 0; i < mCoinChests.Count; i++)
                    {
                        if (mCoinChests[i].GetComponent<ChestValue>()._value == coinValue)
                        {
                            mCoinChests.Remove(mCoinChests[i]);
                        }
                        if (mCoinProbability._coins[i]._coinAmount == coinValue)
                        {
                            mCoinProbability._coins.Remove(mCoinProbability._coins[i]);
                        }
                        mCoinChests[i].GetComponent<ChestValue>()._value = mCoinProbability._coins[i]._coinAmount;
                    }
                    //Invoke(nameof(BackToMainScene), 1f);
                }
            }
        }
    }

    /// <summary>
    /// Loads back to active level
    /// </summary>
    public void BackToMainScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); //Need to change it from zero to some other value. Will be doing that when scene save system is Done.
    }

    
    /// <summary>
    /// Below both the methods are done for natural looking purposes after clicking it shuffles the list randomly which sometimes makes the higher values to be beside what
    /// we clicked creating the anticipation factor.
    /// </summary>
    /// <param name="inEnergyList"></param>
    private void EnergyListShuffle(List<Energy> inEnergyList)
    {
        for (int i = 0; i < inEnergyList.Count; i++)
        {
            Energy temp = inEnergyList[i];
            int randomIndex = Random.Range(i, inEnergyList.Count);
            inEnergyList[i] = inEnergyList[randomIndex];
            inEnergyList[randomIndex] = temp;
        }
    }
    private void CoinListShuffle(List<Coins> inCoinList)
    {
        for (int i = 0; i < inCoinList.Count; i++)
        {
            Coins temp = inCoinList[i];
            int randomIndex = Random.Range(i, inCoinList.Count);
            inCoinList[i] = inCoinList[randomIndex];
            inCoinList[randomIndex] = temp;
        }
    }
}
