//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class MoveSceneManager : MonoBehaviour
//{
//    //싱글플레이 버튼 오브젝트 변수
//    private Button singlebutton;

//    // 씬 시작시 버튼 컴포넌트 설정하기
//    public void Awake()
//    {
//        //싱글플레이 버튼 컴포넌트 찾기
//        singlebutton = GetComponent<Button>();
//        //싱글플레이 버튼에 로딩 화면 함수 연결
//        singlebutton.onClick.AddListener(GoToSinglePlay);
//    }

//    //싱글플레이 버튼을 눌렀을 때, 이동할 씬을 파라미터로 해서 로드씬 함수 부르기
//    void GoToSinglePlay()
//    {
//        LoadingSceneManager.LoadScene("TestScene");
//    }

//}
