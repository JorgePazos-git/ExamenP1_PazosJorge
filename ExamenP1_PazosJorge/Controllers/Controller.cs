using ExamenP1_PazosJorge.Detect_Objetcs;
using ExamenP1_PazosJorge.Entidades;
using ExamenP1_PazosJorge.Ocr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ExamenP1_PazosJorge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Controller : ControllerBase
    {

        // POST: api/Imagen
        [HttpPost]
        public async Task<ActionResult<Imagen>> PostImagen(string ruta)
        {
            //API DE RECONOCIMIENTO DE TEXTO
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-type", "application/octet-stream");
            api.Headers.Add("Ocp-Apim-Subscription-Key", "2269f3ef68244285a2e7267feef9cd8e");
            var qs = "language=es&language=true&model-version=latest";
            var url = "https://eastus.api.cognitive.microsoft.com/vision/v3.2/ocr";


            var resp = api.UploadFile(url + "?" + qs, "POST", ruta);
            var json = System.Text.Encoding.UTF8.GetString(resp);
            var texto = Newtonsoft.Json.JsonConvert.DeserializeObject<ocr_response>(json);


            //API DE DETECCIÓN DE OBJETOS
            var apiO = new System.Net.WebClient();
            apiO.Headers.Add("Content-type", "application/octet-stream");
            apiO.Headers.Add("Ocp-Apim-Subscription-Key", "2269f3ef68244285a2e7267feef9cd8e");
            var qsO = "model-version=latest";
            var urlO = "https://eastus.api.cognitive.microsoft.com/vision/v3.2/detect";


            var respO = apiO.UploadFile(urlO + "?" + qsO, "POST", ruta);
            var jsonO = Encoding.UTF8.GetString(respO);
            var objetos = Newtonsoft.Json.JsonConvert.DeserializeObject<detect_response>(jsonO);

            return Ok("TEXTO: \n" + textoOcr(texto) + "\n" + "LISTA DE OBJETOS: \n" + Objetos(objetos));
        }

        private static string textoOcr(ocr_response resp)
        {
            var txt = "";


            foreach (var region in resp.regions)
            {
                foreach (var line in region.lines)
                {
                    foreach (var word in line.words)
                    {
                        txt += word.text + " ";
                    }
                    txt += "\n";
                }
                txt += "\n";
            }
            return txt;
        }

        private static string Objetos(detect_response resp)
        {
            var text = "";
            var numP = 1;
            var numO = 1;
            foreach (var @object in resp.objects)
            {
                text += numO + ". Object: " + @object.Object + "\n";
                var formato = "";
                var parent = @object.parent;
                while (parent != null)
                {
                    formato += " ";
                    text += formato + "► Parent " + numP + ": " + parent.Object + "\n";
                    parent = parent.parent;
                    numP++;
                }
                formato += "";
                numP = 1;
                numO++;
            }
            return text;
        }
    }
}
