namespace Warehouse.Api.UnitTests;

public class SupplierService
{
    //Test1: create supplier
    [Fact]
    public void CreateSupplier_ValidSupplier_Succeeds()
    {
        
    }
    
    //Test2: deactivate supplier 
    [Fact]
    public void DeactivateSupplier_ValidSupplier_Succeeds()
    {
        
    }
    
    //Test3: assign supplier to product
    [Fact]
    public void AssignSupplierToProduct_ValidSupplierAndProduct_Succeeds()
    {
    }
    
    //Test4: cannot assign archived product 
    [Fact]
    public void AssignArchivedProduct_ArchivedProduct_ShouldFail()
    {
    }
    
    //Test5: cannot assign missing supplier
    [Fact]
    public void AssignMissingSupplierToProduct_ValidSupplierAndProduct_ShouldFail()
    {
    }
    
    
    
}