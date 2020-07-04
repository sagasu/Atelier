To be honest, I had a tremor when I looked at OrderDataContext file :)
I was told to spend 45 min on this exercise, unfortunately, to address all the issues with a current implementation would require a significant more
amount of time. That's why I had to cut some corners, many of them. I was trying to comment on what am I doing and what challenges I see in code.

Generally speaking, when I looked at this solution I thought that in a real-life it would be easier to trash it and implement it 
using a different approach, but for the sake of this exercise, I left it and just marked my thoughts in comments.

I extracted plenty of code from the original solution to smaller classes, to align more with SOLID principles and make the code testable.
I would refactor code much more if not time limitation.

I understood in requirements that I was not allowed to drop the "static" keyword for OrderDataContext.LoadOrder method, that is why 
I left it as it is. This "static" is a code smell. I can elaborate more when asked.

I didn't introduce async DB calls, because it would change the method signature. In real life, it should be async.

I wasn't sure if the goal of this exercise is to map all the properties (SQL queries) and make sure that when we save Order we also save Customer and so on. 
This same situation is with reading from DB; I wasn't sure if the goal is to read every property when we receive Order.
Generally looking at the project structure and code quality I decided not to map everything, because there are already too many places in
existing code that needs addressing, and I think that it is better to show how I think/refactor/code than spend time on adding extra mappings
that will not show anything new.

I am not introducing an error handling for this application. I am also not adding any defensive code like: calling OrderService with Customer null. In real life, I would do it.

I didn't spend too much time adding test harness for this solution. The only reason is time limitation (I was told not to spend more than 45 min). I can elaborate more when asked.
Please notice that I made internal members of AtelierEntertainment visible for the AtelierEntertainment.Tests project on an assembly level (.csproj file).
I tried to solve typical problems when adding test harness, like dealing with static members, dealing with internal members, dealing with "new" keyword in CUT, dealing with sealed classes (SqlConnection).

This code still needs some love (in hours).

Few extra bits about the solution:
1) Projects are targeting .NET Core. I generally try to use .NET Standard as a default target, it gives much more flexibility and
   it is much easier to deal with such projects as complexity (amount of projects, DLL, NuGet packages that depend on each other) in an organization grows.
2) Project is using raw System Data SQL Client. I tend to use EF Core by default, or if I want something smaller Dapper. I don't think that using pure 
   Data SQL Client is the right choice in the year 2020. It was a lib to consider 10 years ago :)
   When Data SQL Client was designed, it was not yet that popular to write unit tests, that is why it is not pleasant to add a test harness on top of that code.

   I spend more than 2 hours on this solution.