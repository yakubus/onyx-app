using Abstractions.Messaging;
using Identity.Application.Contracts.Models;

namespace Identity.Application.Auth.VerifyEmail;

public sealed record VerifyEmailCommand(string Email, string VerificationCode) : ICommand<AuthorizationToken>;