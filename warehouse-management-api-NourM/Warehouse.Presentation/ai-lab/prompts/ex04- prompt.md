# Exercise 04 Prompt

Inspect the integration tests project, and following the same structure, 
generate an integration test for the following endpoint:
`POST /api/products`  (Full product initialization flow)
`POST /api/products/{id}/image` (Binary image stream processing)
`DELETE /api/products/{id}` (Resource teardown and cascading cleanup rules)

You have to explicitly write assertions verifying HTTP response
headers/status codes, model property match conditions, and persistent server side-effects.
Also, create a seperate file for these tests, do not overwrite any existing code.
When you finish, generate a summary of what you did.

