using Carter;
using TestContainerApi.Domain.Users.Get;

namespace TestContainerApi.WebApi.Endpoints
{
    public class GetUsersEndpoint : CarterModule
    {
        public GetUsersEndpoint() : base("/api/Users")
        {
            WithTags("Users");
            IncludeInOpenApi();
        }

        public override void AddRoutes(IEndpointRouteBuilder app)   
        {
            app.MapGet("", async (IGetUsersRepository getUsersRepository) =>
            {
                var users = await getUsersRepository.GetUsers();
                if (!users.Any())
                    return Results.NoContent();

                return Results.Ok(users);
            });

            app.MapGet("/{id:int}", async (IGetUsersService getUsersService, int id) =>
            {
                var user = await getUsersService.GetUsersById(id);
                if (user == null)
                    return Results.NotFound();

                return Results.Ok(user);
            });
        }
    }
}
