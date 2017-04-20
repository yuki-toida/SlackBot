namespace SlackBot.UI.Dto.Slack
{
    public class MessageDto
    {
        public string Type { get; set; }
        public string Channel { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public string Ts { get; set; }
    }
}
