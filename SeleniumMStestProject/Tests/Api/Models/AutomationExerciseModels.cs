using System.Text.Json.Serialization;

namespace SeleniumMStestProject.Tests.Api.Models
{
    internal sealed record Brand(int Id, [property: JsonPropertyName("brand")] string Name);

    internal sealed record BrandsListResponse(int ResponseCode, List<Brand> Brands);

    internal sealed record UserDetail(int Id, string Name, string Email, string City);

    internal sealed record UserDetailResponse(int ResponseCode, UserDetail User);
}
