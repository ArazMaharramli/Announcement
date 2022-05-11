using System;
using PhoneNumbers;

namespace Common.Validators
{
    public static class PhoneNumberValidator
    {
        public static bool Validate(string phoneNumber)
        {
            try
            {
                PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
                PhoneNumber phoneNo = phoneUtil.Parse(phoneNumber, "");

                //var region = phoneUtil.GetRegionCodeForNumber(phoneNo);
                var isValid = phoneUtil.IsValidNumber(phoneNo);
                return isValid;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
