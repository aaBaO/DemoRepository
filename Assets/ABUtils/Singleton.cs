

public class Singleton<T> where T: class,new()
{
    private static T m_instance = null;
    public static T instance
    {
        get
        {
            if (m_instance == null)
                m_instance = new T();
            return m_instance;
        }
    }
    public static T GetInstance()
    {
        return instance;
    }
}
