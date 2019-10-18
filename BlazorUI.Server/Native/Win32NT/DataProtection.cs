using JetBrains.Annotations;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorUI.Server.Native.Win32NT
{
    internal class DataProtection : IEncryptionScheme, INativeClass
    {
        private RegistryKey _totemKey = null;
        public DataProtection()
        {
            _totemKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\BlazorUI\\Service", RegistryKeyPermissionCheck.ReadWriteSubTree);
        }


        public byte[] ReadData(string name) => _totemKey?.OpenSubKey("Encrypted")?.GetValue(name) as byte[];
        public void WriteData(string name, byte[] data)
        {
            if (EncryptingData() && HasPermission())
                _totemKey.SetValue(name, data, RegistryValueKind.Binary);
        }
        private bool EncryptingData() => _totemKey != null;
        private bool HasPermission() => _totemKey.CreateSubKey("Encypted", RegistryKeyPermissionCheck.ReadWriteSubTree) != null;


        public Task Initialize<T>([CanBeNull] T parent)
        {
            return Task.CompletedTask;
        }
        public Task Terminate(string reason)
        {
            _totemKey = null;
            return Task.CompletedTask;
        }
    }
}
