using System.Net;
using System.Text.Json;

namespace Awesome.Models;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);