#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using AspNet.Core.Template.Dto;
using AspNet.Core.Template.Extensions;
using AspNet.Core.Template.Models;
using AutoMapper;

namespace AspNet.Core.Template.Configurations.Automapper.Profiles
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			// TODO: Especificar la relacion entre DTOs y Modelos (Entidades)
			CreateMap<SampleModel, SampleDto>()
				.ForMember(dest => dest.Age, opt => opt.MapFrom(d => d.DateOfBirth.CalculateAge()))
				.ForMember(dest => dest.CompleteName, opt =>
				{
					opt.MapFrom(d => string.Format("{0} {1}", d.FirstName, d.LastName));
				});
		}
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
