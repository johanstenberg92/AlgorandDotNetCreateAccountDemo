using System;
using Algorand;
using Algorand.V2;

namespace AlgorandDotNetCreateAccountDemo
{
    class Program
    {
        private const string ApiAddress = "http://localhost:4001"; // sandbox API address
        private const string ApiToken = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"; // sandbox API token

        static void Main(string[] args)
        {
            var account1 = CreateNewAccount(1);
            var account2 = CreateNewAccount(2);
            var account3 = CreateNewAccount(3);

            // Put a breakpoint on the line below to fill your test accounts with some Algo using the test net dispenser.
            var algodApi = new AlgodApi(ApiAddress, ApiToken);

            // Get the suggested transaction parameters.
            var transactionParams = algodApi.TransactionParams();

            // Send 1 Algo from account 1 to account 2.
            var paymentTransaction = Utils.GetPaymentTransaction(account1.Address, account2.Address, 1000000, "Payment", transactionParams);

            // Sign the transaction using account 1.
            var signedPaymentTransaction = account1.SignTransaction(paymentTransaction);

            Console.WriteLine("Signed transaction with transaction ID: " + signedPaymentTransaction.transactionID);

            // Send the transaction to the network.
            var id = Utils.SubmitTransaction(algodApi, signedPaymentTransaction);
            Console.WriteLine("Successfully sent transaction with ID: " + id.TxId);

            // Wait for the transaction to complete.
            var response = Utils.WaitTransactionToComplete(algodApi, id.TxId);
            Console.WriteLine("The confirmed round is: " + response.ConfirmedRound);

            GetAndPrintAccountInformation(algodApi, account1, 1);
            GetAndPrintAccountInformation(algodApi, account2, 2);
            GetAndPrintAccountInformation(algodApi, account3, 3);
        }

        private static Account CreateNewAccount(int number)
        {
            // Generate new account.
            var account = new Account();
            Console.WriteLine("Account #" + number + " Address: " + account.Address.ToString());

            // Access private key mnemonic.
            var mnemonic = account.ToMnemonic();
            Console.WriteLine("Account #" + number + " mnemonic: " + mnemonic);

            var recoveredAccount = new Account(mnemonic);
            var isValid = Address.IsValid(recoveredAccount.Address.ToString());
            Console.WriteLine("Account #" + number + " is valid: " + isValid);

            Console.WriteLine("Save account address and mnemonic listed above for account #" + number);

            return account;
        }

        private static void GetAndPrintAccountInformation(AlgodApi algodApi, Account account, int number)
        {
            var accountInformation = algodApi.AccountInformation(account.Address.ToString());
            Console.WriteLine("Account #" + number + " information: " + Environment.NewLine + accountInformation.ToJson());
        }

        /* Sample output:
Account #1 Address: B2N2DQCGEROOYJZLME5QDEQPLKGAAY6VEODYJZYUOK76DRS7W3RA6QTEBI
Account #1 mnemonic: story sheriff track wisdom amazing eye burden essence burger gossip library future peanut drum youth fiction shuffle vacuum knock vivid trick group physical able yellow
Account #1 is valid: True
Save account address and mnemonic listed above for account #1
Account #2 Address: M3FGYVMPEWK4WMOG7SFSC6E6AD5QMCR6XT2VGYSKWKKIE2NOKVB37TG4NU
Account #2 mnemonic: toss tonight grape army hospital worry act sugar fever pitch purity explain runway august snow rug improve tone tissue put nest radio shed about prepare
Account #2 is valid: True
Save account address and mnemonic listed above for account #2
Account #3 Address: O3SBYZS3H5JPLDW3FERCNKPB6DA6QV7GLO6ITWP7JYELF5PLZ4YUAVAPVQ
Account #3 mnemonic: cruise rely universe lock choice book affair deposit ensure firm fiscal course outer juice amused voice grape faculty another tomorrow goose clump minute abstract annual
Account #3 is valid: True
Save account address and mnemonic listed above for account #3
Signed transaction with transaction ID: K4HBW7JOYISZ6I3I43MBSLFCFBTZ7FCU5ZHZYSB4KHQ7NKMNI57Q
Successfully sent transaction with ID: K4HBW7JOYISZ6I3I43MBSLFCFBTZ7FCU5ZHZYSB4KHQ7NKMNI57Q
The confirmed round is: 14282356
Account #1 information:
{
  "address": "B2N2DQCGEROOYJZLME5QDEQPLKGAAY6VEODYJZYUOK76DRS7W3RA6QTEBI",
  "amount": 8999000,
  "amount-without-pending-rewards": 8999000,
  "apps-local-state": [],
  "apps-total-schema": {
    "num-uint": 0,
    "num-byte-slice": 0
  },
  "assets": [],
  "created-apps": [],
  "created-assets": [],
  "pending-rewards": 0,
  "reward-base": 27521,
  "rewards": 0,
  "round": 14282356,
  "status": "Offline"
}
Account #2 information:
{
  "address": "M3FGYVMPEWK4WMOG7SFSC6E6AD5QMCR6XT2VGYSKWKKIE2NOKVB37TG4NU",
  "amount": 11000000,
  "amount-without-pending-rewards": 11000000,
  "apps-local-state": [],
  "apps-total-schema": {
    "num-uint": 0,
    "num-byte-slice": 0
  },
  "assets": [],
  "created-apps": [],
  "created-assets": [],
  "pending-rewards": 0,
  "reward-base": 27521,
  "rewards": 0,
  "round": 14282356,
  "status": "Offline"
}
Account #3 information:
{
  "address": "O3SBYZS3H5JPLDW3FERCNKPB6DA6QV7GLO6ITWP7JYELF5PLZ4YUAVAPVQ",
  "amount": 10000000,
  "amount-without-pending-rewards": 10000000,
  "apps-local-state": [],
  "apps-total-schema": {
    "num-uint": 0,
    "num-byte-slice": 0
  },
  "assets": [],
  "created-apps": [],
  "created-assets": [],
  "pending-rewards": 0,
  "reward-base": 27521,
  "rewards": 0,
  "round": 14282356,
  "status": "Offline"
}
         */
    }
}