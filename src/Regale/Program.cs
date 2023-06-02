class Program {

    /// <summary>
    /// Solves the provided files.<br\>
    /// If the provided path points to a directory all containing files are solved.<br\>
    /// If the provided path points to a single file it is solved.<br\>
    /// Otherwise the execution will be stopped with error.
    /// </summary>
    /// <param name="args">
    /// command line arguments,
    /// only one parameter is allowed
    /// </param>
    public static void Main (params string[] args) {
        if (args.Length != 1) {
            Console.Error.WriteLine($"Expected 1 argument but got {args.Length}.");
            Console.Error.WriteLine("Exiting with error.");
            System.Environment.Exit(-1);
        }
        var filePath = args[0];
        if (File.Exists(filePath)) {
            Console.WriteLine("Found single file. Solving this problem");
            SolveFile(filePath);
        } else if (Directory.Exists(filePath)) {
            Console.WriteLine("Found Directory. Trying to parse and solve each file.");
            var files = Directory.EnumerateFiles(filePath);
            System.Threading.Tasks.Parallel.ForEach(files, SolveFile);
        } else {
            Console.Error.WriteLine($"Path {filePath} does not exist.");
            System.Environment.Exit(-1);
        }
    }

    /// <summary>
    /// Solves the problem represented by <paramref name="file"/>.
    /// Throws an exception if the file is not well formatted.
    /// </summary>
    public static void SolveFile(string file) {
        throw new NotImplementedException("");
    }
}
