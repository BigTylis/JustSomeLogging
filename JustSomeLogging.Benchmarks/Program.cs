using BenchmarkDotNet.Running;

namespace JSL.Benchmarks;

internal static class Program
{
    public static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(GeneralBenchmarks).Assembly).Run(args);
}