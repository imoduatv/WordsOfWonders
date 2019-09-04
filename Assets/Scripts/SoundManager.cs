using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;

	public AudioClip[] audios;

	private AudioSource[] sounds;

	private AudioSource[] selects;

	private AudioSource[] deselects;

	private int selectIndex;

	private bool deselectFlag = true;

	private bool ribbon;

	private void Awake()
	{
		if (instance == null)
		{
			selectIndex = 0;
			instance = this;
			sounds = new AudioSource[audios.Length];
			for (int i = 0; i < audios.Length; i++)
			{
				AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
				audioSource.clip = audios[i];
				audioSource.playOnAwake = false;
				sounds[i] = audioSource;
			}
			selects = new AudioSource[8];
			for (int j = 0; j <= 7; j++)
			{
				selects[j] = sounds[j + 23];
			}
			deselects = new AudioSource[8];
			for (int k = 0; k <= 7; k++)
			{
				deselects[k] = sounds[k + 15];
			}
			Object.DontDestroyOnLoad(this);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		ArrangeMuted();
	}

	public void Click()
	{
		sounds[0].Play();
		VibrationManager.Haptic(HapticTypes.LightImpact);
	}

	public void CoinGained()
	{
		sounds[1].Play();
	}

	public void CoinIncrease()
	{
		sounds[2].Play();
	}

	public void SlideIn()
	{
		sounds[3].Play();
	}

	public void SlideOut()
	{
		sounds[4].Play();
	}

	public void DailyReward()
	{
		sounds[5].Play();
	}

	public void PurchaseComplete()
	{
		sounds[6].Play();
	}

	public void SetUnlocked()
	{
		sounds[7].Play();
	}

	public void DailyPuzzleCollected()
	{
		sounds[9].Play();
	}

	public void WordFound()
	{
		sounds[36].Play();
	}

	public void WordSame()
	{
		sounds[33].Play();
	}

	public void WordInvalid()
	{
		sounds[35].Play();
	}

	public void Shuffle()
	{
		sounds[31].Play();
	}

	public void WordExtra()
	{
		sounds[34].Play();
	}

	public void PlayHint()
	{
		sounds[37].Play();
	}

	public void SelectLetter()
	{
		selectIndex = Mathf.Clamp(selectIndex, 0, selects.Length - 1);
		selects[selectIndex].Play();
		selectIndex++;
	}

	public void DeselectLetter()
	{
		selectIndex = Mathf.Clamp(selectIndex, 0, deselects.Length - 1);
		if (selectIndex == 0)
		{
			if (!deselectFlag)
			{
				return;
			}
			deselectFlag = false;
		}
		else
		{
			deselectFlag = true;
		}
		selectIndex--;
		deselects[Mathf.Clamp(selectIndex, 0, deselects.Length - 1)].Play();
	}

	public void ResetSelectIndex()
	{
		selectIndex = 0;
	}

	public void HintJiggle()
	{
		if (WheelController.running)
		{
			sounds[14].Play();
		}
	}

	public void GameWin()
	{
		sounds[13].Play();
	}

	public void StarCollected()
	{
		sounds[8].Play();
	}

	public void GameStart()
	{
		sounds[12].Play();
	}

	public void RoadPoint()
	{
		sounds[41].Play();
	}

	public void LetterPlace()
	{
		sounds[42].Play();
	}

	public void playRibbon()
	{
		if (ribbon)
		{
			playPop1();
		}
		else
		{
			playPop2();
		}
		ribbon = !ribbon;
	}

	public void playPop1()
	{
		sounds[38].Play();
	}

	public void playPop2()
	{
		sounds[39].Play();
	}

	public void scoreCount()
	{
		sounds[40].Play();
	}

	public void blurReveal()
	{
		sounds[43].Play();
	}

	public void playMegaHint()
	{
		sounds[45].Play();
	}

	public void PlayMusic(float delay = 0f)
	{
		sounds[44].volume = 0.2f;
		if (PlayerPrefsManager.GetMusic() == 1)
		{
			if (!sounds[44].isPlaying)
			{
				sounds[44].PlayDelayed(delay);
			}
		}
		else
		{
			StopMusic();
		}
	}

	public void WheelRotate()
	{
		sounds[45].Play();
	}

	public void StopMusic()
	{
		sounds[44].Stop();
	}

	public void FortuneWheel()
	{
		sounds[46].Play();
		VibrationManager.Haptic(HapticTypes.LightImpact);
	}

	public void ArrangeMuted()
	{
		int soundEffects = PlayerPrefsManager.GetSoundEffects();
		for (int i = 0; i < sounds.Length; i++)
		{
			if (i != 44)
			{
				sounds[i].volume = soundEffects;
			}
		}
	}

	public void ArrangeMusic()
	{
		PlayMusic();
	}
}
