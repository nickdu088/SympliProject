# SympliProject

# Development Environment
* Visual Studio 2019 
* .Net Core 3.1 MVC

No 3rd Party libraries are used except Moq for unit testing only.

# How to run
1. Clone SympliProject to the local drive
2. Open SympliProject using Visual Studio 2019
3. Build the entire soultion
4. Run the WebAPI standalone exe
5. Run the Frontend
5. Run unit test via Visual Studio Test Explorer

# Design Consideration
Split SympliProject into Frontend MVC implementation and Web API, which decouples frontend UI implemention from backend business logic.
The benefits of this desgin include but not limited to
>1. Frontend is exchangable into different implementions (eg, Angular, React) and different client devices (web browser, mobile app) without changing backend code
>2. Both frontend and backend can be developed and released independently
>3. Both frontend and backend can be deployed seperatly into containers and scale up and down according to the demand
>4. Enable agilibity within the organization
>
Unit tests are created for both both Frontend and Web API, which not only ensure the quality of the code but also enable the pipeline of CI/CD.

# Known limitation
1. No Web API authentication or authorization
2. Manully parse html from [google.com.au](https://google.com.au/) with the keyword in the query string
