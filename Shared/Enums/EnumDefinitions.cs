namespace Shared.Enums
{
    public enum CallStatus
    {
        Requested,
        Calling,
        NotAnswered,
        Mailvoiced,
        Active,
        Completed,
        CancelledExternal
    }

    public enum IdentifierType
    {
        SIM,
        PhoneNumber,
        KeychainIdentifier
    }

    public enum Language
    {
        en,
        es,
        fr,
        pt
    }

    public enum Platform
    {
        Missing,
        iOS,
        Android
    }

    public enum Store
    {
        AppStore,
        GooglePlay
    }

    public enum StoreTransactionStatus
    {
        Created,
        Cancelled,
        Refunded,
        Completed
    }

    public enum UserLogType
    {
        IP,
        Login,
        SignUp,
        GoogleLogin,
        GoogleLinked,
        FacebookLogin,
        FacebookLinked,
        Deposit,
        Withdraw,
        Limitation,
        Bet,
        EmailChanged,
        UsernameChanged,
        PasswordChanged,
        BirthChanged,
        CountryChanged,
        ChatMessage,
        Notification,
        Kyc,
        AuthCheck
    }
}
