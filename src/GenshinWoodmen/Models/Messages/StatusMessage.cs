namespace GenshinWoodmen.Models
{
    internal class StatusMessage
    {
        public string Message { get; } = null!;

        public StatusMessage(string message)
        {
            Message = message;
        }

        public override string ToString() => Message;
    }
}
