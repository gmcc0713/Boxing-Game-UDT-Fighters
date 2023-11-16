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
//    //�̵��� ���̸�
//    static string nextScene;

//    //�ε��� ��µ� �α�
//    [SerializeField] TextMeshProUGUI logCountents;

//    //�ε��� �̹���
//    [SerializeField] Image progressImage;

//    //�ε�� �Լ�
//    public static void LoadScene(string sceneName)
//    {
//        //�̵��� �� �̸� �����ϱ�
//        nextScene = sceneName;

//        //�ε������� �̵�
//        SceneManager.LoadScene("LoadingScene");
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        //�ε����� ���۵Ǿ��� ��, �������� ���� �� �α� ����ϱ�
//        LoadXml();
//        //�ε����� ���۵Ǿ��� ��, �ε� �ڷ�ƾ �θ���
//        StartCoroutine(LoadSceneProgress());
//    }

//    //�ε� �ڷ�ƾ
//    IEnumerator LoadSceneProgress()
//    {
//        //�񵿱������� �� �ε��ϱ�, AsyncOperation Ŭ���� ��ü�� �ε� ���൵ �����ϱ�
//        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

//        //���� �ε��� ������ ��, �ڵ����� �ε��� ������ �̵��ϱ� ==> false�� ����
//        op.allowSceneActivation = false;

//        //�� �ε� �ð� �ʱ�ȭ
//        float timer = 0f;

//        //�� �ε��� ������ ������ �ݺ��ϴ� �ڵ�
//        while (!op.isDone)
//        {
//            yield return null;

//            //�� �ε� ���൵�� 90%���� ���� ��, �ε��� �̹����� �ε� ���൵�� ���� �����
//            if (op.progress < 0.9f)
//            {
//                progressImage.fillAmount = op.progress;
//            }
//            //�� �ε� ���൵�� 90%�� �Ѿ��� ��, 1�� ���� ����ũ �ε����� �ε��� ä���
//            else
//            {
//                timer += Time.unscaledDeltaTime;
//                progressImage.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

//                //����ũ �ε����� �ε��ٰ� �� ä������ ��, �ڵ����� �ε��� ������ �̵��ϰ� �ڷ�ƾ ������
//                if (progressImage.fillAmount >= 1f)
//                {
//                    op.allowSceneActivation = true;
//                    yield break;
//                }
//            }
//        }
//    }

//    //�ε� �� ��µǴ� �ؽ�Ʈ xml���� �ε��ϱ�
//    private void LoadXml()
//    {
//        //���ҽ� ���Ͽ��� LoadingTxt��� �̸��� ���� �ε��ϰ� Text������ �����ϱ�
//        TextAsset textAsset = (TextAsset)Resources.Load("LoadingTxt");

//        //xml��ü �����ϱ�
//        XmlDocument xmlDoc = new XmlDocument();

//        //xml��ü�� xml �����ϱ�
//        xmlDoc.LoadXml(textAsset.text);

//        //xml�� ���� �� tip�±׿� log�Ӽ� ����Ʈ �����ϱ�
//        XmlNodeList nodes = xmlDoc.SelectNodes("LoadingLog/tip/log");

//        //log�Ӽ� �� �������� �̾Ƽ� ��Ʈ�� ������ �����ϱ�
//        string text = nodes[Random.Range(0, nodes.Count)].InnerText;

//        //text ui�� ��Ʈ�� ���� ���� �����ϱ�
//        logCountents.text = text;
//    }
//}
