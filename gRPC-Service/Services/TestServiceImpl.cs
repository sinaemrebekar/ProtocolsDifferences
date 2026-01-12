using Grpc.Core;
using GrpcService;

namespace gRPC_Service.Services;

public class TestServiceImpl : TestService.TestServiceBase
{
  public override Task<TestResponse> GetTestData(
      TestRequest request,
      ServerCallContext context)
  {
    return Task.FromResult(new TestResponse
    {
      Id = request.Id,
      Message = "Hello from gRPC Service",
      Timestamp = DateTime.UtcNow.ToString("O"),
      Payload = request.Payload
    });
  }
}