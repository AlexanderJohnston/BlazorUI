using BlazorUI.Service.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Totem.Runtime;

namespace BlazorUI.Service.Services
{
    public class EncryptionReader : Notion
    {
        private byte[] _entropy = null;
        private IEncryptionScheme _protectedData { get; set; }

        public async Task<ICryptoTransform> Decrypt(string reason)
        {
            var encryptedInitVector = await ReadClientEncryptionKey(reason);
            if (encryptedInitVector == null) throw new ArgumentException($"{reason} is not a valid reason to decrypt anything.");

            // If this gets changed to LocalSystem protection scope then anyone on the machine can decrypt.
            var vector = ProtectedData.Unprotect(encryptedInitVector, _entropy, DataProtectionScope.CurrentUser);
            var algorithm = await CryptographicProvider();
            algorithm.IV = vector;
            return algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);
        }

        public async Task<ICryptoTransform> Encrypt(string reason)
        {
            var algorithm = await CryptographicProvider();
            algorithm.GenerateIV();

            // If this is changed to LocalMachine protection scope then you're gonna have a bad time with snooping.
            byte[] encryptedInitVector = ProtectedData.Protect(algorithm.IV, _entropy, DataProtectionScope.CurrentUser);
            await StoreClientEncryptionKey(reason, encryptedInitVector);
            return algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);
        }

        public async Task<Stream> EncryptedStream(Stream stream, string reason) =>
            new CryptoStream(stream, await Encrypt(reason), CryptoStreamMode.Write);

        public async Task<Stream> DecryptedStream(Stream stream, string reason) =>
            new CryptoStream(stream, await Decrypt(reason), CryptoStreamMode.Read);

        private async Task<byte[]> ReadTotemEncryptionKey() => _protectedData.ReadData("Totem");
        private async Task StoreTotemEncryptionKey(byte[] key) => _protectedData.WriteData("Totem", key);

        private async Task<byte[]> ReadClientEncryptionKey(string reason) =>
            _protectedData.ReadData($"Client.{reason}");
        private async Task StoreClientEncryptionKey(string reason, byte[] initVector) =>
            _protectedData.WriteData($"Client.{reason}", initVector);

        private async Task<SymmetricAlgorithm> CryptographicProvider()
        {
            var key = await ReadTotemEncryptionKey();
            return key == null ? await NewKey() : await ExistingKey(key);
        }
        private async Task<SymmetricAlgorithm> NewKey()
        {
            Log.Debug("Creating a new encryption key for Totem.");
            try
            {
                SymmetricAlgorithm provider = AES256Provider();

                // If this is changed to LocalMachine protection scope then it will be vulnerable to any process.
                byte[] encryptedKey = ProtectedData.Protect(provider.Key, null, DataProtectionScope.CurrentUser);
                StoreTotemEncryptionKey(encryptedKey).Wait();
                Log.Info("A new key has been created and stored successfully.");
                return provider;
            }
            catch (CryptographicException exc) { Log.Info(exc, "Failed to generate the key."); }
            catch (Exception ex) { Log.Info(ex, "Failed to generate the key."); }
            return new AesCryptoServiceProvider();
        }
        private async Task<SymmetricAlgorithm> ExistingKey(byte[] encrypted)
        {
            // If this is set to LocalMachine protection scope then it will be vulnerable to any process.
            var key = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
            SymmetricAlgorithm provider = AES256Provider(key);

            if (provider.KeySize != 256)
                throw new ArgumentException($"Key is the wrong size!");
            Log.Info("An existing key has been loaded successfully.");
            return provider;
        }

        private AesCryptoServiceProvider AES256Provider() => 
            new AesCryptoServiceProvider { KeySize = 256, BlockSize = 128 };
        private AesCryptoServiceProvider AES256Provider(byte[] key) =>
            new AesCryptoServiceProvider { KeySize = 256, BlockSize = 128, Key = key };

    }
}