using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class LoadSceneTarget : ArrowTarget
{
	[SerializeField]
	private int _sceneToLoad;

	void Start ()
	{
		TargetHit.Subscribe(_ => SceneManager.LoadSceneAsync(_sceneToLoad)).AddTo(this);
	}
}
