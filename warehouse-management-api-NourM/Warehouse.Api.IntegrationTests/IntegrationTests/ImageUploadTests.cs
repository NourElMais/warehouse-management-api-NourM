using System.Net;

namespace Warehouse.Api.IntegrationTests.IntegrationTests;

public class ImageUploadTests:IClassFixture<CustomWebApplicationFactory>
{
    private readonly  HttpClient _client;

    public ImageUploadTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    //helper method that builds the HTTP request that will be sent to the API
    private static MultipartFormDataContent CreateForm(byte[] bytes, string fileName)
    {
        var form = new MultipartFormDataContent();

        var file = new ByteArrayContent(bytes);

        form.Add(file, "image", fileName);
        return form;
    }
    //Test1: upload jpg image
    [Fact]
    public async Task UploadImage_ValidJpg_ShouldReturnOk()
    {
        using var form = CreateForm(new byte[] { 1, 2, 3 }, "image.jpg");

        var response = await _client.PostAsync("/api/products/c50d9e28-60be-407d-a163-1af84755c3e0/image", form);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    //Test2: upload png image
    [Fact]
    public async Task UploadImage_ValidPng_ShouldReturnOk()
    {
        using var form = CreateForm(new byte[] { 1, 2, 3 }, "image.png");

        var response = await _client.PostAsync("/api/products/c50d9e28-60be-407d-a163-1af84755c3e0/image", form);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    //Test3: reject txt file 
    [Fact]
    public async Task UploadImage_TxtExtension_ShouldReturnBadRequest()
    {
        using var form = CreateForm(new byte[] { 1, 2, 3 }, "image.txt");

        var response = await _client.PostAsync("/api/products/c50d9e28-60be-407d-a163-1af84755c3e0/image", form);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    //Test4: reject oversized file 
    [Fact]
    public async Task UploadImage_ImageSizeGreaterThan2MB_ShouldReturnBadRequest()
    {
        var largeFile = new byte[3 * 1024 * 1024 ]; //3MB
        using var form = CreateForm(largeFile, "image.jpg");

        var response = await _client.PostAsync("/api/products/c50d9e28-60be-407d-a163-1af84755c3e0/image", form);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    

    
}