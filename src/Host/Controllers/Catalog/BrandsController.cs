using MCH.Application.Catalog.Brands;

namespace MCH.Host.Controllers.Catalog;

public class BrandsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(MCHAction.Search, MCHResource.Brands)]
    [OpenApiOperation("Search brands using available filters.", "")]
    public Task<PaginationResponse<BrandDto>> SearchAsync(SearchBrandsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(MCHAction.View, MCHResource.Brands)]
    [OpenApiOperation("Get brand details.", "")]
    public Task<BrandDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetBrandRequest(id));
    }

    [HttpPost]
    [MustHavePermission(MCHAction.Create, MCHResource.Brands)]
    [OpenApiOperation("Create a new brand.", "")]
    public Task<Guid> CreateAsync(CreateBrandRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(MCHAction.Update, MCHResource.Brands)]
    [OpenApiOperation("Update a brand.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateBrandRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(MCHAction.Delete, MCHResource.Brands)]
    [OpenApiOperation("Delete a brand.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteBrandRequest(id));
    }

    [HttpPost("generate-random")]
    [MustHavePermission(MCHAction.Generate, MCHResource.Brands)]
    [OpenApiOperation("Generate a number of random brands.", "")]
    public Task<string> GenerateRandomAsync(GenerateRandomBrandRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpDelete("delete-random")]
    [MustHavePermission(MCHAction.Clean, MCHResource.Brands)]
    [OpenApiOperation("Delete the brands generated with the generate-random call.", "")]
    [ApiConventionMethod(typeof(MCHApiConventions), nameof(MCHApiConventions.Search))]
    public Task<string> DeleteRandomAsync()
    {
        return Mediator.Send(new DeleteRandomBrandRequest());
    }
}