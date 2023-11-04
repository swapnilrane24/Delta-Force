#UNTIY PLUGIN

Setup:
 - Drag the prefab "LaggedAPIUnity" into your scene
 - Replace the DEV_ID and PUBLISHER_ID value with your IDs
 - Test the Demo to make sure everything is working
 - View the GameManager.cs for examples of how to implement the API
 - Use LaggedAPIUnity.Instance.ShowAd() to show an advertisement
 - Use LaggedAPIUnity.Instance.CheckRewardAd() to check if reward is available
 - Use LaggedAPIUnity.Instance.PlayRewardAd() to play reward ad
 - Make use of the events OnPauseGame and OnResumeGame for resuming/pausing your game in between ads
 - Make use of the events onRewardAdReady, onRewardAdSuccess and onRewardAdFailure for Reward Ads
 - Use LaggedAPIUnity.Instance.CallHighScore(score, boardID) to save a high score
 - Use LaggedAPIUnity.Instance.SaveAchievement(awardID) to save an achievement

Example:

public class GameManager : MonoBehaviour
{
    public Text gameControlText;
    public Button callRewardAd;

    private string usingBoardID;

    void Awake()
    {
        LaggedAPIUnity.OnResumeGame += OnResumeGame;
        LaggedAPIUnity.OnPauseGame += OnPauseGame;
        LaggedAPIUnity.onRewardAdReady += onRewardAdReady;
        LaggedAPIUnity.onRewardAdSuccess += onRewardAdSuccess;
        LaggedAPIUnity.onRewardAdFailure += onRewardAdFailure;
    }

    public void OnResumeGame()
    {
        //
        // Ad is complete, Resume game/music
        //

        gameControlText.text = "Ad completed - RESUME GAME";
    }

    public void OnPauseGame()
    {
        //
        // Ad is started, pause game/music
        //

        gameControlText.text = "Ad running - GAME PAUSED";
    }

    public void onRewardAdReady()
    {
        //
        // reward ad is avaible and ready to play, you can show user reward button
        //

        gameControlText.text = "Reward ad is ready";

        //
        // show/create reward button
        //

        callRewardAd.interactable=true;

    }

    public void onRewardAdSuccess()
    {
        //
        // reward ad is sucessful, give user a reward
        //

        gameControlText.text = "Reward ad succesful, give user reward";

        //
        // hide/remove reward button
        //

        callRewardAd.interactable=false;

    }

    public void onRewardAdFailure()
    {
        //
        // reward ad failed, do not give user a reward
        //

        gameControlText.text = "Reward ad failure";

        //
        // hide/remove reward button
        //

        callRewardAd.interactable=false;

    }

    public void ShowAd()
    {

        //
        // Show a normal ad
        //

        LaggedAPIUnity.Instance.ShowAd();
    }

    public void CheckRewardAd()
    {
      //
      // Check if reward ad is available
      //

      gameControlText.text = "Checking reward ad...";

      LaggedAPIUnity.Instance.CheckRewardAd();

    }

    public void PlayRewardAd()
    {
      //
      // play reward ad if avaible
      //

      gameControlText.text = "Playing reward ad if available...";

      LaggedAPIUnity.Instance.PlayRewardAd();

    }

    public void SetBoardID(string board){

      //
      // sets board_id for High scores
      //

        usingBoardID=board;
    }

    public void CallHighScore(int score)
    {
      //
      // save high score
      //
      LaggedAPIUnity.Instance.CallHighScore(score, usingBoardID);
    }

    public void SaveAchievement(string award)
    {
      //
      // save achievement
      //
        LaggedAPIUnity.Instance.SaveAchievement(award);
    }
}