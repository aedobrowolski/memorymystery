namespace MemoryMystery
{
    /// <summary>
    /// Stores a command and its result.
    /// </summary>
    internal class HistoryItem
    {
        private string[] Args { get; }
        public object Result { private get; set; }
        public string CmdKey => Args?[0];

        public HistoryItem(string[] args) => Args = args;

        public override string ToString() => string.Join(" ", Args) + " => " + Program.Show(Result);
    }
}