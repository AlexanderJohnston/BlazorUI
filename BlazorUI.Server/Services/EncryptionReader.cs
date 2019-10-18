using BlazorUI.Server.Native;
using Microsoft.Extensions.Hosting;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Totem.Runtime;

namespace BlazorUI.Server.Services
{
    public class EncryptionReader : Notion, IHostedService
    {
        public EncryptionReader(IEncryptionScheme nativeProvider)
        {
            _protectedData = nativeProvider;
        }
        private byte[] _entropy = null;
        private IEncryptionScheme _protectedData { get; set; }

        public async Task<ICryptoTransform> Decrypt([Required] string reason)
        {
            var encryptedInitVector = await ReadClientEncryptionKey(reason);
            if (encryptedInitVector == null) throw new ArgumentException($"{reason} is not a valid reason to decrypt anything.");

            // If this gets changed to LocalSystem protection scope then anyone on the machine can decrypt.
            var vector = ProtectedData.Unprotect(encryptedInitVector, _entropy, DataProtectionScope.CurrentUser);
            var algorithm = await CryptographicProvider();
            algorithm.IV = vector;
            return algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);
        }

        public async Task<ICryptoTransform> Encrypt([Required] string reason)
        {
            var algorithm = await CryptographicProvider();
            algorithm.GenerateIV();

            // If this is changed to LocalMachine protection scope then you're gonna have a bad time with snooping.
            byte[] encryptedInitVector = ProtectedData.Protect(algorithm.IV, _entropy, DataProtectionScope.CurrentUser);
            await StoreClientEncryptionKey(reason, encryptedInitVector);
            return algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);
        }

        public async Task<Stream> EncryptedStream([Required] Stream stream, [Required] string reason) =>
            new CryptoStream(stream, await Encrypt(reason), CryptoStreamMode.Write);

        public async Task<Stream> DecryptedStream([Required] Stream stream, [Required] string reason) =>
            new CryptoStream(stream, await Decrypt(reason), CryptoStreamMode.Read);

        private async Task<byte[]> ReadTotemEncryptionKey() => _protectedData.ReadData("Totem");
        private async Task StoreTotemEncryptionKey([Required] byte[] key) => _protectedData.WriteData("Totem", key);

        private async Task<byte[]> ReadClientEncryptionKey([Required] string reason) =>
            _protectedData.ReadData($"Client.{reason}");
        private async Task StoreClientEncryptionKey([Required] string reason, [Required] byte[] initVector) =>
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
        private async Task<SymmetricAlgorithm> ExistingKey([Required] byte[] encrypted)
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

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Info("Attempting an encryption test.");
            byte[] output;
            var plainValue = "Hello encrypted world!";
            var reason = "None";

            using (var memory = new MemoryStream())
            {
                using (Stream encrypted = EncryptedStream(memory, reason).Result)
                {
                    //  Stream writer writes our unencrypted text in.
                    using (var writer = new StreamWriter(encrypted, Encoding.UTF8))
                        writer.Write(plainValue);
                    output = memory.ToArray();
                }

                if (output.Length == 0)
                    Log.Info("Could not decrypt the test value!");
            }

            var encryptedValue = Convert.ToBase64String(output);
            Log.Info($"Encrypted Value: {encryptedValue}");

            Log.Info("Attempting a decryption test.");
            string decryptedValue = string.Empty;
            using (var memory = new MemoryStream(Convert.FromBase64String(encryptedValue)))
            {
                using (Stream decrypted = DecryptedStream(memory, reason).Result)
                {
                    using (var reader = new StreamReader(decrypted, Encoding.UTF8))
                        decryptedValue = reader.ReadToEnd();
                }
            }
            Log.Info($"Decrypted Value: {decryptedValue}");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}