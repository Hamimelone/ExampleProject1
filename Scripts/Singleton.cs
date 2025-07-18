using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance; // ����ʵ��

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // ���ҳ������Ƿ��Ѿ����ڸõ���
                _instance = FindObjectOfType<T>();

                // ��������в����ڣ��򴴽�һ���µ� GameObject �����ص����ű�
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // ȷ������Ψһ��
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject); // ��ѡ�����ֵ����ڳ����л�ʱ������
        }
        else
        {
            Destroy(gameObject); // ����Ѿ����ڵ����������ٵ�ǰ����
        }
    }
}
