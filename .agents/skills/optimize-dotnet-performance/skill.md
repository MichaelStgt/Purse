---
name: optimize-dotnet-performance
description: Optimize .NET application performance by measuring bottlenecks first, then improving hot paths, allocations, async usage, LINQ, logging, and data access patterns. Use when code is slow, allocates too much memory, or shows high CPU usage.
license: MIT
---

# Optimize .NET Performance

## When to Use

- Requests are slow or throughput is low
- CPU usage is unexpectedly high
- Memory allocations are excessive
- Large object graphs or collections are causing pressure
- Hot paths are using inefficient LINQ, string handling, or logging
- Async code is blocking threads or causing thread-pool starvation
- Database access is part of the slowdown

## When Not to Use

- The issue is purely functional and not performance-related
- The slowdown is caused by external infrastructure only
- No measurable symptom exists yet
- The code needs migration or modernization rather than optimization

## Inputs

| Input                      | Required | Description                                                |
| -------------------------- | -------- | ---------------------------------------------------------- |
| Slow code path or scenario | Yes      | The method, request, screen, job, or workflow that is slow |
| Measurements or logs       | No       | Timing, profiler output, allocations, traces, SQL logs     |
| Relevant code              | Yes      | The code to inspect and improve                            |

## Workflow

### Step 1: Measure before changing code

- Identify the exact slow scenario
- Capture elapsed time, allocation data, and call counts
- Prefer profiling or benchmarking over guessing
- Focus on the hottest path first

### Step 2: Remove obvious hot-path inefficiencies

Check for:

- Repeated enumeration of `IEnumerable<T>`
- Unnecessary `ToList()` or `ToArray()`
- Expensive LINQ chains inside loops
- Repeated string concatenation in tight loops
- Reflection or serialization on hot paths
- Excessive logging in frequently executed code
- Blocking calls like `.Result`, `.Wait()`, or sync-over-async

### Step 3: Reduce allocations

Prefer:

- Reusing buffers or objects where safe
- `StringBuilder` for repeated string composition
- Avoiding closure allocations in hot loops
- Avoiding boxing and unnecessary conversions
- Returning lightweight projections instead of large graphs

### Step 4: Improve collection and lookup choices

Prefer:

- `Dictionary<TKey, TValue>` / `HashSet<T>` for repeated lookups
- Pre-sizing collections when counts are known
- Avoiding nested loops when a lookup structure is better
- Using arrays or spans for tight, performance-critical paths when appropriate

### Step 5: Fix async and concurrency issues

Check for:

- Blocking async calls
- Missing cancellation tokens
- Too much parallelism causing contention
- Unnecessary `Task.Run()` in server code
- Sequential awaits that can safely run concurrently

### Step 6: Optimize data access

Check for:

- N+1 queries
- Missing `AsNoTracking()` for read-only EF Core queries
- Over-fetching columns or entities
- Premature materialization
- Missing batching for updates/deletes

### Step 7: Re-measure and validate

- Compare before/after timings
- Confirm allocation reductions
- Ensure readability and correctness remain acceptable
- Keep only changes with measurable benefit

## Heuristics

### Prefer simple loops on hot paths

- Avoid complex LINQ queries in performance-critical sections
- Use `for` or `foreach` loops when iteration is straightforward
- Minimize allocations inside loops
- Consider `Span<T>` or `Memory<T>` for array slicing without allocations

### Avoid repeated enumeration
### Use `Any()` instead of `Count() > 0`
### Use `AsNoTracking()` for read-only EF Core queries


## Validation

- [ ] A measurable baseline exists
- [ ] The hottest path was optimized first
- [ ] Allocations were reduced where relevant
- [ ] Blocking async patterns were removed
- [ ] Data access was reviewed for over-fetching or N+1 issues
- [ ] Before/after measurements confirm improvement

## Common Pitfalls

| Pitfall                                                  | Solution                                        |
| -------------------------------------------------------- | ----------------------------------------------- |
| Optimizing without measurement                           | Profile or benchmark first                      |
| Replacing clear code with micro-optimizations everywhere | Limit optimizations to hot paths                |
| Calling `ToList()` too early                             | Filter/project before materializing             |
| Using LINQ heavily inside tight loops                    | Consider a simple loop                          |
| Sync-over-async with `.Result` or `.Wait()`              | Use `await` end-to-end                          |
| Logging too much in hot paths                            | Reduce log volume or defer expensive formatting |
| Large EF Core graphs for simple reads                    | Project only needed fields                      |
| Parallelizing everything                                 | Measure contention and throughput first         |
