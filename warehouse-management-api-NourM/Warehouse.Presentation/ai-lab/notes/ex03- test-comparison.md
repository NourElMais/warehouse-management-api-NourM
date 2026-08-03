## Exercise 03 —  AI for Unit Test Generation 

Note: First, the AI model generated the tests inside an already existing testing file (it overwrote the methods),
so I asked the model to put them in a seperate file for a clearer comparison.

## Manual Test Strategy

Before using AI, I had four tests related to product creation:

- `CreateProduct_ValidProduct_Succeeds`
- `CreateProduct_duplicateSKU_ShouldFail`
- `CreateProduct_AssignCreatedDate_ShouldNotBeEqualToDefaultDateTimeValue`
- `CreateProduct_ValidProduct_IdShouldNotBeEmpty`

The AI expanded the test suite by adding many additional scenarios, including:

### Positive Tests
- Product is created successfully.
- Product is persisted.
- ProductCreated event is published.
- Maximum valid string lengths.
- Maximum integer quantity.
- Minimum future expiry date boundary.
- CancellationToken is passed to dependencies.

### Negative Tests
- Duplicate SKU.
- Repository `AddAsync` failure.
- RabbitMQ publish failure.
- AutoMapper failure after persistence.
- Invalid product name.
- Invalid SKU format.
- Invalid supplier ID.
- Negative quantity.
- Zero price.

### Edge Cases
- Maximum string length boundaries.
- Maximum integer quantity.
- Expiry date close to UTC boundary.
- CancellationToken propagation.

## Gaps Identified

Compared to my original tests, the AI suggested several scenarios that I had not considered, especially infrastructure failure cases (repository, mapper, and RabbitMQ exceptions)
and edge cases involving validation boundaries and date handling.

However, the AI-generated tests still required manual review (by running the tests, and checking the logic of each one), 
to ensure they matched the actual business rules and existing validations implemented in the project.
