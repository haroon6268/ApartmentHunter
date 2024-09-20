using System.Buffers.Text;
using System.Collections;
using Apartments.Models;
using Imagekit.Sdk;

public class ImageService{
    private readonly ImagekitClient _imagekit;
    private static string PublicKey = "public_SwlkjmhigRrWjttyEUevhOPPbc0=";
    private static string PrivateKey = "private_ideM0C1w78jyHuQKHbNK6i9AJg4=";
    private static string EndPoint = "https://ik.imagekit.io/haroon2003";
    
    
    public ImageService(){
        _imagekit = new ImagekitClient(PublicKey, PrivateKey, EndPoint);
    }

    public List<Result> uploadImg(List<string> base64){
         List<Result> resp = new List<Result>();
        foreach(var x in base64){
            var fileName = Guid.NewGuid().ToString();
            FileCreateRequest ob2 = new FileCreateRequest{
            file = x,
            fileName = fileName
        };
        resp.Add(_imagekit.Upload(ob2));
        
        }
        return resp;
    }

}