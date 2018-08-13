using AyerLechon.Repo.Domains;
using AyerLechon.Service.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace AyerLechon.Api.Controllers
{
    [RoutePrefix("api/Images")]
    public class ImagesController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {

            using (var ctx = new AyerLechonContext())
            {
                var imageService = new ImageService(ctx);
                var image = imageService.Get(id);

                var result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(image.UploadedFile);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(image.MIMEType);
                return result;
            }
        }

    }
}
