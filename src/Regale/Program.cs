using Regale.Estimation;
using Regale.Solver;
using System.Text.Json;
using OneOf;
using OneOf.Types;

namespace Regale;

class Program
{

    /// <summary>
    /// Solves the provided json files.<br\>
    /// If the provided path points to a directory all containing json files are solved.<br\>
    /// If the provided path points to a single file it is solved (the second argument is the name of the solution).<br\>
    /// Otherwise the execution will be stopped with error.
    /// </summary>
    /// <param name="args">
    /// command line arguments,
    /// only one parameter is allowed
    /// </param>
    public static async Task<int> Main(params string[] args)
    {
        if (args.Length < 1 || args.Length > 2)
        {
            Console.Error.WriteLine($"Expected 1 or 2 arguments but got {args.Length}.");
            Console.Error.WriteLine("Exiting with error.");
            return -1;
        }
        var filePath = args[0];
        if (File.Exists(filePath))
        {
            Console.WriteLine("Found single file. Solving this problem");
            var solutionPath = args.Length == 2 ? args[1] : FromFileNameToSolutionName(filePath);
            if (solutionPath != "-")
            {
                var dir = Path.GetDirectoryName(solutionPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(solutionPath))
                    Directory.CreateDirectory(dir);
            }
            await SolveFile(filePath, solutionPath);
        }
        else if (Directory.Exists(filePath))
        {
            Console.WriteLine("Found Directory. Trying to parse and solve each file.");
            var files = Directory.EnumerateFiles(filePath, "*.json");
            await System.Threading.Tasks.Parallel.ForEachAsync(
                files, async (file, _) =>
                    await SolveFile(file, FromFileNameToSolutionName(file))
            );
        }
        else
        {
            Console.Error.WriteLine($"Path {filePath} does not exist.");
            return -1;
        }
        return 0;
    }

    /// <summary>
    /// Solves the problem represented by <paramref name="file"/>.
    /// Writes to stderr, if the file is not well formatted.
    /// </summary>
    public static async Task SolveFile(string file, string solutionName)
    {
        await ParseWrapper(file).Match(
            async (problem) => { await SolveProblem(problem, solutionName); },
            async _ =>
            {
                Console.Error.WriteLine($"THe given file {file} was malformed.");
                await Task.CompletedTask;
            }
        );
    }

    public static async Task SolveProblem(Problem problem, string file)
    {
        var orchestration = new Orchestration<MMCost, Solver.Routing.PresentSpaceRouting>(problem);
        var mapIter = orchestration.IterateSteps();
        await WriteSolutionAsync(mapIter, file);
    }

    private static OneOf<Problem, Error<string>> ParseWrapper(string file)
    {
        using var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Problem.Parse(stream);
    }

    private static string FromFileNameToSolutionName(string fileName) =>
        Path.GetDirectoryName(fileName) +
        Path.DirectorySeparatorChar +
        Path.GetFileNameWithoutExtension(fileName) +
        "_sol.json";

    /// <summary>
    /// Writes solution represented by the enumerator <paramref name="solution"/> to <paramref name="file"/> asynchronously.
    /// </summary>
    private static async Task WriteSolutionAsync(IEnumerable<MoveMap> solution, string file)
    {
        // open file
        using Stream stream = file == "-" ?
            new MemoryStream() :
            new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
        // collect json
        using var jsonWriter = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });

        jsonWriter.WriteStartArray();
        foreach (MoveMap map in solution)
        {
            map.Save(jsonWriter);
        }
        jsonWriter.WriteEndArray();
        await jsonWriter.FlushAsync();
        stream.SetLength(stream.Position);

        if (file == "-")
        {
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(((MemoryStream)stream).ToArray()));
        }
    }
}
