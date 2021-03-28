# SympliProject

# Environment
Visual Studio 2019 .Net Core 3.1 MVC

No 3rd Party libraries that are not part of the .Net framework

*3rd Party library, Moq, is used for Unit Testing only*

# Consideration
Split SympliProject into Front End and Web API two parts. This gives the flexiblity in future development. 

Microservice brings lots of benefits:
>1. Highly maintainable and testable
>2. Loosely coupled
>3. Independently deployable
>4. Organized around business capabilities
>5. Owned by a small team

# Known limitation
1. No Web API authentication or authorization
2. Manully parse html from [google.com.au](https://google.com.au/) with the keyword in the query string
