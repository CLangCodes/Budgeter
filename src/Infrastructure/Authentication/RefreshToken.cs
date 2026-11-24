using System.ComponentModel.DataAnnotations;
using Infrastructure.Identity;
using SharedKernel;

namespace Infrastructure.Authentication;

public class RefreshToken : Entity
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    public ApplicationUser User { get; set; }
}