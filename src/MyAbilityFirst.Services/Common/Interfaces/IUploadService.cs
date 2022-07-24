using System.Web;

namespace MyAbilityFirst.Services.Common
{
	public interface IUploadService
	{
		string UploadToAzureStorage(HttpPostedFileBase file, string containerName);
		bool DeleteFromAzureStorage(string fileURL, string containerName);
		void ListBlobItemFromAzure(string containerName);
	}
}
