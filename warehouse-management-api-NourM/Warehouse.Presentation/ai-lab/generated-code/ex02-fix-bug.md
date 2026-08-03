## AI summary after fixing the logical issue

**Summary**

Root cause

Product.AssignSupplier() was missing EnsureNotArchived().

Fix
Added: EnsureNotArchived(); at the start of AssignSupplier.

Test
Added/updated an isolated unit test to verify:
assigning a supplier to an archived product
throws BusinessRuleException
with message "Archived products cannot be updated.