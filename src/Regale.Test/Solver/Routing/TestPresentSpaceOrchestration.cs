using System.Text.Json;
using Regale.Estimation;
using Regale.Solver;
using Regale.Solver.Routing;

namespace Regale.Test.Solver.Routing;

#pragma warning disable NUnit2007, NUnit2005, NUnit2003, NUnit2019, NUnit2017

public sealed class TestPresentSpaceOrchestration
{
    private static void TestTermination(Problem problem)
    {
        var orchestration = new Orchestration<MMCost, PresentSpaceRouting>(problem);
        var counter = 0;
        using var m = new MemoryStream();
        using var w = new Utf8JsonWriter(m, new JsonWriterOptions
        {
            Indented = true
        });
        w.WriteStartArray();
        var success = false;
        try
        {
            foreach (var map in orchestration.IterateSteps())
            {
                counter++;
                map.Save(w);
                if (counter > 100)
                {
                    Console.WriteLine($"Abort at 100 steps");
                    break;
                }
            }
            Console.WriteLine($"Finished after {counter} steps");
            success = true;
        }
        finally
        {
            if (!success)
                Console.WriteLine($"Error at {counter} step");
            w.WriteEndArray();
            w.Flush();
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(m.ToArray()));
        }
    }

    [Test]
    public void Test1()
    {
        var map = new Map(3, 3);
        map.Init(new[]
        {
            new[] { Field.Package, Field.Package, Field.Package, },
            new[] { Field.Present, Field.Package, Field.Package, },
            new[] { Field.None,    Field.Present, Field.Package, },
        });
        var depot = new Position[]
        {
            new(2, 0),
        };
        TestTermination(new Problem(map, depot));
    }

    [Test]
    public void Test2()
    {
        var map = new Map(3, 3);
        map.Init(new[]
        {
            new[] { Field.Package, Field.Package, Field.Package, },
            new[] { Field.Present, Field.None,    Field.Package, },
            new[] { Field.Package, Field.Present, Field.Package, },
        });
        var depot = new Position[]
        {
            new(2, 0),
        };
        TestTermination(new Problem(map, depot));
    }

    [Test]
    public void Test3()
    {
        var map = new Map(3, 3);
        map.Init(new[]
        {
            new[] { Field.Package, Field.Package, Field.None,    },
            new[] { Field.Package, Field.Package, Field.Package, },
            new[] { Field.Present, Field.Package, Field.Package, },
        });
        var depot = new Position[]
        {
            new(2, 0),
        };
        TestTermination(new Problem(map, depot));
    }
}
