## Exercise 01 — AI-Assisted API Feature Development 

- for this endpoint, I let the user choose the margin of expiry days, and the DTO validates that the day to be specified is between 1 and 365.
- After generating the `GET /api/products/expiring-soon` endpoint with AI, the project built successfully, but calling the endpoint returned a **500 Internal Server Error**.

The exception was:
Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone', only UTC is supported.
The generated repository code used `DateTime.Today` to filter products by their expiry date. Since the `ExpiryDate` column in PostgreSQL is stored as `timestamp with time zone`, Npgsql expects UTC `DateTime` values. Passing a local `DateTime` caused the query to fail before it could be executed.

I fixed the issue by replacing `DateTime.Today` with `DateTime.UtcNow.Date`, ensuring that the query used UTC values compatible with PostgreSQL.
After this change, the endpoint executed successfully and returned the expected list of products expiring within the specified time period.

