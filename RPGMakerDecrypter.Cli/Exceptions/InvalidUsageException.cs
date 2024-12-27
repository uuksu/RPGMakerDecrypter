using System;

namespace RPGMakerDecrypter.Cli.Exceptions;

public class InvalidUsageException(string message) : Exception(message);