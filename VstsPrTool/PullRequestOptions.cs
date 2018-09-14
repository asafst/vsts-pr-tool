namespace VstsPrTool
{
    public class PullRequestOptions
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return $"Title: {this.Title}, Description: {this.Description}";
        }
    }
}