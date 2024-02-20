# SantanderTest
ASP.NET Core API app targeting .NET 8

# Running the app
I recommend using Visual Studio and profile 'http' to run the app. This will open a browser and load a Swagger UI - which should make testing straightforward.

# Assumptions
The Hacker News API documentation states that there's currently no rate limit. I have however attempted to both reduce the number of calls to the API and increase performance by using a cache.

# Enhancements
Given the time, I would add some additional `HackerNewsService` tests. If the app was ever going to be scaled horizontally, a distributed cache would be necessary.

# Edit - 20/02/24
Apologies for the edit, I was under the weather yesterday (fine excuse) and forgot to put a couple of points in this doc that I should have. I realised late last night when thinking about the task.
1. I assumed that IDs returned from the `/v0/beststories.json` API are ordered (descending) by their associated story's score. I did check a number of random the stories, they appeared to be in the aforementioned order.
2. I also made a mistake here, I forgot to perform a not-null check on the result of the call to the `/v0/beststories.json` API; in the case of a null result, appropriate action should've been taken.
3. A further enhancement that I would make would be to send the logs to a remote logging service/repo.

# Edit - 20/02/24 (2)
I've made changes under the commit `Fixes` to perform error handling when calling the Hacker News API. I realise that this is somewhat cheeky, but I couldn't ignore the mistake I'd made and felt I had to show you how I'd correct it.
