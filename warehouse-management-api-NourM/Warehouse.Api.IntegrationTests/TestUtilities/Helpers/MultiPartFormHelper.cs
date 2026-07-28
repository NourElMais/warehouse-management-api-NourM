namespace Warehouse.Api.IntegrationTests.TestUtilities.Helpers;

public class MultiPartFormHelper
{
    
    //helper method that builds the HTTP request that will be sent to the API
    public static MultipartFormDataContent Create(byte[] bytes, string fileName)
    {
        var form = new MultipartFormDataContent();
        var file = new ByteArrayContent(bytes);

        form.Add(file, "image", fileName);

        return form;
    }
}