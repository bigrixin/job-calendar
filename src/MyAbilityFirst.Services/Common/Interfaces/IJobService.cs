using MyAbilityFirst.Models;
using System.Collections.Generic;

namespace MyAbilityFirst.Services.Common
{
	public interface IJobService
	{
		List<JobsViewModel> GetPublicJobsViewModelList();
		JobsViewModel GetJobViewModel(int id);
	}
}
