namespace Shell
{
    public interface IConsoleView
    {
        string Key { get; }
        void Print(string[] args);
    }
}
