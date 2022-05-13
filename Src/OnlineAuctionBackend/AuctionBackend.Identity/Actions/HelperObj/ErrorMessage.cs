namespace AuctionBackend.Identity.Actions.HelperObj
{
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            Message = message;
        }
        public string Message { get; }
        public override string ToString()
        {
            return Message;
        }
    }
}
