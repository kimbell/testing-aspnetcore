namespace Testing.AspNetCore.PetStore
{
    /// <summary>
    /// OPtions for the PetStore client
    /// </summary>
    public class PetStoreOptions
    {
        /// <summary>
        /// The name of the HttpClient used against petstore
        /// </summary>
        public const string HttpClientName = "PetStore";

        /// <summary>
        /// The url to use
        /// </summary>
        public string Url { get; set; }
    }
}
