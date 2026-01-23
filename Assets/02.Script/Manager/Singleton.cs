using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected bool _IsDestroyOnLoad = true;
    protected static T _instance;

    public static T Instance
    {
        get { return _instance; }
    }

    protected void Awake()
    {
        Init(); 
    }

    protected virtual void Init()
    {
        if (_instance == null)
        {
            _instance = (T)this;

            if (_IsDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
