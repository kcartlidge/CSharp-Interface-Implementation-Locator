# C# Interface Implementation Locator
Fetches all current-asembly implementations of a given interface dynamically without needing full-blown IOC containers.

Dependency injection is great. You know, inversion of control, constructor injection and all the other buzzwords.

Sometimes however it can be overkill.

I recently knocked up a tool that, having no UI and just a progress indicator, was ideally suited to a console application. One of the things it did was to create an export in a variety of formats (text, RTF and EPUB). I wanted it to be easy to add new export implementations, ideally without needing setup code.

Naturally I coded to an interface (*IGenerator*) and all the exporters implemented it. What I needed was a way to gather all things that implement that interface and run them in turn, in such a way that if new ones were added they were automatically picked up (the nature of the code meant that an actual IOC container setup was not needed).

This is the class I created, and it's called like so:

``` c#
static List<IGenerator> generators = ImplementationLocator.GetImplementations<IGenerator>();
```

Its return value is a collection of actual instances of each implementation that I can call like this:

``` c#
foreach (var generator in generators) generator.Generate(document, folder);
```
