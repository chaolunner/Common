﻿namespace Common
{
    public interface ISession
    {
        bool IsConnected { get; }
        void Send(byte[] buffer);
        void Receive();
        void Close();
    }
}
