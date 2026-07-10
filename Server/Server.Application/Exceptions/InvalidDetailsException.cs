namespace Server.Application.Exceptions;

public class InvalidDetailsException()
    : Exception("given password didn't match");