# ginny

repo for learning ASP.NET Core and EF Core

## 01_TaskAPI

### What is this?

I learn ASP.NET Core, EF Core (with PostgreSQL running on a Docker container) with this project.

It's a very dumb app with just a couple entities, Todos and Users.

Has the basic CRUD functionality for Todos, with GET, POST, PATCH, PUT and DELETE methods, all requiring authorization. But again, there are two exceptional routes GET /tasksAll and /tasksDeep that don't require any authorization. You can just enter them and see everybody's resources in a single request. Why two? Well, /tasksAll passes the TodoItem entity through TodoItemResponseDto, but /tasksDeep just sends it out straight out of the database with a few LINQ Select() operations to include User object and SubTasks object. Oh, I almost forgot about the SubTasks entity. That's just something I added to experiment with one to many relationships in EF Core, working with migrations and learning to not wipe the database in the process. And fixing the circular reference errors.

The SubTasks entity has no endpoints for itself. It doesn't have an interface in my API. I didn't bother doing so after adding the migrations. I just wanted to test /tasksDeep and whether it correctly categorized SubTasks by TodoItems, so I inserted some SubTasks rows from psql.

After that, the bigger and I assume the more important part of what I learned while doing this was, user authentication and authorization. I worked with BCrypt, JWT configuration and understood what they were. This was probably the part where most of my time was spent not writing code. Writing code here was easy. Finding subtle bugs here and there, and reading the cryptic C# stack traces were not so easy. One such example I'll probably remember for a long time is using `_config["Issuer"]` where I should've used `_config["Jwt:Issuer"]`. Only after about half an hour did I notice that on `jwt.io` my decoded payload did not have Issuer and Audience, meaning the problem was when I'm creating the Jwt token, not when reading it. From there it was quite easy.

### Current State

It works. There is no frontend, and I still haven't figured out how to send requests with Authorization headers from chrome. I'm just using this VS Code / REST Client Extension feature with .http files, and Hoppscotch.

As long as you pass in good DTO style JSON bodies for POST, PATCH and PUT requests, everything's fine. The moment you pass in an empty string instead of null, something other than a date for a date, or if a date with a timezone `+05:30` instead of UTC `Z`, you run into some problems.

### How to run

1. Start the `postgres-db-1` container on Docker
   * If starting from scratch, create a container from the official PostgreSQL Docker Image
   * Create a user and a database with `psql`
   * Plug in the hostname, port, credentials and database name in `appsettings.Development.json`
   * Run `dotnet ef database update`
2. Run `dotnet run` on the terminal

### Notes to future me

* Does literally nothing, but a great learning exercise
* Maybe I should try integrating OpenAPI docs with Swagger/Scalar
* After this, I'm going to start with URL Shortener
