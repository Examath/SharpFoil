# SharpFoil

A WebAssembly application hat generates approximated equations of airfoils for use in Autodesk Inventor.

[Try it out now!](https://github.com/Examath/SharpFoil)

This app is designed to meet a unique challenge – parametrically adding airfoils to inventor. The current process of using splines is tedious and difficult to edit, whilst most other software that could do this is currently propietary.

Instead, _SharpFoil_ uses multiple regression to generate a orthogonal pair of polynomials that approximate any given airfoil. _SharpFoil_ supports importing points in the Selig format. These equations can then be copied into inventor.

_SharpFoil_ is the succesor to _[yFoil](https://github.com/Examath/yFoil)_, a MATLAB App I made earlier. It is designed to be faster, easier and more accesable. _SharpFoil_ is written in C# using the [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) framework, with [Fluent UI](https://www.fluentui-blazor.net/) and [Math.NET](https://numerics.mathdotnet.com/) for computations.
