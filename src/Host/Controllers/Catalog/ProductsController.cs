using MCH.Application.Catalog.Products;

namespace MCH.Host.Controllers.Catalog;

public class ProductsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(MCHAction.Search, MCHResource.Products)]
    [OpenApiOperation("Search products using available filters.", "")]
    public Task<PaginationResponse<ProductDto>> SearchAsync(SearchProductsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(MCHAction.View, MCHResource.Products)]
    [OpenApiOperation("Get product details.", "")]
    public Task<ProductDetailsDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetProductRequest(id));
    }

    [HttpGet("dapper")]
    [MustHavePermission(MCHAction.View, MCHResource.Products)]
    [OpenApiOperation("Get product details via dapper.", "")]
    public Task<ProductDto> GetDapperAsync(Guid id)
    {
        return Mediator.Send(new GetProductViaDapperRequest(id));
    }

    [HttpPost]
    [MustHavePermission(MCHAction.Create, MCHResource.Products)]
    [OpenApiOperation("Create a new product.", "")]
    public Task<Guid> CreateAsync(CreateProductRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(MCHAction.Update, MCHResource.Products)]
    [OpenApiOperation("Update a product.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateProductRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(MCHAction.Delete, MCHResource.Products)]
    [OpenApiOperation("Delete a product.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteProductRequest(id));
    }

    [HttpPost("export")]
    [MustHavePermission(MCHAction.Export, MCHResource.Products)]
    [OpenApiOperation("Export a products.", "")]
    public async Task<FileResult> ExportAsync(ExportProductsRequest filter)
    {
        var result = await Mediator.Send(filter);
        return File(result, "application/octet-stream", "ProductExports");
    }
    }