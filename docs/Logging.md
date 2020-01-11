# Logging

I use logging for multiple purposes. 

1. A message for debugging purposes.
2. As a replacement for a code comment.
3. To check if something has occured. 

The final point can be very handy when writing tests. Your code may do something that isn't immediately visible. 
Verifying that the code reached a certain point can be used as an alternative. 

The ```TestContext``` captures all logged messages and allows tests to inspect them. 
This is one of the tests from the test project

```CS
[Fact]
public async Task TraceMessageNotLogged()
{
    const string message = "This is a trace message";

    using (var tc = new TestContext(_output))
    {
        tc.AdditionalConfiguration = (context, builder) =>
        {
            SetMinimumLoggingLevel(builder, LogLevel.None);
        };

        await tc.ApiClient.LogMessageAsync(LogLevel.Trace, message);

        tc.VerifyLog(MS.LogLevel.Trace, message, count: 0);
    }
}
```

The ```VerifyLog``` can be used to check if a certain message has or hasn't been logged. 