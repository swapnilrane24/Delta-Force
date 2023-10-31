#UNTIY PLUGIN V3
############################
GameMonetize.com
############################

Setup:
 - Drag the prefab "GameMonetize" into your scene but dont rename this or else audio or game not pause
 - Replace the GAME_ID values with your own keys
 - Use GameMonetize.Instance.ShowAd() to show an advertisement or drag in scene the prefab "AdsOnStart"
 - Make use of the events GameMonetize.OnResumeGame and GameMonetize.OnPauseGame for resuming/pausing your game in between ads
 - Do not rename prefab GameMonetize or ResumeGame() & PauseGame() will not work anymore
 - if you use unity 5.6, 2017, 2018, 2019 you will need to create new prefab and drag script "GameMonetize" 
 - read this https://gamemonetize.com/blog/speed-up-your-game-release-on-gamemonetize-com

Example:

public class ExampleClass: MonoBehaviour {
	void Awake()
	{
	  GameMonetize.OnResumeGame += ResumeGame;
	  GameMonetize.OnPauseGame += PauseGame;
	}
	
	void OnDestroy()
	{
	  GameMonetize.OnResumeGame -= ResumeGame;
	  GameMonetize.OnPauseGame -= PauseGame;
	}

	public void ResumeGame()
	{
	  // RESUME MY GAME
	    AudioListener.volume = 1f;
            Time.timeScale = 1f;
	}

	public void PauseGame()
	{
	  // PAUSE MY GAME
	   Time.timeScale = 0.01f;
           AudioListener.volume = 0f;
	}

	public void ShowAd()
	{
	  GameMonetize.Instance.ShowAd();	
	}
}

Unity Typs Tutorials - please read to optimize your game
- Compress Audio - https://gamemonetize.com/blog/optimize-your-unity-game-size-with-efficient-audio-compression
- Compress FBX - https://gamemonetize.com/blog/improving-game-performance-how-to-reduce-fbx-file-sizes-in-unity
- Compress Image - https://gamemonetize.com/blog/unlocking-better-performance-in-unity-games-through-image-optimization
- Template Webgl - https://gamemonetize.com/blog/the-complete-guide-to-unity-webgl-templates-and-customization