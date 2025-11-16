using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리 기능 사용을 위해 필수

public class SceneLoader : MonoBehaviour
{
    // 버튼의 OnClick() 이벤트에 연결할 공개 함수입니다.
    // 인자로 씬 이름을 받아서 해당 씬으로 전환합니다.
    public void LoadSceneByName(string Intro)
    {
        // SceneManager.LoadScene() 함수가 씬 전환을 수행합니다.
        SceneManager.LoadScene(Intro);
    }
}