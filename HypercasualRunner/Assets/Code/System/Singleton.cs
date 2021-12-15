using UnityEngine;
using UnityEngine.SceneManagement;

//Base class for any class passing in the T component for the singleton 
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance; 

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //Create instance if instance null
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                obj.hideFlags = HideFlags.HideAndDontSave;

                //Adding instance component to the object of the current _instance
                _instance = obj.AddComponent<T>();
            }

            return _instance;
        } 
    }

    //If correct instance then set _instance null
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}


public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Scene activeScene = SceneManager.GetActiveScene();
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));

                //Create instance if instance null
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                obj.hideFlags = HideFlags.HideAndDontSave;

                //Adding instance component to the object of the current _instance
                _instance = obj.AddComponent<T>();

                SceneManager.SetActiveScene(activeScene);
            }

            return _instance;
        }
    }

    //If correct instance then set _instance null
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    //public virtual void awake()
    //{
    //    if (_instance == null)
    //    {
    //        _instance = this as t;
    //        dontdestroyonload(gameobject);
    //    }
    //    else
    //    {
    //        destroy(this);
    //    }
    //}
}