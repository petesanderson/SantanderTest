# SantanderTest
ASP.NET Core API app targeting .NET 8

# Running the app
I recommend using Visual Studio and profile 'http' to run the app. This will open a browser and load a Swagger UI - which should make testing straightforward.

# Assumptions
The Hacker News API documentation states that there's currently no rate limit. I have however attempted to both reduce the number of calls to the API and increase performance by using a cache.

# Enhancements
Given the time, I would add some additional `HackerNewsService` tests. If the app was ever going to be scaled horizontally, a distributed cache would be necessary.
