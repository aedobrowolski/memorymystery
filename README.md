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

## Exercise

In the exercise below you will be asked to run a console application that
performs certain arithmetic operations related to prime numbers. The application
has a fixed heap size 20Mb to establish a level playing field. These require
lots of memory. Your job will be to achieve these simultaneous goals:

* Lower the peak memory usage
* Remove all memory leaks
* Run fewer GC cycles

Use a memory profiler to find the leaks and causes of GC pressure.

## Memory Management Mystery

The application is a command line tool to perform some math operations. I have
deliberately introduced some memory leaks and bad memory traffic. Your job will
be to run the application under your favorite memory diagnostic tool to find
the root causes of all memory problems. Optionally you can try to fix the
issues and make the application run with optimal performance.
