using UnityEngine;
using PlayFab.ClientModels;

public class FoxCharacterWinScreenAddCrystals : FoxCharacterWinScreen
{
    protected override void OnWin()
    {
        // Data from the level we just finished
        int crystalsCount = this.FoxCharacterInventory.jewelsCount;

        // TODO: Use player stats to register virtual currency increase
        AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest()
		{
			VirtualCurrency = "CR",
			Amount = crystalsCount
		};
        PlayFab.PlayFabClientAPI.AddUserVirtualCurrency(request, (result) =>
            {
                Debug.Log("You have received " + crystalsCount + " crystals!");
            }, (error) =>
            {
                Debug.LogError(error.GenerateErrorReport());
            }
        );

        // Call base function from the class "FoxCharacterWinScreen" to display our score on the end screen & show buttons to go back to the Menu
        base.OnWin();
    }
}
