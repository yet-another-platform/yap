using System;

namespace Client.Exceptions;

public class UnknownViewException(string viewName) : Exception($"Unknown view '{viewName}'");