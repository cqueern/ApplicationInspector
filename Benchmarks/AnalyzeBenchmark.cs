﻿using System;
using System.IO;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.ApplicationInspector.Commands;

namespace ApplicationInspector.Benchmarks;
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net60)]
//[ConcurrencyVisualizerProfiler]
public class AnalyzeBenchmark
{
    // Manually put the file you want to benchmark. But don't put this in a path with "Test" in the name ;)
    private const string path = "C:\\Users\\gstocco\\Documents\\GitHub\\ApplicationInspector\\AppInspector.RulesEngine";

    [Benchmark(Baseline = true)]
    public void AnalyzeSingleThreaded()
    {
        AnalyzeCommand command = new(new AnalyzeOptions
        {
            SourcePath = new string[1] { path },
            SingleThread = true,
            IgnoreDefaultRules = false,
            FilePathExclusions = new[] { "**/bin/**", "**/obj/**" },
            NoShowProgress = true
        });

        _ = command.GetResult();
    }

    [Benchmark]
    public void AnalyzeMultiThread()
    {
        AnalyzeCommand command = new(new AnalyzeOptions
        {
            SourcePath = new string[1] { path },
            SingleThread = false,
            IgnoreDefaultRules = false,
            FilePathExclusions = new[] { "**/bin/**", "**/obj/**" },
            NoShowProgress = true
        });

        _ = command.GetResult();
    }

    public static string GetExecutingDirectoryName()
    {
        if (Assembly.GetEntryAssembly()?.GetName().CodeBase is string codeBaseLoc)
        {
            var location = new Uri(codeBaseLoc);
            return new FileInfo(location.AbsolutePath).Directory?.FullName ?? string.Empty;
        }

        return string.Empty;
    }
}