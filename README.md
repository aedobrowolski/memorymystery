# Memory Mystery

The code in this repository is a practical introduction to good memory management.

## Introduction

**Memory Leaks** and **GC Pressure** are common problems in complex managed
languages. The existence of a garbage collector means we developers can focus on
implementing features. .NET Core has an excellent generational garbage
collector. Unlike developers working in C and C++ we do not need to carefully
dispose of all the memory that we allocate on the heap, since we trust the
garbage collector to do this for us.

There are still dangers. Memory "leaks" are a huge issue in big applications and
they can be easily caused. In addition, a lack of care in creating objects can
lead to more GC cycles aka memory pressure which will adversely affect your
performance.

## Memory Leaks

The GC will not release memory that can be traced back to a GC Root. For example
appending log messages to a linked list can quickly lead to an
OutOfMemoryException.

There are four root types in .NET Framework:

Type | Definition
-----|-------------------
Stack references | references to local objects. Such roots live during a method execution.
Static | references to static objects. These roots live the entire app domain life time.
Handles | these are references used for communication between managed and unmanaged code.
Finalizer references | references to objects waiting to be finalized.

Finalizer roots live until the finalizer is run. For performance reasons this
happens after the GC that discovers the object since the finalizer can run
arbitrary code. The object is truly collected in the next GC.

A managed object that cannot be traced back to at least one GC root is eligible
for being collected. There are a number of common causes for memory leaks

* Static references to an object that should be temporary
* Local objects captured in a lambda function that lives beyond the life of the enclosing method
* Native memory leaks

Mitigation strategies include

* Avoiding static reference handles or nulling them out when no longer needed
* Always unsubscribing from events
* Calling the Dispose() method on objects that implement IDisposable

## GC Pressure

We are not out of the woods once we have eliminated any memory leaks in our
program. It is still possible that the program is allocating and collecting too
many objects in the course of a computation. This is known as memory traffic.

The causes of excessive memory traffic in .NET are

* Use of the heap instead of the stack for temporary objects
* Use of immutable objects (great for concurrency; bad for memory)
* Use of multiple threads (each stack variable adds more GC roots)

Mitigation strategies include

* Using value types (`structs` or `records` in C# 9) to make objects on the stack
* Reusing objects, for example in a cache, when instances rotate
* Nulling local variables before a long running async process to allow them to be collected
* Know when to use Finalizers. Objects with finalizers are promoted at least one additional generation.

## Memory Management Mystery

`MemoryMystery` is a command line tool (CLI) that performs some math operations.
I have deliberately introduced some memory leaks and bad memory traffic into the
application. Your job will be to run it under your favorite memory diagnostic
tool and to find the root causes of all memory problems. Optionally you can try
to fix the issues and make the application run with optimal performance.

The CLI runs a REPL (read-execute-print-loop) with these commands:

Command | Description
--------| --------------
`isPrime` _n_ | check if _n_  is prime and return a boolean
`factor` _n_ | return an ordered list of prime factors of _n_ and their powers
`divisors` _n_ | return a list of divisors of _n_ including itself
`countDivisors` _n_ | return the number of inclusive divisors of _n_
`multiply` _n m_ | multiply _n_ by _m_
`history` | return a list of the last 100 commands and results
`times` _n_  _cmd_ | execute command _cmd_ _n_ times
`repeat` _cmd_ | execute command _cmd_ twenty times

The last two commands are for load testing. They will not appear in the history
but the commands that they execute will. In most terminals you will be able to
use the arrow keys to repeat and modify a command that has already been entered.

The application works with signed `int32` values. Out of range errors will be
reported. All primes up to the Mersenne prime $2^{31}-1$ are correctly reported
as prime. However factoring requires a list of primes limiting the largest prime
that can be factored to 2147483587. Building the list using Erastothenes'
algorithm will take a very long time.

Try the following commands, listed in approximate order of increasing memory
pressure.

```text
countDivisors 28 => 6
divisors 28 => 1 2 4 7 14 28
isPrime 8192 => False
isPrime 8191 => True
factor 8192 => 1 * 2^13
isPrime 2147483647 => True
factor 2146654199 => 1 * 46327 * 46337
times 500 factor 2146654199 => 1 * 46327 * 46337
factor 987654321 => 1 * 3^2 * 17^2 * 379721
factor 897654321 => 1 * 3^2 * 31 * 3217399
```

## Exercise

Build and run the `memorymystery.exe` console application with some of the sample
commands listed above. These require lots of memory. The challenge is to achieve
these goals:

* Lower the peak memory usage
* Remove all memory leaks
* Run fewer GC cycles

Use a memory profiler to find the leaks and causes of GC pressure.