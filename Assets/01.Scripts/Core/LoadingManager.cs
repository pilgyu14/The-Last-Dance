using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LoadingManager : MonoBehaviour
{
	//정적 변수
	protected static string nextScene;

	//변수
	protected int previousRandomNum = 0;

	//인스펙터 변수
	[SerializeField]
	protected Slider progressBar;
	[SerializeField]
	protected TextMeshProUGUI _progressPercentText;
	[SerializeField]
	protected GameObject decoObject;
	//[SerializeField]
	//private LoadingToolTipSO loadingToolTipSO;
	[SerializeField]
	protected TextMeshProUGUI tip_Text;
	[Range(0, 5)]
	public float repeatTerm;

	protected void Awake()
	{
		//StartCoroutine(Random_Tips());
		LoadingAnim();
	}
	protected virtual void Start()
	{
		StartCoroutine(LoadSceneProcess());
	}
	/// <summary>
	/// 씬 로드하는 함수
	/// </summary>
	/// <param name="sceneName">로드할 씬의 이름</param>
	public static void LoadScene(string sceneName)
	{
		nextScene = sceneName;
		SceneManager.LoadScene("LoadingSceneRework");
	}
	protected void LoadingAnim()
	{
		decoObject.transform.DORotate(new Vector3(0, 0, 360), 0.1f).SetLoops(-1, LoopType.Incremental);
	}
	protected IEnumerator LoadSceneProcess()
	{
		yield return new WaitForSeconds(0.5f);
		AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
		op.allowSceneActivation = false;

		float timer = 0.0f;
		while (!op.isDone)
		{
			yield return null;
			if (op.progress < 0.9f)
			{
				progressBar.value = op.progress;

			}
			else
			{
				timer += Time.deltaTime;
				progressBar.value = Mathf.Lerp(0.9f, 1f, timer * 0.5f);
				if (progressBar.value >= 1.0f)
				{
					op.allowSceneActivation = true;
					yield break;
				}
			}
			_progressPercentText.text = $"{(int)(progressBar.value * 100)}%";

		}
	}

	//protected virtual IEnumerator Random_Tips()
	//{
	//	int random = Random.Range(0, loadingToolTipSO.toolTips.Count);
	//	if (previousRandomNum == random)
	//		random = Random.Range(0, loadingToolTipSO.toolTips.Count);
	//	tip_Text.text = loadingToolTipSO.toolTips[random];
	//	previousRandomNum = random;
	//	Debug.Log("radom");
	//	yield return new WaitForSeconds(repeatTerm);
	//}
}
