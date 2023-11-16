//using System.Collections;
//using System.Collections.Generic;
//using System.Xml;
//using TMPro;
//using UnityEditor.VersionControl;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class LoadingSceneManager : MonoBehaviour
//{
//    //이동한 씬이름
//    static string nextScene;

//    //로딩시 출력될 로그
//    [SerializeField] TextMeshProUGUI logCountents;

//    //로딩바 이미지
//    [SerializeField] Image progressImage;

//    //로드씬 함수
//    public static void LoadScene(string sceneName)
//    {
//        //이동할 씬 이름 저장하기
//        nextScene = sceneName;

//        //로딩씬으로 이동
//        SceneManager.LoadScene("LoadingScene");
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        //로딩씬이 시작되었을 때, 무작위의 게임 팁 로그 출력하기
//        LoadXml();
//        //로딩씬이 시작되었을 때, 로딩 코루틴 부르기
//        StartCoroutine(LoadSceneProgress());
//    }

//    //로딩 코루틴
//    IEnumerator LoadSceneProgress()
//    {
//        //비동기방식으로 씬 로드하기, AsyncOperation 클래스 객체에 로딩 진행도 저장하기
//        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

//        //씬의 로딩이 끝났을 때, 자동으로 로딩할 씬으로 이동하기 ==> false로 설정
//        op.allowSceneActivation = false;

//        //씬 로딩 시간 초기화
//        float timer = 0f;

//        //씬 로딩이 끝나기 전까지 반복하는 코드
//        while (!op.isDone)
//        {
//            yield return null;

//            //씬 로딩 진행도가 90%보다 적을 때, 로딩바 이미지를 로딩 진행도와 같게 만들기
//            if (op.progress < 0.9f)
//            {
//                progressImage.fillAmount = op.progress;
//            }
//            //씬 로딩 진행도가 90%를 넘었을 때, 1초 동안 페이크 로딩으로 로딩바 채우기
//            else
//            {
//                timer += Time.unscaledDeltaTime;
//                progressImage.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

//                //페이크 로딩으로 로딩바가 다 채워졌을 때, 자동으로 로딩할 씬으로 이동하고 코루틴 끝내기
//                if (progressImage.fillAmount >= 1f)
//                {
//                    op.allowSceneActivation = true;
//                    yield break;
//                }
//            }
//        }
//    }

//    //로딩 시 출력되는 텍스트 xml파일 로드하기
//    private void LoadXml()
//    {
//        //리소스 파일에서 LoadingTxt라는 이름의 파일 로드하고 Text변수에 저장하기
//        TextAsset textAsset = (TextAsset)Resources.Load("LoadingTxt");

//        //xml객체 선언하기
//        XmlDocument xmlDoc = new XmlDocument();

//        //xml객체에 xml 저장하기
//        xmlDoc.LoadXml(textAsset.text);

//        //xml의 내용 중 tip태그에 log속성 리스트 저장하기
//        XmlNodeList nodes = xmlDoc.SelectNodes("LoadingLog/tip/log");

//        //log속성 중 무작위로 뽑아서 스트링 변수에 저장하기
//        string text = nodes[Random.Range(0, nodes.Count)].InnerText;

//        //text ui에 스트링 변수 내용 저장하기
//        logCountents.text = text;
//    }
//}
