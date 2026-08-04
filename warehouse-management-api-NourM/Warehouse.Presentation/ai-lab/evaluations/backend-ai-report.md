# Backend AI Evaluation Report

## Best Prompt
The most effective prompt was the one used for Exercise 04 (Integration Testing).
It clearly described the endpoints to be tested, the expected assertions, and the requirement to follow the project's current structure. 
Because the prompt was specific, the AI generated integration tests that required only minor adjustments before they could be executed.

## Best Generated Code

The best AI-generated code was the integration test suite for:

- `POST /api/products`
- `POST /api/products/{id}/image`
- `DELETE /api/products/{id}`
--> (Exercise 04)

The generated tests correctly used `CustomWebApplicationFactory` and `HttpClient` (as I did manually), verified HTTP status codes and response headers, 
checked the properties of the returned model, verified that the database is working as expected...

## Incorrect Generated Code

- In Exercise 01, the generated code used `DateTime.Today`, which caused a PostgreSQL exception because the database expected UTC `DateTime` values.
- During Exercise 09, the weak prompt initially generated a Node.js solution instead of an ASP.NET Core API because the programming language was not specified.

## Human Corrections

- Replaced `DateTime.Today` with `DateTime.UtcNow.Date` to ensure compatibility with PostgreSQL.
- Ran the generated integration tests and ensured they match the existing repositories and API behavior.

## Lessons Learned

This lab showed that AI can speed up backend development, however, the developer should understand the 
generated code and architecture to be able to debug and own their project.

Also, the quality of the generated output depends on the quality of the prompt, and its specificity.
More detailed prompts produce more accurate and useful results than general ones.