# Notino homework implementation

Solution consists of  4 projects:

 1. **Converter.Sample** .NET core console application with base version of conversion sample code (with comments + refactored version).
 2. **Document.Conversion** .NET core library containing all domain logic/interfaces/implementations for document conversion.
 3. **DocumentConversionApi** ASP .NET core project with API conversion endpoint and some DI wiring code.
 4. **Document.Conversion.Tests**  xUnit test project with some "test server" request made to API + some skipped placeholder tests which would be great to have.


# TODOs

Application is far from production ready, due to time limit constrains. There is no logging, only basic request validations, and not enough unit tests for conversion components and for possible attack vectors (directory traversal, request file size validation, file type validation,  HTTP file size download limit validation, ...).