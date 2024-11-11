﻿namespace Domain.Shared;

public record Error(string Code, string? Description)
{
    public static Error None => new Error(string.Empty, null);
}