using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace IKende.com.core
{
    public static class ObjectCopyExt
    {

        public static void MemberCopyTo(this object source, object target)
        {
            ObjectCopy mc = ObjectCopy.GetObjectCopy(source.GetType(), target.GetType());
            mc.Copy(source, target);
        }

    }
    class ObjectCopy
    {
        private List<CastProperty> mProperties = new List<CastProperty>();

        static Dictionary<Type, Dictionary<Type, ObjectCopy>> mCasters = new Dictionary<Type, Dictionary<Type, ObjectCopy>>(256);

        private static Dictionary<Type, ObjectCopy> GetModuleCast(Type sourceType)
        {
            Dictionary<Type, ObjectCopy> result;
            lock (mCasters)
            {
                if (!mCasters.TryGetValue(sourceType, out result))
                {
                    result = new Dictionary<Type, ObjectCopy>(8);
                    mCasters.Add(sourceType, result);
                }
            }
            return result;
        }

        public static ObjectCopy GetObjectCopy(Type sourceType, Type targetType)
        {
            Dictionary<Type, ObjectCopy> casts = GetModuleCast(sourceType);
            ObjectCopy result;
            lock (casts)
            {
                if (!casts.TryGetValue(targetType, out result))
                {
                    result = new ObjectCopy(sourceType, targetType);
                    casts.Add(targetType, result);
                }
            }
            return result;
        }

        public ObjectCopy(Type sourceType, Type targetType)
        {
            foreach (PropertyInfo sp in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (PropertyInfo tp in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (sp.Name == tp.Name && sp.PropertyType == tp.PropertyType)
                    {
                        CastProperty cp = new CastProperty();
                        cp.SourceProperty = new PropertyHandler(sp);
                        cp.TargetProperty = new PropertyHandler(tp);
                        mProperties.Add(cp);
                    }
                }
            }
        }

        public void Copy(object source, object target)
        {
            for (int i = 0; i < mProperties.Count; i++)
            {
                CastProperty cp = mProperties[i];
                cp.TargetProperty.Set(target, cp.SourceProperty.Get(source));
            }
        }

        public class CastProperty
        {
            public PropertyHandler SourceProperty
            {
                get;
                set;
            }

            public PropertyHandler TargetProperty
            {
                get;
                set;
            }
        }
    }
}
