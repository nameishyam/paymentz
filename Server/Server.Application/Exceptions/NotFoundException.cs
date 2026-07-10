namespace Server.Application.Exceptions;

public class NotFoundException(string email)
    : Exception($"user with the email {email} not exists");