/*using Security;
using Foundation;
using System;
*/
namespace MauiPranksterApp.Native.iOS
{/*
    public static class KeyChain
    {
        public static string ValueForKey(string key, SecRecord record = null)
        {
            if (record is null)
            record = ExistingRecordForKey(key);

            SecStatusCode resultCode;
            var match = SecKeyChain.QueryAsRecord(record, out resultCode);

            if (resultCode == SecStatusCode.Success)
                return NSString.FromData(match.ValueData, NSStringEncoding.UTF8);
            else
                return string.Empty;
        }

        public static void SetValueForKey(string value, string key)
        {
            var record = ExistingRecordForKey(key);
            string valueForKey = ValueForKey(key, record); // debug purposes
            Console.WriteLine("keyValueBefore: => " + (string.IsNullOrEmpty(valueForKey) ? "nothing" : valueForKey));

            // if the key already exists, remove it
            if (!string.IsNullOrEmpty(valueForKey))
                RemoveRecord(record);

            if (string.IsNullOrEmpty(value))
                return;

                var result = SecKeyChain.Add(CreateRecordForNewKeyValue(key, value));
            if (result != SecStatusCode.Success)
            {
                throw new Exception(string.Format("Error adding record: {0}", result));
            }
        }

        private static SecRecord CreateRecordForNewKeyValue(string key, string value)
        {
            return new SecRecord(SecKind.GenericPassword)
            {
                Account = key,
                Service = "PranksterApp",
                Label = key,
                ValueData = NSData.FromString(value, NSStringEncoding.UTF8),
            };
        }

        private static SecRecord ExistingRecordForKey(string key)
        {
            return new SecRecord(SecKind.GenericPassword)
            {
                Account = key,
                Service = "PranksterApp",
                Label = key,
            };
        }

        private static bool RemoveRecord(SecRecord record)
        {
            var result = SecKeyChain.Remove(record);
            if (result != SecStatusCode.Success)
            {
                throw new Exception(string.Format("Error removing record: {0}", result));
            }

            return true;
        }


    }
    */
}
