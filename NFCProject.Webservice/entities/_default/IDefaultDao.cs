using System;
using System.Collections.Generic;

namespace NFCProject.Webservice.entities._default
{
    public interface IDefaultDao<T> : IDisposable where T : class, new()
    {
        T Save(T model);
        T Load(Object id);
        List<T> LoadAll();
        bool Remove(T model);
    }
}
