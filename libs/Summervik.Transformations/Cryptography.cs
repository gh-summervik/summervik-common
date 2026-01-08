using System.Security.Cryptography;
using System.Text;

namespace Summervik.Transformations;

/// <summary>
/// Utility for common cryptography needs (hashing and authenticated AES encryption).
/// </summary>
public static class Cryptography
{
    // Hashing
    public static string GetHash(HashAlgorithm hashAlgorithm, string input)
    {
        ArgumentNullException.ThrowIfNull(hashAlgorithm);
        ArgumentNullException.ThrowIfNull(input);

        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be empty or whitespace.", nameof(input));

        byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(data).ToLowerInvariant();
    }

    public static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash) =>
        StringComparer.OrdinalIgnoreCase.Equals(GetHash(hashAlgorithm, input), hash);

    public static string GetHashForFile(HashAlgorithm hashAlgorithm, FileInfo fileInfo)
    {
        ArgumentNullException.ThrowIfNull(hashAlgorithm);
        ArgumentNullException.ThrowIfNull(fileInfo);

        using var stream = fileInfo.OpenRead();
        byte[] data = hashAlgorithm.ComputeHash(stream);
        return Convert.ToHexString(data).ToLowerInvariant();
    }

    public static bool VerifyHashForFile(HashAlgorithm hashAlgorithm, FileInfo fileInfo, string hash) =>
        StringComparer.OrdinalIgnoreCase.Equals(GetHashForFile(hashAlgorithm, fileInfo), hash);

    // Secure AES-GCM encryption (authenticated, handles arbitrary bytes)
    private const int SaltSize = 16;
    private const int NonceSize = 12; // Recommended
    private const int TagSize = 16;   // Recommended

    public static byte[] EncryptAes(byte[] original, string passkey)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(passkey);
        ArgumentNullException.ThrowIfNull(original);

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] key = Rfc2898DeriveBytes.Pbkdf2(passkey, salt, 100000, HashAlgorithmName.SHA256, 32);

        using var aes = new AesGcm(key, TagSize);
        byte[] nonce = RandomNumberGenerator.GetBytes(NonceSize);
        byte[] tag = new byte[TagSize];
        byte[] ciphertext = new byte[original.Length];

        aes.Encrypt(nonce, original, ciphertext, tag);

        // Format: salt + nonce + tag + ciphertext
        byte[] result = new byte[SaltSize + NonceSize + TagSize + ciphertext.Length];
        Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
        Buffer.BlockCopy(nonce, 0, result, SaltSize, NonceSize);
        Buffer.BlockCopy(tag, 0, result, SaltSize + NonceSize, TagSize);
        Buffer.BlockCopy(ciphertext, 0, result, SaltSize + NonceSize + TagSize, ciphertext.Length);
        return result;
    }

    public static byte[] EncryptAes(byte[] original, byte[] key)
    {
        ArgumentNullException.ThrowIfNull(original);

        if (key is null || (key.Length != 16 && key.Length != 24 && key.Length != 32))
            throw new ArgumentException("Key must be 16, 24, or 32 bytes.");

        using var aes = new AesGcm(key, TagSize);
        byte[] nonce = RandomNumberGenerator.GetBytes(NonceSize);
        byte[] tag = new byte[TagSize];
        byte[] ciphertext = new byte[original.Length];

        aes.Encrypt(nonce, original, ciphertext, tag);

        byte[] result = new byte[NonceSize + TagSize + ciphertext.Length];
        Buffer.BlockCopy(nonce, 0, result, 0, NonceSize);
        Buffer.BlockCopy(tag, 0, result, NonceSize, TagSize);
        Buffer.BlockCopy(ciphertext, 0, result, NonceSize + TagSize, ciphertext.Length);
        return result;
    }

    public static byte[] DecryptAes(byte[] cipher, string passkey)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(passkey);
        ArgumentNullException.ThrowIfNull(cipher);

        if (cipher.Length < SaltSize + NonceSize + TagSize)
            throw new ArgumentException("Invalid cipher length.");

        byte[] salt = new byte[SaltSize];
        byte[] nonce = new byte[NonceSize];
        byte[] tag = new byte[TagSize];
        byte[] ciphertext = new byte[cipher.Length - (SaltSize + NonceSize + TagSize)];

        Buffer.BlockCopy(cipher, 0, salt, 0, SaltSize);
        Buffer.BlockCopy(cipher, SaltSize, nonce, 0, NonceSize);
        Buffer.BlockCopy(cipher, SaltSize + NonceSize, tag, 0, TagSize);
        Buffer.BlockCopy(cipher, SaltSize + NonceSize + TagSize, ciphertext, 0, ciphertext.Length);

        byte[] key = Rfc2898DeriveBytes.Pbkdf2(passkey, salt, 100000, HashAlgorithmName.SHA256, 32);

        using var aes = new AesGcm(key, TagSize);
        byte[] plaintext = new byte[ciphertext.Length];

        aes.Decrypt(nonce, ciphertext, tag, plaintext);
        return plaintext;
    }

    public static byte[] DecryptAes(byte[] cipher, byte[] key)
    {
        ArgumentNullException.ThrowIfNull(cipher);

        if (key is null || (key.Length != 16 && key.Length != 24 && key.Length != 32))
            throw new ArgumentException("Key must be 16, 24, or 32 bytes.");

        if (cipher.Length < NonceSize + TagSize)
            throw new ArgumentException("Invalid cipher length.");

        byte[] nonce = new byte[NonceSize];
        byte[] tag = new byte[TagSize];
        byte[] ciphertext = new byte[cipher.Length - (NonceSize + TagSize)];

        Buffer.BlockCopy(cipher, 0, nonce, 0, NonceSize);
        Buffer.BlockCopy(cipher, NonceSize, tag, 0, TagSize);
        Buffer.BlockCopy(cipher, NonceSize + TagSize, ciphertext, 0, ciphertext.Length);

        using var aes = new AesGcm(key, TagSize);
        byte[] plaintext = new byte[ciphertext.Length];

        aes.Decrypt(nonce, ciphertext, tag, plaintext);
        return plaintext;
    }
}