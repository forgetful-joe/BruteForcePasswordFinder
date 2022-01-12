using Nethereum.KeyStore;
using Nethereum.KeyStore.Crypto;
using Nethereum.KeyStore.Model;

namespace BruteForcePasswordFinder.Helpers
{
    // Nethereum wasn't designed for brute force, so it doesn't optimize memory and other things for this purpose
    // and it's overcomplicated for what we need
    // this class provides some of those facilities, inspired by https://refactoring.guru/design-patterns/facade/csharp/example
    public class KeyStoreFacade
    {
        readonly KeyStorePbkdf2Service pbkService;
        readonly KeyStore<Pbkdf2Params> pdkKeyStore;

        readonly KeyStoreScryptService scryptService;
        readonly KeyStore<ScryptParams> scryptKeyStore;

        private readonly KeyStoreKdfChecker _keyStoreKdfChecker;

        public KeyStoreFacade(string json)
        {
            _keyStoreKdfChecker = new KeyStoreKdfChecker();

            var type = _keyStoreKdfChecker.GetKeyStoreKdfType(json);
            switch (type)
            {
                case KeyStoreKdfChecker.KdfType.scrypt:
                    scryptService = new KeyStoreScryptService();
                    scryptKeyStore = scryptService.DeserializeKeyStoreFromJson(json);
                    break;
                case KeyStoreKdfChecker.KdfType.pbkdf2:
                    pbkService = new KeyStorePbkdf2Service();
                    pdkKeyStore = pbkService.DeserializeKeyStoreFromJson(json);
                    break;
            }
        }

        public bool Decrypt(string possiblePwd)
        {
            try
            {
                if (pbkService != null)
                    pbkService.DecryptKeyStore(possiblePwd, pdkKeyStore);
                else
                    scryptService.DecryptKeyStore(possiblePwd, scryptKeyStore);
            }
            catch (DecryptionException)
            {
                // Unfortunately Nethereum throws an exception of this type with the text
                // "Cannot derive the same mac as the one provided from the cipher and derived key"
                // when the password is wrong, so we handle it here

                return false;
            }


            return true;
        }

    }
}
