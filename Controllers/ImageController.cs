using Microsoft.AspNetCore.Mvc;
using Imagekit;
using Imagekit.Sdk;
using System.Security.Cryptography.X509Certificates;
using System.Buffers.Text;
using Apartments.Models;
using System.Collections;

[ApiController]
    [Route("api/[controller]")]
public class ImageController : ControllerBase{
    private readonly ImagekitClient _imagekit;
    private static string PublicKey = "public_SwlkjmhigRrWjttyEUevhOPPbc0=";
    private static string PrivateKey = "private_ideM0C1w78jyHuQKHbNK6i9AJg4=";
    private static string EndPoint = "https://ik.imagekit.io/haroon2003";
    
    
    
    public ImageController(){
        _imagekit = new ImagekitClient(PublicKey, PrivateKey, EndPoint);
    }

    [HttpGet]
    public async Task<IActionResult> Get(){
        var authParams = _imagekit.GetAuthenticationParameters();
        return Ok(authParams);
    }

    [HttpPost]
    public IActionResult Post(Base64Model base64){
        ArrayList resp = new ArrayList();
        foreach(var x in base64.Base64String){
            var fileName = Guid.NewGuid().ToString();
            FileCreateRequest ob2 = new FileCreateRequest{
            file = x,
            fileName = fileName
        };
        resp.Add(_imagekit.Upload(ob2));
        
        }
        return Ok(new {resp});
    }

}