namespace Commands
{
    public interface IConsoleCommand 
    {
        string[] Keys { get; }
        string Usage { get; }

        void Build(string[] args);
    }
}