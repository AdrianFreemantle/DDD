namespace Shell
{
    public interface IConsoleView
    {
        string Key { get; }
        string Usage { get; }
        void Print(string[] args);
    }
}
