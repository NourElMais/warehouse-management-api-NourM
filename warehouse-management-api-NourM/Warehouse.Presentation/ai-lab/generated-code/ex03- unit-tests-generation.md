## AI answer to Prompt 03

**Positive tests**
CreateProduct_ValidProduct_Succeeds
CreateProduct_ValidProduct_ShouldPersistProduct
CreateProduct_ValidProduct_ShouldPublishProductCreatedEvent
CreateProduct_ValidProduct_ShouldPassCancellationTokenToDependencies

**Negative tests**
CreateProduct_DuplicateSku_ShouldFail
CreateProduct_AddAsyncThrows_ShouldPropagateException_AndNotPublishEvent
CreateProduct_PublishAsyncThrows_ShouldPropagateException_AfterPersistingProduct
CreateProduct_MapperThrows_ShouldPropagateException_AfterSuccessfulPersistenceAndPublishing

**Invalid parameter-format/domain-construction cases:**
invalid name
invalid SKU
zero price
negative quantity
invalid supplier id

**Edge cases**
CreateProduct_MaximumIntegerQuantity_ShouldSucceed
CreateProduct_MinimumFutureDateBoundary_ShouldPreserveExactExpiryDate
CreateProduct_MaximumStringLengthsWithinHandler_ShouldSucceed
retained domain-level checks:
created date assigned
generated id not empty

Important architectural note
Some items from your prompt do not actually belong to CreateProductHandler in this project:

Missing database entities: CreateProductHandler does not load/check a supplier entity from the database.

Maximum string length breaches: enforced by request DTO validation, not by the handler/domain constructor.

Future date / timezone validation: enforced at the API contract layer via validation attributes, not in the handler.
Arithmetic overflow: there is no arithmetic/checked logic in the handler.
So I kept the suite honest to the existing architecture:
tested what the handler truly owns,

