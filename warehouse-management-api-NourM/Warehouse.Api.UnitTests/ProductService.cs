namespace Warehouse.Api.UnitTests;

public class ProductService
{ 
   //4 tests when creating a product
   //Test1: create valid product succeeds
   [Fact]
   public void CreateProduct_ValidProduct_Succeeds() {
       
       
       
   }
   
   //Test2: duplicate SKU throws exception 
   [Fact]
   public void CreateProduct_duplicateSKU_ShouldFail() {
      
   }
   
   //Test3: created date assigned 
   [Fact]
   public void CreateProduct_AssignCreatedDate_ShouldFail() {
      
   }
   
   
   //Test4: generated id not empty 
   [Fact]
   public void CreateProduct_IdNotEmpty_Succeeds() {
      
   }
   
   //4 tests when searching for a product
   //Test1: search by name returns matches 
   [Fact]
   public void SearchProduct_ValidProductNameProvided_Succeeds()
   {
      
   }
   
   //Test2: search by supplier returns matches
   [Fact]
   public void SearchProduct_ValidSupplierNameProvided_Succeeds()
   {
      
   }
   
   //Test3: search by both filters returns intersection
   [Fact]
   public void SearchProduct_ValidSupplierAndProductNameProvided_Succeeds()
   {
      
   }
   
   //Test4: empty filters returns bad request exception
   [Fact]
   public void SearchProduct_EmptyInput_ShouldReturnException()
   {
      
   }
   
   //3 tests for updating stock quantity
   //Test1: valid quantity updates stock
   [Fact]
   public void UpdateQuantity_ValidQuantity_Succeeds()
   {
      
   }
   
   //Test2: negative quantity rejected 
   [Fact]
   public void UpdateQuantity_NegativeQuantity_ShouldFail()
   {
      
   }
   
   //Test3: last updated changes
   [Fact]
   public void UpdateQuantity_ValidQuantity_LastUpdatedShouldChange()
   {
      
   }
   
   //2 tests for updating product price
   //Test1: valid price updates 
   [Fact]
   public void UpdatePrice_ValidPrice_Succeeds()
   {
   }
   
   //Test2: invalid price rejected
   [Fact]
   public void UpdatePrice_NegativePrice_ShouldFail()
   {
   }
   
   //2 tests for archiving a product
   //Test1: delete marks archived only 
   [Fact]
   public void DeleteProduct_ValidProduct_ShouldUpdateIsArchivedToTrue()
   {
   }
   
   //Test2: archived item remains in list
   [Fact]
   public void DeleteProduct_ValidProduct_ShouldRemainInList()
   {
   }

   


}
