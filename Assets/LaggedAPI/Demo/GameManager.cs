using System;
using UnityEngine;
using UnityEngine.UI;

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
