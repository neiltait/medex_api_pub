using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;

namespace MedicalExaminer.Common
{
    public class ExaminationPersistence : PersistenceBase, IExaminationPersistence
    {
        public ExaminationPersistence(Uri endpointUri, string primaryKey, string databaseId) : base(endpointUri,
            primaryKey, databaseId, "Examinations")
        {
        }

        public async Task<bool> SaveExaminationAsync(Examination examination)
        {
            await EnsureSetupAsync();
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, "Examinations");
            await Client.UpsertDocumentAsync(documentCollectionUri, examination);
            return true;
        }

        public async Task<Examination> GetExaminationAsync(string ExaminationId)
        {
            await EnsureSetupAsync();
            var documentUri = UriFactory.CreateDocumentUri(DatabaseId, "Examinations", ExaminationId);
            var result = await Client.ReadDocumentAsync<Examination>(documentUri);
            if (result.Document == null) throw new ArgumentException("Invalid Argument");
            return result.Document;
        }

        public async Task<IEnumerable<Examination>> GetExaminationsAsync()
        {
            await EnsureSetupAsync();
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, "Examinations");
            var feedOptions = new FeedOptions {MaxItemCount = -1};
            var query = Client.CreateDocumentQuery<Examination>(documentCollectionUri, "SELECT * FROM Examinations",
                feedOptions);
            var queryAll = query.AsDocumentQuery();
            var results = new List<Examination>();
            while (queryAll.HasMoreResults) results.AddRange(await queryAll.ExecuteNextAsync<Examination>());
            return results;
        }

        public async Task<Guid> CreateExaminationAsync(Examination examinationItem)
        {
            await EnsureSetupAsync();
            var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseId, "Examinations");
            examinationItem.Id = Guid.NewGuid().ToString();
            var examinationItemAsJson = JsonConvert.SerializeObject(examinationItem);

            var feedOptions = new FeedOptions { MaxItemCount = -1 };

            var result = Client.CreateDocumentQuery<Examination>(documentCollectionUri, examinationItemAsJson).FirstOrDefault();

            return Guid.Parse(result.Id);
        }
    }
}