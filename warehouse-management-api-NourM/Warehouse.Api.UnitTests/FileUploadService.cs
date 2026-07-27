namespace Warehouse.Api.UnitTests;

public class FileUploadService
{
    //Test1: valid jpg upload 
    [Fact]
    public void UploadImage_ValidJPG_Succeeds()
    {
        
    }
    
    //Test2: valid png upload 
    [Fact]
    public void UploadImage_ValidPNG_Succeeds()
    {
        
    }
    
    //Test3: invalid extension rejected 
    [Fact]
    public void UploadImage_InvalidExtension_ShouldFail()
    {
        
    }
    
    //Test4: file > 2MB rejected 
    [Fact]
    public void UploadImage_FileSizeGreaterThan2MB_ShouldFail()
    {
        
    }
    
    //Test5: upload path generated correctly
    [Fact]
    public void UploadImage_ValidImage_UploadPathShouldBeCorrect()
    {
        
    }
}