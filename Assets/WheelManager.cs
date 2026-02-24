using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class WheelManager : MonoBehaviour
{
    public Transform wheelTransform; // Assign your wheel GameObject's Transform here
    public Button spinButton;
    public float spinDuration = 4f;   // Duration of the spin in seconds
    public int fullRotations = 5;     // Full 360° rotations before slowing down
    public TextMeshProUGUI resultText;           // Optional: UI text to show prize result

    private bool isSpinning = false;
    private int numberOfPrizes = 4;

    public bool hasSpun = false;

    public GameObject EarnedRewardUI;
    public TextMeshProUGUI earnedStatementText;
    void Start()
    {
        spinButton.onClick.AddListener(SpinWheel);
    }

    public void SpinWheel()
    {
        if (isSpinning) return;

        isSpinning = true;
        spinButton.interactable = false;

        // Spin to a random angle (0 to 360) + full rotations
        //  float finalAngle = Random.Range(0f, 360f);

       
        // Choose from fixed target angles
        float[] fixedAngles = { 0f, 90f, 180f, 270f };
        float chosenAngle = fixedAngles[Random.Range(0, fixedAngles.Length)];

        // Add full rotations for animation
        float totalRotation = (360f * fullRotations) + chosenAngle;

        StartCoroutine(Spin(totalRotation));
    }

    private IEnumerator Spin(float totalAngle)
    {
        float elapsed = 0f;
        float startRotation = wheelTransform.eulerAngles.z;
        float endRotation = startRotation + totalAngle;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / spinDuration;

            // Ease out (deceleration)
            float easedT = 1f - Mathf.Pow(1f - t, 3); // Cubic ease-out

            float zRotation = Mathf.Lerp(startRotation, endRotation, easedT);
            wheelTransform.eulerAngles = new Vector3(0f, 0f, zRotation);
            yield return null;
        }

        // Final rotation
        float finalZ = endRotation % 360f;
        wheelTransform.eulerAngles = new Vector3(0f, 0f, finalZ);

        isSpinning = false;
        spinButton.interactable = false;// cannot spin again unless allowed

        // Determine the prize index based on where the wheel stopped
        int prizeIndex = GetPrizeIndexFromAngle(finalZ);

        GrantPrize(prizeIndex);
    }

    private int GetPrizeIndexFromAngle(float angle)
    {
        float anglePerPrize = 360f / numberOfPrizes;

        // Convert angle to clockwise from top (Unity's rotation is counter-clockwise)
        float clockwiseAngle = (360f - angle + anglePerPrize / 2f) % 360f;

        int index = Mathf.FloorToInt(clockwiseAngle / anglePerPrize);
        return index;
    }

    private void GrantPrize(int prizeIndex)
    {
        // Replace this with your actual prize-granting logic
        string[] prizeNames = { "Respawn", "5 Coins", "Extra Spin", "10 Coins" };
        string prize = prizeNames[prizeIndex];

        Debug.Log("Player won: " + prize);

        if (resultText != null)
        {
            resultText.text = "You won: " + prize;
        }

        // TODO: Add logic to actually give prize to the player

        switch (prize)
        {
            case "Respawn":
                EndGameManager.instance.ExtraLifeRespawn();
                spinButton.interactable = true;

                break;
            case "5 Coins":
                CoinBalanceHolder.Instance.AddVirtualCurrency(5);

                EarnedRewardUI.SetActive(true);
                earnedStatementText.text = "5 Coins(0.0033$GUND)";

                StartCoroutine(waitAndMoveToNextStep());
                break;
            case "Extra Spin":
                // allow for another spin

                spinButton.interactable = true;

                break;
            case "10 Coins":
                CoinBalanceHolder.Instance.AddVirtualCurrency(10);

                EarnedRewardUI.SetActive(true);
                earnedStatementText.text = "10 Coins(0.0066$GUND)";

                StartCoroutine(waitAndMoveToNextStep());
                break;
        }

        IEnumerator waitAndMoveToNextStep()
        {
            yield return new WaitForSeconds(3f);
            EndGameManager.instance.CompletedSpin();
        }
    }

}
