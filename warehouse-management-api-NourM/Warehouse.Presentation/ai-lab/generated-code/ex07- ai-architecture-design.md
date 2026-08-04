## AI proposed design to create a Shipment Tracking Module


Goal
Add a new Shipment module that fits your current Clean Architecture + CQRS/MediatR setup and supports:
• creating shipments
• assigning products to shipments
• tracking shipment status
• updating delivery state
• notifying suppliers on important changes

Domain Layer
Create a new Shipments domain area with:
• Shipment: main aggregate root, holds shipment info, current status, delivery state, items, and history
• ShipmentItem: one product inside a shipment, stores product id, product name snapshot, quantity
• ShipmentStatus : enum like: Pending, Prepared, Dispatched, InTransit, Delivered, Cancelled
• DeliveryState: enum like: NotStarted, OutForDelivery, Delivered, DeliveryFailed, Returned
• ShipmentStatusEntry: keeps shipment history/audit trail for status changes

Important business rules
Keep these inside Shipment:
• cannot assign products to cancelled/delivered shipments
• cannot assign archived products
• product must belong to the same supplier as the shipment
• quantity must be greater than 0
• cannot dispatch an empty shipment
• cannot cancel a delivered shipment
• invalid status transitions are blocked in domain logic

Repository
Add:
• IShipmentRepository in Warehouse.Domain.Repositories
Purpose:
• load/save shipments
• get shipment by id
• list shipments
• filter by supplier/status if needed

Application Layer
Create a new Warehouse.Application/Shipments module with:
Commands
• CreateShipmentCommand
• AssignProductToShipmentCommand
• UpdateShipmentStatusCommand
• UpdateDeliveryStateCommand
• CancelShipmentCommand

Queries
• GetShipmentByIdQuery
• ListShipmentsQuery
• GetShipmentTrackingQuery
• GetShipmentStatusHistoryQuery

ViewModels
• ShipmentViewModel
• ShipmentItemViewModel
• ShipmentTrackingViewModel
• ShipmentStatusHistoryItemViewModel

Purpose
Handlers should:
• load data from repositories
• call domain methods
• save changes
• publish shipment-related events through IRabbitMqPublisher

Notifications
Reuse your existing RabbitMQ pattern.
Add integration events like:
• ShipmentCreatedEvent
• ShipmentStatusChangedEvent
• ShipmentDeliveryStateChangedEvent
• ShipmentCancelledEvent

Purpose:
• notify suppliers when important shipment changes happen
• keep notification sending outside the API itself

Presentation Layer
Add:
• ShipmentsController
Suggested endpoints:
• POST /api/shipments → create shipment
• POST /api/shipments/{id}/items → assign product
• GET /api/shipments/{id} → shipment details
• GET /api/shipments → list shipments
• GET /api/shipments/{id}/tracking → tracking info
• GET /api/shipments/{id}/history → status history
• POST /api/shipments/{id}/status → update shipment status
• POST /api/shipments/{id}/delivery-state → update delivery state
• POST /api/shipments/{id}/cancel → cancel shipment

Also add request DTOs in Warehouse.Presentation.Contracts:
• CreateShipmentRequest
• AssignProductToShipmentRequest
• UpdateShipmentStatusRequest
• UpdateDeliveryStateRequest
• CancelShipmentRequest

Infrastructure Layer
Add:
• ShipmentRepository
• EF Core mappings/configuration for:
Shipment
ShipmentItem
ShipmentStatusEntry

Purpose:
• persist shipments and related items/history
• load shipments with their items and tracking data

Folder Structure
- Domain
Shipments/
Repositories/IShipmentRepository.cs

- Application
Shipments/Commands/...
Shipments/Queries/...
Shipments/ViewModels/...
IntegrationEvents/...

- Presentation
Controllers/ShipmentsController.cs
Contracts/...shipment request DTOs...

- Infrastructure
Repositories/ShipmentRepository.cs
shipment EF configurations


Overall Flow
1.Client creates shipment
2.Shipment is saved with a supplier
3.Products are assigned to it
4.Shipment status changes over time
5.Delivery state is updated separately
6.Important changes publish RabbitMQ events
7.Supplier gets notified by a notification consumer/service
