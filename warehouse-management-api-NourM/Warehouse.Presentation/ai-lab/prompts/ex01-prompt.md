# Exercise 01 Prompt

Take a look at the existing project structure and generate a new feature for this project.
Create a new endpoint: GET /api/products/expiring-soon

Required functionality:
- Return products expiring within the next days (specified by the user) from the current day.
- Do not return already expired products.
- Create the controller action and route.
- Create the MediatR Query and QueryHandler.
- Add request validation.
- Generate unit tests using xUnit and Moq.
- Follow the existing architecture, and the same naming style.
