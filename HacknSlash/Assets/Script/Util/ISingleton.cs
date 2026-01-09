using Unity.VisualScripting;
using UnityEngine;

public class ISingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬에 있는 T 타입을 찾아본다.
                instance = FindFirstObjectByType<T>();

                // 그래도 없으면? (선택 사항: 에러를 띄우거나 새로 만들거나)
                if (instance == null)
                {
                    Debug.LogError($"씬에 {typeof(T)} 컴포넌트가 없습니다!");
                }
            }
            return instance;
        }
    }
}
