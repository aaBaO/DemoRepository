namespace ABUtils
{
    public delegate void VoidDelegate();
    public delegate void VoidDelegate<T0>(T0 t0);
    public delegate void VoidDelegate<T0, T1>(T0 t0, T1 t1);
    public delegate void VoidDelegate<T0, T1, T2>(T0 t0, T1 t1, T2 t2);
    public delegate void VoidDelegate<T0, T1, T2, T3>(T0 t0, T1 t1, T2 t2, T3 t3);

    public class SubjectEvent
    {
        private VoidDelegate subjectEvent;
        public void AddObserver(VoidDelegate target)
        {
            subjectEvent += target;
        }
        public void RemoveObserver(VoidDelegate target)
        {
            subjectEvent -= target;
        }
        public void Invoke()
        {
            subjectEvent.Invoke();
        }
        public void Reset()
        {
            subjectEvent = null;
        }

        public bool IsNull()
        {
            return subjectEvent == null ? true : false;
        }
    }

    public class SubjectEvent<T0>
    {
        private VoidDelegate<T0> subjectEvent;
        public void AddObserver(VoidDelegate<T0> target)
        {
            subjectEvent += target;
        }
        public void RemoveObserver(VoidDelegate<T0> target)
        {
            subjectEvent -= target;
        }
        public void Invoke(T0 t0)
        {
            subjectEvent.Invoke(t0);
        }
        public void Reset()
        {
            subjectEvent = null;
        }
        public bool IsNull()
        {
            return subjectEvent == null ? true : false;
        }
    }

    public class SubjectEvent<T0, T1>
    {
        private VoidDelegate<T0, T1> subjectEvent;
        public void AddObserver(VoidDelegate<T0, T1> target)
        {
            subjectEvent += target;
        }
        public void RemoveObserver(VoidDelegate<T0, T1> target)
        {
            subjectEvent -= target;
        }
        public void Invoke(T0 t0, T1 t1)
        {
            subjectEvent.Invoke(t0, t1);
        }
        public void Reset()
        {
            subjectEvent = null;
        }
        public bool IsNull()
        {
            return subjectEvent == null ? true : false;
        }
    }

    public class SubjectEvent<T0, T1, T2>
    {
        private VoidDelegate<T0, T1, T2> subjectEvent;
        public void AddObserver(VoidDelegate<T0, T1, T2> target)
        {
            subjectEvent += target;
        }
        public void RemoveObserver(VoidDelegate<T0, T1, T2> target)
        {
            subjectEvent -= target;
        }
        public void Invoke(T0 t0, T1 t1, T2 t2)
        {
            subjectEvent.Invoke(t0, t1, t2);
        }
        public void Reset()
        {
            subjectEvent = null;
        }
        public bool IsNull()
        {
            return subjectEvent == null ? true : false;
        }
    }

    public class SubjectEvent<T0, T1, T2, T3>
    {
        private VoidDelegate<T0, T1, T2, T3> subjectEvent;
        public void AddObserver(VoidDelegate<T0, T1, T2, T3> target)
        {
            subjectEvent += target;
        }
        public void RemoveObserver(VoidDelegate<T0, T1, T2, T3> target)
        {
            subjectEvent -= target;
        }
        public void Invoke(T0 t0, T1 t1, T2 t2, T3 t3)
        {
            subjectEvent.Invoke(t0, t1, t2, t3);
        }
        public void Reset()
        {
            subjectEvent = null;
        }
        public bool IsNull()
        {
            return subjectEvent == null ? true : false;
        }
    }
}