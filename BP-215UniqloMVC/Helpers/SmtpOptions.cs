namespace BP_215UniqloMVC.Helpers
{
    public class SmtpOptions
    {
        public const string Name = "Smtp";
        public string Host {  get; set; }
        public int Port { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }

    }
}
