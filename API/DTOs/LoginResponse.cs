﻿namespace API.DTOs
{
    public record LoginResponse(bool Flag, string Message = null!, string Token = null!);
}