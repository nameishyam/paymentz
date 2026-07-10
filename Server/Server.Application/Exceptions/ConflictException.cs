namespace Server.Application.Exceptions;

public class ConflictException(string email) 
    : Exception($"user with the email {email} already exists");