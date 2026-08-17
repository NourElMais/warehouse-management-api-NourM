## Exercise 07 Prompt

Inspect my current Warehouse Management API architecture and design
a new Shipment Tracking Module that fits the existing project.

The module should support:

- Creating a shipment.
- Assigning products to a shipment.
- Tracking the current shipment status.
- Updating the delivery state.
- Notifying the supplier when important shipment changes occur.

Design:
- domain models
- controllers
- services
- DTOs
- folder structure
- Provide a summary explaining the overall flow at the end.
 
Notes: 
- Follow the existing Clean Architecture and CQRS/MediatR style used in this project.
- Keep business rules inside the domain layer.
- Produce an architecture design only, do not implement anything.
- Clearly explain the purpose of every class.