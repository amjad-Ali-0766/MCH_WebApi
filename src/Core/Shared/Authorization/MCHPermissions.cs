using System.Collections.ObjectModel;

namespace MCH.Shared.Authorization;

public static class MCHAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class MCHResource
{
    public const string Tenants = nameof(Tenants);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Products = nameof(Products);
    public const string Brands = nameof(Brands);
}

public static class MCHPermissions
{
    private static readonly MCHPermission[] _all = new MCHPermission[]
    {
        new("View Dashboard", MCHAction.View, MCHResource.Dashboard),
        new("View Hangfire", MCHAction.View, MCHResource.Hangfire),
        new("View Users", MCHAction.View, MCHResource.Users),
        new("Search Users", MCHAction.Search, MCHResource.Users),
        new("Create Users", MCHAction.Create, MCHResource.Users),
        new("Update Users", MCHAction.Update, MCHResource.Users),
        new("Delete Users", MCHAction.Delete, MCHResource.Users),
        new("Export Users", MCHAction.Export, MCHResource.Users),
        new("View UserRoles", MCHAction.View, MCHResource.UserRoles),
        new("Update UserRoles", MCHAction.Update, MCHResource.UserRoles),
        new("View Roles", MCHAction.View, MCHResource.Roles),
        new("Create Roles", MCHAction.Create, MCHResource.Roles),
        new("Update Roles", MCHAction.Update, MCHResource.Roles),
        new("Delete Roles", MCHAction.Delete, MCHResource.Roles),
        new("View RoleClaims", MCHAction.View, MCHResource.RoleClaims),
        new("Update RoleClaims", MCHAction.Update, MCHResource.RoleClaims),
        new("View Products", MCHAction.View, MCHResource.Products, IsBasic: true),
        new("Search Products", MCHAction.Search, MCHResource.Products, IsBasic: true),
        new("Create Products", MCHAction.Create, MCHResource.Products),
        new("Update Products", MCHAction.Update, MCHResource.Products),
        new("Delete Products", MCHAction.Delete, MCHResource.Products),
        new("Export Products", MCHAction.Export, MCHResource.Products),
        new("View Brands", MCHAction.View, MCHResource.Brands, IsBasic: true),
        new("Search Brands", MCHAction.Search, MCHResource.Brands, IsBasic: true),
        new("Create Brands", MCHAction.Create, MCHResource.Brands),
        new("Update Brands", MCHAction.Update, MCHResource.Brands),
        new("Delete Brands", MCHAction.Delete, MCHResource.Brands),
        new("Generate Brands", MCHAction.Generate, MCHResource.Brands),
        new("Clean Brands", MCHAction.Clean, MCHResource.Brands),
        new("View Tenants", MCHAction.View, MCHResource.Tenants, IsRoot: true),
        new("Create Tenants", MCHAction.Create, MCHResource.Tenants, IsRoot: true),
        new("Update Tenants", MCHAction.Update, MCHResource.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", MCHAction.UpgradeSubscription, MCHResource.Tenants, IsRoot: true)
    };

    public static IReadOnlyList<MCHPermission> All { get; } = new ReadOnlyCollection<MCHPermission>(_all);
    public static IReadOnlyList<MCHPermission> Root { get; } = new ReadOnlyCollection<MCHPermission>(_all.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<MCHPermission> Admin { get; } = new ReadOnlyCollection<MCHPermission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<MCHPermission> Basic { get; } = new ReadOnlyCollection<MCHPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record MCHPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}
