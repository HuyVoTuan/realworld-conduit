﻿namespace RealWorldConduit_Infrastructure.Services.Cache
{
    public interface ICacheService
    {
        T GetData<T>(string key);
        bool SetData<T>(string key, T value, TimeSpan expirationTime);
        object RemoveData(string key);
    }
}
